using Microsoft.AspNetCore.Mvc;
using AcadDeCria.Models;
using AcadDeCria.Services;
using AcadDeCria.Data;

namespace AcadDeCria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/user
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("não foi possivel cadastrar usuario.");
            }

            // Adiciona o produto no banco de dados
            _context.users.Add(user);
            _context.SaveChanges();

            // Retorna um código 201 (Created) com a localização do novo recurso
            return CreatedAtAction(nameof(GetById), new { id = user.id }, user);
        }

        // GET api/produto/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var produto = _context.users.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }
        // POST api/user/authenticate
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.senha))
            {
                return BadRequest("E-mail e senha são obrigatórios.");
            }

            // Busca o usuário pelo e-mail
            var existingUser = _context.users.FirstOrDefault(u => u.email == user.email);

            if (existingUser == null)
            {
                return Unauthorized("Usuário não encontrado.");
            }

            // Verifica a senha
            if (existingUser.senha != user.senha)
            {
                return Unauthorized("Senha incorreta.");
            }

            return Ok(new { message = "Autenticação bem-sucedida", userId = existingUser.id });
        }
    }
}
