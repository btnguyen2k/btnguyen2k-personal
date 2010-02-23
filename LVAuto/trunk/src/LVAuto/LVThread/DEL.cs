using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace LVAuto.LVForm.LVThread {
    public class DEL {

		
        delegate void SetTextCallback(string text);
		public  Thread InThread;
		public string threadID;
		public  bool IsRun = false;
		//public  bool IsBuildAll = false;
		//public  string[] Cookies;
		//public  int[] Id;
		//public  string[] City;
		//public  bool[] Check;
		//public  string[][] Building;
		public  int Sleep = 60000;
		public  Label Message;

		public int mainProcessResult = 0;

		public Hashtable buildDown;
        
		public DEL(Label lbl) 
		{
            Message = lbl;
        }

		/* public   void GetParameter1(TreeView tvBUILD,int sleep) {
			 if (IsRun == false) {

				 IsBuildAll = false;
				 City = new string[50];
				 Id = new int[50];
				 Check = new bool[50];
				 Building = new string[50][];
				 Cookies = new string[50];
				 Sleep = sleep;
				 Xay dung cookies
				 for (int i = 0; i < LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length;i++) {
					 Cookies[i] = LVAuto.Web.LVWeb.CurrentLoginInfo.MakeCookiesString(LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].id);
					 Id[i] = LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].id;
				 }

				 //Xay dung mang de xay nha
				 TreeNode root = tvBUILD.Nodes["root"];
				 if(root.Checked)IsBuildAll=true;

				 for (int i = 0; i < root.Nodes.Count; i++) {
					 TreeNode tt = root.Nodes[i];
					 City[i] = tt.Name;
                    
					 Building[i] = new string[50];
					 for (int j = 0; j < tt.Nodes.Count; j++) {
						 if (tt.Nodes[j].Checked) Check[i] = true;
						 Building[i][j] = tt.Nodes[j].Checked.ToString() + "." + tt.Nodes[j].Name;
					 }
				 }
			 }
		 }
		  */ 
		public void GetParameter(TreeView tvBUILD, int sleep)
		{
			ArrayList arBuilding;


			if (IsRun == false)
			{
				buildDown = new Hashtable();

				
				//IsBuildAll = false;
				//City = new string[500];
				//Id = new int[500];
				//Check = new bool[500];
				
				//Building = new string[500][];		// [citypost][buildingpost]
				//Cookies = new string[500];
				Sleep = sleep;
				


				bool hasDown = false;

				for (int i = 0; i < LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
				{
					hasDown = false;
					if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings != null)
					{
						arBuilding = new ArrayList();
						for (int j = 0; j < LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings.Length; j++)
						{
							if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings[j].IsDown)
							{
								//arBuilding.Add(j);
								arBuilding.Add(LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings[j]);
								hasDown = true;
							}
						}

						if (hasDown) buildDown.Add(LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].Id, arBuilding);	//buildDown.Add(i, arBuilding);
					}
				}

			}
		}

		/** public  void run1()		 
		{
            IsRun = true;
            while (true) {
                SetText("Chờ tới phiên (0%) - chờ từ lúc " + DateTime.Now.ToString("HH:mm:ss"));
                lock (LVAuto.Web.LVWeb.islock) {
                    //lock (LVAuto.Web.LVWeb.ispause) {
                        try {
                            SetText("Đang chạy (0%)");
                            for (int i = 0; i < City.Length; i++) {
                                SetText("Đang chạy (" + (i) * 100 / 50 + "%)");
                                if(Check[i] && LVAuto.LVObj.City.Canbuild(Id[i])){
                                    try {
                                        Cookies[i] = LVAuto.Web.LVWeb.CurrentLoginInfo.MakeCookiesString(Id[i]);
                                        LVAuto.Command.City.SwitchCitySlow(Id[i]);
                                        if (City[i] != null) {
                                            int buildcount = 0;
                                            for (int j = 0; j < Building[i].Length; j++) {
                                                if (Building[i][j] != null) {
                                                    if (buildcount < 3) {
                                                        string[] info = Building[i][j].Split(new char[] { '.' });
                                                        if (info[0].ToLower() == "true") {
                                                            if (Command.Build.SelectBuilding(Id[i], int.Parse(info[2]), int.Parse(info[1]), 0, Cookies[i]) == false) {
                                                                for (int k = -2; k < 3; k++) {
                                                                    if (Command.Build.SelectBuilding(Id[i], int.Parse(info[2]), int.Parse(info[1]), k, Cookies[i])) {
                                                                        Command.OPT.Build(int.Parse(info[1]), int.Parse(info[2]), int.Parse(info[3]),2, Cookies[i]);
                                                                        break;
                                                                    };
                                                                }
                                                            } else {
                                                                Command.OPT.Build(int.Parse(info[1]), int.Parse(info[2]), int.Parse(info[3]),2, Cookies[i]);
                                                            }
                                                            buildcount++;
                                                        }
                                                    }
                                                }
                                            }
                                            //Nap lai danh sach nha
                                            Command.City.GetAllBuilding(i, Cookies[i]);

                                            string[] oldbulding = Building[i];
                                            Building[i] = new string[50];

                                            for (int j = 0; j < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuilding.Length; j++) {
                                                LVObj.Building onebuilding = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuilding[j];
                                                //Check thu da duoc check chua
                                                for (int k = 0; k < oldbulding.Length; k++) {
                                                    if (oldbulding[k] != null) {
                                                        string[] info = oldbulding[k].Split(new char[] { '.' }, 2);
                                                        if (info[1] == onebuilding.pid.ToString() + "." + onebuilding.gid.ToString() + "." + City[i]) {
                                                            Building[i][j] = oldbulding[k];
                                                        }
                                                    }
                                                }
                                                //có nhà mới
                                                if (Building[i][j] == null) {
                                                    Building[i][j] = "false." + onebuilding.pid.ToString() + "." + onebuilding.gid.ToString() + "." + City[i];
                                                }
                                            }
                                        }
                                    } catch (Exception ex1) {
                                    }
                                }
                            }
                        } catch (Exception ex) {
                            //MessageBox.Show(ex.ToString());
                        }
                    //}
                }
                SetText("Đang ngủ " + Sleep/(1000*60) + " phút, chờ tí (mới chạy lúc: " + DateTime.Now.ToString("HH:mm:ss") + ")");
                Thread.Sleep(Sleep);
            }
        }
		 */


		private void mainprocess()
		{
			//lock (LVAuto.Web.LVWeb.islock)
			{
				//lock (LVAuto.Web.LVWeb.ispause) {
				try
				{
					Message.ForeColor = System.Drawing.Color.Red; 
					int cityID;
					LVAuto.LVObj.City city;
					LVAuto.LVObj.Building building;
					bool canBuild = false;

					int[] arcityid = null;
					ArrayList arBuilding;
					string cookies;
					int maxbuild = 1;
					Hashtable hcitytask;
					ArrayList taskBuild;
					bool found;
					int citypos;
					bool isBuilding;
					ArrayList arTemp = null;

					SetText("Đang chạy (0%)");
					//LVAuto.Command.City.UpdateAllSimpleCity();

					if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.IsDowndAll)
					{
						buildDown.Clear();
						for (int i = 0; i < LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
						{
							Command.City.GetAllBuilding(i, false);

							if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings != null)
							{
								arBuilding = new ArrayList();
								for (int j = 0; j < LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings.Length; j++)
								{
									arBuilding.Add(LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings[j]);
								}

								buildDown.Add(LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].Id, arBuilding);
							}
						}
					}

					
					
					if (buildDown == null || buildDown.Count <= 0)
					{
						SetText("Chẳng có gì để chạy cả");
						return;
					}

					arcityid = new int[buildDown.Count];
					buildDown.Keys.CopyTo(arcityid, 0);

					// check xem duoc xay may cong trinh 1 luc--------
					maxbuild = LVAuto.LVForm.Command.Build.GetMaxNumCanBuild();

					int buildcount;
					for (int i = 0; i < arcityid.Length; i++)
					{
						try
						{
							cityID = arcityid[i];

							citypos = LVAuto.LVForm.Command.City.GetCityPostByID(cityID);
							//city = LVAuto.Command.City.GetCityByID(cityID);
							city = LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos];

							if (city == null) continue;
							SetText("Đang chạy: " + city.Name + " (đã hoàn thành " + i + "/" + arcityid.Length + " thành. )");

							LVAuto.LVForm.Command.City.SwitchCitySlow(cityID);

							//city.citytask = Command.City.GetCityTaskByCityID(cityID);
							LVAuto.LVForm.Command.City.UpdateAllBuilding(citypos);


							cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityID);
							buildcount = 0;
							arBuilding = (ArrayList)buildDown[arcityid[i]];


							hcitytask = Command.City.GetCityTask(cookies);
							if (hcitytask == null) continue;
							taskBuild = (ArrayList)hcitytask["list"];
							buildcount = ((ArrayList)(((ArrayList)taskBuild[0])[4])).Count;				// so luong cong trinh dang xay
							int oldbuildcount = buildcount;

							LVAuto.LVObj.Building buildingtmp;
							
							//for (int j = arBuilding.Count - 1; j >= 0; j--)
							for (int m = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings.Length - 1; m >= 0; m--)
							{


								// khoong lam gi neu da xay du con trinh
								if ((maxbuild - buildcount) <= 0) break;


								//building = (LVAuto.LVObj.Building)arBuilding[j];
								building = null;
								// tim nha co level cao nhat de ha
								for (int n = 0; n < arBuilding.Count; n++)
								{
									buildingtmp = (LVAuto.LVObj.Building)arBuilding[n];
									if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings[m].GId == buildingtmp.GId
										&& LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings[m].PId == buildingtmp.PId)
									{
										building = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings[m];
										break;
									}
								}

								if (building == null) continue;



								// check xem cong trinh co dang xay khong
								if (oldbuildcount > 0)
								{
									isBuilding = false;
									// ctrinh dang xay : gid= 3; pid = 18
									for (int k = 0; k < oldbuildcount; k++)
									{
										arTemp = (ArrayList)(((ArrayList)(((ArrayList)taskBuild[0])[4]))[k]);
										if (building.GId == int.Parse(arTemp[1].ToString())
												&& building.PId == int.Parse(arTemp[0].ToString()))
										{
											isBuilding = true;
											break;
										}
									}

									if (isBuilding) continue;

								}


								Hashtable result = null;

								//if (buildcount < 5)
								{
									//string[] info = Building[i][j].Split(new char[] { '.' });

									// "False.30.9.93917" = isbuild.pid.gid.parentid
									if (Command.Build.SelectBuilding(cityID, building.GId, building.PId, 0, cookies) == false)
									{
										for (int k = -2; k < 3; k++)
										{
											if (Command.Build.SelectBuilding(cityID, building.GId, building.PId, k, cookies))
											{
												result = Command.OPT.Build(building.PId, building.GId, cityID, 2, cookies);
												break;
											};
										}
									}
									else
									{
										result = Command.OPT.Build(building.PId, building.GId, cityID, 2, cookies);
									}


									if (result == null)  // fail
									{
										mainProcessResult = -1;
									}
									else
									{

										mainProcessResult = int.Parse(result["ret"].ToString());

										if (mainProcessResult == 0)   // thanh cong
										{
											buildcount++;
										}
										if (mainProcessResult > 1000000) // bi lock
										{
											return;
										}
									}

								}
							}   // end for building
							//Array.Sort((arBuilding.ToArray()));
						}						 
						catch (Exception ex)
						{
						}
					}		// end for city
				}
				catch (Exception ex)
				{
					//MessageBox.Show(ex.ToString());
				}
				//}
			}
		}
		public void run()
		{


			IsRun = true;
			
			while (true)
			{
				if (buildDown == null || buildDown.Count <= 0)
				{
					SetText( "Chẳng có gì để chạy cả");
					break;
				}
				SetText("Chờ tới phiên (0%) - chờ từ lúc " + DateTime.Now.ToString("HH:mm:ss"));
				threadID = "DEL_" + DateTime.Now.Ticks;
				LVCommon.ThreadManager.TakeResourceAndRun(threadID , mainprocess);
				Message.ForeColor = System.Drawing.Color.Blue; 
				SetText("Đang ngủ " + Sleep/(1000*60) + " phút, chờ tí (mới chạy lúc: " + DateTime.Now.ToString("HH:mm:ss") + ")");
				if (mainProcessResult > 1000000)
				{
					SetText("Bị khóa đến " + DateTime.Now.AddSeconds(mainProcessResult - 1000000).ToString("HH:mm:ss")
						+ ". Đang ngủ " + Sleep / (1000 * 60) + " phút, chờ tí (mới chạy lúc: " + DateTime.Now.ToString("HH:mm:ss") + ")");
				}

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
		public  void Stop()
		{
            if (IsRun) {
                InThread.Abort();
                InThread.Join();  LVCommon.ThreadManager.RemoveThread(threadID);
                Message.ForeColor = System.Drawing.Color.Blue ; Message.Text = "Đã dừng bởi người sử dụng";
                IsRun = false;
            }
        }
		public  void SetText(String str)
		{
            if (this.Message.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Message.Invoke(d, new object[] { str });
            } else {
                this.Message.Text = str;
            }
        }
		  
		
	}
		 
}
