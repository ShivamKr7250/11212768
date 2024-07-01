using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// Make sure you have the necessary namespaces
namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class TopProductsController : ControllerBase
    {
        private readonly ExternalApiService _externalApiService;

        public TopProductsController()
        {
            _externalApiService = new ExternalApiService();
        }

        [HttpGet("{categoryName}/products")]
        public async Task<IActionResult> GetTopProducts(string categoryName, [FromQuery] string companyName, [FromQuery] int top, [FromQuery] int minPrice, [FromQuery] int maxPrice)
        {
            var products = await _externalApiService.GetTopProducts(companyName, categoryName, top, minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("{categoryName}/products/{productId}")]
        public async Task<IActionResult> GetProductDetails(string categoryName, string productId, [FromQuery] string companyName)
        {
            var product = await _externalApiService.GetProductDetails(companyName, categoryName, productId);
            return Ok(product);
        }
    }
}
