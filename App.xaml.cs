using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Store
{
    public partial class App : Application
    {
        public const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\C#\ШАГ_ADO_NET\Store\Store\Store.mdf;Integrated Security=True";
        public static SqlConnection Connection = null!;

        public App()
        {
            InitializeComponent();
            ConnectionToDb();
        }

        private static void ConnectionToDb()
        {
            try
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();  // подключаемся к БД
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void CloseConnectionToDb()
        {
            if (Connection != null) Connection?.Close();
        }
    }
}
