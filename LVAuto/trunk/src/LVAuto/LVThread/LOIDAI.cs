using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace LVAuto.LVForm.LVThread
{
	public class LOIDAI
	{
		delegate void SetTextCallback(string text);
		public Thread InThread; public string threadID;
		public bool IsRun = false;
		private int Sleep = 45*60*1000;		//45p moi check 1 lan
		private ArrayList ListTuong;
		public Label Message;
		public LOIDAI(Label lbl) 
		{
		 Message = lbl;
		}
	
		public void GetParameter(ArrayList listTuong, int sleep)
		{
			if (IsRun == false)
			{
				this.ListTuong = listTuong;
				this.Sleep = sleep;
			}
		}

		private void mainprocess()
		{
			//lock (LVAuto.Web.LVWeb.islock)
			{
				LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong g;
				int cityid;
				int genid;
				int level;
				string cookies;
				string para;
				Hashtable ret;

				try
				{
					Message.ForeColor = System.Drawing.Color.Red; 
					bool found1 = false;

					DiLoiDai(ListTuong, 1);	// dieu phai quan van di loi dai
					DiLoiDai(ListTuong, 2); // dieu phai quan vo di loi dai

					/*

					for (int i = 0; i < ListTuong.Count; i++)
					{
						SetText("Đang chạy (" + (i) * 100 / ListTuong.Count + "%)");

						g = ((LVAuto.Command.CommonObj.DieuPhaiTuong)ListTuong[i]);
							
						if (g.tasktype == LVAuto.Command.CommonObj.DieuPhaiTuong.TASKTYPE.LoiDai)
						{
							try
							{
								cityid = g.cityID;
								genid = g.generalid;
								level = g.loidailevel;

								cookies = LVAuto.Web.LVWeb.CurrentLoginInfo.MakeCookiesString(cityid);
								LVAuto.Command.City.SwitchCitySlow(cityid);

								para = "lCityTentID=" + cityid + "&lGeneralID=" + genid + "&lLevel=" + level;
						SetText("Chờ tới phiên (0%) - chờ từ lúc " + DateTime.Now.ToString("HH:mm:ss"));		ret = Command.OPT.Execute(21, para, false);
								if (ret != null && ret["DATA"].ToString() == "{\"ret\":0}")
								{
									// thanh cong

								}
							}
							catch (Exception ex) { }
						}
					}						
					*/
				}
				catch (Exception ex)
				{
				}
				//}
			}
		}
		public void run()
		{
			IsRun = true;
			
			while (true)
			{
				SetText("Chờ tới phiên (0%) - chờ từ lúc " + DateTime.Now.ToString("HH:mm:ss"));
				threadID = "LOIDAI_" + DateTime.Now.Ticks;
				LVCommon.ThreadManager.TakeResourceAndRun(threadID, mainprocess);
				Message.ForeColor = System.Drawing.Color.Blue; 
				SetText("Đang ngủ " + Sleep/(1000*60) + " phút, chờ tí (mới chạy lúc: " + DateTime.Now.ToString("HH:mm:ss") + ")");
				Thread.Sleep(Sleep);

			}
		}

		// Loailoidai: 1: van, 2:vo
		private void DiLoiDai(ArrayList danhsachtuong, int loailoidai)
		{
			LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong g=null;
			bool foundG = false;
			int cityid = 0;
			int citypost = -1;
			int buildingpost;
			int pid;
			int gid=0;
			string para;
			Hashtable result = null;
			string cookies; 
			int count = 0;
			ArrayList temp;
			long[] time = new long[3]{0,0,0};

			try
			{
				cityid = 0;
				for (int i = 0; i < ListTuong.Count; i++)
				{
					g = ((LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong)ListTuong[i]);
					if (g.tasktype == LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong.TASKTYPE.LoiDai && g.generaltype == loailoidai)
					{
						cityid = g.cityID;
						break;
					}
				}

				// khong co thang nao ca
				if (cityid == 0) return;


				if (loailoidai == 1)				// van
					gid = 9;			// tu hien cac
				else
					gid = 10;				// diem tuong dai

				
				// lay thanh cos diem tuong dai hoacj tu hien cac
				citypost = Command.City.GetCityPostByGID(gid, false);
				if (citypost == -1) return;

				cityid = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].Id;
			
				buildingpost = LVAuto.LVForm.Command.City.GetBuildingPostById(citypost, gid);
				if (buildingpost == -1) return;

				pid = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].AllBuildings[buildingpost].PId;

				cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityid);
				LVAuto.LVForm.Command.City.SwitchCitySlow(cityid);


				// lay thong tin loi dai
				//{"ret":0,"current":[313841,"Ð? D?*",3],"arena":[[1,4,412,1248244800,16],[2,0,5212,1248249600,141],[3
				//,0,12412,1248256800,11]]}			
				//{"ret":0,"current":[0,"",0],"arena":[[1,4,354,1248244800,16],[2,0,5154,1248249600,141],[3,0,12354,1248256800
				//,10]]}
				// arena: [0]: cap loi dai, [1]:hiep thi dau, [2]:time con lai (s)		; [3]: ;  [4]: so nguoi thi dau 


				para = "gid=" + gid + "&pid=" + pid + "&tab=2&tid=0";
				para = "gid=" + gid + "&pid=-1&tab=2&tid=" + cityid;
				result = Command.Build.Execute(2, para, true, cookies);


				if (result == null) return;

				temp = (ArrayList)result["current"];

				// da co tuogn di loi dai
				if (int.Parse(temp[0].ToString()) != 0) return;

				temp = (ArrayList)result["arena"];

				for (int i = 0; i < 3; i++)
				{
					time[i] = long.Parse((((ArrayList)temp[i])[2]).ToString());		// lay time con lai
					if (long.Parse((((ArrayList)temp[i])[1]).ToString()) != 0) time[i] = 0;
				}
				long[] level = new long[3] { 1, 2, 3 };

				// sap xep
				long tmp;
				for (int i = 0; i < 3; i++)
					for (int j = i + 1; j < 3; j++)
						if (time[i] > time[j])
						{
							tmp = time[i];
							time[i] = time[j];
							time[j] = tmp;

							tmp = level[i];
							level[i] = level[j];
							level[j] = tmp;
						}


				// dieu di loi dai
				cityid = 0;
				for (int i = 0; i < 3; i++)
				{
					if (time[i] == 0) continue;		// leve nay dang thi dau mat roi

					for (int j = 0; j < ListTuong.Count; j++)
					{
						g = ((LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong)ListTuong[j]);
						if (g.tasktype == LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong.TASKTYPE.LoiDai
								&& g.generaltype == loailoidai && g.loidailevel == level[i])
						{
							cityid = g.cityID;
							break;
						}
					}
				

					if (cityid == 0) continue;


					cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityid);
					LVAuto.LVForm.Command.City.SwitchCitySlow(cityid);
					SetText("Đang điều " + g.generalname + " từ " + g.cityname);

					para = "lCityTentID=" + g.cityID + "&lGeneralID=" + g.generalid + "&lLevel=" + g.loidailevel;
					result = Command.OPT.Execute(21, para, false);
					if (result != null && result["DATA"].ToString() == "{\"ret\":0}")
					{
						// thanh cong
						return;

					}
				}
			}
			catch (Exception ex)
			{

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
				Message.ForeColor = System.Drawing.Color.Blue ; Message.Text = "Đã dừng bởi người sử dụng";
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
