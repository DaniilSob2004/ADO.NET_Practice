using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Store
{
    public partial class App : Application
    {
        public const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\C#\ШАГ_ADO_NET\Store\Store\Store.mdf;Integrated Security=True";
        private static SqlConnection Connection = null!;

        public App()
        {
            InitializeComponent();
            CreateConnection();  // подключаемся к БД
        }

        public static SqlConnection GetConnection()
        {
            if (Connection.State != ConnectionState.Open)
            {
                OpenConnection();
            }
            return Connection;
        }

        private static void OpenConnection()
        {
            try
            {
                Connection.Open();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private static void CreateConnection()
        {
            try
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public static void CloseConnectionToDb()
        {
            if (Connection != null) Connection?.Close();  // закрываем соединение
        }
    }
}
