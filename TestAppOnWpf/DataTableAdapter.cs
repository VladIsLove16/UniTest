using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace TestAppOnWpf
{
    internal class DataTableAdapter
    {
        public MySqlConnection mySqlConnection = new MySqlConnection("server = localhost;user = root; password=root;database=db");
        public MySqlDataAdapter mySqlDataAdapter;
        public MySqlCommand mySqlCommand;
        public bool ConnectionDB()
        {
            try
            {
                mySqlConnection.Open();
                mySqlCommand = new MySqlCommand();
                mySqlCommand.Connection = mySqlConnection;
                mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                return true;
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с базой данных");
                return false;
            }
        }
        public void Close()
        {
            mySqlConnection.Close();
        }
        public MySqlConnection GetMySqlConnection()
        {
            return mySqlConnection;
        }

    }
}
