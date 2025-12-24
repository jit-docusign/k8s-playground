using Microsoft.AspNetCore.Mvc;
using OrderApi.Services;
using System.Text.Json;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderService orderService,
            IHttpClientFactory httpClientFactory,
            ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Order>> GetAll()
        {
            _logger.LogInformation("Getting all orders");
            return Ok(_orderService.GetAllOrders());
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetById(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return NotFound(new { message = "Order not found" });
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] CreateOrderRequest request)
        {
            _logger.LogInformation("Creating order for product {ProductId}", request.ProductId);

            try
            {
                var client = _httpClientFactory.CreateClient();
                var productApiUrl = Environment.GetEnvironmentVariable("PRODUCT_API_URL")
                    ?? "http://product-api:80";

                // Add API Key authentication header if configured
                var apiKey = Environment.GetEnvironmentVariable("PRODUCT_API_KEY");
                if (!string.IsNullOrEmpty(apiKey))
                {
                    client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
                }

                var response = await client.GetAsync($"{productApiUrl}/api/product/{request.ProductId}");

                if (!response.IsSuccessStatusCode)
                    return BadRequest(new { message = "Product not found" });

                var productJson = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(productJson);
                var priceElement = doc.RootElement.GetProperty("price");
                var productPrice = priceElement.GetDecimal();

                var order = _orderService.CreateOrder(request.ProductId, request.Quantity, productPrice);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, new { message = "Error creating order", error = ex.Message });
            }
        }

        [HttpGet("health")]
        public ActionResult<object> Health()
        {
            var hostname = System.Environment.GetEnvironmentVariable("HOSTNAME") ?? "unknown";
            return Ok(new { status = "Healthy", service = "OrderApi", hostname });
        }
    }

    public class CreateOrderRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
