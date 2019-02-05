﻿using System.Windows;
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

                if(isStoredProcedure)
                    cmd.CommandType = CommandType.StoredProcedure;

                cmd = new SqlCommand(sqlCommand, con);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable("Customers");

                sda.Fill(dt);

                grdEmployee.ItemsSource = dt.DefaultView;

                JsonConvertText(con, cmd);
            }

        }

        private void JsonConvertText(SqlConnection con, SqlCommand cmd)
        {
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Person> persons = new List<Person>();

            while (reader.Read())
            {
                if (reader[0].ToString() == string.Empty)
                    break;

                var person = new Person();
                person.Name = reader[0].ToString();
                person.City = reader[1].ToString();
                person.Zip = Convert.ToInt32(reader[2]);
                persons.Add(person);
            }
            reader.Close();
            con.Close();

            string json = JsonConvert.SerializeObject(persons);
            File.WriteAllText(@"c:\temp\jsonTest.json", json);
            labJson.Content = json;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string storedProcedure = $"EXEC SelectCustomersFromCity @City = \"{txtCity.Text}\"";
            FillDataGrid($"EXEC SelectCustomersFromCity @City = \"{txtCity.Text}\"");
        }
    }
}
