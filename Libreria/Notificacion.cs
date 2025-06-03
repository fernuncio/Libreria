using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Libreria
{
    internal class Notificacion : WebSocketBehavior
    {
        public static List<Notificacion> Clientes = new List<Notificacion>();

        protected override void OnOpen()
        {
            Clientes.Add(this);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Clientes.Remove(this);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            // Opcional: aquí puedes recibir mensajes del cliente
            System.Windows.Forms.Application.OpenForms[0].BeginInvoke(new Action(() =>
            {
                MessageBox.Show("📨 Mensaje recibido: " + e.Data, "Servidor WebSocket");
            }));
        }

        public void Enviar(string mensaje)
        {
            Send(mensaje);
        }
    }
}
