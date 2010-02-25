using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using System.Collections;

namespace LVAuto.LVThread 
{
    /// <summary>
    /// "Auto Thread" class for auto buying of resources.
    /// </summary>
    public class AutoBuyResources : BaseThread {
        private static LVThread.AutoBuyResources instance;

        public static LVThread.AutoBuyResources getInstance(Label labelMessage)
        {
            if (instance == null)
            {
                instance = new LVThread.AutoBuyResources(labelMessage);
            }
            return instance;
        }

        /// <summary>
        /// Constructs a new AutoBuyResources object.
        /// </summary>
        /// <param name="labelMessage"></param>
        protected AutoBuyResources(Label labelMessage)
            : base(labelMessage)
        {
        }

        /// <summary>
        /// Sets up resource buying settings
        /// </summary>
        /// <param name="sleepTime">sleep time in millisecs</param>
        public void SetUp(int sleepTime)
        {
            if (IsRunning)
            {
                return;
            }

            #if (DEBUG)
                sleepTime = 10; //10 seconds sleep in debug mode
            #endif
            this.SleepTimeInSeconds = sleepTime;
        }

        /// <summary>
        /// Finds the market building in a city.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        private LVAuto.LVObj.Building FindMarketBuilding(LVAuto.LVObj.City city)
        {
            LVHelper.CityCommandHelper.GetAndPopulateBuildings(city, false);
            if (city.AllBuildings == null)
            {
                return null;
            }
            foreach (LVAuto.LVObj.Building building in city.AllBuildings)
            {
                if (building.GId == 11 /* market in normal castle */ || building.GId == 64 /* market in castle level 4 */)
                {
                    return building;
                }
            }
            return null;
        }

