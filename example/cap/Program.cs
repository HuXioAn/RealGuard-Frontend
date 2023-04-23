using Intel.RealSense;
using static System.Console;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using realSense;


var d430 = new realSense.realSense();

d430.laserOff();

var irImg = d430.getIrImg();

irImg.Save("./pic/irImg.jpg");


d430.laserOn();

var depth = d430.getDepthData();







