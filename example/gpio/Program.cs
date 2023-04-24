using System;
using static System.Console;

using System.Device.Gpio;
using realGuardGpio;



var ioController = new realGuardGpio.realGuardGpio(18,200);

ioController.bodySensorCallbackRegister(false,isr);


while (true)
{


    //controller.Write(i, PinValue.High);
    ioController.gateIoSet(true);
    Thread.Sleep(1000);
    //controller.Write(i, PinValue.Low);
    ioController.gateIoSet(false);
    Thread.Sleep(1000);

    WriteLine("Pin:level{1}",ioController.bodySensorRead());

    
}



void isr(object sender, PinValueChangedEventArgs pinValueChangedEventArgs){
    WriteLine("Triggered");
}