using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand = null, string? type = null, string? sort = null)
    {
        ProductSpecification spec = new ProductSpecification(brand, type, sort);
        var products = await repo.GetListAsync(spec);
        return Ok(products);
    }

    [HttpGet("{id:int}")] //api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost] //api/products/2
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        return BadRequest("Failed to create product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
        {
            return BadRequest("Cannot update this product");
        }

        repo.Update(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }
        
        return BadRequest("Failed to update product");

    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null) return NotFound();

        repo.Remove(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Failed to delete product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        var brands =  await repo.GetListAsync(spec);
        return Ok(brands);
    }
    
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        var types = await repo.GetListAsync(spec);
        return Ok(types);
    }

    private bool ProductExists(int id)
    {
        return repo.Exists(id);
    }
}
