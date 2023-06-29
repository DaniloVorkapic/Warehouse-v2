    using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Contracts.ClassificationValues;
using WarehouseWeb.Contracts.ProductDto;
using WarehouseWeb.Data;
using WarehouseWeb.Model;
using WarehouseWeb.Services;

namespace WarehouseWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("GetAllProducts")]

        public async Task<ActionResult<Result<IEnumerable<Product>>>> GetAllProducts(int pageNumber, int pageSize, string search,string sortBy, string sortValue, [FromQuery] string classificationValuesIdList)
        {
            long[] listOfIds = ArrayHelper.ConvertToListOfIds(classificationValuesIdList);

            InputProductDto input = new InputProductDto();
            input.pageNumber = pageNumber;
            input.pageSize = pageSize;
            input.Search = search;
            input.sortBy = sortBy;
            input.sortValue = sortValue;
            input.classificationValuesIdList = listOfIds;

          Result r =  await _productService.GetAllProducts(input);
            return GetReturnResultByStatusCode(r);          
        }

        [HttpGet]
        [Route("GetAllProductCategories")]

            public async Task<ActionResult<IEnumerable<ClassificationValues>>> GetAllProductCategories(string search)
            {
            InputProductCategoriesDto input = new InputProductCategoriesDto();
            input.Search = search;

            Result r = await _productService.GetAllProductCategories(input);
            return GetReturnResultByStatusCode(r);

        }


        [HttpGet("{id}")]

        public async Task<ActionResult<Result<Product>>> GetProductById(long id)
        {
            Result r = await _productService.GetProductById(id);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPost]
        [Route("api/controller/AddProduct")]

        public async Task<ActionResult<Result<bool>>> AddProduct(ProductContract pc)
                {
            Result r  = await _productService.AddProduct(pc);
            return GetReturnResultByStatusCode(r);
        }

        [HttpPut]
        [Route("api/controller/UpdateProduct")]

        public async Task<ActionResult<Result<bool>>> UpdateProduct(ProductContract pc)
        {             
            Result r = await _productService.UpdateProduct(pc);
            return GetReturnResultByStatusCode(r);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Result<bool>>> DeleteProduct(long id)
        {
            // var orderItemToDelete =  _orderItemService.GetOrderItemById(id);
            Result r = await _productService.DeleteProduct(id);
            return GetReturnResultByStatusCode(r);           
        }
        //   // List<Product> products = new List<Product>();
        //    private readonly DataContext _context;

        //    public ProductController(DataContext context)
        //    {
        //        _context = context;
        //    }

        //    [HttpGet]

        //    public async Task<ActionResult<Product>> GetProducts()
        //    {
        //        return Ok(await _context.Product.ToListAsync());
        //    }

        //    [HttpGet("{id}")]

        //    public async Task<ActionResult<IEnumerable<Product>>> GetOneProduct(long id)
        //    {
        //        var product = await _context.Product.FindAsync(id);

        //        if(product == null)
        //        {
        //            return BadRequest("Product not found");
        //        }
        //        return Ok(product);
        //    }


        //    [HttpPost]

        //    public async Task<ActionResult<Product>> AddProduct(Product product)
        //    {

        //        _context.Product.Add(product);
        //        await _context.SaveChangesAsync();
        //         return Ok(await _context.Product.ToListAsync());
        //       // return CreatedAtAction("GetOneProduct", new { id = product.Id }, product);




        //    }

        //    [HttpPut]

        //    public async Task<ActionResult<Product>> UpdateProduct(Product request)
        //    {
        //        var product = await _context.Product.FindAsync(request.Id);

        //        if(product == null)
        //        {
        //            return BadRequest("Product not found");
        //        }

        //        product.Description = request.Description;
        //        product.Price = request.Price;
        //        product.ClassificationValuesId = request.ClassificationValuesId;

        //        await _context.SaveChangesAsync();

        //        return Ok(await _context.Product.ToListAsync());


        //    }   

        //    [HttpDelete("{id}")]

        //    public async Task<ActionResult<Product>> DeleteProdcut(long id)
        //    {
        //        var product = await _context.Product.FindAsync(id);

        //        if (product == null)
        //        {
        //            return BadRequest("Product not found");
        //        }

        //        _context.Product.Remove(product);
        //        await _context.SaveChangesAsync();
        //        return Ok(await _context.Product.ToListAsync());
        //    }


        //private readonly DataContext _dbContext;

        //public ProductController(DataContext dbContext)
        //{

        //    _dbContext = dbContext;

        //}

        //   [HttpGet]
        //   public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        //{
        //    if(_dbContext.Product == null)
        //    {
        //        return NotFound();
        //    }

        //    return await _dbContext.Product.ToListAsync();
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Product>> GetOneProduct(int id)
        //{
        //    if (_dbContext.Product == null)
        //    {
        //        return NotFound();
        //    }
        //    var product = await _dbContext.Product.FindAsync(id);

        //    if(product == null)
        //    {
        //        return NotFound();
        //    }

        //    return product ;
        //}

        //[HttpPost]

        //public async Task<ActionResult<Product>>AddProduct(Product product)
        //{

        //    _dbContext.Product.Add(product);
        //    await _dbContext.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetOneProduct), new { id = product.Id }, product);

        //}

        //[HttpPut]

        //public async Task<ActionResult>ChangeProduct(int id,Product product)
        //{
        //   if(id != product.Id){
        //        return BadRequest();
        //    }

        //    _dbContext.Entry(product).State = EntityState.Modified;



        //    try
        //    {
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch ( DbUpdateConcurrencyException)
        //    {
        //        if (!ProductAvailable(id))
        //        {
        //            return NotFound();

        //        }
        //        else
        //        {
        //            throw;
        //        }

        //    }

        //    return Ok();    


        //}

        //private bool ProductAvailable(int id)
        //{
        //    return (_dbContext.Product?.Any(x => x.Id == id)).GetValueOrDefault();
        //}

        //[HttpDelete("{id}")]


        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    if(_dbContext.Product == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _dbContext.Product.FindAsync(id);
        //    if(product == null)
        //    {
        //        return NotFound();
        //    }

        //    _dbContext.Product.Remove(product);

        //    await _dbContext.SaveChangesAsync();
        //    return Ok();
        //}
    }
}

