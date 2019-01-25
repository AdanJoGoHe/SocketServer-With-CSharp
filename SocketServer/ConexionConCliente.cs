using SocketClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    
    class ConexionConCliente
    {
        Socket s;
        Recursos rc;
        public string mensaje = null;
        public  ConexionConCliente(Socket s)
        {
            this.s = s;
        }

        public void Conexion()
        {
            rc = new Recursos();
            
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint + "\n");
            try
            {
                while(true)
                { 
                    byte[] b = new byte[300];
                    int k = s.Receive(b);
                    rc.pintar_verde();
                    Console.Write(s.RemoteEndPoint + " Says : ");
                    mensaje = null;
                    for (int i = 0; i < k; i++)
                    {
                        mensaje += Convert.ToChar(b[i]);
                                       
                    }
                    Console.Write(mensaje);
                    rc.pintar_blanco();
                    ASCIIEncoding asen = new ASCIIEncoding();
                    mensaje += "\n";
                    sentToAll(mensaje,s);
                }                
            }
            catch (SocketException e)
            {
                Console.WriteLine("Fin de la conexion con : " + s.RemoteEndPoint + "\n");
                Program.clientSockets.Remove(s);
                s.Close();
                
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        private static void sentToAll(string s, Socket so)
        {
            foreach (Socket socket in Program.clientSockets)
            {
                IPEndPoint remoteIpEndPoint = so.RemoteEndPoint as IPEndPoint;
                byte[] data = Encoding.ASCII.GetBytes(remoteIpEndPoint + " Says : " + s + "\n");
                    socket.Send(data);
                
                
            }
        }
    }
    
}
