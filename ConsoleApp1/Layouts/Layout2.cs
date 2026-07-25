using ConsoleApp1.Models;
using GeradorTxt;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class Layout2
    {
        public class SumDTO
        {
            public int qtd00 { get; set; }
            public int qtd01 { get; set; }
            public int qtd02 { get; set; }
            public int qtd03 { get; set; }
            public int qtdTotal => qtd00 + qtd01 + qtd02 + qtd03;
        }

        public void Gerar(List<EmpresaL2> empresas, string outputPath)
        {
            var sb = new StringBuilder();

            var sum = new SumDTO
            {
                qtd00 = empresas.Count(),
                qtd01 = empresas.Sum(x => x.Documentos.Count()),
                qtd02 = empresas.Sum(x => x.Documentos.Sum(y => y.Itens.Count())),
                qtd03 = empresas.Sum(x => x.Documentos.Sum(y => y.Itens.Sum(i => i.Categorias.Count())))
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
                        foreach (var cat in item.Categorias)
                        {
                            EscreverTipo03(sb, cat);
                        }
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

        protected void EscreverTipo00(StringBuilder sb, EmpresaL2 emp)
        {
            // 00|CNPJEMPRESA|NOMEEMPRESA|TELEFONE
            sb.Append("00").Append("|")
              .Append(emp.CNPJ).Append("|")
              .Append(emp.Nome).Append("|")
              .Append(emp.Telefone).AppendLine();
        }

        protected void EscreverTipo01(StringBuilder sb, DocumentoL2 doc)
        {
            // 01|MODELODOCUMENTO|NUMERODOCUMENTO|VALORDOCUMENTO
            sb.Append("01").Append("|")
              .Append(doc.Modelo).Append("|")
              .Append(doc.Numero).Append("|")
              .Append(ToMoney(doc.Valor)).AppendLine();
        }

        protected void EscreverTipo02(StringBuilder sb, ItemDocumentoL2 item)
        {
            // 02|NUMEROITEM|DESCRICAOITEM|VALORITEM
            sb.Append("02").Append("|")
              .Append(item.NumeroItem).Append("|")
              .Append(item.Descricao).Append("|")
              .Append(ToMoney(item.Valor)).AppendLine();
        }
        protected void EscreverTipo03(StringBuilder sb, CategoriasL2 cat)
        {
            // 03|NUMEROCATEGORIA|DESCRICAOCATEGORIA
            sb.Append("03").Append("|")
              .Append(cat.NumeroCategoria).Append("|")
              .Append(cat.DescricaoCategoria).Append("|").AppendLine();
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

            // 09|03|QUANTIDADE_LINHAS_DO_TIPO_03
            sb.Append("09").Append("|")
              .Append($"03").Append("|")
              .Append($"{dto.qtd03}").AppendLine();
        }

        protected void EscreverTipo99(StringBuilder sb, SumDTO dto)
        {
            // 99|QUANTIDADE_LINHAS_NO_ARQUIVO
            sb.Append("99").Append("|") // A soma do número 5 corresponde as linhas somadas do 00, 01, 02, 03 e 99.
              .Append($"{dto.qtdTotal + 5}");
        }


        [Test]
        public void Test()
        {
            SumDTO sum = new SumDTO
            {
                qtd00 = 10,
                qtd01 = 15,
                qtd02 = 3,
                qtd03 = 10
            };

            Assert.That(sum.qtdTotal, Is.EqualTo(25));
        }

        [Test]
        public void TestVl()
        {
            string Path = "C:/CAMINHO_JSON/base-dados-nunittest.json";
            var dados = JsonRepository.LoadEmpresasL1(Path);


            foreach (var dado in dados)
            {
                foreach (var doc in dado.Documentos)
                {
                    decimal sumVl = doc.Itens.Sum(i => i.Valor);

                    Assert.That(sumVl, Is.EqualTo(doc.Valor));
                }
            }
        }
    }
}
