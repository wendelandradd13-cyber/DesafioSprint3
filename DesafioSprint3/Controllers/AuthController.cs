using DesafioSprint3.Data;
using DesafioSprint3.DTOs;
using DesafioSprint3.Models;
using DesafioSprint3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesafioSprint3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // 1. POST: api/auth/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDTO dto)
        {
            // Verifica se já existe alguém com esse e-mail no banco
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (emailExiste)
            {
                return BadRequest(new { mensagem = "Este e-mail já está cadastrado." });
            }

            var novoUsuario = new Usuario
            {
                NomeUsuario = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                Perfil = dto.Perfil
            };

            await _context.Usuarios.AddAsync(novoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Usuário registrado com sucesso!" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email
            == dto.Email && u.Senha == dto.Senha);
           if (usuario == null)
            {
                return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
            }

            var token = _tokenService.GerarToken(usuario);
            return Ok(new { token = token, usuario = usuario.NomeUsuario, perfil = usuario.Perfil });
        }
    }
}
