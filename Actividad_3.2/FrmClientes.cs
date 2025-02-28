using Actividad_3._2;
using CrystalDecisions.ReportAppServer.ReportDefModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConexionSQL
{
    public partial class Clientes : Form
    {
        public Clientes()
        {
            InitializeComponent();
        }
        CLSConexion DB_CONN = new CLSConexion();
        SqlCommand cm = new SqlCommand();
        FrmPrincipal InicioPrincipal;
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.txtIdCliente.Text == "" || this.txtCompañia.Text == "")
            {
                MessageBox.Show("Por favor llena los campos faltantes: Id Cliente, Nombre Compañia");
            }
            else
            {
                try
                {
                    String st = "UPDATE Customers SET CompanyName = '" + this.txtCompañia.Text + "',ContactName ='" + this.txtContacto.Text + "'," +
                        "City = '" + this.txtCiudad.Text + "', Phone = '" + this.txtTelefono.Text + "' WHERE CustomersID = '" + this.txtIdCliente.Text + "'";
                    cm = new SqlCommand(st, DB_CONN.DB_CONN);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("El cliente Actalizado");

                    this.btnCancelar.PerformClick();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ha ocurrido un error" + ex.Message);
                }
            }
        }
        DataTable dt;
        public DataTable GetData(string consulta)
        {
            cm = new SqlCommand(consulta, DB_CONN.DB_CONN);
            SqlDataAdapter adp = new SqlDataAdapter(cm);
            dt = new DataTable();
            adp.Fill(dt);
            return dt;
        }
        private void Clientes_Load(object sender, EventArgs e)
        {
            //Cargar la informacion de todos los clientes registrados cuando se abre la pantalla
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.DataSource = GetData("EXEC GetAllCustomersOrdered");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InicioPrincipal = new FrmPrincipal();
            InicioPrincipal.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.txtIdCliente.Text == "")
            {
                MessageBox.Show("Por favor llena los campos faltantes: Id Cliente");
            }
            else
            {
                try
                {
                    //cm = new SqlCommand("SELECT CompanyName, ContactName, City, Phone FROM Customers WHERE CustomersID='" + this.txtIdCliente.Text + "'", DB_CONN.DB_CONN);
                    //cm.CommandType = CommandType.Text;
                    cm = new SqlCommand("GetAllCustomersOrdered", DB_CONN.DB_CONN);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.Add("@IdCliente", SqlDbType.VarChar).Value = this.txtIdCliente.Text;
                    SqlDataReader dr = cm.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        this.txtIdCliente.Text = dr.GetString(0);
                        this.txtCompañia.Text = dr.GetString(1);
                        this.txtContacto.Text = dr.GetString(2);
                        this.txtCiudad.Text = dr.GetString(3);
                        this.txtTelefono.Text = dr.GetString(4);
                        dr.Close();
                        this.txtIdCliente.Enabled = false;
                        this.btnGuardar.Enabled = true;
                        this.btnEditar.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("El cliente no Existe.");
                        this.txtIdCliente.Enabled = true;
                        this.btnGuardar.Enabled = true;
                        this.btnEditar.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error" + ex.Message);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (this.txtIdCliente.Text == "" || this.txtCompañia.Text == "")
            {
                MessageBox.Show("Por favor llena los campos faltantes: Id Cliente, Nombre Compañia");
            }
            else
            {
                try
                {
                    String st = "INSERT INTO Customers (CustomerID, CompanyName, ContactName, City, Phone)VALUES" +
                        "('" + this.txtIdCliente.Text + "','" + this.txtCompañia.Text + "','" + this.txtContacto.Text +
                        "','" + this.txtCiudad.Text + "','" + this.txtTelefono.Text + "')";
                    cm = new SqlCommand(st, DB_CONN.DB_CONN);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Cliente Guardado");

                    this.btnCancelar.PerformClick();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ha ocurrido un error" + ex.Message);
                }
            }
        }        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmReporte Reporte = new FrmReporte();
            CrReporte ReporteCliente = new CrReporte();
            ReporteCliente.SetDataSource(GetData("EXEC GetAllCustomersOrdered"));
            //ReporteCliente.SetParameterValue("Titulo", this.txtIdCliente);
            Reporte.ReporteExporta = ReporteCliente;
            Reporte.ShowDialog();
            Reporte.Focus();
        }
        public void SumaTotalOrdenes()
        {
            Int64 Total = 0;
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (row.Cells["TotalOrdenes"].Value != null)
                {
                    Total += Convert.ToInt64(row.Cells["TotalOrdenes"].Value);
                }
            }
            this.txtTotalClientes.Text = Total.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0 && e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                this.txtIdCliente.Text = row.Cells[0].Value?.ToString() ?? "";
                this.txtCompañia.Text = row.Cells[1].Value?.ToString() ?? "";
                this.txtContacto.Text = row.Cells[2].Value?.ToString() ?? "";
                this.txtCiudad.Text = row.Cells[3].Value?.ToString() ?? "";
                this.txtTelefono.Text = row.Cells[4].Value?.ToString() ?? "";
                this.txtTotalClientes.Text = row.Cells[5].Value?.ToString() ?? "";

                SumaTotalOrdenes();

                this.txtIdCliente.Enabled = false;
                this.btnGuardar.Enabled = false;
                this.btnEditar.Enabled = true;
            }
        }
    }
}
