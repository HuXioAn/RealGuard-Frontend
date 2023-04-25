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
                    while(ioController!.bodySensorEventWait(true));

                    //人体触发
                    cam!.laserOff();
                    Thread.Sleep(500);
                    var irImg = cam.getIrImg();
                    cam.laserOn();
                    var depthData = cam.getDepthData();

                    var filePath = string.Format("./pic/irImg_{0}.jpg",(UInt64)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds);
                    irImg.Save(filePath);

                    var reply = rpcClient!.authRequstAsync(filePath,depthData);

                    WriteLine("Status:{0},Result:{1}",reply.Status,reply.Result);

                    


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

            
        }



    }



}
