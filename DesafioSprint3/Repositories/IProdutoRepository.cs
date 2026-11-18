using DesafioSprint3.Models;

namespace DesafioSprint3.Repositories
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> ObterTodosAsync();
        Task<Produto?> ObterPorIdAsync(int id);
        Task<Produto> CriarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task RemoverAsync(int id);
    }
}
