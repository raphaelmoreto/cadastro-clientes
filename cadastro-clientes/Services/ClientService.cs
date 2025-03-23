using Entities;
using Repository;
using Dapper;
using System.Data;
using System.Text;
using MySqlX.XDevAPI.Common;

namespace Services
{
    internal class ClientService
    {
        //OBTEM UMA CONEXÃO COM O BANCO DE DADOS ATRAVÉS DA CLASSE "DBConnection"
        private readonly DBConnection _dbConnection;

        //PREPARANDO A CLASSE PARA SE CONECTAR AO BANCO DE DADOS
        public ClientService()
        {
            _dbConnection = new DBConnection();
        }

        public async Task<bool> SelectEmailAsync(string email)
        {
            using IDbConnection db = _dbConnection.GetConnection();
            
            var sb = new StringBuilder();
            sb.AppendLine("SELECT email");
            sb.AppendLine("FROM cliente");
            sb.AppendLine("WHERE email = @email");

            var parameters = new
            {
                email,
            };

            var result = db.QueryFirstOrDefault<string>(sb.ToString(), parameters);
            return result != null;
        }

        public async Task<bool> SelectIdAsync(int idClient)
        {
            using IDbConnection db = _dbConnection.GetConnection();
            
            var sb = new StringBuilder();
            sb.AppendLine("SELECT id");
            sb.AppendLine("FROM cliente");
            sb.AppendLine("WHERE id = @id");

            var parameters = new
            {
                id = idClient,
            };

            int? id = db.QueryFirstOrDefault<int?>(sb.ToString(), parameters);

            return id.HasValue;
        }

        public async Task<bool> InsertAsync(Client client)
        {
            bool response = await SelectEmailAsync(client.Email);

            if (response)
            {
                return false;
            }

            //O "using" GARANTE QUE A CONEXÃO COM O BANCO D E DADOS SERÁ ENCERRADO APÓS O USO
            using IDbConnection db = _dbConnection.GetConnection();

            var sb = new StringBuilder();
            sb.AppendLine("INSERT cliente (nome, email)");
            sb.AppendLine("          VALUES (@nome, @email)");

            var parameters = new
            {
                nome = client.Nome,
                email = client.Email
            };

            //VERIFICA SE O NÚMERO DE LINHAS É MAIOR QUE 0
            //O MÉTODO "Execute()" DEVE SER UTILIZADO APENAS PARA "INSERT", "UPDATE" OU "DELETE"
            bool result = db.Execute(sb.ToString(), parameters) > 0;
            return result;
        }

        public async Task<IEnumerable<Client>> SelectClientsAsync()
        {
            try
            {
                using IDbConnection db = _dbConnection.GetConnection();

                var sb = new StringBuilder();
                sb.AppendLine("SELECT id,");
                sb.AppendLine("            nome,");
                sb.AppendLine("            email");
                sb.AppendLine("FROM cliente");

                var result = db.Query<Client>(sb.ToString());
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Enumerable.Empty<Client>();
            }
        }

        public async Task<bool> UpdateAsync(int idClient, string? name, string? email)
        {
            bool result = false;

            using IDbConnection db = _dbConnection.GetConnection();
       
            var sb = new StringBuilder();
            object parameters;

            //VERIFICA SE O "name" É DIFERENTE DE "null" E SE O "email" É "null"
            if (!string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email))
            {
                sb.AppendLine("UPDATE cliente");
                sb.AppendLine("SET nome = @nome");
                sb.AppendLine("WHERE id = @id");

                parameters = new
                {
                    nome = name,
                    id = idClient
                };
            }
            //VERIFICA SE O "email" É DIFERENTE DE "null" E SE O "name" É "null"
            else if (string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
            {
                sb.AppendLine("UPDATE cliente");
                sb.AppendLine("SET email = @email");
                sb.AppendLine("WHERE id = @id");

                parameters = new
                {
                    email = email,
                    id = idClient
                };
            }
            //VERIFICA SE AMBOS AS VARIÁVEIS É DIFERENTE DE "null"
            else if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
            {
                sb.AppendLine("UPDATE cliente");
                sb.AppendLine("SET nome = @nome, email = @email");
                sb.AppendLine("WHERE id = @id");

                parameters = new
                {
                    nome = name,
                    email = email,
                    id = idClient
                };
            }
            else
            {
                return result;
            }

            result = db.Execute(sb.ToString(), parameters) > 0;
            return result;
        }

        public async Task<bool> DeleteAsync(int idClient)
        {
            bool result = await SelectIdAsync(idClient);

            if(!result)
            {
                return false;
            }

            using IDbConnection db = _dbConnection.GetConnection();
                
            var sb = new StringBuilder();
            sb.AppendLine("DELETE FROM cliente");
            sb.AppendLine("WHERE id = @id");

            var parameters = new
            {
                id = idClient,
            };

            result = db.Execute(sb.ToString(), parameters) > 0;
            return result;
        }
    }
}
