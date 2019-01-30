using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

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

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string storedProcedure = $"EXEC SelectCustomersFromCity @City = \"{txtCity.Text}\"";
            FillDataGrid($"EXEC SelectCustomersFromCity @City = \"{txtCity.Text}\"");
        }
    }
}
