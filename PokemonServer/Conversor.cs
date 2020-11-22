using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using XmlWorker;

namespace PokemonServer
{
    class Conversor
    {
        private TcpClient client = null;
        public Conversor(TcpClient client)
        {
            this.client = client;
        }
        public void ProcessRequest()
        {
            XmlWorker.XmlConverter converter = new XmlWorker.XmlConverter();
            Byte[] bytes = new Byte[256];
            String data = null;
            NetworkStream stream = client.GetStream();
            Int32 i = stream.Read(bytes, 0, bytes.Length);
            // Traducimos los datos enviados a un string ASCII
            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i).Trim();
            // Determinamos el tipo de conversión y la efectuamos
            if (data == "tipos")
            {
                data = SendAllTypes();
            }
            else
            {
                data = converter.ProcessXmlResponse(GetWeaknessTypes(converter.GetXmlData(data)), true);
            }
            Byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(msg, 0, msg.Length);
            // Cerramos el cliente y la conexión con él
            client.Close();
        }
        /**
         * 
         */
        public String[] GetWeaknessTypes(String[] rootType)
        {
            Console.WriteLine("Weakness");
            List<String> weaknesses = new List<string>();
            String[] tempWeaknesses;
            foreach (String type in rootType)
            {
                Console.WriteLine(type);
                tempWeaknesses = FindWeaknessInType(type);
                foreach (String weak in tempWeaknesses)
                {
                    weaknesses.Add(weak);
                }
            }
            return weaknesses.ToArray();
        }
        /**
         * This method uses a switch for a reference of the type and stores in list and then converted in
         * an array
         * 
         * 
         */
        public String[] FindWeaknessInType(String type)
        {
            List<String> responseType = new List<String>();
            switch (type.ToLower())
            {
                case "0":
                case "normal":
                    responseType.Add("Lucha");
                    break;
                case "1":
                case "fuego":
                    responseType.Add("Agua");
                    responseType.Add("Tierra");
                    responseType.Add("Roca");
                    break;
                case "2":
                case "agua":
                    responseType.Add("Planta");
                    responseType.Add("Electrico");
                    break;
                case "3":
                case "planta":
                    responseType.Add("Fuego");
                    responseType.Add("Hielo");
                    responseType.Add("Veneno");
                    responseType.Add("Volador");
                    break;
                default:
                    responseType.Add("NO HAY DATOS");
                    break;
            }
            return responseType.ToArray();
        }
        /*
         * Normal: débil frente a Lucha
Fuego: débil frente a Agua, Tierra, Roca
Agua: débil frente a Planta, Eléctrico
Planta: débil frente a Fuego, Hielo, Veneno, Volador, Bicho
Eléctrico: débil frente a Tierra
Hielo: débil frente a Fuego, Lucha, Roca, Acero
Lucha: débil frente a Volador, Psíquico, Hada
Veneno: débil frente a Tierra, Psíquico
Tierra: débil frente a Agua, Planta, Hielo
Volador: débil frente a Eléctrico, Hielo, Roca
Psíquico: débil frente a Bicho, Fantasma, Siniestro
Bicho: débil frente a Volador, Roca, Fuego
Roca: débil frente a Agua, Planta, Lucha, Tierra, Acero
Fantasma: débil frente a Fantasma, Siniestro
Dragón: débil frente a Hielo, Dragón, Hada
Siniestro: débil frente a Lucha, Bicho, Hada
Acero: débil frente a Fuego, Lucha, Tierra
Hada: débil frente a Veneno, Acero
         * 
         */
        public String SendAllTypes()
        {
            List<String> ListTypes = new List<String>();
            ListTypes.Add("Normal");
            ListTypes.Add("Fuego");
            ListTypes.Add("Agua");
            XmlWorker.XmlConverter converter = new XmlWorker.XmlConverter();
            return converter.ProcessXmlResponse(ListTypes.ToArray(), true);
        }
        
    }
}
