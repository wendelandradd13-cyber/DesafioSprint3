namespace DesafioSprint3.DTOs
{
    public class ProdutoResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
