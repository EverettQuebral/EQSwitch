using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.EQSwitch;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace ASCOM.EQSwitch
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm(string driverInfo)
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();

            label1.Text = "EQSwitch ASCOM Driver " + driverInfo;
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            Switch.comPort = (string)comboBoxComPort.SelectedItem;
            Switch.traceState = chkTrace.Checked;
            Switch.showUI = showUI.Checked;
            Switch.portOneName = txtBoxSwitch1.Text;
            Switch.portTwoName = txtBoxSwitch2.Text;
            Switch.portThreeName = txtBoxSwitch3.Text;
            Switch.portFourName = txtBoxSwitch4.Text;
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            chkTrace.Checked = Switch.traceState;
            // set the list of com ports to those that are currently available
            comboBoxComPort.Items.Clear();

            String[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                Debug.WriteLine("Port Here: " + port);
                if (DetectEQSwitch(port))
                {
                    comboBoxComPort.Items.Add(port);
                }
            }
            //comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            // select the current port if possible
            if (comboBoxComPort.Items.Contains(Switch.comPort))
            {
                comboBoxComPort.SelectedItem = Switch.comPort;
            }

            txtBoxSwitch1.Text = Switch.portOneName;
            txtBoxSwitch2.Text = Switch.portTwoName;
            txtBoxSwitch3.Text = Switch.portThreeName;
            txtBoxSwitch4.Text = Switch.portFourName;
        }

        private bool DetectEQSwitch(string portname)
        {
            SerialPort testPort = new SerialPort(portname, 115200);
            try
            {
                testPort.Open();
                Thread.Sleep(100);
                testPort.WriteLine("X");

                Thread.Sleep(300);
                string returnMessage = testPort.ReadExisting().ToString();
                testPort.Close();
                Debug.WriteLine(returnMessage);

                if (returnMessage.Contains("EQSwitch"))
                {
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("PORT NOT FOUND");
                    return false;
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitUI();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}