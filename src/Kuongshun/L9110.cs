using System;
using System.Device.Gpio;
using System.Device.Pwm.Drivers;
using UnitsNet;

namespace Kuongshun
{
    public class L9110 : IDisposable
    {
        private readonly GpioController gpioController;
        private readonly int inputBPinNumber;
        private readonly SoftwarePwmChannel pwmChannel;

        public L9110(GpioController gpioController, int inputAPinNumber, int inputBPinNumber, Frequency pwmFrequency)
        {
            if (gpioController == null) throw new ArgumentNullException(nameof(gpioController));

            pwmChannel = new SoftwarePwmChannel(inputAPinNumber, (int) pwmFrequency.Hertz, 0, false, gpioController,
                false);
            gpioController.OpenPin(inputBPinNumber);
            gpioController.SetPinMode(inputBPinNumber, PinMode.Output);

            WriteInputB(gpioController, inputBPinNumber, InputB.Low);

            this.gpioController = gpioController;
            this.inputBPinNumber = inputBPinNumber;
        }

        public int Frequency
        {
            get => pwmChannel.Frequency;
            set => pwmChannel.Frequency = value;
        }

        public double DutyCycle
        {
            get => pwmChannel.DutyCycle;
            set => pwmChannel.DutyCycle = value;
        }

        public void Dispose()
        {
            gpioController.ClosePin(inputBPinNumber);
            gpioController.Dispose();
            pwmChannel.Dispose();
        }

        public void StartHigh(double dutyCycle)
        {
            pwmChannel.DutyCycle = dutyCycle;
            WriteInputB(gpioController, inputBPinNumber, InputB.High);
        }

        public void StartLow(double dutyCycle)
        {
            pwmChannel.DutyCycle = dutyCycle;
            pwmChannel.Start();
        }

        public void Stop()
        {
            WriteInputB(gpioController, inputBPinNumber, InputB.Low);
            pwmChannel.Stop();
        }

        private static void WriteInputB(GpioController gpioController, int inputBPinNumber, InputB inputB)
        {
            switch (inputB)
            {
                case InputB.Low:
                    gpioController.Write(inputBPinNumber, PinValue.Low);
                    break;
                case InputB.High:
                    gpioController.Write(inputBPinNumber, PinValue.High);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputB), inputB, null);
            }
        }
    }
}