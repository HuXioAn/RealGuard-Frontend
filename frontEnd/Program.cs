using System;
using static System.Console;
using realSense;
using grpcClient;
using System.Device.Gpio;



namespace realGuardFrontEnd{

    public class frontEnd{



        public static void Main(){

            



        }


        private static void systemInit(string address){
            //Grpc客户端 realSense Gpio
            rpcClient = new grpcClient.grpcClient(address);
            cam = new realSense.realSense();

            
        }



    }



}
