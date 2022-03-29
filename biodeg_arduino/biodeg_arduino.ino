int readPins[] = {15, 13, 11, 9, 7, 5, 3, 1, 0};

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  //Check if we have a character in the Serial buffer
  if (Serial.available() > 0)
  {
    int incByte = Serial.read(); //Read out a character
    if (incByte == 84) //if it is the letter T, send a T back to confirm
    {
      Serial.println("BioDeg v1");
    }
    else if (incByte == 77) //if the character is not a line break, do stuff
    {
      for (int i = 0; i < 9; i++)
      {
        Serial.println(analogRead(readPins[i]));
      }
    }
  }
}
