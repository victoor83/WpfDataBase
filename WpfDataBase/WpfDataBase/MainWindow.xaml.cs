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
            FillDataGrid();
        }
        private void FillDataGrid()
        {

            string ConString = ConfigurationManager.ConnectionStrings["ConnectStringKundenDB"].ConnectionString;

            string CmdString = string.Empty;

            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT CustomerName, City, PostalCode FROM Data";

                SqlCommand cmd = new SqlCommand(CmdString, con);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable("Customers");

                sda.Fill(dt);

                grdEmployee.ItemsSource = dt.DefaultView;

            }

        }
    }
}
