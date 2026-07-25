using ConsoleApp1.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class Layout1
    {
        public class SumDTO
        {
            public int qtd00 {  get; set; }
            public int qtd01 {  get; set; }
            public int qtd02 {  get; set; }
            public int qtdTotal => qtd00 + qtd01 + qtd02;
        }

        public void Gerar(List<EmpresaL1> empresas, string outputPath)
        {
            var sb = new StringBuilder();

            var sum = new SumDTO
            {
                qtd00 = empresas.Count(),
                qtd01 = empresas.Sum(x => x.Documentos.Count()),
                qtd02 = empresas.Sum(x => x.Documentos.Sum(y => y.Itens.Count()))
            };

            foreach (var emp in empresas)
            {
                EscreverTipo00(sb, emp);
                foreach (var doc in emp.Documentos)
                {
                    EscreverTipo01(sb, doc);
                    foreach (var item in doc.Itens)
                    {
                        EscreverTipo02(sb, item);
                    }
                }
            }

            EscreverTipo09(sb, sum);
            EscreverTipo99(sb, sum);
            File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
        }

        protected string ToMoney(decimal val)
        {
            // Força ponto como separador decimal, conforme muitos leiautes.
            return val.ToString("0.00", CultureInfo.InvariantCulture);
        }

        protected void EscreverTipo00(StringBuilder sb, EmpresaL1 emp)
        {
            // 00|CNPJEMPRESA|NOMEEMPRESA|TELEFONE
            sb.Append("00").Append("|")
              .Append(emp.CNPJ).Append("|")
              .Append(emp.Nome).Append("|")
              .Append(emp.Telefone).AppendLine();
        }

        protected void EscreverTipo01(StringBuilder sb, DocumentoL1 doc)
        {
            // 01|MODELODOCUMENTO|NUMERODOCUMENTO|VALORDOCUMENTO
            sb.Append("01").Append("|")
              .Append(doc.Modelo).Append("|")
              .Append(doc.Numero).Append("|")
              .Append(ToMoney(doc.Valor)).AppendLine();
        }

        protected void EscreverTipo02(StringBuilder sb, ItemDocumentoL1 item)
        {
            // 02|DESCRICAOITEM|VALORITEM
            sb.Append("02").Append("|")
              .Append(item.Descricao).Append("|")
              .Append(ToMoney(item.Valor)).AppendLine();
        }

        protected void EscreverTipo09(StringBuilder sb, SumDTO dto)
        {
            // 09|00|QUANTIDADE_LINHAS_DO_TIPO_00
            sb.Append("09").Append("|")
              .Append($"00").Append("|")
              .Append($"{dto.qtd00}").AppendLine();

            // 09|01|QUANTIDADE_LINHAS_DO_TIPO_01
            sb.Append("09").Append("|")
              .Append($"01").Append("|")
              .Append($"{dto.qtd01}").AppendLine();

            // 09|02|QUANTIDADE_LINHAS_DO_TIPO_02
            sb.Append("09").Append("|")
              .Append($"02").Append("|")
              .Append($"{dto.qtd02}").AppendLine();
        }

        protected void EscreverTipo99(StringBuilder sb, SumDTO dto)
        {
            // 99|QUANTIDADE_LINHAS_NO_ARQUIVO
            sb.Append("99").Append("|") // A soma do número 4 corresponde as linhas somadas do 00, 01, 02 e 99.
              .Append($"{dto.qtdTotal + 4}");
        }

        [Test]
        public void Test()
        {
            SumDTO sum = new SumDTO
            {
                qtd00 = 10,
                qtd01 = 15,
                qtd02 = 3
            };

            Assert.That(sum.qtdTotal, Is.EqualTo(28));
        }
    }
}
