//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Switch driver for EQSwitch
//
// Description:	An ASCOM Driver for EQSwitch
//
// Implements:	ASCOM Switch interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	            Vers	Description
// -----------	---------------	-----	-------------------------------------------------------
// 19-10-2016	Everett Quebral	6.2.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
#define Switch

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.IO.Ports;

namespace ASCOM.EQSwitch
{
    //
    // Your driver's DeviceID is ASCOM.EQSwitch.Switch
    //
    // The Guid attribute sets the CLSID for ASCOM.EQSwitch.Switch
    // The ClassInterface/None addribute prevents an empty interface called
    // _EQSwitch from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Switch Driver for EQSwitch.
    /// </summary>
    [Guid("2627d1fd-e105-41c7-b4e2-8fe31d635dff")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switch : ISwitchV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.EQSwitch.Switch";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM Switch Driver for EQSwitch.";

        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string showUIProfileName = "Show Controller";
        internal static string comPortDefault = "COM1";
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";
        internal static string showUIDefault = "true";

        internal static string portOneName = "Telescope";
        internal static string portTwoName = "USB HUB";
        internal static string portThreeName = "Camera Cooler";
        internal static string portFourName = "Focuser";

        internal static string[] switchNames = new string[] { portOneName, portTwoName, portThreeName, portFourName };
        internal static Boolean[] switchStates = new Boolean[] { false, false, false, false };

        internal static string comPort; // Variables to hold the currrent device configuration
        internal static bool traceState;
        internal static bool showUI;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        private TraceLogger tl;

        private MainWindow mainWindow;

        private SerialPort serialPort;

        public event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="EQSwitch"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Switch()
        {
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl = new TraceLogger("", "EQSwitch");
            tl.Enabled = traceState;
            tl.LogMessage("Switch", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object
            //TODO: Implement your additional construction here

            tl.LogMessage("Switch", "Completed initialisation");
        }

        private string message;
        //
        // PUBLIC COM INTERFACE ISwitchV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected) {
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");
            }
               
            else
            {
                using (SetupDialogForm F = new SetupDialogForm(this.DriverInfo))
                {
                    var result = F.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                    }
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            CheckConnected("Action");
            if (IsConnected)
            {
                serialPort.WriteLine(actionName);

                // we need to set the state here

            }
            return actionName;
        }

        public void CommandBlind(string command, bool raw = false)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            this.CommandString(command, raw);
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
            // DO NOT have both these sections!  One or the other
        }

        public bool CommandBool(string command, bool raw = false)
        {
            CheckConnected("CommandBool");
            string ret = CommandString(command, raw);
            // TODO decode the return string and return true or false
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBool");
            // DO NOT have both these sections!  One or the other
        }

