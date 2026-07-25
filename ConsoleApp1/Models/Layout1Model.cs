using System.Collections.Generic;

namespace ConsoleApp1.Models
{
    public class EmpresaL1
    {
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public List<DocumentoL1> Documentos { get; set; }
    }

    public class DocumentoL1
    {
        public string Modelo { get; set; }
        public string Numero { get; set; }
        public decimal Valor { get; set; }
        public List<ItemDocumentoL1> Itens { get; set; }
    }

    public class ItemDocumentoL1
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }
}
