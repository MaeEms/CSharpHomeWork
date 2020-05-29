using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderWeb.Models;

namespace OrderWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrdersController(OrderContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        /*  [HttpGet]
          public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
          {
              return await _context.Orders.ToListAsync();
          }
         */

        //GET: api/order
        [HttpGet]
        public ActionResult<List<Order>> GetAllOrders()
        {
            var orders = AllOrders();
            return orders.ToList();
        }

        //GET: api/order/{id}
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderByID(string id)
        {
            var order = AllOrders().FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();
            return order;
        }

        //GET: api/order/goods?name=
        [HttpGet("goods")]
        public ActionResult<List<Order>> GetOrderByGoodsName(string name)
        {
            var orders = AllOrders()
          .Where(o => o.Items.Count(i => i.GoodsItem.Name == name) > 0);
            return orders.ToList();
        }

        //GET: api/order/customer?name=
        [HttpGet("customer")]
        public ActionResult<List<Order>> GetOrderByCustomerName(string name)
        {
            var orders = AllOrders()
          .Where(o => o.Customer.Name == name);
            return orders.ToList();
        }

        //POST: api/order
        [HttpPost]
        public ActionResult<Order> AddOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return order;
        }

        //PUT: api/order/{id}
        [HttpPut("{id}")]
        public ActionResult<Order> UpdateOrder(string id, Order order)
        {
            if (id != order.Id)
                return BadRequest("Id cannot be modified!");
            try
            {
               _context.Entry(order).State = EntityState.Modified;
               _context.SaveChanges();
            }
            catch (Exception e)
            {
                string error = e.Message;
                if (e.InnerException != null) error = e.InnerException.Message;
                return BadRequest(error);
            }
            return NoContent();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(string id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        /* [HttpDelete("{id}")]
         public async Task<ActionResult<Order>> DeleteOrder(string id)
         {
             var order = await _context.Orders.FindAsync(id);
             if (order == null)
             {
                 return NotFound();
             }

             _context.Orders.Remove(order);
             await _context.SaveChangesAsync();

             return order;
         }
         */

        //DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public ActionResult<Order> DeleteOrder(string id)
        {
            try
            {
                var order = AllOrders().FirstOrDefault(o => o.Id == id);
                if (order != null)
                {
                    for (int i = 0; i < order.Items.Count; i++)
                    {
                        _context.GoodItems.Remove(order.Items[i].GoodsItem);
                        _context.OrderItems.Remove(order.Items[i]);
                    }
                    _context.Customers.Remove(order.Customer);
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return NoContent();
        }

        private bool OrderExists(string id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private IQueryable<Order> AllOrders()
        {
            return _context.Orders.Include(o => o.Items)
                .ThenInclude(i => i.GoodsItem)
                .Include(o => o.Customer);
        }

    }
}
