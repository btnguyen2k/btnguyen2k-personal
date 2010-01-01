using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace LVAuto.LVForm.LVThread
{
	public class AUTOTASK : ThreadSkeleton
	{		
		Hashtable result;
		private long count = 0;
		
		public AUTOTASK(Label lbl) 
		{
            Message = lbl;
			Sleep = 60 * 1000;	// 1p chay 1 lan
        }

		private void mainprocess()
		{
			//lock (LVAuto.Web.LVWeb.islock)
			{
				try
				{
                    if (count == 1000000) count = 0;

					SetText("Auto task ... ");
					try
					{
						if (count == 0 || (count % 60 == 0))		// 60p chayj 1 lan
						{
							SetText("Updating all city ....");
							LVAuto.LVForm.Command.City.UpdateAllSimpleCity();
						}
					}
                    catch (Exception ex){}

					
					SetText("Updating danh sách tướng ....");
					try
					{
						if (count == 0 || (count % 60 == 0))		// 60p chayj 1 lan
						{
							LVAuto.LVForm.Command.Common.GetAllSimpleMilitaryGeneralInfoIntoCity();
						}
					}
					catch (Exception ex) { }

					
					try
					{
					// Linh bong loc
                        if (DateTime.Now.Hour < 2)          // từ 0h-2h sáng
                        {
                            result = Command.Common.Execute(47, "", false);
                            //Hashtable t = (Hashtable)JSON.JSON.JsonDecode(response["DATA"].ToString());

                           
                            if (result != null)
                            {
                                string str = result["DATA"].ToString();
                                str = str.Replace("\"", "");

                                str = str.Substring(str.IndexOf("has_got") + 8, 1);
                                if (str == "0")
                                {
                                    SetText("Đang lĩnh bổng lộc ....");
                                    result = Command.OPT.Execute(63, "", true);
                                }
                            }
                        }
                        else
						if (LVAuto.LVForm.Main.TuLinhBongLoc && ( count ==0 || (count % (60*6)  == 0)))	// 6h chay 1 lan
						{
				
							SetText("Đang lĩnh bổng lộc ....");
							result = Command.OPT.Execute(63, "", true);
						}
					}
					catch (Exception ex){}

					
					try
					{
					// tự ban thưởng
						if (LVAuto.LVForm.Main.TuBanThuongDoDe && ( count !=0 && (count % (60*48) ==0)) )		// 48h chay 1 lan
						{
							
							SetText("Đang ban thưởng cho đám đồ đệ....");
							//LVThread.BANTHUONG bt = new LVAuto.LVThread.BANTHUONG(false);
							//bt.Auto();
							string ok="", fail="";
							LVAuto.LVForm.Command.OPT.BanThuongAll(ref ok, ref fail);
							
						}
					}
					catch (Exception ex) { }


					try
					{

						if (count != 0 && count % 30 == 0)		// 60p chayj 1 lan
						{
							string cookies;
							int cityid;
							Hashtable citysimple;
							ArrayList aino;
							// an ui khi dan no
							if (LVAuto.LVForm.Main.TuBanAnUiKhiDanNo)
							{

								for (int citypost = 0; citypost < LVAuto.LVForm.Command.CityObj.City.AllCity.Length; citypost++)
								{

									if (LVAuto.LVForm.Command.CityObj.City.AllCity[citypost].id < 0) continue;
									SetText("Kiểm tra và an ủi thành " + LVAuto.LVForm.Command.CityObj.City.AllCity[citypost].name + " ....");
									cityid = LVAuto.LVForm.Command.CityObj.City.AllCity[citypost].id;
									cookies = LVAuto.LVForm.Web.LVWeb.CurrentLoginInfo.MakeCookiesString(cityid);
									LVAuto.LVForm.Command.City.SwitchCitySlow(cityid);
									citysimple = Command.City.GetCitySimpleInfo(cookies);
									
									if (citysimple == null) continue;

									aino = (ArrayList)citysimple["morale"];
									if (int.Parse(aino[2].ToString()) > 0)	  // dan no
									{
										Command.OPT.QuanPhuAnUi(3, cookies, true);	 // tế thiên
									}
								}
							}
						}
					}
					catch (Exception ex) { }

					//tu xóa nhắc nhở cả admin
					try{
						if (LVAuto.LVForm.Main.TuXoaNhacNhoCuaAdmin && count > 0 && (count % 24 == 0))  // 24h chay 1 lan
						{
							
							SetText("Xóa nhắc nhở admin ....");
							LVAuto.LVForm.Main.LVFRMMAIN.DeleteNhacNhoAdmin();
						}
					} catch (Exception ex){}
					
					count++;
					
					SetText("");
				}
				catch (Exception ex)
				{
				}
			}
		}
		public override void run()
		{
			//mainprocess();
            try
            {
                IsRun = true;
                while (true)
                {
                    threadID = "AUTOTASK_" + DateTime.Now.Ticks;
                    Common.ThreadManager.TakeResourceAndRun(threadID, mainprocess);

                    Thread.Sleep(Sleep);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                try
                {
                    Common.ThreadManager.s_alRegister.Remove(threadID);
                }
                catch (Exception ex) { }
            }
		}

		/*
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
				InThread.Join();  Common.ThreadManager.RemoveThread(threadID);
				IsRun = false;
			}
		}
		*/
	}
}
