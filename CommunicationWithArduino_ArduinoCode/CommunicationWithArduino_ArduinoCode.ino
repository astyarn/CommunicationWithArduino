int temp = 0;     //raw temp
int tempC = 0;    //degrees celcius
int lys = 0;      //raw light
double convertTemp = 0.5; //conversion variable for raw to degrees celcius
int k, v, o;

String rcvData;     //buffer string to receive serial data
int tempRange = 0;  //The wanted temperature

//3 LED to indikate Heating, Normal and cooling
byte red = 5;     //Heating
byte green = 6;
byte yellow = 7;  //cooling
byte tempPin = 0;   //analog pin to read temp from (LM35)
byte lightPin = 1;  //analog pin to read light values from LDR


//-----------------------------------------
void CheckTempRange(float iTemp, float tempRange)
{
  if (iTemp < tempRange - 1)
  {
    digitalWrite(red, HIGH);
    v = 1;
    digitalWrite(yellow, LOW);
    k = 0;
    digitalWrite(green, LOW);
    o = 0;

  }
  else if(iTemp > tempRange + 1)
  {
    digitalWrite(yellow, HIGH);
    k = 1;
    digitalWrite(green, LOW);
    o = 0;
    digitalWrite(red, LOW);
    v = 0;
  }
  else
  {
    digitalWrite(green, HIGH);
    o = 1;
    digitalWrite(yellow, LOW);
    k = 0;
    digitalWrite(red, LOW);
    v = 0;
  }
  
}


void setup() {
  pinMode(yellow, OUTPUT);
  pinMode(green, OUTPUT);
  pinMode(red, OUTPUT);
  digitalWrite(yellow, LOW);
  digitalWrite(green, LOW);
  digitalWrite(red, LOW);
  Serial.begin(9600);
}

void loop() {
  //Analog readings are run 3 times before a value is kept to clear and reset the reader
  for(int i = 0; i<3; i++)
  {
  temp = analogRead(tempPin);
  }
  for(int j = 0; j<3; j++)
  {
  lys = analogRead(lightPin);
  }
  tempC = temp*convertTemp;

  if(Serial.available())  //If data is received on the serial connection, start processing it
  {
    rcvData = Serial.readString();
    tempRange = rcvData.toInt();
  }
  
  CheckTempRange(tempC, tempRange);   //Perform Heating/Cooling check

  //Send the data to WPF
  //The data is constructed into a string with data seperated by ,
  Serial.print("D");
  Serial.print(",");
  Serial.print(tempC);
  Serial.print(",");
  Serial.print(k);
  Serial.print(",");
  Serial.print(v);
  Serial.print(",");
  Serial.print(o);
  Serial.print(",");
  Serial.print(tempRange);
  Serial.print(",");
  Serial.println();
  delay(500);   //Delay set to 500 ms 
}
