using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;

namespace SocketServer
{    
    class Program
    {
        //VARIABLES
        public static Socket s = null;
        public static List<Socket> clientSockets = new List<Socket>();
        public static TcpListener myList = null;
        static void Main(string[] args)
        {
            //VARIABLES            
            string localIP = "192.168.201.90";
            int PUERTO = 8000;

            //Obtener la ip del equipo actual (Solo en caso de desplazamiento, se recomiendo utilizar direcciones estaticas.)
            /*
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            Console.WriteLine("Tu ip es : " + localIP)
            */
            //Fin de obtener la ip

            //Convierte una string a un IP
            IPAddress ipAddress = IPAddress.Parse(localIP);
            try
            {             
                myList = new TcpListener(ipAddress, PUERTO);
                
                Console.WriteLine("Server running - Port: " + PUERTO);
                Console.WriteLine("Local end point:" + myList.LocalEndpoint);
                myList.Start();
                zonaEspera();
            }            
            catch (SocketException ex)
            {
                myList.Stop();
            }
            catch (Exception e)
            {

                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        private static void zonaEspera()
        {
            
            try
            {
                while (true)
                {

                    s = myList.AcceptSocket();
                    clientSockets.Add(s);
                    ConexionConCliente ccc = new ConexionConCliente(s);
                    ThreadStart Conexion = new ThreadStart(ccc.Conexion);
                    Thread hilo1 = new Thread(Conexion);
                    hilo1.Start();                    
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
