using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelAgentWeb.Data;
using TravelAgentWeb.Dtos;

namespace TravelAgentWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly TravelAgentDbContext _context;

        public NotificationsController(TravelAgentDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult FlightChanged(FlightDetailUpdateDto flightDetailUpdateDto)
        {
            Console.WriteLine($"Webhook Received from: {flightDetailUpdateDto.Publisher}");

            var secretModel = _context.SubscriptionSecrets.FirstOrDefault(s =>
                s.Publisher == flightDetailUpdateDto.Publisher &&
                s.Secret == flightDetailUpdateDto.Secret);

            if (secretModel == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Secret or Publisher");
                Console.ResetColor();
                return Ok();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Valid Webhook");
                Console.WriteLine(
                    $"Old Price {flightDetailUpdateDto.OldPrice}, New Price {flightDetailUpdateDto.NewPrice}");
                Console.ResetColor();
                return Ok();
            }
        }
    }
}