#define RELAY1 6
#define RELAY2 7
#define RELAY3 8
#define RELAY4 9

const int button1Pin = 3;
int button1State = 0;

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

	// turn off all relays
	digitalWrite(RELAY1, HIGH);
	digitalWrite(RELAY2, HIGH);
	digitalWrite(RELAY3, HIGH);
	digitalWrite(RELAY4, HIGH);
}

void loop() {
	// put your main code here, to run repeatedly:

	button1State = digitalRead(button1Pin);
	if (button1State == HIGH) {
		digitalWrite(RELAY1, relay1);
		Serial.print("STATUS:");
		relay1 = !relay1;
		if (relay1) {
			Serial.println("A#");
		}
		else {
			Serial.println("a#");
		}
	}
	delay(500);
}

void serialCommand(String commandString) {
	char _command = commandString.charAt(0);
	String _answer = "";
	switch (_command) {
		// RELAY 1
	case 'A': // on
		digitalWrite(RELAY1, LOW);
		relay1 = true;
		_answer += "RELAY1 ON";
		break;
	case 'a': // off
		digitalWrite(RELAY1, HIGH);
		relay1 = false;
		_answer += "RELAY1 OFF";
		break;

		// RELAY 2
	case 'B': // on
		digitalWrite(RELAY2, LOW);
		relay2 = true;
		_answer += "RELAY2 ON";
		break;
	case 'b': // off
		digitalWrite(RELAY2, HIGH);
		relay2 = false;
		_answer += "RELAY2 OFF";
		break;

		// RELAY 3
	case 'C': // on
		digitalWrite(RELAY3, LOW);
		relay3 = true;
		_answer += "RELAY3 ON";
		break;
	case 'c': // off
		digitalWrite(RELAY3, HIGH);
		relay3 = false;
		_answer += "RELAY3 OFF";
		break;

		// RELAY 4
	case 'D': // on 
		digitalWrite(RELAY4, LOW);
		relay4 = true;
		_answer += "RELAY4 ON";
		break;
	case 'd': // off
		digitalWrite(RELAY4, HIGH);
		relay4 = false;
		_answer += "RELAY4 OFF";
		break;
	case 'Z':
	case 'z':
		// send the status of the relays, send A for relay1 and so on
		_answer += "STATUS:";
		if (relay1) _answer += "A"; else _answer += "a";
		if (relay2) _answer += "B"; else _answer += "b";
		if (relay3) _answer += "C"; else _answer += "c";
		if (relay4) _answer += "D"; else _answer += "d";
		break;

	default:
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