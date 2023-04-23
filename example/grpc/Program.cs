using System;
using static System.Console;
using System.Threading.Tasks;
using Grpc.Net.Client;
using realGuardRpc;
using realSense;
using grpcClient;



namespace rpc{
    public class exampleRPC{
        static public void Main(){

            var grpc = new grpcClient.grpcClient("http://localhost:50051");

            var d430 = new realSense.realSense();
            d430.laserOff();
            var irImg = d430.getIrImg();
            irImg.Save("./pic/irImg.jpg");
            
            d430.laserOn();
            var depth = d430.getDepthData();



            var reply = grpc.authRequstAsync("./pic/irImg.jpg",depth).Result;

            
            WriteLine("Status:{0},Result:{1}",reply.Status,reply.Result);
            WriteLine("Press any key to close");
            ReadKey();

            
        }
    }
}
