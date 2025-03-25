using Entities;
using Services;

namespace cadastro_clientes
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var clientService = new ClientService();
            var client = new Client(clientService);

            bool executando = true;

            while (executando)
            {
                int opcao = ExibirMenu();

                switch (opcao)
                {
                    case 1:
                        await client.AdicionarClienteAsync();
                        break;
                    case 2:
                        await client.VisualizarClientesAsync();
                        break;
                    case 3:
                        await client.AtualizarClienteAsync();
                        break;
                    case 4:
                        await client.ExcluirClienteAsync();
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
                Console.WriteLine();
            }
        }

        //A PALAVRA "static" SIGNIFICA QUE ESSE MÉTODO PERTENCE À PRÓPRIA CLASSE E NÃO A UMA INSTÂNCIA DELA. ISSO QUE DIZER QUE PODE CHAMÁ-LO DIRETAMENTE, SEM PRECISAR CRIAR UM
        //OBJETO DA CLASSE
        static int ExibirMenu()
        {
            Console.WriteLine("1 - ADICIONAR CLIENTE");
            Console.WriteLine("2 - VISUALIZAR CLIENTE");
            Console.WriteLine("3 - EDITAR CLIENTE");
            Console.WriteLine("4 - EXCLUIR CLIENTE");
            Console.WriteLine("5 - SAIR");
            Console.Write("SELECIONE: ");

            //VERIFICA SE O TEXTO DIGITADO NO CONSOLE PODE SER CONVERTIDO PARA UM NÚMERO INTEIRO. SE SIM, IRÁ ARMAZENAR O NÚMERO NA VARIÁVEL "opcao"
            //O OPERADOR TERNÁRIO "? :" INDICA QUE SE A CONVERSÃO FOR BEM SUCEDIDA RETORNA "true" NA "opcao"
            //SE A CONVERSÃO FALHAR, RETORNA -1 (false) INDICANDO UMA ENTRADA INVÁLIDA
            return int.TryParse(Console.ReadLine(), out int opcao) ? opcao : -1;
        }
    }
}
