using Microsoft.AspNetCore.Mvc;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Product>> GetAll()
        {
            _logger.LogInformation("Getting all products");
            return Ok(_service.GetAllProducts());
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _service.GetProductById(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Create([FromBody] CreateProductRequest request)
        {
            _logger.LogInformation("Creating product: {Name}", request.Name);
            var product = _service.CreateProduct(request.Name, request.Price, request.Description);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpGet("health")]
        public ActionResult<object> Health()
        {
            var hostname = System.Environment.GetEnvironmentVariable("HOSTNAME") ?? "unknown";
            return Ok(new { status = "Healthy", service = "ProductApi", hostname });
        }
    }

    public class CreateProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
