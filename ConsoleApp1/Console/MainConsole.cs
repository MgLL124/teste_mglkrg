using ConsoleApp1;
using ConsoleApp1.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeradorTxt
{
    /// <summary>
    /// Responsável por interagir com o usuário via console.
    /// </summary>
    public static class MainConsole
    {
        private static string _jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "base-dados.json");
        private static string _outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "out");

        public static void Run()
        {
            Directory.CreateDirectory(_outputDir);
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Menu");
                Console.WriteLine("1. Configurar arquivo .json (base de dados)");
                Console.WriteLine("2. Configurar diretório de output");
                Console.WriteLine("3. Gerar arquivo");
                Console.WriteLine("0. Sair");
                Console.Write("Opção: ");

                var opt = Console.ReadLine();
                Console.WriteLine();

                switch (opt)
                {
                    case "1":
                        Console.Write("Informe o caminho completo do arquivo .json: ");
                        var jp = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(jp) && File.Exists(jp))
                        {
                            _jsonPath = jp;
                            Console.WriteLine("OK! JSON configurado: " + _jsonPath);
                        }
                        else
                        {
                            Console.WriteLine("Caminho inválido ou arquivo não encontrado.");
                        }
                        break;

                    case "2":
                        Console.Write("Informe o diretório de saída para o .txt: ");
                        var od = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(od))
                        {
                            _outputDir = od;
                            Directory.CreateDirectory(_outputDir);
                            Console.WriteLine("OK! Diretório de saída configurado: " + _outputDir);
                        }
                        else
                        {
                            Console.WriteLine("Diretório inválido.");
                        }
                        break;

                    case "3":
                        Console.Write("Gerar arquivo \n");
                        try
                        {
                            Console.Write("Informe a versão do layout: ");
                            int generateLayout = Int32.Parse(Console.ReadLine());
                            bool erro = false;

                            if (generateLayout.Equals(1))
                            {
                                var gerador = new Layout1();

                                var dados = JsonRepository.LoadEmpresasL1(_jsonPath);

                                foreach (var dado in dados)
                                {
                                    foreach (var doc in dado.Documentos)
                                    {
                                        decimal sumVl = doc.Itens.Sum(i => i.Valor);

                                        if (sumVl != doc.Valor)
                                        {
                                            Console.WriteLine($"O valor somado dos itens do documento {doc.Numero} não correspondem ao valor declarado. " +
                                                $"\nValor da soma: {sumVl} " +
                                                $"\nValor declarado: {doc.Valor} ");

                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine($"\nO documento não foi gerado.");
                                            Console.ResetColor();

                                            erro = true;
                                            break;
                                        }
                                    }
                                    if (erro)
                                        break;
                                }

                                if (erro)
                                    break;

                                var fileName = $"saida_layout_versão 01_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                                var fullPath = Path.Combine(_outputDir, fileName);

                                gerador.Gerar(dados, fullPath);

                                Console.WriteLine("Arquivo gerado em: ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(fullPath);
                                Console.ResetColor();
                            }
                            else if (generateLayout.Equals(2))
                            {
                                var gerador = new Layout2();

                                var dados = JsonRepository.LoadEmpresasL2(_jsonPath);

                                foreach (var dado in dados)
                                {
                                    foreach (var doc in dado.Documentos)
                                    {
                                        decimal sumVl = doc.Itens.Sum(i => i.Valor);

                                        if (sumVl != doc.Valor)
                                        {
                                            Console.WriteLine($"O valor somado dos itens do documento {doc.Numero} não correspondem ao valor declarado. " +
                                                $"\nValor da soma: {sumVl} " +
                                                $"\nValor declarado: {doc.Valor}");

                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine($"\nO documento não foi gerado.");
                                            Console.ResetColor();

                                            erro = true;
                                            break;
                                        }
                                    }
                                    if (erro)
                                        break;
                                }

                                if (erro)
                                    break;

                                var fileName = $"saida_layout_versão 02_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                                var fullPath = Path.Combine(_outputDir, fileName);

                                gerador.Gerar(dados, fullPath);

                                Console.WriteLine("Arquivo gerado em: ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(fullPath);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine($"Layout {generateLayout.ToString()} é inválido, escolha entre os layouts 1 e 2.");
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao gerar arquivo: " + ex.Message);
                        }
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }
    }
}
