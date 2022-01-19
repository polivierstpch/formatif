using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Web2.API.Models;
using System.Linq;

namespace Web2.API.Controllers
{   
    [ApiController]
    [Route("products")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> _products = new() { new Product { Id = 1, Name = "Patate" } };

        public ActionResult<IEnumerable<Product>> Get()
        {
            return _products;
        }

        public ActionResult<Product> Get(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product is null)
                return NotFound();

            return product;
        }

        [HttpPost]
        public IActionResult Post(Product product)
        {
            var productExists = _products.Any(p => p.Id == product.Id);

            if (productExists)
                return BadRequest();

            product.Id = _products.Count + 1;
            _products.Add(product);

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productToDelete = _products.FirstOrDefault(p => p.Id == id);

            if (productToDelete is null)
                return NotFound();

            _products.Remove(productToDelete);

            return NoContent();
        }
    }
}
