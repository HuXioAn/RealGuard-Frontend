using System;
using static System.Console;
using realSense;
using grpcClient;
using realGuardGpio;
using realWebSocketServer;



namespace realGuardFrontEnd{

    public class frontEnd{
        private static grpcClient.grpcClient? rpcClient;
        private static realSense.realSense? cam;
        private static realGuardGpio.realGuardGpio? ioController;

        private static string rpcAddress = "http://localhost:5051";

        private static string wsAddress = "ws://localhost:5050";

        //public static bool registerFlag = false;

        public static void Main(){

            while(true){

                //资源请求 初始化

                systemInit(rpcAddress);

                var wsServer = new realWebSocketServer.realWebSocketServer(wsAddress);

                while(true){
                    //业务逻辑
                    while((!ioController!.bodySensorRead()) || wsServer.registering == true){
                        Thread.Sleep(100);
                    }
                    Thread.Sleep(500);
                    WriteLine("Body Triggered");//人体触发

                    WriteLine("Fetching IR Image.");
                    var irImgStream = new MemoryStream();
                    var irImgStreamToRequest = new MemoryStream();

                    cam!.getIrImgStream(irImgStream);
                    var onTask = cam.laserOnAsync();

                    //var filePath = string.Format("./pic/irImg_{0}.jpg",(UInt64)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds);
                    onTask.Wait();
                    WriteLine("Fetching Depth Data.");
                    var depthData = cam.getDepthData();
                    var offTask = cam.laserOffAsync();

                        
                    while(true){
                        
                        irImgStreamToRequest.Seek(0,SeekOrigin.Begin);
                        irImgStream.Seek(0,SeekOrigin.Begin);
                        irImgStream.CopyTo(irImgStreamToRequest);

                        WriteLine("Requiring BackEnd.");
                        var replyTask = rpcClient!.authRequstAsync(irImgStreamToRequest,depthData);
                        
                        //后端请求异步采集
                        offTask.Wait();
                        WriteLine("Fetching IR Image.");
                        cam!.getIrImgStream(irImgStream);
                        onTask = cam.laserOnAsync();

                        onTask.Wait();
                        WriteLine("Fetching Depth Data.");
                        depthData = cam.getDepthData();
                        offTask = cam.laserOffAsync();

                        
                        replyTask.Wait();
                        var reply = replyTask.Result;
                        WriteLine("Status:{0},Result:{1},name:{2}",reply.Status,reply.Result,reply.Name);

                        if(reply.Status == 100){
                            //pass
                            ioController.openGateAsync();
                            //识别成功图片保存
                            using(var imageFileStream = File.Create("./pic/"+reply.Name
                            +"_"
                            +reply.Result.ToString()
                            +"_"
                            +DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds.ToString()
                            +".jpg")){
                                irImgStreamToRequest.Seek(0,SeekOrigin.Begin);
                                irImgStreamToRequest.CopyToAsync(imageFileStream);
                            }
                            break;
                        }
                        if(!ioController.bodySensorRead())break;//失败且无人，取消继续检测

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


        public static MemoryStream snap(){

            //拍照、查询后端、返回照片
            cam!.laserOff();
            var irImgStream = new MemoryStream();
            cam!.getIrImgStream(irImgStream);
            
            return irImgStream;
        }

        public static float register(MemoryStream picStream, string name, string studentId){
            //采纳照片

            return -1;
        }

        public static float registerCheck(MemoryStream picStream, string name, string studentId){
            //验证照片

            return -1;
        }

    }



}
