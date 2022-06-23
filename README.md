# CommunicationWithArduino
Use of WPF GUI to read and send data to and from an Arduino over a serial connection (USB)

This project has a temperature sensor (LM35) and a Light-dependent resistor (LDR) connected to an Arduino Uno and three LED's.
The setup is simulation a barn were temperature has to be measured and indicated via turning on one of the LED's.

Data from the Arduino is send to a WPF app via serial communication. Additionally the desired temperature can be set via the WPF app,
and also send to the Arduino via serial communication.

#Code
The code is in two parts. The Visual Studio project containing the wpf app, and the .ino file containing the code uploaded to the Arduino.
