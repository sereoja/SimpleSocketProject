using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//add
using System.Net;
using System.Net.Sockets;

namespace Simple_Socket_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //get ip address of this local computer
            //get the name of this (local) computer
            try
            {
                const int port = 9000;

                string localHost = Dns.GetHostName();

                // get IP address of this computer
                IPHostEntry hostEntry = Dns.GetHostEntry(localHost);
                //the hostEntry defines a property AddressList, w
                //which is an array of type IPAddress

                IPAddress[] ipaddresses = hostEntry.AddressList;
                //display the ip address associated with this computer


                //1. Create an IP EndPoint
                IPEndPoint endP = new IPEndPoint(ipaddresses[1], port);
                //2. Create a socket
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                    ProtocolType.Tcp);
                //3. bind this endpoint to the socket
                server.Bind(endP);

                //4. set the socket to listen for incoming request,
                //   then queue them
                server.Listen(10); //10 means the size of the queue
                //Listen start internal thread that is a loop that listen and puts requeste in a queue
                //5. Accept the requests
                while (true)
                {
                    Console.WriteLine("Waiting for client to connect at {0}:{1}...", ipaddresses[1], port);
                    //get the newxt client (if any) from the queue
                    Socket client = server.Accept();

                    //find out about the client and save or display the client info
                    DisplayClientInfo(client);

          //receive request client
                    byte[] buffer = new byte[1024];
                    int bytesReceived = client.Receive(buffer);

                    //convert byte array to a string
                    string request = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                    Console.WriteLine("\nClient request: ");
                    Console.WriteLine(request);
                    //process the request
                    string response = ProcessClient(client, request);

                    Console.WriteLine("Respond: ");
                    //string response = Console.ReadLine();


                    //string info = "\nrequest received: " + DateTime.Now;
                    //send a response
                    //byte[] msg = Encoding.ASCII.GetBytes(response + info);
                    byte[] msg = Encoding.ASCII.GetBytes(response);
                    client.Send(msg);
                }
               // 
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
            Console.ReadLine();
        } //end of main
        static void DisplayClientInfo(Socket client)
        {
            //get the client endpoin using the RemoteEndPoint property
            IPEndPoint clientEndP = (IPEndPoint)client.RemoteEndPoint;
            //now we can extract its IP Address and port number
            IPAddress clientIPAddress = clientEndP.Address;
            //client port
            int clientPort = clientEndP.Port;
            //get tje client domain name
            string clientName = Dns.GetHostEntry(clientIPAddress).HostName;
            //save to a file or display
            Console.WriteLine("\nClient Request from {0} @ {1}:{2}", clientName, clientIPAddress, clientPort);
        }

        static string ProcessClient(Socket client, string request)
        {
            if (request == string.Empty)
            {
                return "Error: Request not in proper format";
            }

            //split
            string[] tokens = request.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 3)
                return "Error: request not in proper format";
            string response = string.Empty;

            switch (tokens[0].ToLower())
            {
                case "add":
                    {
                        double x, y;
                        double.TryParse(tokens[1], out x);
                        double.TryParse(tokens[2], out y);
                        double result = x + y;
                        response = result.ToString();
                    }
                    break;
                default:
                    response = "invalid operation";
                    break;
            } //end of switch
            return response;
        }
    }
}
