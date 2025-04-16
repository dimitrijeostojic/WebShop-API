using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.API.Models.Dto;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> NewsletterSubscription(NewsletterSubscriptionDto newsletterSubscriptionDto)
        {
            return Ok();
        }
    }
}
