
namespace Project.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Project.Data;
    using Project.Data.Models;
    using Project.Infrastructure.Extensions;
    using Project.Models;
    using Project.Models.Dealers;
    using System.Linq;

 

    public class DealersController : Controller
    {
        private readonly CarRentingDbContext data;

        public DealersController(CarRentingDbContext data)
            => this.data = data;
     
        [Authorize]
        public IActionResult Become()
            =>View();
        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeDealerFormModel dealer)
        {
            var userId = this.User.Id();
            var userIdAlreadyDealer = this.data
                .Dealers
                .Any(d => d.UserId == userId);
            if (userIdAlreadyDealer)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dealer);
            }
            var dealerData = new Dealer
            {
                Name = dealer.Name,
                PhoneNumber = dealer.PhoneNumber,
                UserId = userId

            };
            this.data.Dealers.Add(dealerData);
            this.data.SaveChanges();
            return RedirectToAction("All", "Cars");
        }
        
    }
}
