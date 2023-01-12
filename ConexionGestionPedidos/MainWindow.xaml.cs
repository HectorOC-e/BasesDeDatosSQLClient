using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection miConexionSql = null;
        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            MuestraClientes();
            MuestraTodosPedidos();
        }

        private void MuestraClientes ()
        {
            try
            {
                string consulta = "Select * FROM Cliente";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(consulta,miConexionSql);

                using (dataAdapter)
                {
                    DataTable ClientesTabla = new DataTable();

                    dataAdapter.Fill(ClientesTabla);

                    ListaClientes.DisplayMemberPath = "Nombre";
                    ListaClientes.SelectedValuePath= "Id";
                    ListaClientes.ItemsSource = ClientesTabla.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MuestraTodosPedidos()
        {
            try
            {
                string consulta = "SELECT *, CONCAT(cCliente, ' ', FechaPedido, ' ', FormaPago) AS INFOCOMPLETA FROM Pedido";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(consulta, miConexionSql);

                using (dataAdapter)
                {
                    DataTable pedidosTabla = new DataTable();
                    dataAdapter.Fill(pedidosTabla);

                    ListaTodosPedidos.DisplayMemberPath = "INFOCOMPLETA";
                    ListaTodosPedidos.SelectedValuePath = "Id";
                    ListaTodosPedidos.ItemsSource = pedidosTabla.DefaultView;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void MuestraPedidos()
        {
            try
            {
                string consulta = "SELECT * FROM Pedido P INNER JOIN Cliente C ON P.cCliente = C.Id" +
                    " WHERE C.Id = @ClienteID";

                SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                using (dataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ClienteID", ListaClientes.SelectedValue);

                    DataTable pedidosTabla = new DataTable();

                    dataAdapter.Fill(pedidosTabla);

                    ListaPedido.DisplayMemberPath = "FechaPedido";
                    ListaPedido.SelectedValuePath = "Id";
                    ListaPedido.ItemsSource = pedidosTabla.DefaultView;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(ListaTodosPedidos.SelectedValue.ToString());
            string consulta = "DELETE FROM Pedido WHERE id = @ID";

            SqlCommand sqlCommand = new SqlCommand(consulta,miConexionSql);
            miConexionSql.Open();
            sqlCommand.Parameters.AddWithValue("@ID", ListaTodosPedidos.SelectedValue);
            sqlCommand.ExecuteNonQuery();
            miConexionSql.Close();
            MuestraTodosPedidos();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string consulta = "INSERT INTO Cliente (Nombre) Values (@Nombre)";

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);
            miConexionSql.Open();
            sqlCommand.Parameters.AddWithValue("@Nombre", txtCliente.Text);
            sqlCommand.ExecuteNonQuery();
            miConexionSql.Close();
            MuestraClientes();
            txtCliente.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string consulta = "DELETE FROM Cliente WHERE Id = @ID";

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);
            miConexionSql.Open();
            sqlCommand.Parameters.AddWithValue("@ID", ListaClientes.SelectedValue);
            sqlCommand.ExecuteNonQuery();
            miConexionSql.Close();
            MuestraClientes();
        }

        private void ListaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedidos();
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            Actualiza ventanaActualizar = new Actualiza((int)ListaClientes.SelectedValue);
            ventanaActualizar.Show();
            try
            {
                string consulta = "Select Nombre FROM Cliente WHERE Id = @Id";
                SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                using (dataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@Id", ListaClientes.SelectedValue);
                    DataTable clientesTabla = new DataTable();

                    dataAdapter.Fill(clientesTabla);

                    ventanaActualizar.txtCuadroActualiza.Text = clientesTabla.Rows[0]["Nombre"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //ventanaActualizar.ShowDialog();
            //MuestraClientes();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MuestraClientes();
        }
    }
}
