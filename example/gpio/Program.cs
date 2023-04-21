using System;
using static System.Console;

using System.Device.Gpio;

var dele = new PinChangeEventHandler(isr);

//IO使用sys定义
int i = 18;
int k = 79;

using var controller = new GpioController();

controller.OpenPin(i, PinMode.Output);
controller.OpenPin(k,PinMode.InputPullUp,PinValue.High);
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