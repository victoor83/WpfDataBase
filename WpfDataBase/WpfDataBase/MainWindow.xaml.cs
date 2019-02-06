using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;

namespace WpfDataBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillDataGrid("SELECT CustomerName, City, PostalCode FROM Data");
        }
        private void FillDataGrid(string sqlCommand, bool isStoredProcedure = false)
        {

            string ConString = ConfigurationManager.ConnectionStrings["ConnectStringKundenDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = null;


                cmd = new SqlCommand(sqlCommand, con);
                //if(isStoredProcedure)
                //    cmd.CommandType = CommandType.StoredProcedure;


                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable("Customers");

                sda.Fill(dt);

                grdEmployee.ItemsSource = dt.DefaultView;

                JsonConvertText(con, cmd, isStoredProcedure);
            }

        }

        private void JsonConvertText(SqlConnection con, SqlCommand cmd, bool isStoredProcedure = false)
        {

            var indexes = new List<ushort>();
            if (isStoredProcedure)
                indexes.AddRange(new ushort[3] { 1, 4, 5 });
            else
                indexes.AddRange(new ushort[3] { 0, 1, 2 });



            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Person> persons = new List<Person>();

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
            con.Close();

            string json = JsonConvert.SerializeObject(persons);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "jsonTest.json") , json);
            labJson.Content = json;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string storedProcedure = $"EXEC SelectCustomersFromCity @City = '{txtCity.Text}'";
            FillDataGrid(storedProcedure, true);
        }
    }
}
