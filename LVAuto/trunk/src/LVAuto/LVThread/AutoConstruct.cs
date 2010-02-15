﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace LVAuto.LVThread {
    /// <summary>
    /// "Auto Thread" class for auto construction.
    /// </summary>
    public class AutoConstruct : BaseThread {

        public const int MAXLEVEL = 35;

        //{cityId, building[]}
        private IDictionary<int, object[]> constructionConfig = new Dictionary<int, object[]>();

		private ArrayList CityList = new ArrayList();

        private static LVThread.AutoConstruct instance;

        public static LVThread.AutoConstruct getInstance(Label labelMessage)
        {
            if (instance == null)
            {
                instance = new LVThread.AutoConstruct(labelMessage);
            }
            return instance;
        }
 
        /// <summary>
        /// Constructs a new AutoConstruct object.
        /// </summary>
        /// <param name="labelMessage"></param>
        protected AutoConstruct(Label labelMessage) : base(labelMessage) {
        }

        /// <summary>
        /// Sets up construction settings
        /// </summary>
        /// <param name="sleepTime">sleep time in millisecs</param>
		public void SetUp(int sleepTime)
		{
            if (IsRunning)
            {
                return;
            }

            #if (DEBUG)
                sleepTime = 10000; //10 seconds sleep in debug mode
            #endif
            constructionConfig.Clear();
            this.SleepTime = sleepTime;
			
	        foreach (LVAuto.LVForm.Command.CityObj.City city in LVAuto.LVForm.Command.CityObj.City.AllCity)
            {
                bool hasUp = false;
                if (city.AllBuilding != null)
                {
                    ArrayList arrBuildings = new ArrayList();
                    foreach (LVAuto.LVForm.Command.CityObj.Building building in city.AllBuilding)
                    {
                        if (building.isUp)
                        {
                            arrBuildings.Add(building);
                            hasUp = true;
                        }
                    }
                    if (hasUp)
                    {
                        constructionConfig.Add(city.id, arrBuildings.ToArray());
                    }
                }							
            }						
		}

        /// <summary>
        /// Checks if "build all" checkbox is ticked.
        /// </summary>
        private void CheckBuildAll()
        {
            if (LVAuto.LVForm.Command.CityObj.City.isBuildAll)
            {
                this.constructionConfig.Clear();
                foreach (LVAuto.LVForm.Command.CityObj.City city in LVAuto.LVForm.Command.CityObj.City.AllCity)
                {
                    LVHelper.CityCommandHelper.GetAndPopulateBuildings(city, false);
                    if (city.AllBuilding != null)
                    {
                        this.constructionConfig.Add(city.id, city.AllBuilding);
                    }
                }
            }
        }

        /// <summary>
        /// Main method to constructs buildings in cities, called by run.
        /// </summary>
        /// <returns></returns>
        private int ConstructBuildings()
        {
            CheckBuildAll();
            if (this.constructionConfig.Count == 0)
            {
                SetText("Chẳng có gì để chạy cả");
                return -1;
            }

            //how many buildings we can build simultaneously
            int maxbuild = LVHelper.BuildingCommandHelper.GetSimultaneousBuildings();
            int numCitiesToRun = constructionConfig.Count;
            int countRunCities = 0;
            foreach (int cityId in constructionConfig.Keys)
            {
                LVAuto.LVForm.Command.CityObj.City city = LVHelper.CityCommandHelper.GetCityById(cityId);
                LVHelper.CityCommandHelper.GetAndPopulateBuildings(city, true);
                //LVAuto.LVForm.Command.City.UpdateAllBuilding(cityPos);
                countRunCities++;
                SetText("Đang chạy: " + city.name + " (đã hoàn thành " + countRunCities + "/" + numCitiesToRun + " thành)");
                //LVAuto.LVForm.Command.City.SwitchCitySlow(cityId);
                WriteLog("Constructing city ["+city.name+"]...");

                Hashtable hcitytask = LVHelper.CityCommandHelper.GetCityTasks(city.id);
                if (hcitytask == null)
                {
                    //error while getting city tasks
                    WriteLog("Constructing city [" + city.name + "]: Error while getting city tasks!");
                    continue;
                }
                ArrayList temp = (ArrayList)hcitytask["list"];
                temp = (ArrayList)temp[0];
                temp = (ArrayList)temp[4];
                int numBuildings = temp.Count; //number of buildings under constructing
                int countBuildings = 0;
                foreach (LVAuto.LVForm.Command.CityObj.Building building in city.AllBuilding)
                {
                    WriteLog("Constructing city [" + city.name + ":" + building.name + "]...");
                    if (numBuildings + countBuildings >= maxbuild)
                    {
                        //reach max number of simultaneous buildings
                        WriteLog("Constructing city [" + city.name + ":" + building.name + "]: number of simultaneous buildings (" + maxbuild + ") exceeds!");
                        break;
                    }
                    if (building.level >= MAXLEVEL)
                    {
                        //building has already been at max level
                        WriteLog("Constructing city [" + city.name + ":" + building.name + "]: building has already been at max level!");
                        continue;
                    }
                    bool underConstructing = false;
                    for (int i = 0; i < temp.Count; i++)
                    {
                        int pid = int.Parse(((ArrayList)temp[i])[0].ToString());
                        int gid = int.Parse(((ArrayList)temp[i])[1].ToString());
                        if (building.gid == gid && building.pid == pid)
                        {
                            //this building is under constructing
                            //(building under constructing: gid= 3; pid = 18)
                            underConstructing = true;
                            break;
                        }
                    }
                    if (underConstructing)
                    {
                        WriteLog("Constructing city [" + city.name + ":" + building.name + "]: building is under construction/destruction!");
                        continue;
                    }

                    int numRetries = 0;
                RETRY:
                    if (numRetries > 5)
                    {
                        WriteLog("Constructing city [" + city.name + ":" + building.name + "]: max retries reaches, abort!");
                        continue;
                    }
                    Hashtable result = null;
                    if (LVHelper.BuildingCommandHelper.SelectBuilding(cityId, building))
                    {
                        result = LVHelper.OptCommandHelper.ConstructBuilding(cityId, building);
                    }
                    else
                    {
                        //if select building fails, try again with tab=[-2..3]
                        for (int tab = -2; tab < 3; tab++)
                        {
                            if (LVHelper.BuildingCommandHelper.SelectBuilding(cityId, building, tab))
                            {
                                result = LVHelper.OptCommandHelper.ConstructBuilding(cityId, building);
                                break;
                            };
                        }
                    }
                    if (result == null)
                    {
                        //failed
                        WriteLog("Constructing city [" + city.name + ":" + building.name + "]: failed (null returned from server)!");
                        continue;
                    }
                    int resultStatus = int.Parse(result["ret"].ToString());
                    if (resultStatus == 0)
                    {
                        countBuildings++; //increase number of processed buildings
                        WriteLog("Constructing city [" + city.name + ":" + building.name + "]: successful!");
                        continue;
                    }
                    if (resultStatus == 110)
                    {
                        //image captcha!
                        WriteLog("Constructing city [" + city.name + ":" + building.name + "]: CAPTCHA!");
                        return resultStatus;
                    }
                    if (resultStatus > 1000000)
                    {
                        //activity is locked by the system
                        WriteLog("Constructing city [" + city.name + ":" + building.name + "]: account being locked!");
                        return resultStatus;
                    }
                    if (resultStatus == 32)
                    {
                        //not enough resources
                        if (cityId < 0)
                        {
                            //is barrack
                            WriteLog("Constructing city [" + city.name + ":" + building.name + "]: not enough resource, is barrack, abort!");
                        }
                        else if (LVAuto.LVForm.Command.CityObj.City.isBuyRes)
                        {
                            WriteLog("Constructing city [" + city.name + ":" + building.name + "]: not enough resource, buying more...");
                            if (BuyMoreResources(cityId, building, LVAuto.LVForm.Command.CityObj.City.goldSafe))
                            {
                                numRetries++;
                                goto RETRY;
                            }
                        }
                    }
                }
            } // end foreach
            return 0;
        }

        private bool BuyMoreResources(int cityId, LVAuto.LVForm.Command.CityObj.Building building, long safegold)
		{
            Hashtable ResNeed = LVHelper.BuildingCommandHelper.GetResourcesForUpgrade(cityId, building);
            if (ResNeed == null)
            {
                WriteLog("Auto buy more resources for [" + cityId + ":" + building.name + "]: upgrade condition not meet!");
                return false;
            }
            Hashtable ResHave = LVHelper.CityCommandHelper.GetCurentResourceInCity(cityId);
            int needValue;
            int haveValue;
            int maxkho = int.Parse(ResHave[LVHelper.BaseHelper.RESOURCE_MAX].ToString());
            long gold = long.Parse(ResHave[LVHelper.BaseHelper.RESOURCE_GOLD].ToString());
            gold = gold - safegold;

            if (gold <= 0)
            {
                //not enough gold
                WriteLog("Auto buy more resources for [" + cityId + ":" + building.name + "]: not enough gold (lacking " + Math.Abs(gold) + ")!");
                return false;
            }
            //buy food
            needValue = int.Parse(ResNeed[LVHelper.BaseHelper.RESOURCE_FOOD].ToString());
            haveValue = int.Parse(ResHave[LVHelper.BaseHelper.RESOURCE_FOOD].ToString());
            if (needValue > maxkho)
            {
                //exceed max storage
                WriteLog("Auto buy more FOOD for [" + cityId + ":" + building.name + "]: max storage exceed (" + needValue + "/" + maxkho + ")!");
                return false;
            }
            if (needValue > haveValue)
            {
                int needbuy = needValue - haveValue;
                WriteLog("Auto buy more FOOD for [" + cityId + ":" + building.name + "]: buying (" + needbuy + ")!");
                LVHelper.OptCommandHelper.BuyResource(cityId, LVAuto.LVCommon.Constants.RESOURCE_TYPE_FOOD, needbuy, ref gold);
            }

            if (gold <= 0)
            {
                //not enough gold
                WriteLog("Auto buy more resources for [" + cityId + ":" + building.name + "]: not enough gold (lacking " + Math.Abs(gold) + ")!");
                return false;
            }
            //buy woods
            needValue = int.Parse(ResNeed[LVHelper.BaseHelper.RESOURCE_WOODS].ToString());
            haveValue = int.Parse(ResHave[LVHelper.BaseHelper.RESOURCE_WOODS].ToString());
            if (needValue > maxkho)
            {
                //exceed max storage
                WriteLog("Auto buy more WOODS for [" + cityId + ":" + building.name + "]: max storage exceed (" + needValue + "/" + maxkho + ")!");
                return false;
            }
            if (needValue > haveValue)
            {
                int needbuy = needValue - haveValue;
                WriteLog("Auto buy more WOODS for [" + cityId + ":" + building.name + "]: buying (" + needbuy + ")!");
                LVHelper.OptCommandHelper.BuyResource(cityId, LVAuto.LVCommon.Constants.RESOURCE_TYPE_WOODS, needbuy, ref gold);
            }

            if (gold <= 0)
            {
                //not enough gold
                WriteLog("Auto buy more resources for [" + cityId + ":" + building.name + "]: not enough gold (lacking " + Math.Abs(gold) + ")!");
                return false;
            }
            //buy stone
            needValue = int.Parse(ResNeed[LVHelper.BaseHelper.RESOURCE_STONE].ToString());
            haveValue = int.Parse(ResHave[LVHelper.BaseHelper.RESOURCE_STONE].ToString());
            if (needValue > maxkho)
            {
                //exceed max storage
                WriteLog("Auto buy more STONE for [" + cityId + ":" + building.name + "]: max storage exceed (" + needValue + "/" + maxkho + ")!");
                return false;
            }
            if (needValue > haveValue)
            {
                int needbuy = needValue - haveValue;
                WriteLog("Auto buy more STONE for [" + cityId + ":" + building.name + "]: buying (" + needbuy + ")!");
                LVHelper.OptCommandHelper.BuyResource(cityId, LVAuto.LVCommon.Constants.RESOURCE_TYPE_STONE, needbuy, ref gold);
            }

            if (gold <= 0)
            {
                //not enough gold
                WriteLog("Auto buy more resources for [" + cityId + ":" + building.name + "]: not enough gold (lacking " + Math.Abs(gold) + ")!");
                return false;
            }
            //buy iron
            needValue = int.Parse(ResNeed[LVHelper.BaseHelper.RESOURCE_IRON].ToString());
            haveValue = int.Parse(ResHave[LVHelper.BaseHelper.RESOURCE_IRON].ToString());
            if (needValue > maxkho)
            {
                //exceed max storage
                WriteLog("Auto buy more IRON for [" + cityId + ":" + building.name + "]: max storage exceed (" + needValue + "/" + maxkho + ")!");
                return false;
            }
            if (needValue > haveValue)
            {
                int needbuy = needValue - haveValue;
                WriteLog("Auto buy more STONE for [" + cityId + ":" + building.name + "]: buying (" + needbuy + ")!");
                LVHelper.OptCommandHelper.BuyResource(cityId, LVAuto.LVCommon.Constants.RESOURCE_TYPE_IRON, needbuy, ref gold);
            }

            return true;
		}

        /// <summary>
        /// Called by the thread executor
        /// </summary>
		protected override void Run()
		{
			while (true)
			{
				if (!LVAuto.LVForm.Command.CityObj.City.isBuildAll && ( constructionConfig == null || constructionConfig.Count == 0))
				{
					SetText("Chẳng có gì để chạy cả");
					break;
				}
				SetText("Chờ tới phiên (0%) - chờ từ lúc " + DateTime.Now.ToString("HH:mm:ss"));
                Message.ForeColor = System.Drawing.Color.Red;
                int resultStatus = ConstructBuildings();
				Message.ForeColor = System.Drawing.Color.Blue;
                SetText("Đang ngủ " + SleepTime / 1000 + " giây, chờ tí (mới chạy lúc: " + DateTime.Now.ToString("HH:mm:ss") + ")");
                if (resultStatus > 1000000)
				{
                    SetText("Bị khóa đến " + DateTime.Now.AddSeconds(resultStatus - 1000000).ToString("HH:mm:ss")
                        + ". Đang ngủ " + SleepTime / 1000 + " giây, chờ tí (mới chạy lúc: " + DateTime.Now.ToString("HH:mm:ss") + ")");
				}
				Thread.Sleep(SleepTime);
			}
		}
    } //end class
} //end namespace
