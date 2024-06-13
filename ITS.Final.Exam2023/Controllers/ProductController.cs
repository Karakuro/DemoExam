using ITS.Final.Exam2023.Data;
using ITS.Final.Exam2023.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITS.Final.Exam2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly WarehouseDBContext _ctx;

        public ProductController(ILogger<ProductController> logger, WarehouseDBContext ctx)
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

        [HttpGet]
        [Route("Report")]
        public IActionResult Report()
        {
            var reportData = _ctx.Products.Include(p => p.Inventories).Select(p => new ReportModel
            {
                Code = p.ProductId,
                Description = p.Description,
                Total = p.Inventories.Sum(i => i.Quantity)
            }).Where(r => r.Total > 0).ToList();
            return Ok(reportData);
        }

        [HttpGet]
        [Route("Report/{hint}")]
        public IActionResult GetByHint(string hint)
        {
            hint = hint.Trim().ToLower();
            var results = _ctx.Products.Where(p => p.ProductId.ToLower().Contains(hint)).Select(p => p.ProductId).ToList();
            return Ok(results);
        }
    }
}