        public string CommandString(string command, bool raw = false)
        {
            CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public virtual void OnSwitchStateChanged(SwitchStateChangedEventArgs e)
        {
            if (SwitchStateChanged != null)
            {
                SwitchStateChanged(this, e);
                SetSwitch((Int16)(e.switchNumber), e.state);
            }
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            message = SerialPort.ReadTo("#");
            Debug.WriteLine(message);
            tl.LogMessage("EQSwitch", message);

            if (message.Contains("STATUS"))
            {
                string status = message.Split(':')[1];
                if (status.Contains("A")) OnSwitchStateChanged(new SwitchStateChangedEventArgs(1, true));
                if (status.Contains("a")) OnSwitchStateChanged(new SwitchStateChangedEventArgs(1, false));
                if (status.Contains("B")) OnSwitchStateChanged(new SwitchStateChangedEventArgs(2, true));
                if (status.Contains("b")) OnSwitchStateChanged(new SwitchStateChangedEventArgs(2, false));
                if (status.Contains("C")) OnSwitchStateChanged(new SwitchStateChangedEventArgs(3, true));
                if (status.Contains("c")) OnSwitchStateChanged(new SwitchStateChangedEventArgs(3, false));
                if (status.Contains("D")) OnSwitchStateChanged(new SwitchStateChangedEventArgs(4, true));
                if (status.Contains("d")) OnSwitchStateChanged(new SwitchStateChangedEventArgs(4, false));
            }
        }

        public bool Connected
        {
            get
            {
                tl.LogMessage("Connected Get", IsConnected.ToString());
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected Set", value.ToString());

                ReadProfile();

                if (value == IsConnected)
                {
                    if (!mainWindow.Visible && showUI)
                    {
                        mainWindow = new MainWindow(this);
                        mainWindow.Show();
                    }
                    return;
                }
                    

                if (value)
                {
                    connectedState = true;
                    tl.LogMessage("Connected Set", "Connecting to port " + comPort);
                    // TODO connect to the 
                    serialPort = new SerialPort(comPort, 115200);
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(this.serialPort_DataReceived);
                    serialPort.Open();

                    if (showUI)
                    {
                        mainWindow = new MainWindow(this);
                        mainWindow.Show();
                    }

                }
                else
                {
                    connectedState = false;
                    tl.LogMessage("Connected Set", "Disconnecting from port " + comPort);
                    // TODO disconnect from the device

                    serialPort.Close();
                }
            }
        }

        public SerialPort SerialPort
        {
            get
            {
                return serialPort;
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                tl.LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                string name = "EQ Switch Controller";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        public string PortOneName
        {
            get
            {
                return portOneName;
            }
        }

        public string PortTwoName
        {
            get
            {
                return portTwoName;
            }
        }

        public string PortThreeName
        {
            get
            {
                return portThreeName;
            }
        }

        public string PortFourName
        {
            get
            {
                return portFourName;
            }
        }

        #endregion

        #region ISwitchV2 Implementation

        private short numSwitch = 4;

        /// <summary>
        /// The number of switches managed by this driver
        /// </summary>
        public short MaxSwitch
        {
            get
            {
                tl.LogMessage("MaxSwitch Get", numSwitch.ToString());
                return this.numSwitch;
            }
        }

        /// <summary>
        /// Return the name of switch n
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// The name of the switch
        /// </returns>
        public string GetSwitchName(short id)
        {
            Validate("GetSwitchName", id);
            return switchNames[id];
            //tl.LogMessage("GetSwitchName", string.Format("GetSwitchName({0}) - not implemented", id));
            //throw new MethodNotImplementedException("GetSwitchName");
            // or
            //tl.LogMessage("GetSwitchName", string.Format("GetSwitchName({0}) - default Switch{0}", id));
            //return "Switch" + id.ToString();
        }

        /// <summary>
        /// Sets a switch name to a specified value
        /// </summary>
        /// <param name="id">The number of the switch whose name is to be set</param>
        /// <param name="name">The name of the switch</param>
        public void SetSwitchName(short id, string name)
        {
            Validate("SetSwitchName", id);
            switchNames[id] = name;
            //tl.LogMessage("SetSwitchName", string.Format("SetSwitchName({0}) = {1} - not implemented", id, name));
            //throw new MethodNotImplementedException("SetSwitchName");
        }

        /// <summary>
        /// Gets the switch description.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public string GetSwitchDescription(short id)
        {
            Validate("GetSwitchDescription", id);
            return switchNames[id];
            //tl.LogMessage("GetSwitchDescription", string.Format("GetSwitchDescription({0}) - not implemented", id));
            //throw new MethodNotImplementedException("GetSwitchDescription");
        }

        /// <summary>
        /// Reports if the specified switch can be written to.
        /// This is false if the switch cannot be written to, for example a limit switch or a sensor.
        /// The default is true.
        /// </summary>
        /// <param name="id">The number of the switch whose write state is to be returned</param><returns>
        ///   <c>true</c> if the switch can be written to, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public bool CanWrite(short id)
        {
            Validate("CanWrite", id);
            // default behavour is to report true
            tl.LogMessage("CanWrite", string.Format("CanWrite({0}) - default true", id));
            return true;
            // implementation should report the correct behaviour
            //tl.LogMessage("CanWrite", string.Format("CanWrite({0}) - not implemented", id));
            //throw new MethodNotImplementedException("CanWrite");
        }

        #region boolean switch members

        /// <summary>
        /// Return the state of switch n
        /// a multi-value switch must throw a not implemented exception
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// True or false
        /// </returns>
        public bool GetSwitch(short id)
        {
            Validate("GetSwitch", id);
            return switchStates[id - 1];
            //tl.LogMessage("GetSwitch", string.Format("GetSwitch({0}) - not implemented", id));
            //throw new MethodNotImplementedException("GetSwitch");
        }

        /// <summary>
        /// Sets a switch to the specified state
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// A multi-value switch must throw a not implemented exception
        /// setting it to false will set it to its minimum value.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void SetSwitch(short id, bool state)
        {
            Validate("SetSwitch", id);
            switchStates[id - 1] = state;
            //if (!CanWrite(id))
            //{
            //    var str = string.Format("SetSwitch({0}) - Cannot Write", id);
            //    tl.LogMessage("SetSwitch", str);
            //    throw new MethodNotImplementedException(str);
            //}
            //tl.LogMessage("SetSwitch", string.Format("SetSwitch({0}) = {1} - not implemented", id, state));
            //throw new MethodNotImplementedException("SetSwitch");
        }

        #endregion

        #region analogue members

        /// <summary>
        /// returns the maximum value for this switch
        /// boolean switches must return 1.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MaxSwitchValue(short id)
        {
            Validate("MaxSwitchValue", id);
            // boolean switch implementation:
            return 1;
            // or
            //tl.LogMessage("MaxSwitchValue", string.Format("MaxSwitchValue({0}) - not implemented", id));
            //throw new MethodNotImplementedException("MaxSwitchValue");
        }

        /// <summary>
        /// returns the minimum value for this switch
        /// boolean switches must return 0.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MinSwitchValue(short id)
        {
            Validate("MinSwitchValue", id);
            // boolean switch implementation:
            return 0;
            // or
            //tl.LogMessage("MinSwitchValue", string.Format("MinSwitchValue({0}) - not implemented", id));
            //throw new MethodNotImplementedException("MinSwitchValue");
        }

        /// <summary>
        /// returns the step size that this switch supports. This gives the difference between
        /// successive values of the switch.
        /// The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
        /// boolean switches must return 1.0, giving two states.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double SwitchStep(short id)
        {
            Validate("SwitchStep", id);
            // boolean switch implementation:
            return 1;
            // or
            //tl.LogMessage("SwitchStep", string.Format("SwitchStep({0}) - not implemented", id));
            //throw new MethodNotImplementedException("SwitchStep");
        }

        /// <summary>
        /// returns the analogue switch value for switch id
        /// boolean switches must throw a not implemented exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetSwitchValue(short id)
        {
            Validate("GetSwitchValue", id);
            tl.LogMessage("GetSwitchValue", string.Format("GetSwitchValue({0}) - not implemented", id));
            throw new MethodNotImplementedException("GetSwitchValue");
        }

        /// <summary>
        /// set the analogue value for this switch.
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// If the value is not between the maximum and minimum then throws an InvalidValueException
        /// boolean switches must throw a not implemented exception.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetSwitchValue(short id, double value)
        {
            Validate("SetSwitchValue", id, value);
            if (!CanWrite(id))
            {
                tl.LogMessage("SetSwitchValue", string.Format("SetSwitchValue({0}) - Cannot write", id));
                throw new ASCOM.MethodNotImplementedException(string.Format("SetSwitchValue({0}) - Cannot write", id));
            }
            tl.LogMessage("SetSwitchValue", string.Format("SetSwitchValue({0}) = {1} - not implemented", id, value));
            throw new MethodNotImplementedException("SetSwitchValue");
        }

        #endregion
        #endregion

        #region private methods

        /// <summary>
        /// Checks that the switch id is in range and throws an InvalidValueException if it isn't
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="id">The id.</param>
        private void Validate(string message, short id)
        {
            if (id < 0 || id > numSwitch)
            {
                tl.LogMessage(message, string.Format("Switch {0} not available, range is 0 to {1}", id, numSwitch - 1));
                throw new InvalidValueException(message, id.ToString(), string.Format("0 to {0}", numSwitch - 1));
            }
        }

        /// <summary>
        /// Checks that the switch id and value are in range and throws an
        /// InvalidValueException if they are not.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        private void Validate(string message, short id, double value)
        {
            Validate(message, id);
            var min = MinSwitchValue(id);
            var max = MaxSwitchValue(id);
            if (value < min || value > max)
            {
                tl.LogMessage(message, string.Format("Value {1} for Switch {0} is out of the allowed range {2} to {3}", id, value, min, max));
                throw new InvalidValueException(message, value.ToString(), string.Format("Switch({0}) range {1} to {2}", id, min, max));
            }
        }

        /// <summary>
        /// Checks that the number of states for the switch is correct and throws a methodNotImplemented exception if not.
        /// Boolean switches must have 2 states and multi-value switches more than 2.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="expectBoolean"></param>
        //private void Validate(string message, short id, bool expectBoolean)
        //{
        //    Validate(message, id);
        //    var ns = (int)(((MaxSwitchValue(id) - MinSwitchValue(id)) / SwitchStep(id)) + 1);
        //    if ((expectBoolean && ns != 2) || (!expectBoolean && ns <= 2))
        //    {
        //        tl.LogMessage(message, string.Format("Switch {0} has the wriong number of states", id, ns));
        //        throw new MethodNotImplementedException(string.Format("{0}({1})", message, id));
        //    }
        //}

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Switch";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Switch";
                traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
                showUI = Convert.ToBoolean(driverProfile.GetValue(driverID, showUIProfileName, string.Empty, showUIDefault));
                portOneName = driverProfile.GetValue(driverID, portOneName, string.Empty, portOneName);
                portTwoName = driverProfile.GetValue(driverID, portTwoName, string.Empty, portTwoName);
                portThreeName = driverProfile.GetValue(driverID, portThreeName, string.Empty, portThreeName);
                portFourName = driverProfile.GetValue(driverID, portFourName, string.Empty, portFourName);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Switch";
                driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
                driverProfile.WriteValue(driverID, showUIProfileName, showUI.ToString());
                driverProfile.WriteValue(driverID, portOneName, portOneName);
                driverProfile.WriteValue(driverID, portTwoName, portTwoName);
                driverProfile.WriteValue(driverID, portThreeName, portThreeName);
                driverProfile.WriteValue(driverID, portFourName, portFourName);
            }
        }

        #endregion

    }
}
