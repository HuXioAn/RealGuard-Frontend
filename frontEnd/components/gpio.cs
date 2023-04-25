using System;
using static System.Console;

using System.Device.Gpio;


namespace realGuardGpio{

    public class realGuardGpio{

        private int gateSwitchPin;
        private int bodySensorPin;
        private GpioController controller;
        private PinChangeEventHandler? sensorDelegate;

        public realGuardGpio(int gate,int body){
            gateSwitchPin = gate;
            bodySensorPin = body;

            try{
                controller = new GpioController();
                controller.OpenPin(gateSwitchPin, PinMode.Output);
                controller.OpenPin(bodySensorPin, PinMode.Input);                
            }catch(Exception e){
                throw new Exception("Can not initialize the GPIO:"+e.Message);
            }
        }

        public void gateIoSet(bool level){
            controller.Write(gateSwitchPin,level?PinValue.High:PinValue.Low);
        }

        public bool bodySensorRead(){
            return controller.Read(bodySensorPin) == PinValue.High ? true : false;
        }

        public void bodySensorCallbackRegister(bool edge,Action<object,PinValueChangedEventArgs> callback){
            sensorDelegate = new PinChangeEventHandler(callback);
            controller.RegisterCallbackForPinValueChangedEvent(bodySensorPin,edge?PinEventTypes.Rising:PinEventTypes.Falling,sensorDelegate);
        }

        public bool bodySensorEventWait(bool edge){
            return controller.WaitForEvent(bodySensorPin,edge?PinEventTypes.Rising:PinEventTypes.Falling,new TimeSpan(days:1,0,0,0)).TimedOut;
        }

    }


}