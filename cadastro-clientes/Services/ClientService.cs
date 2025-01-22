using Entities;
using Repository;
using Dapper;
using System.Data;
using System.Text;

namespace Services
{
    internal class ClientService
    {
        //OBTEM UMA CONEXÃO COM O BANCO DE DADOS ATRAVÉS DA CLASSE "DBConnection"
        private readonly DBConnection dbConnection;

        //PREPARANDO A CLASSE PARA SE CONECTAR AO BANCO DE DADOS
        public ClientService()
        {
            dbConnection = new DBConnection();
        }

        public async Task<bool> SelectEmailAsync(string email)
        {
            bool result = false;

            using (IDbConnection db = dbConnection.GetConnection())
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT email");
                sb.AppendLine("FROM cliente");
                sb.AppendLine("WHERE email = @email");

                var parameters = new
                {
                    email = email,
                };

                db.Open();
                string? row = await db.QuerySingleOrDefaultAsync<string>(sb.ToString(), parameters);
                db.Close();

                if (string.IsNullOrEmpty(row))
                {
                    result = true;
                }
            }
            return result;
        }

        public async Task<bool> SelectIdAsync(int idClient)
        {
            var result = false;

            using (IDbConnection db = dbConnection.GetConnection())
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT id");
                sb.AppendLine("FROM cliente");
                sb.AppendLine("WHERE id = @id");

                var parameters = new
                {
                    id = idClient,
                };

                db.Open();
                int? row = db.QuerySingleOrDefault<int?>(sb.ToString(), parameters);
                db.Close();

                if (row.HasValue)
                {
                    result = true;
                }
            }
            return result;
        }

        public async Task<bool> InsertAsync(Client client)
        {
            bool result = await SelectEmailAsync(client.Email);

            if (result)
            {
                //O "using" GARANTE QUE A CONEXÃO COM O BANCO D E DADOS SERÁ ENCERRADO APÓS O USO
                using (IDbConnection db = dbConnection.GetConnection())
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("INSERT cliente (nome, email)");
                    sb.AppendLine("          VALUES (@nome, @email)");

                    var parameters = new
                    {
                        nome = client.Nome,
                        email = client.Email
                    };

                    db.Open();

                    //VERIFICA SE O NÚMERO DE LINHAS É MAIOR QUE 0. SE FOR, IRÁ ATRIBUIR "true" NA VARIÁVEL
                    //O MÉTODO "Execute()" DEVE SER UTILIZADO APENAS PARA "INSERT", "UPDATE" OU "DELETE"
                    result = db.Execute(sb.ToString(), parameters) > 0;

                    db.Close();
                }
            }
            return result;
        }

        public async Task<List<Client>> SelectClientsAsync()
        {
            List<Client> list;

            using (IDbConnection db = dbConnection.GetConnection())
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT id, nome, email");
                sb.AppendLine("FROM cliente");

                db.Open();
                var result = await db.QueryAsync<Client>(sb.ToString());
                list = result.ToList();
                db.Close();
            }
                return list;
        }

        public async Task<bool> UpdateAsync(int idClient, string? name, string? email)
        {
            bool result = false;

            using (IDbConnection db = dbConnection.GetConnection())
            {
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

                db.Open();
                result = db.Execute(sb.ToString(), parameters) > 0;
                db.Close();
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int idClient)
        {
            bool result = await SelectIdAsync(idClient);

            if (result)
            {
                using (IDbConnection db = dbConnection.GetConnection())
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("DELETE FROM cliente");
                    sb.AppendLine("WHERE id = @id");

                    var parameters = new
                    {
                        id = idClient,
                    };

                    db.Open();
                    result = db.Execute(sb.ToString(), parameters) > 0;
                    db.Close();
                }
            }
            return result;
        }
    }
}
