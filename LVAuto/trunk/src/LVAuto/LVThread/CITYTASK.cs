using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using System.Collections;

namespace LVAuto.LVForm.LVThread {
	public  class CITYTASK
	{
        public Thread InThread; public string threadID;
        public bool IsRun = false;
        public int Sleep = 60000;
        public CITYTASK()
		{
        }

		private void mainprocess()
		{
			//lock (LVAuto.Web.LVWeb.islock)
			{
				try
				{
					//Application.DoEvents();
					//lock (LVAuto.Web.LVWeb.ispause) {
					Hashtable att;
					if (LVAuto.LVWeb.LVClient.issendsms == true)
					{
						att = LVAuto.LVForm.Command.Common.GetCityAttack();
						ArrayList ctys = (ArrayList)att["infos"];
						int timeatt = 999999999;
						for (int i = 0; i < ctys.Count; i++)
						{
							//Application.DoEvents();
							ArrayList onecity = (ArrayList)ctys[i];
							ArrayList attak = (ArrayList)onecity[2];
							if (timeatt > int.Parse(attak[1].ToString()) && int.Parse(attak[1].ToString()) > 0)
							{
								timeatt = int.Parse(attak[1].ToString());
							}
						}
						if (timeatt != 999999999)
						{
							if (LVAuto.LVWeb.LVClient.lastsmstype == -1)
							{
								if (timeatt > 60 * 30) LVAuto.LVWeb.LVClient.lastsmstype = 1;
								if (timeatt > 60 * 5 && timeatt < 60 * 30) LVAuto.LVWeb.LVClient.lastsmstype = 2;
								if (timeatt < 60 * 5) LVAuto.LVWeb.LVClient.lastsmstype = 3;
							}
							else
							{
								if (timeatt > 60 * 30)
								{
									//neu chua bi danh
									if (LVAuto.LVWeb.LVClient.lastsmstype != 1)
									{
										LVAuto.LVWeb.LVClient.LoginMobi(
											LVAuto.LVWeb.LVClient.smsusername, LVAuto.LVWeb.LVClient.smspass,
											LVAuto.LVWeb.LVClient.smsto, "Quan dich dang danh nhung con lau moi toi"
										);

										//LVAuto.frmmain.LVFRMMAIN.lblmainmsg.Text= "Quan dich dang danh nhung con lau moi toi";
									}
									LVAuto.LVWeb.LVClient.lastsmstype = 1;
								}
								if (timeatt > 60 * 5 && timeatt < 60 * 30)
								{
									if (LVAuto.LVWeb.LVClient.lastsmstype != 2)
									{
										LVAuto.LVWeb.LVClient.LoginMobi(
											LVAuto.LVWeb.LVClient.smsusername, LVAuto.LVWeb.LVClient.smspass,
											LVAuto.LVWeb.LVClient.smsto, "Quan dich dang danh nhung con 30 phut nua moi toi"
										);
										//LVAuto.frmmain.LVFRMMAIN.lblmainmsg.Text = "Quan dich dang danh nhung con 30 phut nua moi toi";
									}
									LVAuto.LVWeb.LVClient.lastsmstype = 2;
								}
								if (timeatt < 60 * 5)
								{
									if (LVAuto.LVWeb.LVClient.lastsmstype != 3)
									{
										LVAuto.LVWeb.LVClient.LoginMobi(
											LVAuto.LVWeb.LVClient.smsusername, LVAuto.LVWeb.LVClient.smspass,
											LVAuto.LVWeb.LVClient.smsto, "Quan dich con cach 5 phut di duong. AAAA"
										);

										//LVAuto.frmmain.LVFRMMAIN.lblmainmsg.Text = "Quan dich con cach 5 phut di duong. AAAA";

									}
									LVAuto.LVWeb.LVClient.lastsmstype = 3;
								}
							}
						}
						else
						{
							LVAuto.LVWeb.LVClient.lastsmstype = 0;
						}
					}

					// lay thong tin moi nhat cua city va sap xep lai

					//LVAuto.Command.City.UpdateAllSimpleCity();
					///for (int citypost = 0; citypost < LVAuto.Command.CityObj.City.AllCity.Length; citypost++)
					//{
						//	LVAuto.Command.City.UpdateAllBuilding(citypost);
						//LVAuto.Command.Common.GetGeneral(citypost, false);
					//}

					/*
						foreach (LVAuto.Command.CityObj.City cty in LVAuto.Command.CityObj.City.AllCity) 
						{
							try {
								//Application.DoEvents();
								LVAuto.Command.City.SwitchCitySlow(cty.id);
								Hashtable task = Command.City.GetCityTask(LVAuto.Web.LVWeb.CurrentLoginInfo.MakeCookiesString(cty.id));
								//Hashtable attack = Command.Common.GetCityAttack(cty.id,LVAuto.Web.LVWeb.CurrentLoginInfo.MakeCookiesString(cty.id));
                                
								// so luong cong tring dang xay
								int countbuild = ((ArrayList)((ArrayList)((ArrayList)task["list"])[0])[4]).Count;
                                
								
								//upgarade
								int tech = int.Parse(((ArrayList)task["tech"])[2].ToString());
                                
								// thao phat
								int task1 = int.Parse(((ArrayList)task["task"])[3].ToString());
								//int mcb = int.Parse(((ArrayList)((ArrayList)((ArrayList)attack["infos"])[0])[2])[1].ToString());
								cty.citytask = new LVAuto.Command.CityObj.CityTask(countbuild, tech, task1, 0);
							} catch (Exception ex) {
								cty.citytask = new LVAuto.Command.CityObj.CityTask(0, 0, 0, 0);
							}
						}
					 */

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
				threadID = "CITYTASK_" + DateTime.Now.Ticks;
				LVCommon.ThreadManager.TakeResourceAndRun(threadID, mainprocess); 
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
        public void Stop() {
            if (IsRun) {
                InThread.Abort();
                InThread.Join();  LVCommon.ThreadManager.RemoveThread(threadID);
                IsRun = false;
            }
        }
    }
}
