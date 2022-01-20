using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Web2.API.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Web2.API.Controllers
{   
    [ApiController]
    [Route("products")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> _products = new() { new Product { Id = 1, Name = "Patate" } };
        
        /// <summary>
        /// Retourne une liste de produits.
        /// </summary>
        /// <returns>Une liste de produits (peut être vide)</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return _products;
        }
        
        /// <summary>
        /// Retourne le produit avec l'id passé dans l'URI
        /// </summary>
        /// <param name="id">Identifiant unique du produit</param>
        /// <returns>Le produit avec l'id ou bien une réponse 404 NotFound</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> Get(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product is null)
                return NotFound();

            return product;
        }
        
        /// <summary>
        /// Ajoute un produit à la liste, sauf s'il y a déjà un produit avec le même id.
        /// </summary>
        /// <param name="product">Le produit à ajouter</param>
        /// <returns>Une réponse HTTP avec l'URI pour obtenir le produit, ainsi qu'une copie du produit dans le corps de la réponse.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(Product product)
        {
            var productExists = _products.Any(p => p.Id == product.Id);

            if (productExists)
                return BadRequest();

            product.Id = _products.Count + 1;
            _products.Add(product);

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }
        
        /// <summary>
        /// Supprime un produit de la liste selon l'id passé dans l'URI.
        /// </summary>
        /// <param name="id">Id du produit à supprimer</param>
        /// <returns>Une réponse HTTP NoContent si la suppression est effectuée, sinon une réponse 404 NotFound</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
