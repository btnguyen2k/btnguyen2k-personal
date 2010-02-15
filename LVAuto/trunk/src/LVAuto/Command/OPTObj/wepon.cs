using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LVAuto.LVForm.Command.OPTObj {
    public partial class Weapon : UserControl {
        public int cityid=0;
        public int posid_w = 0;
        public int posid_h = 0;
        public int posid_a = 0;
        public Weapon() 
		{
            InitializeComponent();
        }

		private void cboWepon_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
    }
}