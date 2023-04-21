using System;
using static System.Console;

using System.Device.Gpio;


using var controller = new GpioController();
for(int i=1;i<20;i++){
    try{
        controller.OpenPin(i, PinMode.Output);
    }
    catch(System.Exception e){
        WriteLine("Pin{0}:{1}",i,e.Message);
    }
}




while (true)
{


    for(int i=1;i<20;i++){
        try{
            controller.Write(i, PinValue.High);
            Thread.Sleep(1000);
            controller.Write(i, PinValue.Low);
        
        }
        catch(System.Exception e){
            WriteLine("Pin{0}:{1}",i,e.Message);
        }
        
    }

    

}
