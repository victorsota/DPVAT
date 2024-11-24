using Microsoft.AspNetCore.Mvc;
using AcadDeCria.Models;
using AcadDeCria.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AcadDeCria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CalculatorController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/calculator
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Calculator calculator)
        {
            if (calculator == null)
            {
                return BadRequest("Dados inválidos.");
            }

            // Garantir que a data esteja em UTC
            calculator.EnsureUtcDate();

            // Adiciona o novo cálculo à tabela
            _context.calculators.Add(calculator);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = calculator.id }, calculator);
        }

        // GET api/calculator/{userId}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var calculator = await _context.calculators.FindAsync(id);

            if (calculator == null)
            {
                return NotFound();
            }

            return Ok(calculator);
        }

        // GET api/calculator/{userid}
        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetByUserId(int userid)
        {
            // Busca o cálculo com base no userid
            var calculator = await _context.calculators
                                            .Where(c => c.userid == userid)
                                            .ToListAsync();

            if (calculator == null)
            {
                return NotFound();
            }

            return Ok(calculator);
        }


    }
}
