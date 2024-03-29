using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.Threading;
using System.Windows.Forms;
namespace LVAuto.LVForm.LVThread
{
	public class PHAIQUANVANDAOMO
	{
		delegate void SetTextCallback(string text);
		public Thread InThread; public string threadID;
		public bool IsRun = false;
		public int Sleep = 60000;
		private int X;
		private int Y;
		private int cityid;
		public Label Message;

		public PHAIQUANVANDAOMO(Label lbl) 
		 {
            Message = lbl;
        }
		public PHAIQUANVANDAOMO() 
		{
			Sleep = 10 * 60 * 1000; 
        }
		public PHAIQUANVANDAOMO(int time)
		{
			Sleep = time;
		}

		public void GetParameter(int cityid, int x, int y, int sleep)
		{
			if (IsRun == false)
			{
				this.cityid = cityid;
				this.X = x;
				this.Y = y;
				this.Sleep = sleep;
			}
		}

		

		public void run()
		{
			IsRun = true;
			while (true)
			{
				SetText("Chờ tới phiên (0%) - chờ từ lúc " + DateTime.Now.ToString("HH:mm:ss"));
				//lock (LVAuto.Web.LVWeb.islock)
				{
					//lock (LVAuto.Web.LVWeb.ispause) {
					try
					{


						//phaiquanvandidaomo();
						threadID = "PHAIQUANVANDIDAOMO_" + DateTime.Now.Ticks; 
						LVCommon.ThreadManager.TakeResourceAndRun(threadID, phaiquanvandidaomo); 


					}
					catch (Exception ex)
					{
					}
					//}
				}
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
				InThread.Join();
				LVCommon.ThreadManager.RemoveThread(threadID);
				Message.ForeColor = System.Drawing.Color.Blue ; Message.Text = "Đã dừng bởi người sử dụng";
				IsRun = false;
			}
		}


		private void phaiquanvandidaomo()
		{
			string para;
			Hashtable ret;
			try
			{
				//Message.ForeColor = System.Drawing.Color.Red; 
				
				//int cityid = 0;
				

				//if (!LVAuto.frmmain.QuanVanDaoMo.check) return;

				//cityid = LVAuto.frmmain.QuanVanDaoMo.cityid;
				//X = LVAuto.frmmain.QuanVanDaoMo.X;
				//Y = LVAuto.frmmain.QuanVanDaoMo.Y;

				//lay map id
				//http://s3.linhvuong.zooz.vn/GateWay/Common.ashx?id=72&0.24057125736018958&DestX=299&DestY=-111
				para = "DestX=" + X + "&DestY=" + Y;
				ret = LVAuto.LVForm.Command.Common.Execute(72, para, true);

				if (ret == null) return;
				int mapid = int.Parse(ret["map_id"].ToString());

				//get danh sach quan van
				//http://s3.linhvuong.zooz.vn/GateWay/Common.ashx?id=15&0.31696323126782144&lType=1&tid=13139

				string cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityid);

				para = "lType=1&tid=" + cityid;
				ret = LVAuto.LVForm.Command.Common.Execute(15, para, true, cookies);

				ArrayList gen = (ArrayList)ret["generals"];  // 3=0, 5=0, 13=0
				int genid = int.Parse(((ArrayList)gen[0])[0].ToString());

				// cu quan van di
				//http://s3.linhvuong.zooz.vn/GateWay/OPT.ashx?id=100&0.4721581634738514&DestMapID=1004086&HeroID=412545&tid=11697
				para = "DestMapID=" + mapid + "&HeroID=" + genid + "&tid=" + cityid;
				ret = LVAuto.LVForm.Command.OPT.Execute(100, para, true, cookies);
				int iii;
			}
			catch (Exception ex)
			{

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
