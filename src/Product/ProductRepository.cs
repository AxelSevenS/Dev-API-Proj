using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace ApiSevenet;

public class ProductRepository : Repository<Product>
{
    public static readonly string fileName = "data.json";


    public ProductRepository() : base(fileName) {}


    /// <summary>
    /// Save the data to the file
    /// </summary>
    public override void SaveChanges()
    {
        string jsonString = JsonSerializer.Serialize(Data);
        File.WriteAllText(fileName, jsonString);
    }

    /// <summary>
    /// Get a new id for a product
    /// </summary>
    /// <returns>
    /// A new id
    /// </returns>
    public uint GetNewId() =>
        Data.Aggregate((uint)0, (max, p) => p.Id > max ? p.Id : max) + 1;

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>
    /// All products
    /// </returns>
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await Task.Run(() => Data);
    }

    /// <summary>
    /// Get a product by id
    /// </summary>
    /// <param name="id">The id of the product</param>
    /// <remarks>
    /// This will not update the database, use <see cref="SaveChanges"/> to do that
    /// </remarks>
    /// <returns>
    /// The product with the given id
    /// </returns>
    public async Task<Product?> GetProductById(uint id)
    {
        return await Task.Run(() => Data.FirstOrDefault(x => x.Id == id));
    }

    // /// <summary>
    // /// Post a product
    // /// </summary>
    // /// <param name="product">The product to add to the database</param>
    // /// <remarks>
    // /// This will not update the database, use <see cref="SaveChanges"/> to do that
    // /// </remarks>
    // /// <returns>
    // /// The added product
    // /// </returns>
    // public async Task<Product?> CreateProductAsync(Product product)
    // {
    //     return await Task.Run(() =>
    //     {
    //         Data.Add(product);
    //         return product;
    //     });
    // }

    // /// <summary>
    // /// Update a product
    // /// </summary>
    // /// <param name="id">The id of the product</param>
    // /// <param name="product">The product to update</param>
    // /// <remarks>
    // /// This will not update the database, use <see cref="SaveChanges"/> to do that
    // /// </remarks>
    // /// <returns>
    // /// The updated product
    // /// </returns>
    // public async Task<Product?> UpdateProduct(uint id, Product product)
    // {
    //     Product? productToUpdate = await GetProductById(id);
    //     if (productToUpdate is not null)
    //     {
    //         productToUpdate = Data[Data.IndexOf(productToUpdate)] = productToUpdate with
    //         {
    //             Name = product.Name,
    //             Description = product.Description,
    //         };
    //     }
        
    //     return productToUpdate;
    // }

    // /// <summary>
    // /// Delete a product
    // /// </summary>
    // /// <param name="id">The id of the product</param>
    // /// <remarks>
    // /// This will not update the database, use <see cref="SaveChanges"/> to do that
    // /// </remarks>
    // /// <returns>
    // /// The deleted product
    // /// </returns>
    // public async Task<Product?> DeleteProduct(uint id)
    // {
    //     Product? productToDelete = await GetProductById(id);
    //     if (productToDelete is not null)
    //     {
    //         Data.Remove(productToDelete);
    //     }

    //     return productToDelete;
    // }
}