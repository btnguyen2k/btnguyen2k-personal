using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.Data;

namespace LVAuto.LVThread 
{
    /// <summary>
    /// "Auto Thread" class for auto selling of resources.
    /// </summary>
    public class AutoSellResources : BaseThread
	{
		LVAuto.LVForm.Command.CommonObj.BanTaiNguyenObj banobj;

        private static LVThread.AutoSellResources instance;

        public static LVThread.AutoSellResources getInstance(Label labelMessage)
        {
            if (instance == null)
            {
                instance = new LVThread.AutoSellResources(labelMessage);
            }
            return instance;
        }

        /// <summary>
        /// Constructs a new AutoSellResources object.
        /// </summary>
        /// <param name="labelMessage"></param>
        protected AutoSellResources(Label labelMessage)
            : base(labelMessage)
        {
        }

        /// <summary>
        /// Sets up resource selling settings
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
            this.SleepTime = sleepTime;
		}

        /// <summary>
        /// Calculates the sell price for a resource selling configuration.
        /// </summary>
        /// <param name="sellResConfig"></param>
        /// <returns></returns>
        private int CalcSellPrice(LVConfig.AutoConfig.SellResourcesConfig.SellResConfig sellResConfig)
        {
            int calcPrice = -1;
            switch (sellResConfig.SellPriceMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.AVERAGE_PRICE:
                    return (int)Math.Round(sellResConfig.PriceAddOn);
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.FIX_PRICE:
                    calcPrice = LVHelper.OptCommandHelper.GetAvgPrice(sellResConfig.ResourceType);
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.MIN_PRICE:
                    calcPrice = LVHelper.OptCommandHelper.GetMinPrice(sellResConfig.ResourceType);
                    break;
            } //end switch
            if (calcPrice == -1)
            {
                //fallsafe
                calcPrice = LVHelper.OptCommandHelper.GetAvgPrice(sellResConfig.ResourceType);
            }
            if (sellResConfig.PriceAddOnMethod == LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.ADD_PRICE)
            {
                calcPrice = (int)Math.Round(calcPrice + sellResConfig.PriceAddOn);
            }
            else
            {
                calcPrice = (int)Math.Round(calcPrice * sellResConfig.PriceAddOn);
            }
            return calcPrice;
        }

        /// <summary>
        /// Finds the market building in a city.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        private LVAuto.LVForm.Command.CityObj.Building FindMarketBuilding(LVAuto.LVForm.Command.CityObj.City city)
        {
            LVHelper.CityCommandHelper.GetAndPopulateBuildings(city, false);
            if (city.AllBuilding == null)
            {
                return null;
            }
            foreach (LVAuto.LVForm.Command.CityObj.Building building in city.AllBuilding)
            {
                if (building.gid == 11 /* market in normal castle */ || building.gid == 64 /* market in castle level 4 */)
                {
                    return building;
                }
            }
            return null;
        }

        private Hashtable PerformSellResource(int cityId, LVConfig.AutoConfig.SellResourcesConfig.SellResConfig sellResConfig, int price, int currentStorage, int maxStorage)
        {
            int sellAmount = 0, thresholdAmount = 0; //in ton
            switch (sellResConfig.SellAmountMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.FIX_AMOUNT:
                    sellAmount = sellResConfig.SellAmount;
                    thresholdAmount = sellResConfig.SellThreshold;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.PERCENT_STORAGE:
                    sellAmount = (int)Math.Round((sellResConfig.SellAmount / 1000.0 / 100.0) * maxStorage);
                    thresholdAmount = (int)Math.Round((sellResConfig.SellThreshold / 1000.0 / 100.0) * maxStorage);
                    break;
            } //end switch
            if (sellAmount + thresholdAmount <= currentStorage / 1000)
            {
                //sell the resource
                return LVHelper.OptCommandHelper.SellResource(cityId, sellAmount, price, sellResConfig.ResourceType);
            }
            return null;
        }

        /// <summary>
        /// Main method to sell resources in cities, called by run.
        /// </summary>
        /// <returns></returns>
        private int SellResources()
        {
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig == null || LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig.Length == 0)
            {
                return 0;
            }

            int priceFood = CalcSellPrice(LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig);
            int priceWoods = CalcSellPrice(LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig);
            int priceStone = CalcSellPrice(LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig);
            int priceIron = CalcSellPrice(LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig);

