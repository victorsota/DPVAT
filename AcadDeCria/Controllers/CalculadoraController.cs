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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Calculator calculator)
        {
            if (calculator == null)
            {
                return BadRequest("Dados inválidos.");
            }

            calculator.EnsureUtcDate();

            _context.calculators.Add(calculator);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = calculator.id }, calculator);
        }

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

        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetByUserId(int userid)
        {
            var calculators = await _context.calculators
                                             .Where(c => c.userid == userid)
                                             .ToListAsync();

            if (calculators == null || !calculators.Any())
            {
                return NotFound();
            }

            calculators = RadixSort(calculators);

            return Ok(calculators);
        }

        private List<Calculator> RadixSort(List<Calculator> calculators)
        {
            float maxValue = calculators.Max(c => c.resultado ?? 0);

            int exp = 1;

            while (maxValue / exp > 0)
            {
                calculators = CountingSortByDigit(calculators, exp);
                exp *= 10;
            }

            return calculators;
        }

        private List<Calculator> CountingSortByDigit(List<Calculator> calculators, int exp)
        {
            int n = calculators.Count;

            var output = new Calculator[n];
            int[] count = new int[10];

            foreach (var calculator in calculators)
            {
                int digit = (int)((calculator.resultado ?? 0) / exp) % 10;
                count[digit]++;
            }

            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = n - 1; i >= 0; i--)
            {
                int digit = (int)((calculators[i].resultado ?? 0) / exp) % 10;
                output[count[digit] - 1] = calculators[i];
                count[digit]--;
            }

            return output.ToList();
        }

    }
}
