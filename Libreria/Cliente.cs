using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace Libreria
{
    internal class Cliente
    {
        private WebSocket cliente;

        public Cliente(string url)
        {
            cliente = new WebSocket(url);

            cliente.OnOpen += (sender, e) =>
            {
                Console.WriteLine("Conectado al servidor");
            };

            cliente.OnMessage += (sender, e) =>
            {
                MessageBox.Show("Mensaje recibido: " + e.Data, "Cliente");
            };

            cliente.OnClose += (sender, e) =>
            {
                Console.WriteLine("Conexion cerrada");
            };

            cliente.OnError += (sender, e) => 
            {
                Console.WriteLine("Error: " + e.Message);
            };
        }

        public void conectar()
        {
            cliente.Connect();
        }

        public void enviar(string msj)
        {
            cliente.Send(msj);
        }

        public void cerrar()
        {
            cliente.Close();
        }
    }
}
