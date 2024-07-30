using Microsoft.AspNetCore.Mvc;
using OrderManagement.DAL;
using OrderManagement.Models.ViewModels;
using OrderManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrderManagement.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            var orderViewModels = orders.Select(o => new OrderViewModel
            {
                Order = o,
                Customer = o.Customer,
                Products = o.OrderItems.Select(oi => oi.Product).ToList()
            }).ToList();

            return View(orderViewModels);
        }

        public async Task<IActionResult> Invoice(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new OrderViewModel
            {
                Order = order,
                Customer = order.Customer,
                Products = order.OrderItems.Select(oi => oi.Product).ToList()
            };

            return View(viewModel);
        }


        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            return View(new OrderCustomerViewModel());
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCustomerViewModel orderCustomerViewModel, string command = "")
        {
            if (command == "Add")
            {
                orderCustomerViewModel.Items.Add(new OrderItemViewModel());
                ViewBag.Products = _context.Products.ToList();
                return View(orderCustomerViewModel);
            }
            else if (command.Contains("delete"))
            {
                int idx = int.Parse(command.Split('-')[1]);
                orderCustomerViewModel.Items.RemoveAt(idx);
                ModelState.Clear();
                ViewBag.Products = _context.Products.ToList();
                return View(orderCustomerViewModel);
            }

            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Name = orderCustomerViewModel.CustomerName,
                    Address = orderCustomerViewModel.Address,
                    Phone = orderCustomerViewModel.Phone
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                var order = new Order
                {
                    CustomerId = customer.Id,
                    OrderItems = orderCustomerViewModel.Items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId.Value,
                        Quantity = i.Quantity,
                        UnitPrice = _context.Products.First(p => p.Id == i.ProductId.Value).Price,
                        TotalPrice = _context.Products.First(p => p.Id == i.ProductId.Value).Price * i.Quantity
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Products = _context.Products.ToList();
            return View(orderCustomerViewModel);
        }

        [HttpGet]
        public JsonResult GetProductPrice(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return Json(0);
            }
            return Json(product.Price);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderViewModel = new OrderCustomerViewModel
            {
                Id = order.Id,
                CustomerName = order.Customer.Name,
                Address = order.Customer.Address,
                Phone = order.Customer.Phone,
                Items = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Product = oi.Product
                }).ToList()
            };

            return View(orderViewModel);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderCustomerViewModel orderViewModel, string command = "")
        {
            if (id != orderViewModel.Id)
            {
                return NotFound();
            }

            if (command == "Add")
            {
                orderViewModel.Items.Add(new OrderItemViewModel());
                return View(orderViewModel);
            }
            else if (command.Contains("delete"))
            {
                int idx = int.Parse(command.Split('-')[1]);
                orderViewModel.Items.RemoveAt(idx);
                ModelState.Clear();
                return View(orderViewModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.OrderItems)
                        .Include(o => o.Customer)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (order == null)
                    {
                        return NotFound();
                    }

                    order.Customer.Name = orderViewModel.CustomerName;
                    order.Customer.Address = orderViewModel.Address;
                    order.Customer.Phone = orderViewModel.Phone;

                    _context.OrderItems.RemoveRange(order.OrderItems);
                    order.OrderItems = orderViewModel.Items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId.Value,
                        Quantity = i.Quantity,
                        UnitPrice = _context.Products.First(p => p.Id == i.ProductId).Price,
                        TotalPrice = _context.Products.First(p => p.Id == i.ProductId).Price * i.Quantity
                    }).ToList();

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orderViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(orderViewModel);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                var viewModel = new OrderViewModel
                {
                    Order = order,
                    Customer = order.Customer,
                    Products = order.OrderItems.Select(oi => oi.Product).ToList()
                };

                return View(viewModel);
            }

            // POST: Orders/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order != null)
                {
                    _context.OrderItems.RemoveRange(order.OrderItems);
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
        
    }

}
