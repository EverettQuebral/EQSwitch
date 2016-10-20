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
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
