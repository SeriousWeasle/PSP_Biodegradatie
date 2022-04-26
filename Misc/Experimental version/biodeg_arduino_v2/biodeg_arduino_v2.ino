//number of chambers
int pinCount = 11;

//analog pin numbers to which the chambers are connected.
//These are in order, so the first one in the list is CH1 and the last one is CHn.
int readPins[] = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

void setup() {
  // set up serial
  Serial.begin(9600);
  Serial.flush(); //Lets the arduino start first before receiving data
}

void loop() {
  //Check if we have a character in the Serial buffer
  if (Serial.available() > 0)
  {
    char incChar = Serial.read(); //Read out a character
    if (incChar == 'T') //if it is the letter T, confirm connection
    {
      Serial.print("BioDeg v2:INIT_START:"); //version number and init start cmd
      Serial.print(pinCount); //amount of chambers
      Serial.print(":"); //cmd seperator
      Serial.print(pinCount); //second chamber amount for parity check
      Serial.println(":INIT_END"); //init end command
    }
    else if (incChar == 'M') //if it is the letter M, do a measurement
    {
      Serial.print("MS_START:"); //measurement start cmd
      for (int i = 0; i < pinCount; i++) //go over all chambers
      {
        Serial.print(analogRead(readPins[i])); //read out the pin for chamber n
        Serial.print(":"); //cmd seperator
      }
      Serial.println("MS_END"); //measurement end cmd
    }

    else
    {
      Serial.print(incChar);
    }
  }
}
