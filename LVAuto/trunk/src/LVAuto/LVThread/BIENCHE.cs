using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LVAuto.LVForm.LVThread
{
	public class BIENCHE
	{
		delegate void SetTextCallback(string text);
        public Thread InThread; public string threadID;
		public Label Message;
        public bool IsRun = false;
		public int Sleep = 60000;
		public int addsikhi = 10;

		public BIENCHE(Label lbl) 
		{
            Message = lbl;
        }
        public void GetParameter(int addsikhi, int sleep) 
		{
            if (IsRun == false) 
			{
				this.Sleep = sleep;
				this.addsikhi = addsikhi;
			}
		}

		private void mainprocess()
		{
			//lock (LVAuto.Web.LVWeb.islock)
			{
				
				try
				{
					Message.ForeColor = System.Drawing.Color.Red; 
					SetText("Đang chạy..... ");
					LVCommon.BienCheQuan.BienChe(LVAuto.LVForm.FrmMain.ListBienChe, addsikhi);

				}
				catch (Exception ex)
				{
				}
			}
		}
	   public void run() 
	   {
            IsRun = true;
            while (true) 
			{
				SetText("Chờ tới phiên (0%) - chờ từ lúc " + DateTime.Now.ToString("HH:mm:ss"));
				threadID = "BIENCHE_" + DateTime.Now.Ticks; 
				LVCommon.ThreadManager.TakeResourceAndRun(threadID, mainprocess);
				Message.ForeColor = System.Drawing.Color.Blue; 
				SetText("Đang ngủ " + Sleep/(1000*60) + " phút, chờ tí (mới chạy lúc: " + DateTime.Now.ToString("HH:mm:ss") + ")");
				Thread.Sleep(Sleep);
			}
		}




	   public void Auto()
	   {
		   if (!IsRun)
		   {
			   InThread = new Thread(new ThreadStart(run));
			   IsRun = true;  InThread.Start();
		   }
	   }
		public void Stop()
		{
			if (IsRun)
			{
				InThread.Abort();
				InThread.Join();  LVCommon.ThreadManager.RemoveThread(threadID);
				Message.ForeColor = System.Drawing.Color.Blue ; Message.ForeColor = System.Drawing.Color.Blue ; Message.Text = "Đã dừng bởi người sử dụng";
				IsRun = false;
			}
		}
		private void SetText(String str)
		{
			if (this.Message.InvokeRequired)
			{
				SetTextCallback d = new SetTextCallback(SetText);
				this.Message.Invoke(d, new object[] { str });
			}
			else
			{
				this.Message.Text = str;
			}
		}

	}
}
