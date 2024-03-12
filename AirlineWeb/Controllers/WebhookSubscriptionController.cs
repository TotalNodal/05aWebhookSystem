using Microsoft.AspNetCore.Mvc;
using AirlineWeb.Data;
using AirlineWeb.Models;
using AirlineWeb.Dtos;
using System.Linq;
using AutoMapper;
using System;


namespace AirlineWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookSubscriptionController : ControllerBase
    {
        private readonly AirlineDbContext _context;
        private readonly IMapper _mapper;

        public WebhookSubscriptionController(AirlineDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{secret}", Name = "GetSubscriptionBySecret")]
        public ActionResult<WebhookSubscriptionReadDto> GetSubscriptionBySecret(string secret)
        {
            var subscription = _context.WebhookSubscriptions.FirstOrDefault(s => s.Secret == secret);
            if (subscription == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<WebhookSubscriptionReadDto>(subscription));
        }


        //pass back read dto and expects to receive a create dto as an input
        [HttpPost]
        public ActionResult<WebhookSubscriptionReadDto> CreateSubscription(WebhookSubscriptionCreateDto webhookSubscriptionCreateDto)
        {
            var subscription =
                _context.WebhookSubscriptions.FirstOrDefault(s => s.WebhookURI == webhookSubscriptionCreateDto.WebhookURI);
            if (subscription == null)
            {
                //map the create dto to the subscription model
                subscription = _mapper.Map<WebhookSubscription>(webhookSubscriptionCreateDto);
                //add the subscription to the context
                subscription.Secret = Guid.NewGuid().ToString();
                //save the changes
                subscription.WebhookPublisher = "CardoAir";
                try
                {
                    _context.WebhookSubscriptions.Add(subscription);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                //map the subscription to the read dto
                var webhookSubscriptionReadDto = _mapper.Map<WebhookSubscriptionReadDto>(subscription);

                //return the read dto
                return CreatedAtRoute(nameof(GetSubscriptionBySecret), new {secret = webhookSubscriptionReadDto.Secret},
                                       webhookSubscriptionReadDto);
            }
            else
            {
                return BadRequest("Webhook URI already exists");
            }
        }
    }
}