using Microsoft.AspNetCore.Mvc;
using static ApiSevenet.JWT;

namespace ApiSevenet;

[ApiController]
[Route("api/products")]
public class ProductController(ProductRepository repository) : Controller<ProductRepository, Product>(repository)
{


	/// <summary>
	/// Get a new id for a new product
	/// </summary>
	/// <returns>
	/// A new id for a new product
	/// </returns>
	[HttpGet("newId")]
    public ActionResult<uint> GetNewId()
    {
        return repository.GetNewId();
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>
    /// All products
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        if ( !IsAuthValid(Request, out JWT token) )
		{
            return Unauthorized();
        }

		JWTPayload payload = token.GetDecodedPayload();

        return Ok(
			(await repository.GetProducts())
				.Where(p => IsAuthorized(token, p))
		);
    }

    /// <summary>
    /// Get a product by id
    /// </summary>
    /// <param name="id">The id of the product</param>
    /// <returns>
    /// The product with the given id
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(uint id)
    {
		ActionResult error = await FindAuthorizedProduct(id);
		if ( error is not OkObjectResult ok || ok.Value is not Product product )
		{
			return error;
		}
		
		return product;
    }

    // /// <summary>
    // /// Creates a new product
    // /// </summary>
    // /// <param name="product">The Product to add to the database</param>
    // /// <returns>
    // /// The added Product
    // /// </returns>
    // [HttpPut]
    // public async Task<ActionResult<Product>> Create([FromForm] Product product)
    // {
    //     if ( !IsAuthValid(Request, out JWT token) )
	// 	{
    //         return Unauthorized();
    //     }

    //     await repository.CreateProductAsync( product with
	// 		{
	// 			Id = repository.GetNewId(),
	// 			Author = token.GetDecodedPayload().id
	// 		}
	// 	);
        
    //     repository.SaveChanges();
    //     return Ok(product);
    // }

    // /// <summary>
    // /// Updates a product
    // /// </summary>
    // /// <param name="id">The id of the product</param>
    // /// <param name="product">The product to update</param>
    // /// <returns>
    // /// The updated product
    // /// </returns>
    // [HttpPut("{id}")]
    // public async Task<ActionResult<Product>> Update(uint id, [FromForm] Product product)
    // {
    //     if (id == 0)
    //     {
    //         return BadRequest();
    //     }
	// 	ActionResult error = await FindAuthorizedProduct(id);
	// 	if ( error is not OkObjectResult ok || ok.Value is not Product currentProduct )
	// 	{
	// 		return error;
	// 	}

    //     Product? result = await repository.UpdateProduct(id, product with {
	// 		Id = id,
	// 		Author = currentProduct.Author,
    //         Name = product.Name ?? currentProduct.Name,
    //         Description = product.Description ?? currentProduct.Description,
    //     });
    //     if (result is null)
    //     {
    //         return NotFound();
    //     }

    //     repository.SaveChanges();
    //     return Ok(result);
    // }

    // /// <summary>
    // /// Delete a product
    // /// </summary>
    // /// <param name="id">The id of the product</param>
    // /// <returns></returns>
    // /// <response code="200">The product was deleted</response>
    // /// <response code="404">The product was not found</response>
    // /// <response code="400">The id was 0</response>
    // [HttpDelete("{id}")]
    // public async Task<ActionResult> Delete(uint id)
    // {
    //     if (id == 0)
    //     {
    //         return BadRequest();
    //     }
	// 	ActionResult error = await FindAuthorizedProduct(id);
	// 	if (error is not OkObjectResult)
	// 	{
	// 		return error;
	// 	}
		
    //     await repository.DeleteProduct(id);

    //     repository.SaveChanges();
    //     return Ok();
    // }

	private static bool IsAuthorized(JWT token, Product product) {
		JWTPayload payload = token.GetDecodedPayload();
		return payload.user.Admin || product.Author == payload.user.Id;
	}

	private async Task<ActionResult> FindAuthorizedProduct(uint productId) {
        if ( !IsAuthValid(Request, out JWT token) )
		{
            return Unauthorized();
        }

        Product? product = await repository.GetProductById(productId);
        if (product is null)
        {
            return NotFound();
        }

		if (IsAuthorized(token, product) )
		{
			return Unauthorized();
		}

		return Ok(product);
	}
}