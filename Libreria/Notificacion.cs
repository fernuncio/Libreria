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
        public static event Action<string> MensajeRecibido;
        protected override void OnOpen()
        {
            Clientes.Add(this);
            Console.WriteLine("Cliente Conectado");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Clientes.Remove(this);
        }

        // Opcional pero útil para depurar
        protected override void OnMessage(MessageEventArgs e)
        {
            Application.OpenForms[0].BeginInvoke(new Action(() =>
            {
                MessageBox.Show("Mensaje recibido: " + e.Data, "Servidor WebSocket");
                MensajeRecibido?.Invoke(e.Data);
            }));
            
        }

        public void Enviar(string mensaje)
        {
            Send(mensaje);
        }
        
        
    }
}
