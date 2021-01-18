# L9110 2 Channel Motor Driver library for Raspberry Pi.

This library controls the L9110 2 channel motor driver using a soft Pulse Width Modulation.

```c#
var gpioController = GpioController.GetDefault();
var speedPinNumber = 18; // input A is used to control speed
var directionPinNumber = 21; // input B is used to control direction
var dutyCycle = 0.5d;

var l9110 = new L9110(gpioController, speedPinNumber, directionPinNumber);

l9110.StartLow(dutyCycle); // input B is low; clockwise
l9110.Stop();

l9110.StartHigh(dutyCycle); // input B is high; anticlockwise
l9110.Stop();

```
