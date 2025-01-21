using System.Data;
using MySql.Data.MySqlClient;

namespace Repository
{
    internal class DBConnection
    {
        private readonly string _connection;

        public DBConnection()
        {
            _connection = "Server=localhost;Database=clientes;User=root;Password='';";
        }

        //RETORNA UMA INTERFACE "IDbConnection". UTIL PARA GARANTIR QUE O CÓDIGO POSSA FUNCIONAR COM DIFERENTES TIPOS DE BANCO DE DADOS SEM DEPENDER DE UM TIPO ESPECÍFICO
        public IDbConnection GetConnection()
        {
            //CASO FOSSE PARA UTILIZAR O SQL Server APENAS INSERIR SqlConnection(_connection)
            return new MySqlConnection(_connection);
        }
    }
}
