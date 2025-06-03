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
        private WebSocket cliente;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void mostrarDatos()
        {
            Conexion conecta = new Conexion();
            DataSet resultado = conecta.consulta("SELECT id_libro AS 'ID LIBRO', " +
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
            //modificar ip
            servidor = new WebSocketServer("ws://0.0.0.0:8080"); // Acepta conexiones de cualquier IP
            //servidor = new WebSocketServer("ws://192.168.85.115:8080");
            servidor.AddWebSocketService<Notificacion>("/notificar");
            servidor.Start();
            MessageBox.Show("Servidor WebSocket iniciado en ws://TU_IP:8080/notificar", "Servidor Activo");

            //cliente 
            cliente = new WebSocket("ws://192.168.85.157:8080/notificar"); // IP del servidor

            cliente.OnMessage += (ws_sender, ws_e) =>
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show("Notificación recibida: " + ws_e.Data);
                    mostrarDatos();
                }));
            };

            cliente.OnOpen += (ws_sender, ws_e) =>
            {
                Console.WriteLine("✅ Conectado al servidor WebSocket");
            };

            cliente.OnError += (ws_sender, ws_e) =>
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show("Error en WebSocket: " + ws_e.Message);
                }));
            };

            cliente.Connect();
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
            NotificarATodos("SE AGREGO UN NUEVO LIBRO");
            mostrarDatos();
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
            mostrarDatos();
            NotificarATodos("SE ACTUALIZO UN LIBRO");
            mostrarDatos();
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
                    Conexion conexion = new Conexion();
                    conexion.consulta("DELETE FROM libros WHERE id_libro =" + id);
                    MessageBox.Show("Libro Eliminado Con Exito", "REGISTRO",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    NotificarATodos("SE ELIMINO UN LIBRO");
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

    }



    //public class Notificacion : WebSocketBehavior
    //{
    //    protected override void OnMessage(MessageEventArgs e)
    //    {
    //        // Esto ejecuta el MessageBox en el hilo de la UI (evita problemas de hilos cruzados)
    //        System.Windows.Forms.Application.OpenForms[0].BeginInvoke(new Action(() =>
    //        {
    //            MessageBox.Show("📨 Mensaje recibido: " + e.Data, "Servidor WebSocket");
    //        }));
    //    }
    //}

}
