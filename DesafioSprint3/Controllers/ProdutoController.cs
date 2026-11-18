using DesafioSprint3.DTOs;
using DesafioSprint3.Models;
using DesafioSprint3.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DesafioSprint3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        // 1 - GET: api/Produtos
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var produtos = await _produtoRepository.ObterTodosAsync();

            // Mapeia a lista de entidades para uma lista de DTOs
            var response = produtos.Select(p => new ProdutoResponseDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque,
                DataCriacao = p.DataCriacao
            }).ToList();

            return Ok(response);
        }

        // 2 - GET: api/Produtos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
            {
                return NotFound(new {mensagem = "Produto não encontrado."});
            }
            // Mapeia a entidade para um DTO
            var response = new ProdutoResponseDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                DataCriacao = produto.DataCriacao
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ProdutoCreateDTO dto)
        {
            // 1: Passamos os dados do DTO para a entidade Produto (que vai pro banco)
            var novoProduto = new Produto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco,
                QuantidadeEstoque = dto.QuantidadeEstoque
            };

            // 2: Pedimos para o repositório salvar no MySQL (ele vai preencher o Id automaticamente)
            await _produtoRepository.CriarAsync(novoProduto);

            // 3: Montamos o DTO de resposta para devolver ao cliente
            var response = new ProdutoResponseDTO
            {
                Id = novoProduto.Id,
                Nome = novoProduto.Nome,
                Descricao = novoProduto.Descricao,
                Preco = novoProduto.Preco,
                QuantidadeEstoque = novoProduto.QuantidadeEstoque,
                DataCriacao = novoProduto.DataCriacao
            };

            // 4: Devolvemos o status 201 Created indicando onde consultar esse novo item
            return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ProdutoCreateDTO dto)
        {
            var produtoExistente = await _produtoRepository.ObterPorIdAsync(id);
            if (produtoExistente == null)
            {
                return NotFound(new { mensagem = "Produto não encontrado." });
            }
            // Atualiza os campos do produto existente com os dados do DTO
            produtoExistente.Nome = dto.Nome;
            produtoExistente.Descricao = dto.Descricao;
            produtoExistente.Preco = dto.Preco;
            produtoExistente.QuantidadeEstoque = dto.QuantidadeEstoque;
            await _produtoRepository.AtualizarAsync(produtoExistente);
            return NoContent(); // Retorna 204 No Content indicando que a atualização foi bem-sucedida
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deletar(int id)
        {
            var produtoExistente = await _produtoRepository.ObterPorIdAsync(id);
            if (produtoExistente == null)
            {
                return NotFound(new { mensagem = "Produto não encontrado." });
            }
            await _produtoRepository.RemoverAsync(id);
            return NoContent(); // Retorna 204 No Content indicando que a exclusão foi bem-sucedida
        }

    }
}
