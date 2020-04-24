using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace myCrawler
{
    

    public class Crawler
    {
        //所有已下载和待下载URL，key是URL，value表示是否下载成功
        private Dictionary<string, bool> urls = new Dictionary<string, bool>();
        //待下载队列
        private Queue<string> pending = new Queue<string>();

        public string HostFilter { get; set; } //主机过滤规则
        public string FileFilter { get; set; } //文件过滤规则
       

        private int count = 0;

        static void Main(string[] args)
        {
            Crawler myClawler = new Crawler();
            string startUrl = "http://www.cnblogs.com/datang2000/";
            if (args.Length >= 1) startUrl = args[0];

            myClawler.urls.Add(startUrl, false);

            new Thread(myClawler.Crawl).Start();
            Console.ReadKey();

        }

        private void Crawl()
        {
           
           
           
            Console.WriteLine("begin to work");
            while (true)
            {
                string current = null;
                foreach(string url in urls.Keys)
                {
                    if ((bool)urls[url]) continue;
                    current = url;
                   
                }
                if (current == null || count > 10) break;
                Console.WriteLine("爬行" + current + "页面!");
                string html = DownLoad(current);
                urls[current] = true;
                Parse(html, current);
                count++;
                
            }
            Console.WriteLine("work end");
        }

        private void Parse(string html,string pageUrl)
        {
           
           


            string strRef = @"(href|HREF)[]*=[]*[""'](?<url>[^""'#>]+)[""']";
            MatchCollection matches = new Regex(strRef).Matches(html);
            foreach(Match match in matches)
            {
                string linkUrl = match.Groups["url"].Value;
                if (linkUrl == null || linkUrl == "") continue;
                linkUrl = FixUrl(linkUrl, pageUrl);//转绝对路径
                                                   //解析出host和file两个部分，进行过滤
                Match linkUrlMatch = Regex.Match(linkUrl, @"^(?<site>https?://(?<host>[\w.-]+)(:\d+)?($|/))(\w+/)*(?<file>[^#?]*)");
                string host = linkUrlMatch.Groups["host"].Value;
                string file = linkUrlMatch.Groups["file"].Value;
                HostFilter = "^" + host + "$";
                FileFilter = ".html?$";
                if (file == "") file = "index.html";
                if (Regex.IsMatch(host, HostFilter) && Regex.IsMatch(file, FileFilter))
                {
                    if (!urls.ContainsKey(linkUrl))
                    {
                        pending.Enqueue(linkUrl);
                        urls.Add(linkUrl, false);
                    }
                }
            }
        }

        private string DownLoad(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string html = webClient.DownloadString(url);

                string fileName = count.ToString();
                File.WriteAllText(fileName, html, Encoding.UTF8);
                return html;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }


        //将相对路径转为绝对路径
        static private string FixUrl(string url, string pageUrl)
        {
            if (url.Contains("://"))
            {
                return url;
            }
            if (url.StartsWith("//"))
            {
                return "http:" + url;
            }
            if (url.StartsWith("/"))
            {
                Match urlMatch = Regex.Match(pageUrl, @"^(?<site>https?://(?<host>[\w.-]+)(:\d+)?($|/))(\w+/)*(?<file>[^#?]*)");
                String site = urlMatch.Groups["site"].Value;
                return site.EndsWith("/") ? site + url.Substring(1) : site + url;
            }

            if (url.StartsWith("../"))
            {
                url = url.Substring(3);
                int idx = pageUrl.LastIndexOf('/');
                return FixUrl(url, pageUrl.Substring(0, idx));
            }

            if (url.StartsWith("./"))
            {
                return FixUrl(url.Substring(2), pageUrl);
            }

            int end = pageUrl.LastIndexOf("/");
            return pageUrl.Substring(0, end) + "/" + url;
        }
    }
}

