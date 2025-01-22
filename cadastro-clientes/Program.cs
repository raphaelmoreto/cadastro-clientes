using Entities;

namespace cadastro_clientes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool executando = true;
            Client client = new Client();

            while (executando)
            {
                Console.WriteLine("1 - ADICIONAR CLIENTE");
                Console.WriteLine("2 - VISUALIZAR CLIENTE");
                Console.WriteLine("3 - EDITAR CLIENTE");
                Console.WriteLine("4 - EXCLUIR CLIENTE");
                Console.WriteLine("5 - SAIR");
                Console.Write("SELECIONE: ");

                //VERIFICA SE O TEXTO DIGITADO NO CONSOLE PODE SER CONVERTIDO PARA UM NÚMERO INTEIRO. SE SIM, IRÁ ARMAZENAR O NÚMERO NA VARIÁVEL "opcao"
                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            client.AdicionarClienteAsync();
                            break;
                        case 2:
                            client.VisualizarClienteAsync();
                            break;
                        case 3:
                            client.EditarClienteAsync();
                            break;
                        case 4:
                            client.ExcluirClienteAsync();
                            break;
                        case 5:
                            Console.WriteLine();
                            Console.WriteLine("SISTEMA ENCERRADO.");
                            executando = false;
                            break;
                        default:
                            Console.WriteLine();
                            Console.WriteLine("OPÇÃO INVÁLIDA!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("INSIRA APENAS NÚMEROS.");
                }
                Console.WriteLine();
            }
        }
    }
}
