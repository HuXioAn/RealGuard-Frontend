using System;
using static System.Console;
using System.Threading.Tasks;
using Grpc.Net.Client;
using grpcClient;



namespace rpc{
    public class exampleRPC{
        static public void Main(){

            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:5141");
            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(
                            new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            
        }
    }
}
