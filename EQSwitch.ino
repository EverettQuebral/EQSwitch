// EQ Switch
// Arduino code in controlling relay switches

#define RELAY1 8
#define RELAY2 9
#define RELAY3 10
#define RELAY4 11

const int button1Pin = 7;
const int button2Pin = 6;
const int button3Pin = 5;
const int button4Pin = 4;

int button1State = 0;
int button1StatePrevious = 0;
int button2State = 0;
int button3State = 0;
int button4State = 0;

String inputString;
// for status of the relay, on = true
bool relay1 = false;
bool relay2 = false;
bool relay3 = false;
bool relay4 = false;

void setup() {
  Serial.begin(115200);
  Serial.println("EQSwitch#");

  pinMode(RELAY1, OUTPUT);
  pinMode(RELAY2, OUTPUT);
  pinMode(RELAY3, OUTPUT);
  pinMode(RELAY4, OUTPUT);

  // use the internal resistors for the pins
  pinMode(button1Pin, INPUT_PULLUP);
  pinMode(button2Pin, INPUT_PULLUP);
  pinMode(button3Pin, INPUT_PULLUP);
  pinMode(button4Pin, INPUT_PULLUP);

  // turn off all relays
  digitalWrite(RELAY1, HIGH);
  digitalWrite(RELAY2, HIGH);
  digitalWrite(RELAY3, HIGH);
  digitalWrite(RELAY4, HIGH);
}

void loop() {
  // put your main code here, to run repeatedly:
  
  button1State = digitalRead(button1Pin);
  button2State = digitalRead(button2Pin);
  button3State = digitalRead(button3Pin);
  button4State = digitalRead(button4Pin);
 
  if (button1State != button1StatePrevious){
    button1StatePrevious = button1State;
    Serial.print("STATUS:");
    digitalWrite(RELAY1, !button1State);
    if (button1State){
      Serial.println("A#");
    }
    else {
      Serial.println("a#");
    }
  }

  if (button2State == LOW){
    digitalWrite(RELAY2, relay2);
    Serial.print("STATUS:");
    relay2 = !relay2;
    if (relay2) {
      Serial.println("B#");
    }
    else {
      Serial.println("b#");
    }
    delay(300);
  }
  if (button3State == LOW){
    digitalWrite(RELAY3, relay3);
    Serial.print("STATUS:");
    relay3 = !relay3;
    if (relay3) {
      Serial.println("C#");
    }
    else {
      Serial.println("c#");
    }
    delay(300);
  }
  if (button4State == LOW){
    digitalWrite(RELAY4, relay4);
    Serial.print("STATUS:");
    relay4 = !relay4;
    if (relay4) {
      Serial.println("D#");
    }
    else {
      Serial.println("d#");
    }
    delay(300);
  }
  
  
}

void serialCommand(String commandString){
  char _command = commandString.charAt(0);
  String _answer = "";
  switch(_command){
    // RELAY 1
    case 'A' : // on
      digitalWrite(RELAY1, LOW);
      relay1 = true;
      _answer += "RELAY1 ON";
      break;
    case 'a' : // off
      digitalWrite(RELAY1, HIGH);
      relay1 = false;
      _answer += "RELAY1 OFF";
      break;
      
    // RELAY 2
    case 'B' : // on
      digitalWrite(RELAY2, LOW);
      relay2 = true;
      _answer += "RELAY2 ON";
      break;
    case 'b' : // off
      digitalWrite(RELAY2, HIGH);
      relay2 = false;
      _answer += "RELAY2 OFF";
      break;
      
    // RELAY 3
    case 'C' : // on
      digitalWrite(RELAY3, LOW);
      relay3 = true;
      _answer += "RELAY3 ON";
      break;
    case 'c' : // off
      digitalWrite(RELAY3, HIGH);
      relay3 = false;
      _answer += "RELAY3 OFF";
      break;

    // RELAY 4
    case 'D' : // on 
      digitalWrite(RELAY4, LOW);
      relay4 = true;
      _answer += "RELAY4 ON";
      break;
    case 'd' : // off
      digitalWrite(RELAY4, HIGH);
      relay4 = false;
      _answer += "RELAY4 OFF";
      break;
    case 'Z' :
    case 'z' :
      // send the status of the relays, send A for relay1 and so on
      _answer += "STATUS:";
      if (relay1) _answer += "A"; else _answer += "a";
      if (relay2) _answer += "B"; else _answer += "b";
      if (relay3) _answer += "C"; else _answer += "c";
      if (relay4) _answer += "D"; else _answer += "d";
      break;
    case 'X' :
    case 'x' :
      _answer += "EQSwitch";
      break;
      
    default : 
      _answer += "EQSwitch";
      break;
  }

  _answer += "#";
  Serial.println(_answer);
}

/**
 * handler for the serial communicationes
 * calls the SerialCommand whenever a new command is received
 */
void serialEvent() {
  while (Serial.available()) {
    char inChar = (char)Serial.read();
    inputString += inChar;
    if (inChar == '\n') {
      serialCommand(inputString);
      inputString = "";
    }
  }
}
