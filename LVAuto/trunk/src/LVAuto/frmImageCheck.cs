using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace LVAuto.LVForm
{
	public partial class frmImageCheck : Form
	{
		public static int[] imgChoseID = new int[4];
		public static int returnCheckImgValue = 0;   //0: ok, right; -1: wrong, check again; x: thoi gian bi dong bang
		public static bool imageChecking = false;

		private Thread InThread;

		public frmImageCheck()
		{
			InitializeComponent();
		}

		private void frmImageCheck_Load(object sender, EventArgs e)
		{
			LVAuto.LVForm.FrmMain.imagecheckid = -1;
			timer1.Enabled = false;
			timer1.Interval = (2 *60 * 1000);  // se tat form trong 2phut
			timer1.Enabled = true;
			timer1.Start();

			this.Text = "Chọn hình không có - " + LVAuto.LVWeb.LVClient.lvusername;
			//InThread = new Thread(new ThreadStart(autoclose));
			//InThread.Start();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void frmImageCheck_FormClosed(object sender, FormClosedEventArgs e)
		{
			//LVAuto.frmImageCheck.imageChecking = false;
			//LVAuto.frmmain.LVFRMMAIN.startAllThread();

			//InThread.Abort();
			//InThread.Join();
		}

		private void pic1_Click(object sender, EventArgs e)
		{
			LVAuto.LVForm.FrmMain.imagecheckid = LVAuto.LVForm.frmImageCheck.imgChoseID[0];
			//LVAuto.Web.LVWeb.sendChoiceImageCheck(LVAuto.frmImageCheck.imgChoseID[0]);
			this.Close();
		}

		private void pic2_Click(object sender, EventArgs e)
		{
			LVAuto.LVForm.FrmMain.imagecheckid = LVAuto.LVForm.frmImageCheck.imgChoseID[1];

			this.Close();
			//LVAuto.Web.LVWeb.sendChoiceImageCheck(LVAuto.frmImageCheck.imgChoseID[1]);
		}

		private void pic3_Click(object sender, EventArgs e)
		{
			LVAuto.LVForm.FrmMain.imagecheckid = LVAuto.LVForm.frmImageCheck.imgChoseID[2];

			this.Close();
			//LVAuto.Web.LVWeb.sendChoiceImageCheck(LVAuto.frmImageCheck.imgChoseID[2]);
		}

		private void pic4_Click(object sender, EventArgs e)
		{
			LVAuto.LVForm.FrmMain.imagecheckid = LVAuto.LVForm.frmImageCheck.imgChoseID[3];

			this.Close();
			//LVAuto.Web.LVWeb.sendChoiceImageCheck(LVAuto.frmImageCheck.imgChoseID[3]);
		}


		private void autoclose()
		{
			try
			{
				System.Threading.Thread.Sleep(30000); // ngu 30s
				LVAuto.LVForm.FrmMain.imagecheckid = -1;

				this.Close();

			}
			catch (Exception ex)
			{
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();
			this.Close();
		}

	}
}