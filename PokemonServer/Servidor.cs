using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PokemonServer
{
    class Servidor
    {
        public void ArrancarMultiThread()
        {
            try
            {
                // Hacemos que el TcpListener escuche en host:port.
                IPAddress localAddr = IPAddress.Parse(ConfigurationManager.AppSettings["host"]);
                TcpListener server = new TcpListener(localAddr, Int32.Parse(ConfigurationManager.AppSettings["port"]));
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    lock (client)
                    {
                        //requestCounter += 1;
                    }
                    Conversor procesoConversion = new Conversor(client);
                    Thread hiloProcesador = new Thread(new ThreadStart(procesoConversion.ProcessRequest));
                    hiloProcesador.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
