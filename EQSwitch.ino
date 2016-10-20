#define RELAY1 6
#define RELAY2 7
#define RELAY3 8
#define RELAY4 9

String inputString;

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
}

void serialCommand(String commandString) {
	char _command = commandString.charAt(0);
	String _answer = "";
	switch (_command) {
		// RELAY 1
	case 'A': // on
		digitalWrite(RELAY1, LOW);
		_answer += "RELAY1 ON";
		break;
	case 'a': // off
		digitalWrite(RELAY1, HIGH);
		_answer += "RELAY1 OFF";
		break;

		// RELAY 2
	case 'B': // on
		digitalWrite(RELAY2, LOW);
		_answer += "RELAY2 ON";
		break;
	case 'b': // off
		digitalWrite(RELAY2, HIGH);
		_answer += "RELAY2 OFF";
		break;

		// RELAY 3
	case 'C': // on
		digitalWrite(RELAY3, LOW);
		_answer += "RELAY3 ON";
		break;
	case 'c': // off
		digitalWrite(RELAY3, HIGH);
		_answer += "RELAY3 OFF";
		break;

		// RELAY 4
	case 'D': // on 
		digitalWrite(RELAY4, LOW);
		_answer += "RELAY4 ON";
		break;
	case 'd': // off
		digitalWrite(RELAY4, HIGH);
		_answer += "RELAY4 OFF";
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