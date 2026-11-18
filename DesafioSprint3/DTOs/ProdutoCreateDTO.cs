using System.ComponentModel.DataAnnotations;

namespace DesafioSprint3.DTOs
{
    public class ProdutoCreateDTO
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
