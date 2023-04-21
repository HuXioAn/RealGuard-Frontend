using System;
using static System.Console;

using System.Device.Gpio;



//IO使用sys定义
int i = 18;

using var controller = new GpioController();

controller.OpenPin(i, PinMode.Output);






while (true)
{


    controller.Write(i, PinValue.High);
    Thread.Sleep(1000);
    controller.Write(i, PinValue.Low);
    Thread.Sleep(1000);

    

}
