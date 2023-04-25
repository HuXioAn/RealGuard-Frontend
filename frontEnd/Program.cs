using System;
using static System.Console;
using realSense;
using grpcClient;
using realGuardGpio;



namespace realGuardFrontEnd{

    public class frontEnd{
        private static grpcClient.grpcClient? rpcClient;
        private static realSense.realSense? cam;
        private static realGuardGpio.realGuardGpio? ioController;

        private static string rpcAddress = "http://localhost:5051";

        public static void Main(){

            while(true){

                //资源请求 初始化

                systemInit(rpcAddress);

                while(true){
                    //业务逻辑
                    while(!ioController!.bodySensorRead()){
                        Thread.Sleep(100);
                    }
                    Thread.Sleep(500);
                    WriteLine("Body Triggered");
                    //人体触发
                    WriteLine("Fetching IR Image.");
                    var irImg = cam!.getIrImg();
                    cam.laserOn();
                    WriteLine("Fetching Depth Data.");
                    var depthData = cam.getDepthData();
                    cam.laserOff();

                    var filePath = string.Format("./pic/irImg_{0}.jpg",(UInt64)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds);
                    irImg.Save(filePath);

                    WriteLine("Requiring BackEnd.");
                    var replyTask = rpcClient!.authRequstAsync(filePath,depthData);
                    var reply = replyTask.Result;
                    WriteLine("Status:{0},Result:{1},name:{2}",reply.Status,reply.Result,reply.Name);

                    if(reply.Status == 100){
                        //pass
                        ioController.gateIoSet(true);
                        Thread.Sleep(1000);
                        ioController.gateIoSet(false);
                    }


                }

                //资源回收善后

            }

        }


        private static void systemInit(string address){
            //Grpc客户端 realSense Gpio

            try{
                rpcClient = new grpcClient.grpcClient(address);
                cam = new realSense.realSense();
                ioController = new realGuardGpio.realGuardGpio(18,200);
            }catch(Exception e){
                //WriteLine("[!]Problems initiating the system components: \n {0}",e.Message);
                throw new Exception("[!]Problems initiating the system components: \n {0}"+e.Message);
            }
            

            ioController.gateIoSet(false);
            cam.laserOff();

            
        }



    }



}
