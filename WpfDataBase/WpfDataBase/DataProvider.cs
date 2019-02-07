using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfDataBase
{
    public class DataProvider
    {
        SqlConnection _connection;
        string _connectString;
        public DataProvider()
        {
            _connectString = ConfigurationManager.ConnectionStrings["ConnectStringKundenDB"].ConnectionString;
        }

        public void FillDataGrid(DataGrid grid, string sqlCommand, bool isStoredProcedure = false)
        {
            _connection = new SqlConnection(_connectString);
            using (_connection)
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, _connection);

                //if(isStoredProcedure)
                //    cmd.CommandType = CommandType.StoredProcedure;


                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable("Customers");

                sda.Fill(dt);

                grid.ItemsSource = dt.DefaultView;
            }

        }

        public string JsonConvertText(string sqlCommand, bool isStoredProcedure = false)
        {
            var indexes = new List<ushort>();
            if (isStoredProcedure)
                indexes.AddRange(new ushort[3] { 1, 4, 5 });
            else
                indexes.AddRange(new ushort[3] { 0, 1, 2 });


            List<Person> persons = new List<Person>();

            _connection = new SqlConnection(_connectString);
            using (_connection)
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, _connection);

                SqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    if (reader[i].ToString() == string.Empty)
                        break;

                    var person = new Person();
                    person.Name = reader[indexes[0]].ToString();
                    person.City = reader[indexes[1]].ToString();
                    person.Zip = (int)reader[indexes[2]];
                    persons.Add(person);
                }
                reader.Close();
            }

            string json = JsonConvert.SerializeObject(persons);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "jsonTest.json"), json);

            return json;
        }
    }
}
