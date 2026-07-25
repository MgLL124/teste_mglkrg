using System.Collections.Generic;

namespace ConsoleApp1.Models
{
    public class EmpresaL2
    {
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public List<DocumentoL2> Documentos { get; set; }
    }

    public class DocumentoL2
    {
        public string Modelo { get; set; }
        public string Numero { get; set; }
        public decimal Valor { get; set; }
        public List<ItemDocumentoL2> Itens { get; set; }
    }

    public class ItemDocumentoL2
    {
        public int NumeroItem { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public List<CategoriasL2> Categorias { get; set; }
    }

    public class CategoriasL2
    {
        public int NumeroCategoria { get; set; }
        public string DescricaoCategoria { get; set; }
    }
}
