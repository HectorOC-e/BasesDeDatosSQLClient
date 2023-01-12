using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para Actualiza.xaml
    /// </summary>
    public partial class Actualiza : Window
    {
        SqlConnection miConexionSql = null;
        private int z;
        public Actualiza(int ID)
        {
            InitializeComponent();

            z = ID;

            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string consulta = "UPDATE Cliente SET Nombre = @Nombre WHERE Id = " + z;

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);
            miConexionSql.Open();
            sqlCommand.Parameters.AddWithValue("@Nombre", txtCuadroActualiza.Text);
            sqlCommand.ExecuteNonQuery();
            miConexionSql.Close();
            this.Close();
        }
    }
}
