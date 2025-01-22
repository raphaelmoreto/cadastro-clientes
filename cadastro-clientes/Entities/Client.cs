﻿using Services;
using Utils;
using MySql.Data.MySqlClient;

namespace Entities
{
    internal class Client
    {
        private int ID { get; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public Client()
        {
        }

        public Client(string name, string email)
        {
            Nome = name;
            Email = email;
        }

        public async Task AdicionarClienteAsync()
        {
            Console.Clear();
            ClientService clientService = new ClientService();

            Console.Write("INSIRA O NOME DO CLIENTE: ");
            string name = Console.ReadLine();

            Console.WriteLine();

            Console.Write("INSIRA O EMAIL DO CLIENTE: ");
            string email = Console.ReadLine();

            //O MÉTODO "string.IsNullOrEmpty" VERIFICA SE A STRING É "null" OU VÁZIA ("")
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
            {
                Console.Clear();

                //VALIDA O EMAIL ATRAVÉS DA FUNÇÃO "ValidarEmail" DA CLASSE "ValidationHelper" CRIADA NA PASTA "Utils"
                if (ValidationHelper.ValidarEmail(email))
                {
                    Client client = new Client(name, email);

                    try
                    {
                        var response = await clientService.InsertAsync(client);

                        if (response)
                        {
                            Console.WriteLine("CLIENTE INSERIDO COM SUCESSO.");
                        }
                        else
                        {
                            Console.WriteLine("EMAIL JÁ CADASTRADO!");
                        }
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine($"ERRO AO CADASTRAR CLIENTE! {e.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("EMAIL INVÁLIDO!");
                }
            }
            else
            {
                Console.WriteLine("É OBRIGATÓRIO O PREENCHIMENTO DO NOME E EMAIL DO CLIENTE!");
            }
            Console.WriteLine();
        }

        public async Task VisualizarClienteAsync()
        {
            Console.Clear();
            ClientService clientService = new ClientService();

            var clients = await clientService.SelectClientsAsync();

            if (clients.Count == 0)
            {
                Console.WriteLine("NENHUM CLIENT ENCONTRADO!");
            }
            else
            {
                Console.WriteLine("CLIENTES:");
                foreach (var client in clients)
                {
                    Console.WriteLine($"ID: {client.ID} | NOME: {client.Nome} | EMAIL: {client.Email}");
                }
            }
        }

        public async Task EditarClienteAsync()
        {
            Console.Clear();
            ClientService clientService = new ClientService();

            Console.Write("INSIRA O ID DO CLIENTE QUE DESEJA EDITAR: ");

            if (int.TryParse(Console.ReadLine(), out int idClient))
            {
                Console.WriteLine();

                try
                {
                    var response = await clientService.SelectIdAsync(idClient);

                    if (response)
                    {
                        Console.WriteLine("O QUE DESEJA ATUALIZAR:");
                        Console.WriteLine("1 - NOME");
                        Console.WriteLine("2 - EMAIL");
                        Console.WriteLine("3 - NOME E EMAIL");
                        Console.Write("SELECIONE: ");

                        if (int.TryParse(Console.ReadLine(), out int opcao))
                        {
                            Console.Clear();

                            string name = null, email = null;

                            switch (opcao)
                            {
                                case 1:
                                    Console.Write("INSIRA O NOME: ");
                                    name = Console.ReadLine();
                                    break;
                                case 2:
                                    Console.Write("INSIRA O EMAIL: ");
                                    email = Console.ReadLine();
                                    break;
                                case 3:
                                    Console.Write("INSIRA O NOME: ");
                                    name = Console.ReadLine();
                                    Console.WriteLine();
                                    Console.Write("INSIRA O EMAIL: ");
                                    email = Console.ReadLine();
                                    break;
                                default:
                                    Console.WriteLine("OPÇÃO INVÁLIDA!");
                                    return;
                            }

                            Console.WriteLine();

                            if (opcao == 2 || opcao == 3)
                            {
                                if (!string.IsNullOrEmpty(email) && ValidationHelper.ValidarEmail(email))
                                {
                                    var responseSelectEmail = await clientService.SelectEmailAsync(email);

                                    if (!responseSelectEmail)
                                    {
                                        Console.WriteLine("EMAIL JÁ É UTILIZADO POR OUTRO CLIENTE.");
                                        return;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("EMAIL INVÁLIDO!");
                                    return;
                                }
                            }

                            var responseUpdate = await clientService.UpdateAsync(idClient, name, email);

                            if (responseUpdate)
                            {
                                Console.WriteLine("CLIENTE ATUALIZADO");
                            }
                            else
                            {
                                Console.WriteLine("ERRO AO ATUALIZAR CLIENTE!");
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("INSIRA APENAS NÚMEROS.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("CLIENTE NÃO ENCONTRADO!");
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"ERRO! {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("INSIRA APENAS NÚMEROS.");
            }
        }

        public async Task ExcluirClienteAsync()
        {
            Console.Clear();
            ClientService clientService = new ClientService();

            Console.Write("INSIRA O ID DO CLIENTE: ");

            if (int.TryParse(Console.ReadLine(), out int idClient))
            {
                Console.Clear();

                try
                {
                    var response = await clientService.DeleteAsync(idClient);

                    if (response)
                    {
                        Console.WriteLine("CIENTE EXCLUIDO COM SUCESSO.");
                    }
                    else
                    {
                        Console.WriteLine("ID DO CLIENTE NÃO ENCONTRADO!");
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"ERRO AO EXCLUIR CLIENTE! {e.Message}");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("APENAS NÚMERO!");
            }
            Console.WriteLine();
        }
    }
}