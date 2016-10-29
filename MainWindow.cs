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
        }


        private void MainWindow_Load(object sender, EventArgs e)
        {
                       
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked){
                eqSwitch.Action("A", "");
            }
            else
            {
                eqSwitch.Action("a", "");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                eqSwitch.Action("B", "");
            }
            else
            {
                eqSwitch.Action("b", "");
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                eqSwitch.Action("C", "");
            }
            else
            {
                eqSwitch.Action("c", "");
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                eqSwitch.Action("D", "");
            }
            else
            {
                eqSwitch.Action("d", "");
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
