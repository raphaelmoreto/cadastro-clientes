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
            sb.AppendLine("SELECT COUNT(email)");
            sb.AppendLine("FROM cliente");
            sb.AppendLine("WHERE email = @email");

            int result = await db.QueryFirstOrDefaultAsync<int>(sb.ToString(), param: new { email });
            return result > 0;
        }

        public async Task<bool> SelectIdAsync(int idClient)
        {
            using IDbConnection db = _dbConnection.GetConnection();
            
            var sb = new StringBuilder();
            sb.AppendLine("SELECT COUNT(id)");
            sb.AppendLine("FROM cliente");
            sb.AppendLine("WHERE id = @id");

            int id = await db.QueryFirstOrDefaultAsync<int>(sb.ToString(), param: new { id = idClient });
            return id > 0;
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

            //VERIFICA SE O NÚMERO DE LINHAS É MAIOR QUE 0
            //O MÉTODO "Execute()" DEVE SER UTILIZADO APENAS PARA "INSERT", "UPDATE" OU "DELETE"
            int result = await db.ExecuteAsync(sb.ToString(), param: new {client.Nome, client.Email});
            return result > 0;
        }

        public async Task<IEnumerable<Client>> SelectClientsAsync()
        {
            try
            {
                using IDbConnection db = _dbConnection.GetConnection();

                var sb = new StringBuilder();
                sb.AppendLine("SELECT id,");
                sb.AppendLine("           nome,");
                sb.AppendLine("           email");
                sb.AppendLine("FROM cliente");

                var result = await db.QueryAsync<Client>(sb.ToString());
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
                sb.AppendLine("SET nome = @nome,");
                sb.AppendLine("      email = @email");
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
                return false;
            }

            int result = await db.ExecuteAsync(sb.ToString(), parameters);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int idClient)
        {
            bool consult = await SelectIdAsync(idClient);

            if(!consult)
            {
                return false;
            }

            using IDbConnection db = _dbConnection.GetConnection();

            var sb = new StringBuilder();
            sb.AppendLine("DELETE FROM cliente");
            sb.AppendLine("WHERE id = @id");

            int response = await db.ExecuteAsync(sb.ToString(), param: new { id = idClient });
            return response > 0;
        }
    }
}
