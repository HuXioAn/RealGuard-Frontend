using System;
using static System.Console;

using System.Device.Gpio;

var dele = new PinChangeEventHandler(isr);

//IO使用sys定义
int i = 18;
int k = 200;

using var controller = new GpioController();

controller.OpenPin(i, PinMode.Output);

if(controller.IsPinModeSupported(k,PinMode.InputPullUp))
    //上拉无望
    /*
    https://developer.nvidia.com/embedded/dlc/Jetson-Nano-40-Pin-Expansion-Header-1.1
    */

    controller.OpenPin(k,PinMode.InputPullUp,PinValue.High);
else
    WriteLine("Pin{0} can not be inside pullup.",k);
    controller.OpenPin(k,PinMode.Input,PinValue.High);

controller.RegisterCallbackForPinValueChangedEvent(k,PinEventTypes.Falling,dele);


while (true)
{


    controller.Write(i, PinValue.High);
    Thread.Sleep(1000);
    controller.Write(i, PinValue.Low);
    Thread.Sleep(1000);

    WriteLine("Pin{0}:level{1}",k,controller.Read(k));

    
}



void isr(object sender, PinValueChangedEventArgs pinValueChangedEventArgs){
    WriteLine("Triggered");
}