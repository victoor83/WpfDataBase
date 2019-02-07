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
        DataProvider _dataProvider;
        public MainWindow()
        {
            InitializeComponent();
            _dataProvider = new DataProvider();

            string sqlCommand = "SELECT CustomerName, City, PostalCode FROM Data";
            _dataProvider.FillDataGrid(grdEmployee, sqlCommand);
            _dataProvider.JsonConvertText(sqlCommand, false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string storedProcedure = $"EXEC SelectCustomersFromCity @City = '{txtCity.Text}'";
            _dataProvider.FillDataGrid(grdEmployee, storedProcedure);
            _dataProvider.JsonConvertText(storedProcedure, true);
        }
    }
}
