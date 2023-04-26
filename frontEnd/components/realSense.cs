using Intel.RealSense;
using static System.Console;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;


namespace realSense{

    public class realSense{

        private Int16 width = 640;
        private Int16 height = 480;

        private Pipeline pipe;

        private Sensor laser;

        public realSense(Int16 Width = 640,Int16 Height = 480){

            width = Width;
            height = Height;

            using (var ctx = new Context())
                {
                    var devices = ctx.QueryDevices();
                    if(devices.Count() == 0){
                        throw new Exception("Can not find a RealSense device.");
                    }
                    var dev = devices[0];

                    Console.WriteLine("\nUsing device 0, an {0}", dev.Info[CameraInfo.Name]);
                    Console.WriteLine("    Serial number: {0}", dev.Info[CameraInfo.SerialNumber]);
                    Console.WriteLine("    Firmware version: {0}", dev.Info[CameraInfo.FirmwareVersion]);
                }


            var config = new Config();
            config.EnableStream(Intel.RealSense.Stream.Depth,width,height,Format.Z16,15);

            config.EnableStream(Intel.RealSense.Stream.Infrared,1,width,height,Format.Y8,15);//左眼
            config.EnableStream(Intel.RealSense.Stream.Infrared,2,width,height,Format.Y8,15);//右眼

            pipe = new Pipeline();
            PipelineProfile selection = pipe.Start(config);

            var selected_device = selection.Device;
            laser = selected_device.Sensors[0];
        }


        public void laserOn(){
            if (laser.Options.Supports(Option.EmitterEnabled))
                {
                    laser.Options[Option.EmitterEnabled].Value = 1f; // Disable emitter
                    Thread.Sleep(300);//Time to react
                }
        }

        public async Task laserOnAsync(){
            if (laser.Options.Supports(Option.EmitterEnabled))
                {
                    laser.Options[Option.EmitterEnabled].Value = 1f; // Disable emitter
                    await Task.Delay(250);//Time to react
                }
        }

        public void laserOff(){
            if (laser.Options.Supports(Option.EmitterEnabled))
                {
                    laser.Options[Option.EmitterEnabled].Value = 0f; // Disable emitter
                    Thread.Sleep(150);//Time to react
                }
        }

        public async Task laserOffAsync(){
            if (laser.Options.Supports(Option.EmitterEnabled))
                {
                    laser.Options[Option.EmitterEnabled].Value = 0f; // Disable emitter
                    await Task.Delay(150);//Time to react
                }
        }


        public Image getIrImg(){
            using(var frames = pipe.WaitForFrames()){
                var irFrame = frames.InfraredFrame.DisposeWith(frames);
                var irByte = new byte[irFrame.DataSize];
                Marshal.Copy(irFrame.Data,irByte,0,irFrame.DataSize);
                return Image.LoadPixelData<L8>(irByte,irFrame.Width,irFrame.Height);
            }
        }


        public byte[] getDepthData(){
             using(var frames = pipe.WaitForFrames()){
    
                var depthFrame = frames.DepthFrame.DisposeWith(frames);
                var depthByte = new byte[depthFrame.DataSize];
                Marshal.Copy(depthFrame.Data,depthByte,0,depthFrame.DataSize);

                return depthByte;

            }
        }
    }
}