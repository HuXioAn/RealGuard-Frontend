using System;
using static System.Console;

using System.Device.Gpio;
using realGuardGpio;



var ioController = new realGuardGpio.realGuardGpio(18,200);

ioController.bodySensorCallbackRegister(true,isr);


while (true)
{

    Thread.Sleep(1000);

    WriteLine("Pin:level{0}",ioController.bodySensorRead());

    
}



void isr(object sender, PinValueChangedEventArgs pinValueChangedEventArgs){
    WriteLine("Triggered");
}