using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.DAO
{
    internal class UserDao
    {
        private readonly SqlConnection _connection;

        public UserDao(SqlConnection connection)
        {
            _connection = connection;
        }

        public bool CheckUser(Entity.User user)
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"SELECT COUNT(*) FROM [User]
                                     WHERE login COLLATE Latin1_General_BIN = @login AND password COLLATE Latin1_General_BIN = @password";
            command.Prepare();

            command.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 50));

            command.Parameters[0].Value = user.Login;
            command.Parameters[1].Value = user.Password;

            return (int)command.ExecuteScalar() == 1;
        }

        public void Add(Entity.User user)
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"INSERT INTO [User](name, login, password, email)
                                    VALUES (@name, @login, @password, @email)";
            command.Prepare();

            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 50));

            command.Parameters[0].Value = user.Name;
            command.Parameters[1].Value = user.Login;
            command.Parameters[2].Value = user.Password;
            command.Parameters[3].Value = user.Email;

            command.ExecuteNonQuery();
        }

        public bool CheckUniqueByLogin(string login)
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = "SELECT Count(*) FROM [User] WHERE login COLLATE Latin1_General_BIN = @login";
            command.Prepare();

            command.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 50));
            command.Parameters[0].Value = login;

            return (int)command.ExecuteScalar() == 0;
        }

        public bool CheckUniqueByPassword(string password)
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = "SELECT Count(*) FROM [User] WHERE password COLLATE Latin1_General_BIN = @password";
            command.Prepare();

            command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 50));
            command.Parameters[0].Value = password;

            return (int)command.ExecuteScalar() == 0;
        }

        public bool CheckUniqueByEmail(string email)
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = "SELECT Count(*) FROM [User] WHERE email = @email";
            command.Prepare();

            command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 50));
            command.Parameters[0].Value = email;

            return (int)command.ExecuteScalar() == 0;
        }
    }
}
