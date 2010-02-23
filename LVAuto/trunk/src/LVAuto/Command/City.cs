using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace LVAuto.LVForm.Command {
    public class City 
	{
		private static object CityIsLock = new Object();
		private static object BuildingIsLock = new Object();
		private static object GeneralIsLock = new Object();

		public static int GetCityPostByID(int cityid)
		{
            for (int i = 0; i < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
			{
                if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].Id == cityid) return i;
			}
			return -1;
		}

		public static LVObj.City GetCityByID(int cityid)
		{
            for (int i = 0; i < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
			{
                if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].Id == cityid)
                    return LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i];
			}
			return null;
		}

		/// <summary>Lấy vị trí thành (citypost) đầu tiên có chưa công trình gid</summary>
		/// <param name="gid">GID của công trình.</param>
		/// <param name="isall">True: tìm cả ở trại. False: không tìm ở trại.</param>
		/// <returns>Returns -1 nếu không thấy, nếu thấy trả về citypos.</returns>

		// ham nay tra ve city post dau tien co chua cong trinh gid, 
		// isall: true neu quet ca trai, neu khong chi quet thanh
		public static int GetCityPostByGID(int gid, bool isall)
		{
			if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null)
				GetAllSimpleCity();

			if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null) return -1;
			
			for (int i = 0; i < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
			{
				if (!isall && LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].Id < 0) continue;


				if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings == null)
					GetAllBuilding(i, true);

				if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings == null) continue;
				for (int j = 0; j < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings.Length; j++)
				{
					if (gid == LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings[j].GId)
						return i;
				}
			}
			return -1;
		}

        public static LVObj.MilitaryGeneral GetGeneralByID(int genaralID)
		{
			try
			{
				for (int i = 0; i < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
				{
					for (int j = 0; j < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].MilitaryGenerals.Length; j++)
					{
						if (genaralID == LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].MilitaryGenerals[j].Id) return LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].MilitaryGenerals[j];
					}
				}
				return null;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
        public static LVObj.MilitaryGeneral GetGeneralByID(int citypost, int genaralID)
		{
			try
			{
                for (int j = 0; j < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].MilitaryGenerals.Length; j++)
					{
                        if (genaralID == LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].MilitaryGenerals[j].Id) return LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].MilitaryGenerals[j];
					}
			
				return null;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

        public static LVObj.City GetCityInfo(int id) 
		{
			try
			{
				// get all city
                Hashtable result = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(7, "", true);
				ArrayList infos = (ArrayList)result["infos"];
				for (int i = 0; i < infos.Count; i++)
				{
					ArrayList city = (ArrayList)infos[i];
					if (id == int.Parse(city[1].ToString()))
					{
						return new LVAuto.LVObj.City(int.Parse(city[1].ToString()), city[2].ToString(), int.Parse(city[4].ToString()),
                                                           int.Parse(city[5].ToString()), int.Parse(city[7].ToString()), int.Parse(city[6].ToString()));
					}
				}
				return null;
			}
			catch (Exception ex)
			{
				return null;
			}
        }
        public static Hashtable GetCityTask(string cookies) 
		{
            Hashtable r = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(37, "ttid=0", true, cookies);
            //{"ret":0,"list":[[25547,"Âm Phong Thực Cốt",192433,4242485,[[1,3,29,1,45632],[12,3,30,1,313803],[22,3
            //,29,1,39808]],[],[],-1]],"tech":[7,23,62765],"task":[0,"",0,0]}
            return r;
        }

        public static LVObj.City GetCityInfo(int id, string cookies)
        {
			// get all city
            Hashtable result = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(7, "", true, cookies);
            ArrayList infos = (ArrayList)result["infos"];
            for (int i = 0; i < infos.Count; i++) {
                ArrayList city = (ArrayList)infos[i];
                if (id == int.Parse(city[1].ToString())) {
                    return new LVAuto.LVObj.City(int.Parse(city[1].ToString()), city[2].ToString(), int.Parse(city[4].ToString()),
                                                                            int.Parse(city[5].ToString()), int.Parse(city[7].ToString()));
                }
            }
            return null;
        }
        public static Hashtable GetResource() {
            return LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(20, "", true);
        }
        public static Hashtable GetResource(string cookies) 
		{
            return LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(20, "", true, cookies);
        }

		public static Hashtable GetCurentResourceInCity(string cookies) 
		{
			try
			{
				Hashtable ret = GetResource(cookies);
				Hashtable Result = new Hashtable();

				Result.Add("LUA", int.Parse(((ArrayList)ret["food"])[0].ToString()));
				Result.Add("GO", int.Parse(((ArrayList)ret["wood"])[0].ToString()));
				Result.Add("DA", int.Parse(((ArrayList)ret["stone"])[0].ToString()));
				Result.Add("SAT", int.Parse(((ArrayList)ret["iron"])[0].ToString()));
				Result.Add("MAXKHO", int.Parse(ret["max_storage"].ToString()));

				ret = Command.City.GetCitySimpleInfo(cookies);
				ArrayList goldarr = (ArrayList)ret["money"];
				long gold = int.Parse(goldarr[0].ToString());
				Result.Add("GOLD", gold);
				
				return Result;	


			}
			catch (Exception ex)
			{
				return null;
			}


		}

		public static void GetAllBuilding(int citypos, bool reload)
		{
			GetAllBuilding(citypos, reload, LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].Id));
			
		}
		public static void GetAllBuilding(int citypos, bool reload, string cookies)
		{
			lock (BuildingIsLock)
			{
				try
				{
					if (!reload && LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings != null) return;

					int count = 0;
					Hashtable temp = null;
					do
					{
                        temp = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(5, "", true, cookies);
						count++;
					} while (temp == null && count < 5);

					if (temp == null) return;

					ArrayList infosbuilding = (ArrayList)temp["infos"];
					//LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuilding = new LVAuto.LVObj.Building[infosbuilding.Count];

					LVAuto.LVObj.Building[] arNewBuilding = new LVAuto.LVObj.Building[infosbuilding.Count];

					ArrayList building;
					for (int j = 0; j < infosbuilding.Count; j++)
					{
						//System.Windows.Forms.Application.DoEvents();
						building = (ArrayList)infosbuilding[j];
						//LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuilding[j] = new LVAuto.LVObj.Building(int.Parse(building[0].ToString()), int.Parse(building[1].ToString()), building[2].ToString(), int.Parse(building[3].ToString()));
						arNewBuilding[j] = new LVAuto.LVObj.Building(int.Parse(building[0].ToString()), int.Parse(building[1].ToString()), building[2].ToString(), int.Parse(building[3].ToString()));
					}

					//Array.Sort(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuilding);
					Array.Sort(arNewBuilding);
					LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings = arNewBuilding;

					int k;
				}
				catch (Exception ex)
				{
				}
			}
		}

        public static void GetAllBuilding(int citypos, string cookies) 
		{
            Hashtable temp = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(5, "", true, cookies);
			//System.Windows.Forms.Application.DoEvents();
			
            ArrayList infosbuilding = (ArrayList)temp["infos"];
            //LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuilding = new LVAuto.LVObj.Building[infosbuilding.Count];
			LVAuto.LVObj.Building[] newBuilding = new LVAuto.LVObj.Building[infosbuilding.Count];

            for (int j = 0; j < infosbuilding.Count; j++) 
			{
				//System.Windows.Forms.Application.DoEvents();
                ArrayList building = (ArrayList)infosbuilding[j];
				
                //LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuilding[j] = new LVAuto.LVObj.Building(int.Parse(building[0].ToString()), int.Parse(building[1].ToString()), building[2].ToString(), int.Parse(building[3].ToString()));
				newBuilding[j] = new LVAuto.LVObj.Building(int.Parse(building[0].ToString()), int.Parse(building[1].ToString()), building[2].ToString(), int.Parse(building[3].ToString()));
 
			}
            //Array.Sort(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuilding);
			Array.Sort(newBuilding);
			LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings = newBuilding;

		}

		// cap nhat thong tin ve level cua Building, neu co building moi se add them vao
		public static void UpdateAllBuilding(int citypos) 
		{
			lock (BuildingIsLock)
			{
				try
				{
					if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings == null)
					{
						GetAllBuilding(citypos, true);
						return;
					}

                    Hashtable temp = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(5, "", true, LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].Id));

                    try
                    {
                        int size = int.Parse(temp["size"].ToString());
                    }
                    catch (Exception ex) { }

                    ArrayList infosbuilding = (ArrayList)temp["infos"];

					LVObj.Building[] newBuilding = new LVAuto.LVObj.Building[infosbuilding.Count];

					LVObj.Building oldBuilding;
					ArrayList building;
					for (int i = 0; i < infosbuilding.Count; i++)
					{
						building = (ArrayList)infosbuilding[i];
						newBuilding[i] = new LVAuto.LVObj.Building(int.Parse(building[0].ToString()), int.Parse(building[1].ToString()), building[2].ToString(), int.Parse(building[3].ToString()));
						if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings != null)
						{
							// tim xem da co chua
							for (int j = 0; j < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings.Length; j++)
							{
								oldBuilding = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings[j];

								if (newBuilding[i].GId == oldBuilding.GId && newBuilding[i].PId == oldBuilding.PId)
								{
									newBuilding[i].IsDown = oldBuilding.IsDown;
									newBuilding[i].IsUp = oldBuilding.IsUp;

									break;
								}
							}
						}
					}

					if (newBuilding != null) Array.Sort(newBuilding);

					LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings = newBuilding;
					//if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuilding != null) Array.Sort(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuilding);
				}
				catch (Exception ex)
				{
				}
			} //lock
		}

        public static void GetAllCity()
        {
            //{"ret":0,"infos":[[4,23910,"Prepare for War",185871,81,515,0],[2,-33311,"Prepare for Peace",162216,60
            //,533,23910],[1,63000,"Prepare for Sleep",4683,87,653,0],[2,-22087,"Đừng oánh tui",4678,82,653,63000]
            //]}
            Hashtable result = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(7, "", true);
            ArrayList infos = (ArrayList)result["infos"];
            LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities = new LVAuto.LVObj.City[infos.Count];


            for (int i = 0; i < infos.Count; i++)
            {
                ArrayList city = (ArrayList)infos[i];
                // public City(int id, string name, int x, int y)
                LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i] = new LVAuto.LVObj.City(int.Parse(city[1].ToString()), city[2].ToString(), 
                    int.Parse(city[4].ToString()), int.Parse(city[5].ToString()), int.Parse(city[7].ToString()));
                
                
                Command.City.SwitchCitySlow(int.Parse(city[1].ToString()));
                while (true)
                {
                    try
                    {
                        GetAllBuilding(i);
                        break;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                while (true)
                {
                    try
                    {
                        // laays thong tin ve tuong trong trai/thanh
                        Command.Common.GetGeneral(i);
                        break;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }


        // Lay thong tin co ban ve city
        public static void GetAllSimpleCity()
        {
            //{"ret":0,"infos":[[4,23910,"Prepare for War",185871,81,515,0],[2,-33311,"Prepare for Peace",162216,60
            //,533,23910],[1,63000,"Prepare for Sleep",4683,87,653,0],[2,-22087,"Đừng oánh tui",4678,82,653,63000]
            //]}

            int count = 0;
            Hashtable result = null;
            do
            {
                result = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(7, "", true);
                count++;
            } while (result == null && count < 5);

            ArrayList infos = (ArrayList)result["infos"];
            //LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities = new LVAuto.LVObj.City[infos.Count];

            LVObj.City[] newcity = new LVAuto.LVObj.City[infos.Count];

            ArrayList city;
            for (int i = 0; i < infos.Count; i++)
            {
                //System.Windows.Forms.Application.DoEvents();

                city = (ArrayList)infos[i];
                // public City(int id, string name, int x, int y)
                newcity[i] = new LVAuto.LVObj.City(int.Parse(city[1].ToString()), city[2].ToString(), int.Parse(city[4].ToString()), 
                                                                int.Parse(city[5].ToString()), int.Parse(city[7].ToString()), int.Parse(city[6].ToString()));
                


                Command.City.SwitchCitySlow(int.Parse(city[1].ToString()));
            }

            LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities = newcity;
        }


		public static void UpdateAllSimpleCity()
		{
			lock (CityIsLock)
			{

				if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null)
				{
					GetAllSimpleCity();
					return;
				}

				//{"ret":0,"infos":[[4,23910,"Prepare for War",185871,81,515,0],[2,-33311,"Prepare for Peace",162216,60
				//,533,23910],[1,63000,"Prepare for Sleep",4683,87,653,0],[2,-22087,"Đừng oánh tui",4678,82,653,63000]
				//]}
                Hashtable result = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(7, "", true);
				if (result == null) return;

				ArrayList infos = (ArrayList)result["infos"];

				LVObj.City[] newcity = new LVAuto.LVObj.City[infos.Count];
				LVObj.City oldcity;

				ArrayList city;
				string newname;
				for (int i = 0; i < infos.Count; i++)
				{
					//System.Windows.Forms.Application.DoEvents();

					city = (ArrayList)infos[i];
					// public City(int id, string name, int x, int y)
					newcity[i] = new LVAuto.LVObj.City(int.Parse(city[1].ToString()), city[2].ToString(), int.Parse(city[4].ToString()), 
                        int.Parse(city[5].ToString()), int.Parse(city[7].ToString()), int.Parse(city[6].ToString()));
                    
                    // cap thanh
                    newcity[i].Size = int.Parse(city[7].ToString());

                    oldcity = LVAuto.LVForm.Command.City.GetCityByID(newcity[i].Id);

                    if (oldcity != null)
                    {
                        newcity[i].AllBuildings = oldcity.AllBuildings; 
                    }

					
                    //Command.City.SwitchCitySlow(int.Parse(city[1].ToString()));
					
                    /*
                    for (int j = 0; j < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; j++)
					{
						oldcity = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[j];
						if (newcity[i].id == oldcity.id)
						{
							// thay doi moi ten, giu lai het cac thuoc tinh khac
							newname = newcity[i].name;
							newcity[i] = oldcity;
							newcity[i].name = newname;


							break;
						}
					}
                     */ 
				}

				LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities = newcity;
			}
		}


		public static int GetBuildingPostById(int citypost, int gid)
		{
			LVObj.Building building;

			if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null) return -1;
			if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].AllBuildings == null)
				LVAuto.LVForm.Command.City.UpdateAllBuilding(citypost);

			for (int i = 0; i < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].AllBuildings.Length; i++)
			{
				building = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].AllBuildings[i];
				if (building.GId == gid ) return i;
			}

			return -1;


		}

		public static int GetBuildingPostById(int citypost, int gid, int pid)
		{
			LVObj.Building building;

			if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null) return -1;
			if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].AllBuildings == null) return -1;

			for (int i = 0; i < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].AllBuildings.Length; i++)
			{
				building = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].AllBuildings[i];
				if (building.GId == gid && building.PId == pid) return i;
			}

			return -1;

		}
		public static void GetAllBuilding(int citypos)
		{
			//hungtv add
			//Command.City.SwitchCitySlow(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].id);
			//---
            Hashtable temp = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(5, "", true);
            ArrayList infosbuilding = (ArrayList)temp["infos"];
            LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings = new LVAuto.LVObj.Building[infosbuilding.Count];
            for (int j = 0; j < infosbuilding.Count; j++) {
                ArrayList building = (ArrayList)infosbuilding[j];
                LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings[j] = new LVAuto.LVObj.Building(int.Parse(building[0].ToString()), int.Parse(building[1].ToString()), building[2].ToString(), int.Parse(building[3].ToString()));
            }
            Array.Sort(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].AllBuildings);
        }
       
        public static void SwitchCitySlow(int idcity) {
            //Execute(50, "tid=" + idcity, true);
            LVAuto.LVWeb.LVClient.CurrentLoginInfo.cid = "" + idcity;
        }
        public static void SwitchCity(int idcity) {
            //Execute(50, "tid=" + idcity);
            lock (LVAuto.LVWeb.LVClient.CurrentLoginInfo.cid) {
                LVAuto.LVWeb.LVClient.CurrentLoginInfo.cid = ""+idcity;
            }
        }
        public static Hashtable GetCitySimpleInfo() {
            return LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(6, "", true);
        }
        public static Hashtable GetCitySimpleInfo(string cookies) 
		{
            return LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(6, "", true, cookies);
        }

		// hungtv add 
		public static LVAuto.LVObj.CityTask GetCityTaskByCityID(int cityid)
		{
			//int cityid = LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].id;
				try
				{
					LVAuto.LVForm.Command.City.SwitchCitySlow(cityid);
					Hashtable task = Command.City.GetCityTask(LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityid));
					//Hashtable attack = Command.Common.GetCityAttack(cty.id,LVAuto.Web.LVWeb.CurrentLoginInfo.MakeCookiesString(cty.id));
					int countbuild = ((ArrayList)((ArrayList)((ArrayList)task["list"])[0])[4]).Count;
					int tech = int.Parse(((ArrayList)task["tech"])[2].ToString());
					int task1 = int.Parse(((ArrayList)task["task"])[3].ToString());
					//int mcb = int.Parse(((ArrayList)((ArrayList)((ArrayList)attack["infos"])[0])[2])[1].ToString());
					return (new LVAuto.LVObj.CityTask(countbuild, tech, task1, 0));
				}
				catch (Exception ex)
				{
					return ( new LVAuto.LVObj.CityTask(0, 0, 0, 0));
				}			
		}

			// hungtv add 
		public static LVAuto.LVObj.CityTask GetCityTaskByCityPos(int citypos)
		{
			return GetCityTaskByCityID(LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypos].Id);
		}
        
    }
}
