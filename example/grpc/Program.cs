﻿using System;
using static System.Console;
using System.Threading.Tasks;
using Grpc.Net.Client;
using realGuardRpc;
using realSense;



namespace rpc{
    public class exampleRPC{
        static public void Main(){

            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:5141");
            var client = new auth.authClient(channel);



            var d430 = new realSense.realSense();

            d430.laserOff();

            var irImg = d430.getIrImg();
            irImg.Save("./pic/irImg.jpg");
            

            d430.laserOn();
            var depth = d430.getDepthData();



            var reply = client.do_auth(
                            new auth_request { 
                                TimeStamp = (UInt64)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds,
                                IrImg = Google.Protobuf.ByteString.CopyFrom(File.ReadAllBytes("./pic/irImg.jpg")),
                                DepthData = Google.Protobuf.ByteString.CopyFrom(depth)
                                });

            
            WriteLine("Status:{},Result:{}",reply.Status,reply.Result);
            WriteLine("Press any key to close");
            ReadKey();

            
        }
    }
}
