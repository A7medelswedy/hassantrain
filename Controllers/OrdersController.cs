using assignmentt.Models;
using assignmentt.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assignmentt.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AbbDbContext _context;

        public OrdersController(AbbDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }




        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders.Include(o => o.Customer).Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
            return View(orders);
        }





        public async Task<IActionResult> CreateOrder()
        {
            var customers = await _context.Customers.ToListAsync();
            var products = await _context.Products.ToListAsync();

            var viewModel = new OrderViewModel
            {
                Customers = customers,
                OrderDate = DateTime.Now,
                Products = products.Select(p => new OrderProductViewModel
                {
                    ProductName = p.Name,
                    Quantity = 0,
                    UnitPrice = p.Price
                }).ToList()
            };

            return View(viewModel); 
        }



        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderViewModel vm)
        {
            var order = new Order
            {
                CustomerID = vm.CustomerID,
                TotalAmount = vm.TotalAmount,
                OrderDate = vm.OrderDate,
                OrderItems = new List<OrderItem>()
            };

          
            for (int i = 0; i < vm.Products.Count; i++)
            {
                if (vm.Quantities[i] > 0) 
                {
                    var product = await _context.Products
                                                 .FirstOrDefaultAsync(p => p.ProductID == vm.Products[i].ProductID);
                    var orderItem = new OrderItem
                    {
                        ProductID = product.ProductID,
                        Quantity = vm.Quantities[i],
                        UnitPrice = product.Price
                    };
                    order.OrderItems.Add(orderItem);

                  
                    vm.TotalAmount += orderItem.Quantity * orderItem.UnitPrice;
                }
            }

           
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetAll");
        }



        //[HttpPost]
        //public async Task<IActionResult> CreateOrder(OrderViewModel vm)
        //{
        //    Order order = new Order()
        //    {
        //        CustomerID = vm.CustomerID,
        //        TotalAmount = vm.TotalAmount,
        //        OrderDate = vm.OrderDate,
        //        OrderItems = (ICollection<OrderItem>)vm.Products

        //    };
        //    await _context.AddAsync(order);
        //    await _context.SaveChangesAsync();
        //    return View();
        //}

        //public async Task<IActionResult> ViewOrdersByCustomer(int customerId)
        //{
        //    var orders = await _context.Orders
        //        .Where(o => o.CustomerID == customerId)
        //        .Select(o => new OrderDetailsViewModel
        //        {
        //            OrderID = o.OrderID,
        //            CustomerName = o.Customer.Name,
        //            Items = o.OrderItems.Select(oi => new OrderItemViewModel
        //            {
        //                ProductName = oi.Product.Name,
        //                Quantity = oi.Quantity,
        //                UnitPrice = oi.UnitPrice
        //            }).ToList(),
        //            TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
        //        }).ToListAsync();

        //    return View(orders);
        //}


        //public async Task<IActionResult> Details(int id)
        //{
        //    var order = await _context.Orders
        //        .Where(o => o.OrderID == id)
        //        .Select(o => new OrderDetailsViewModel
        //        {
        //            OrderID = o.OrderID,
        //            CustomerName = o.Customer.Name,
        //            Items = o.OrderItems.Select(oi => new OrderItemViewModel
        //            {
        //                ProductName = oi.Product.Name,
        //                Quantity = oi.Quantity,
        //                UnitPrice = oi.UnitPrice
        //            }).ToList(),
        //            TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
        //        }).FirstOrDefaultAsync();

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}
    }
}