        // <summary>
        /// Main method to buy resources for cities, called by run.
        /// </summary>
        /// <returns></returns>
        private int BuyResources()
        {
            if (LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig == null || LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig.Length == 0)
            {
                return 0;
            }

            int totalCities = LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig.Length;
            int numRunCities = 0;
            foreach (LVConfig.AutoConfig.BuyResourcesConfig.BuyResCityConfig cityConfig in LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig)
            {
                numRunCities++;
                SetText("Đang mua ở thành " + cityConfig.CityName + " (" + numRunCities + "/" + totalCities + ")");
                WriteLog("Buying resources [" + cityConfig.CityName + "]...");

                int cityId = cityConfig.CityId;
                LVAuto.LVObj.City city = LVHelper.CityCommandHelper.GetCityById(cityId);
                LVAuto.LVObj.Building marketBuilding = FindMarketBuilding(city);
                if (marketBuilding == null)
                {
                    WriteLog("Buying resources [" + cityConfig.CityName + "]: No market building!");
                    //there is no market building!
                    continue;
                }
                if (!LVHelper.BuildingCommandHelper.SelectBuilding(cityConfig.CityId, marketBuilding))
                {
                    WriteLog("Buying resources [" + cityConfig.CityName + "]: Can not select market building (error?)!");
                    //error?
                    continue;
                }
                Hashtable currentResources = LVHelper.CityCommandHelper.GetCurentResourceInCity(cityId);
                if (currentResources == null)
                {
                    WriteLog("Buying resources [" + cityConfig.CityName + "]: Can not get current resource info (error?)!");
                    //error?
                    continue;
                }
                int currentFood = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_FOOD].ToString());
                int currentWoods = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_WOODS].ToString());
                int currentStone = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_STONE].ToString());
                int currentIron = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_IRON].ToString());
                long currentGold = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_GOLD].ToString());
                int maxStorage = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_MAX].ToString());
                long gold = currentGold - LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.GoldThreshold;
                if (gold < 0)
                {
                    WriteLog("Buying resources [" + cityConfig.CityName + "]: Exceed gold threshold!");
                    continue;
                }
                int maxValueCanBuy = 0;
                switch (LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmountMethod)
                {
                    case LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.EnumBuyAmountMethod.FIX_AMOUNT:
                        maxValueCanBuy = LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmount > maxStorage ? maxStorage : LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmount;
                        break;
                    case LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.EnumBuyAmountMethod.PERCENT_STORAGE:
                        maxValueCanBuy = (maxStorage / 100) * LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmount;
                        if (maxValueCanBuy > maxStorage)
                        {
                            maxValueCanBuy = maxStorage;
                        }
                        break;
                }
                if (cityConfig.BuyFood && gold > 0 && currentFood < maxValueCanBuy)
                {
                    int amountToBuy = maxValueCanBuy - currentFood;
                    WriteLog("Buying resources [" + cityConfig.CityName + "/Food: " + amountToBuy + "/Gold: " + gold + "]");
                    LVHelper.OptCommandHelper.BuyResource(city.Id, LVAuto.LVCommon.Constants.RESOURCE_TYPE_FOOD, amountToBuy, ref gold);
                }
                if (cityConfig.BuyWoods && gold > 0 && currentWoods < maxValueCanBuy)
                {
                    int amountToBuy = maxValueCanBuy - currentWoods;
                    WriteLog("Buying resources [" + cityConfig.CityName + "/Woods: " + amountToBuy + "/Gold: " + gold + "]");
                    LVHelper.OptCommandHelper.BuyResource(city.Id, LVAuto.LVCommon.Constants.RESOURCE_TYPE_WOODS, amountToBuy, ref gold);
                }
                if (cityConfig.BuyStone && gold > 0 && currentStone < maxValueCanBuy)
                {
                    int amountToBuy = maxValueCanBuy - currentStone;
                    WriteLog("Buying resources [" + cityConfig.CityName + "/Stone: " + amountToBuy + "/Gold: " + "]");
                    LVHelper.OptCommandHelper.BuyResource(city.Id, LVAuto.LVCommon.Constants.RESOURCE_TYPE_STONE, amountToBuy, ref gold);
                }
                if (cityConfig.BuyIron && gold > 0 && currentIron < maxValueCanBuy)
                {
                    int amountToBuy = maxValueCanBuy - currentIron;
                    WriteLog("Buying resources [" + cityConfig.CityName + "/Iron: " + amountToBuy + "/Gold: " + gold + "]");
                    LVHelper.OptCommandHelper.BuyResource(city.Id, LVAuto.LVCommon.Constants.RESOURCE_TYPE_IRON, amountToBuy, ref gold);
                }
            } //end foreach
            return 0;
        }

        private void PerformBuyResource(LVAuto.LVObj.City city, int resType, int amountToBuy, ref long gold)
        {
            LVHelper.OptCommandHelper.BuyResource(city.Id, resType, amountToBuy, ref gold);
        }

        /// <summary>
        /// Called by the thread executor
        /// </summary>
        protected override void Run()
        {
            while (true)
            {
                SetText("Chờ tới phiên (0%) - chờ từ lúc " + DateTime.Now.ToString("HH:mm:ss"));
                Message.ForeColor = System.Drawing.Color.Red;
                int resultStatus = BuyResources();
                Message.ForeColor = System.Drawing.Color.Blue;
                //SetText("Đang ngủ " + SleepTimeInSeconds + " giây, chờ tí (mới chạy lúc: " + DateTime.Now.ToString("HH:mm:ss") + ")");
                if (resultStatus > 1000000)
                {
                    WriteLog("Bị khóa đến " + DateTime.Now.AddSeconds(resultStatus - 1000000).ToString("HH:mm:ss"));
                //    SetText("Bị khóa đến " + DateTime.Now.AddSeconds(resultStatus - 1000000).ToString("HH:mm:ss")
                //        + ". Đang ngủ " + SleepTimeInSeconds + " giây, chờ tí (mới chạy lúc: " + DateTime.Now.ToString("HH:mm:ss") + ")");
                }
                Sleep();
                //Thread.Sleep(SleepTimeInSeconds);
            }
        }
    } //end class
} //end namespace
