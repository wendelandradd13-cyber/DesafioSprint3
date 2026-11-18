using DesafioSprint3.Data;
using DesafioSprint3.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioSprint3.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        // Construtor: recebe o DbContext por Injeção de Dependência
        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. Obter todos os produtos
        public async Task<List<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        // 2. Obter um produto pelo Id
        public async Task<Produto?> ObterPorIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        // 3. Criar um novo produto
        public async Task<Produto> CriarAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        // 4. Atualizar um produto existente
        public async Task AtualizarAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        // 5. Remover um produto pelo Id
        public async Task RemoverAsync(int id)
        {
            var produto = await ObterPorIdAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }
        }
    }
}