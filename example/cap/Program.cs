using Intel.RealSense;
using static System.Console;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;


Int16 WIDTH = 640;
Int16 HEIGHT = 480;

using (var ctx = new Context())
                {
                    var devices = ctx.QueryDevices();
                    var dev = devices[0];

                    Console.WriteLine("\nUsing device 0, an {0}", dev.Info[CameraInfo.Name]);
                    Console.WriteLine("    Serial number: {0}", dev.Info[CameraInfo.SerialNumber]);
                    Console.WriteLine("    Firmware version: {0}", dev.Info[CameraInfo.FirmwareVersion]);
                }


var config = new Config();
config.EnableStream(Intel.RealSense.Stream.Depth,640,480,Format.Z16,15);
//config.EnableStream(Intel.RealSense.Stream.Infrared,Format.Y16);
config.EnableStream(Intel.RealSense.Stream.Infrared,1,WIDTH,HEIGHT,Format.Y8,15);//左眼
config.EnableStream(Intel.RealSense.Stream.Infrared,2,WIDTH,HEIGHT,Format.Y8,15);//右眼

//config.EnableStream(Intel.RealSense.Stream.Infrared,1);

var pipe = new Pipeline();
PipelineProfile selection = pipe.Start(config);

var selected_device = selection.Device;
var depth_sensor = selected_device.Sensors[0];

if (depth_sensor.Options.Supports(Option.EmitterEnabled))
{
    depth_sensor.Options[Option.EmitterEnabled].Value = 0f; // Enable emitter
    //depth_sensor.Options[Option.EmitterEnabled].Value = 0f; // Disable emitter
    // Thread.Sleep(1000);
    // depth_sensor.Options[Option.EmitterEnabled].Value = 0f; // Disable emitter
    // Thread.Sleep(1000);
    // depth_sensor.Options[Option.EmitterEnabled].Value = 1f;
}

Colorizer color_map = new Colorizer();

using(var frames = pipe.WaitForFrames()){
    

    // var depthFrame = frames.DepthFrame.DisposeWith(frames);
    // var colorizedDepth = color_map.Process<VideoFrame>(depthFrame).DisposeWith(frames);
    // Bitmap DepthImg = new System.Drawing.Bitmap(depthFrame.Width, depthFrame.Height, depthFrame.Stride, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, colorizedDepth.Data);
    // WriteLine("depthImg:Height:{0},Width:{1}",DepthImg.Height,DepthImg.Width);
    // var newDepth = new Bitmap(DepthImg);
    // newDepth.Save("./pic/depth.png",System.Drawing.Imaging.ImageFormat.Png);
    // WriteLine("depthFrame:Height:{0},Width:{1}",depthFrame.Height,depthFrame.Width);



    var irFrame = frames.InfraredFrame.DisposeWith(frames);
    //Bitmap irImg = new System.Drawing.Bitmap(irFrame.Width, irFrame.Height, irFrame.Stride, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, irFrame.Data);
    //Bitmap irImg = new System.Drawing.Bitmap(irFrame.Height, irFrame.Width, irFrame.Stride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, irFrame.Data);
    
    //var img = SixLabors.ImageSharp.Image.Load();
    var irByte = new byte[irFrame.DataSize];
    Marshal.Copy(irFrame.Data,irByte,0,irFrame.DataSize);
    
    //var irImg = Image.Load(irByte);
    var irImg = Image.LoadPixelData<L8>(irByte,irFrame.Width,irFrame.Height);
    
    WriteLine("depthFrame:Height:{0},Width:{1}",irImg.Height,irImg.Width);
    
    irImg.Save("./pic/irImg.jpg");
    //Bitmap newImage = new Bitmap(irImg);
    //newImage.Save("./pic/ir.png",System.Drawing.Imaging.ImageFormat.Png);
    WriteLine("irFrame:Height:{0},Width:{1}",irFrame.Height,irFrame.Width);

    

}