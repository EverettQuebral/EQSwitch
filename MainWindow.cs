using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.EQSwitch
{
    public partial class MainWindow : Form
    {
        ASCOM.EQSwitch.Switch eqSwitch;
        public MainWindow(Switch eqSwitch)
        {
            this.eqSwitch = eqSwitch;
            this.eqSwitch.SwitchStateChanged += SwitchStateChanged;
            InitializeComponent();

            checkBox1.Text = eqSwitch.PortOneName;
            checkBox2.Text = eqSwitch.PortTwoName;
            checkBox3.Text = eqSwitch.PortThreeName;
            checkBox4.Text = eqSwitch.PortFourName;
        }

        private void SwitchStateChanged(object sender, SwitchStateChangedEventArgs e)
        {
            switch (e.switchNumber)
            {
                case 1:
                    checkBox1.InvokeIfRequired(checkBox1 => { checkBox1.Checked = e.state; });
                    break;
                case 2:
                    checkBox2.InvokeIfRequired(checkBox2 => { checkBox2.Checked = e.state; });
                    break;
                case 3:
                    checkBox3.InvokeIfRequired(checkBox3 => { checkBox3.Checked = e.state; });
                    break;
                case 4:
                    checkBox4.InvokeIfRequired(checkBox4 => { checkBox4.Checked = e.state; });
                    break;
            }
            eqSwitch.SetSwitch((Int16)e.switchNumber, e.state);
        }


        private void MainWindow_Load(object sender, EventArgs e)
        {
                       
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked){
                eqSwitch.Action("A", "");
                eqSwitch.SetSwitch(1, true);
            }
            else
            {
                eqSwitch.Action("a", "");
                eqSwitch.SetSwitch(1, false);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                eqSwitch.Action("B", "");
                eqSwitch.SetSwitch(2, true);
            }
            else
            {
                eqSwitch.Action("b", "");
                eqSwitch.SetSwitch(2, false);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                eqSwitch.Action("C", "");
                eqSwitch.SetSwitch(3, true);
            }
            else
            {
                eqSwitch.Action("c", "");
                eqSwitch.SetSwitch(3, false);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                eqSwitch.Action("D", "");
                eqSwitch.SetSwitch(4, true);
            }
            else
            {
                eqSwitch.Action("d", "");
                eqSwitch.SetSwitch(4, false);
            }
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            eqSwitch.Action("Z", "");
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
