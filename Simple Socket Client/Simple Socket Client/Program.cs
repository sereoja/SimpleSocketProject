using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//add
using System.Net;
using System.Net.Sockets;

namespace Simple_Socket_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("Enter server ip address");
            //string strIPaddress = Console.ReadLine();
            //Console.Write("Enter port #: ");
            //int port = int.Parse(Console.ReadLine());

            Socket client = null;
            try
            {

           
            IPAddress ipaddress = IPAddress.Parse("172.20.2.247");
            int port = 9000;

            //create an endpoint that represesnts the server endpoint
            //1. Create an IP EndPoint
            IPEndPoint serverEndP = new IPEndPoint(ipaddress, port);
            //2. Create a socket
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);
            //connect to the server
            client.Connect(serverEndP);
            //causes the listen method of the sercer to add the client
            //to the queue and causes the Accept method of the server
            //to get this client

            //now have the client send a request
            //use the send method


            Console.WriteLine("\nEnter Request: ");
            string request = Console.ReadLine();
            //convert the request to a byte array
            byte[] msg = Encoding.ASCII.GetBytes(request);
            //send it
            client.Send(msg);

            //then have the client wait for a response from server
            //use the receive method to get a response from server
            byte[] buffer = new byte[1024];
            int bytesReceived = client.Receive(buffer);

            //convert byte array to a string
            string response = Encoding.ASCII.GetString(buffer, 0, bytesReceived);

            //display
            Console.WriteLine("\nServer Response: \n");
            Console.WriteLine(response);
           // 
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
            finally
            {
                //close the socket
                if (client!=null)
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
                
            }
            Console.ReadLine();
        }
    }
}