            int totalCities = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig.Length;
            int numRunCities = 0;
            foreach (LVConfig.AutoConfig.SellResourcesConfig.SellCityConfig cityConfig in LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig)
            {
                numRunCities++;
                SetText("Đang bán ở thành " + cityConfig.CityName + " (" + numRunCities + "/" + totalCities + ")");
                int cityId = cityConfig.CityId;
                LVAuto.LVForm.Command.CityObj.City city = LVHelper.CityCommandHelper.GetCityById(cityId);
                LVAuto.LVForm.Command.CityObj.Building marketBuilding = FindMarketBuilding(city);
                if (marketBuilding == null)
                {
                    //there is no market building!
                    continue;
                }
                if (!LVHelper.BuildingCommandHelper.SelectBuilding(cityConfig.CityId, marketBuilding))
                {
                    //error?
                    continue;
                }
                Hashtable currentResources = LVHelper.CityCommandHelper.GetCurentResourceInCity(cityId);
                if (currentResources == null)
                {
                    //error?
                    continue;
                }
                int currentFood = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_FOOD].ToString());
                int currentWoods = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_WOODS].ToString());
                int currentStone = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_STONE].ToString());
                int currentIron = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_IRON].ToString());
                int maxStorage = int.Parse(currentResources[LVHelper.BaseHelper.RESOURCE_MAX].ToString());
                if (cityConfig.SellFood)
                {
                    //sell food
                    LVConfig.AutoConfig.SellResourcesConfig.SellResConfig sellResConfig = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig;
                    Hashtable result = PerformSellResource(cityId, sellResConfig, priceFood, currentFood, maxStorage);
                    if (result != null)
                    {
                        int status = int.Parse(result["ret"].ToString());
                        if (status == 249 || status > 1000000)
                        {
                            return status;
                        }
                    }
                }
                if (cityConfig.SellWoods)
                {
                    //sell woods
                    LVConfig.AutoConfig.SellResourcesConfig.SellResConfig sellResConfig = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig;
                    Hashtable result = PerformSellResource(cityId, sellResConfig, priceWoods, currentWoods, maxStorage);
                    if (result != null)
                    {
                        int status = int.Parse(result["ret"].ToString());
                        if (status == 249 || status > 1000000)
                        {
                            return status;
                        }
                    }
                }
                if (cityConfig.SellStone)
                {
                    //sell woods
                    LVConfig.AutoConfig.SellResourcesConfig.SellResConfig sellResConfig = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig;
                    Hashtable result = PerformSellResource(cityId, sellResConfig, priceStone, currentStone, maxStorage);
                    if (result != null)
                    {
                        int status = int.Parse(result["ret"].ToString());
                        if (status == 249 || status > 1000000)
                        {
                            return status;
                        }
                    }
                }
                if (cityConfig.SellIron)
                {
                    //sell woods
                    LVConfig.AutoConfig.SellResourcesConfig.SellResConfig sellResConfig = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig;
                    Hashtable result = PerformSellResource(cityId, sellResConfig, priceIron, currentIron, maxStorage);
                    if (result != null)
                    {
                        int status = int.Parse(result["ret"].ToString());
                        if (status == 249 || status > 1000000)
                        {
                            return status;
                        }
                    }
                }
            } //end foreach
            return 0;
        }
		
        /// <summary>
        /// Saleoff when market queue is full.
        /// </summary>
        /// <returns></returns>
        private void SaleOff()
		{
            if (!LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.SaleOff)
            {
                return;
            }

            const int MaxItemsChange = 10;
            const int MinPriceThreshold = 1;

            int minPriceFood = LVHelper.OptCommandHelper.GetMinPrice(LVAuto.LVConfig.AutoConfig.RESOURCE_TYPE_FOOD);
            int minPriceWoods = LVHelper.OptCommandHelper.GetMinPrice(LVAuto.LVConfig.AutoConfig.RESOURCE_TYPE_WOODS);
            int minPriceStone = LVHelper.OptCommandHelper.GetMinPrice(LVAuto.LVConfig.AutoConfig.RESOURCE_TYPE_STONE);
            int minPriceIron = LVHelper.OptCommandHelper.GetMinPrice(LVAuto.LVConfig.AutoConfig.RESOURCE_TYPE_IRON);
            ArrayList myQueue = LVHelper.CommonCommandHelper.GetMyMarketQueue();
            if (myQueue == null)
            {
                return;
            }
            //sort by queue: ASC by amount
            for (int i = 0; i < myQueue.Count; i++)
            {
                for (int j = i + 1; j < myQueue.Count; j++)
                {
                    int amount1 = int.Parse(((Hashtable)myQueue[i])["count"].ToString());
                    int amount2 = int.Parse(((Hashtable)myQueue[2])["count"].ToString());
                    if (amount1 > amount2)
                    {
                        object temp = myQueue[i];
                        myQueue[i] = myQueue[j];
                        myQueue[j] = temp;
                    }
                }
            }

            for (int i = 0; i < MaxItemsChange && i < myQueue.Count; i++)
            {
                Hashtable myEntry = (Hashtable)myQueue[i];
                int seqno = int.Parse(myEntry["seqno"].ToString());
                int type = int.Parse(myEntry["type"].ToString());
                int price = -1;
                switch (type)
                {
                    case LVAuto.LVConfig.AutoConfig.RESOURCE_TYPE_FOOD:
                        price = minPriceFood - 1;
                        break;
                    case LVAuto.LVConfig.AutoConfig.RESOURCE_TYPE_WOODS:
                        price = minPriceWoods - 1;
                        break;
                    case LVAuto.LVConfig.AutoConfig.RESOURCE_TYPE_STONE:
                        price = minPriceStone - 1;
                        break;
                    case LVAuto.LVConfig.AutoConfig.RESOURCE_TYPE_IRON:
                        price = minPriceIron - 1;
                        break;
                }
                if (price < MinPriceThreshold)
                {
                    continue;
                }
                LVHelper.OptCommandHelper.UpdateMyMarketPrice(seqno, price);
            }
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
                int resultStatus = SellResources();
                if (resultStatus == 249)
                {
                    SaleOff();
                }
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
