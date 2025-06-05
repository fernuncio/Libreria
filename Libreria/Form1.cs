using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace Libreria
{
    public partial class Form1 : Form
    {
        WebSocketServer servidor;
        private Cliente cliente,cliente2;
        private Conexion conexion;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            conexion = new Conexion();
        }

        private void mostrarDatos()
        {
            //Conexion conecta = new Conexion();
            DataSet resultado = conexion.consulta("SELECT id_libro AS 'ID LIBRO', " +
                                     "titulo AS 'TITULO', " +
                                     "autor AS 'AUTOR', " +
                                     "editorial AS 'EDITORIAL', " +
                                     "anio_publicacion AS 'AÑO PUBLICACION', " +
                                     "genero AS 'GENERO', " +
                                     "precio AS 'PRECIO', " +
                                     "stock AS 'STOCK' " +
                                     "FROM libros;");

            if (resultado != null)
            {
                dgvLibros.DataSource = resultado.Tables[0];
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            mostrarDatos();
            //servidor
            iniciarServidor();
            Notificacion.MensajeRecibido += Notificacion_MensajeRecibido;
            //cliente = new Cliente("ws://10.13.54.113:8080/notificar");
            cliente2 = new Cliente("ws://10.13.58.210:8080/notificar");
            //cliente.conectar();
            cliente2.conectar();

        }

        private void Notificacion_MensajeRecibido(string msj)
        {
            if (msj != "")
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        mostrarDatos();
                    }));
                }
                else
                {
                    mostrarDatos();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (servidor != null && servidor.IsListening)
                servidor.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Libro libro = new Libro();
            libro.ShowDialog();
            mostrarDatos();
            //NotificarATodos("SE AGREGO UN NUEVO LIBRO");
            mostrarDatos();
            //cliente.enviar("Libro Agregado");
            cliente2.enviar("Libro Agregado");
        }

        private void actualizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dgvLibros.CurrentRow.Index;
            Libro libro = new Libro(dgvLibros.Rows[i].Cells[0].Value.ToString(),
                dgvLibros.Rows[i].Cells[1].Value.ToString(),
                dgvLibros.Rows[i].Cells[2].Value.ToString(),
                dgvLibros.Rows[i].Cells[3].Value.ToString(),
                dgvLibros.Rows[i].Cells[4].Value.ToString(),
                dgvLibros.Rows[i].Cells[5].Value.ToString(),
                dgvLibros.Rows[i].Cells[6].Value.ToString(),
                dgvLibros.Rows[i].Cells[7].Value.ToString());
            libro.ShowDialog();
            //mostrarDatos();
            //NotificarATodos("SE ACTUALIZO UN LIBRO");
            //cliente.enviar("SE ACTUALIZO UN LIBRO");
            cliente2.enviar("SE ACTUALIZO UN LIBRO");
            //mostrarDatos();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dgvLibros.CurrentRow.Index;

            string nombre = dgvLibros.Rows[i].Cells[1].Value.ToString();

            DialogResult resultado = MessageBox.Show("¿Seguro que deseas eliminar " +
                nombre + "?", "CONFIRMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvLibros.Rows[i].Cells[0].Value.ToString());
                    //Conexion conexion = new Conexion();
                    string consulta = "DELETE FROM libros WHERE id_libro =" + id;
                    conexion.consulta(consulta);
                    //Console.WriteLine(consulta);
                    MessageBox.Show("Libro Eliminado Con Exito", "REGISTRO",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //NotificarATodos("SE ELIMINO UN LIBRO");
                    //cliente.enviar("SE ELIMINO UN LIBRO");
                    cliente2.enviar("SE ELIMINO UN LIBRO");
                    mostrarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Algo Salio Mal" + ex.Message, "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            mostrarDatos();
        }
        private void NotificarATodos(string mensaje)
        {
            foreach (var cliente in Notificacion.Clientes)
            {
                try
                {
                    cliente.Enviar(mensaje);
                    
                }
                catch (Exception ex)
                {
                    // Por si un cliente se desconectó de forma inesperada
                    Console.WriteLine("Error al enviar: " + ex.Message);
                }
            }
        }

        //servidor
        private void iniciarServidor()
        {
            //modificar ip
            servidor = new WebSocketServer("ws://0.0.0.0:8080"); // Acepta conexiones de cualquier IP
            //servidor = new WebSocketServer("ws://192.168.85.115:8080");
            servidor.AddWebSocketService<Notificacion>("/notificar");
            servidor.Start();
            MessageBox.Show("Servidor WebSocket iniciado en ws://TU_IP:8080/notificar", "Servidor Activo");
        }

    }
}
