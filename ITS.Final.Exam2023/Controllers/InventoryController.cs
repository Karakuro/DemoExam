using ITS.Final.Exam2023.Data;
using ITS.Final.Exam2023.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITS.Final.Exam2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly WarehouseDBContext _ctx;

        public InventoryController(ILogger<InventoryController> logger, WarehouseDBContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        /// <summary>
        /// Metodo per verificare che il back-end sia raggiungibile
        /// </summary>
        /// <returns>Ok</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody]InventoryModel model)
        {
            if(!_ctx.Products.Any(p => p.ProductId == model.Code))
                return UnprocessableEntity();

            Inventory newInv = new Inventory() 
            { 
                ProductId = model.Code, 
                Quantity = model.Quantity, 
                Timestamp = DateTime.Now 
            };
            _ctx.Add(newInv);

            if(_ctx.SaveChanges() > 0)
                return NoContent();
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
