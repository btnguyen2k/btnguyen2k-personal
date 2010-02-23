using System;
using System.Collections.Generic;
using System.Text;

namespace LVAuto.LVConfig
{
    public class AutoConfig
    {
        public const int RESOURCE_TYPE_FOOD = 1;
        public const int RESOURCE_TYPE_WOODS = 2;
        public const int RESOURCE_TYPE_STONE = 3;
        public const int RESOURCE_TYPE_IRON = 4;

        public static SellResourcesConfig CONFIG_SELL_RESOURCES = new SellResourcesConfig();
        public static CityConstructConfig CONFIG_CITY_CONSTRUCT = new CityConstructConfig();

        public class CityConstructConfig
        {
            public LVObj.City[] AllCities;
            public bool IsBuildAll       = false;
            public bool IsDowndAll       = false;
            public bool AutoBuyResources = true;  
            public long GoldThreshold    = 5000000; 
        }

        public class SellResourcesConfig
        {
            public enum EnumSellAmountMethod { FIX_AMOUNT, PERCENT_STORAGE };
            public enum EnumSellPriceMethod  { AVERAGE_PRICE, FIX_PRICE, MIN_PRICE };
            public enum EnumPriceAddOnMethod { ADD_PRICE, MULTIPLY_PRICE };

            public int SleepTimeInMinutes = 1;
            public bool SaleOff = true;
            public SellResConfig FoodConfig = new SellResConfig(RESOURCE_TYPE_FOOD), 
                WoodsConfig = new SellResConfig(RESOURCE_TYPE_WOODS),
                StoneConfig = new SellResConfig(RESOURCE_TYPE_STONE),
                IronConfig = new SellResConfig(RESOURCE_TYPE_IRON);
            public SellCityConfig[] CityConfig;
            
            public class SellResConfig
            {
                public int ResourceType;
                public EnumSellAmountMethod SellAmountMethod = EnumSellAmountMethod.FIX_AMOUNT;
                public EnumSellPriceMethod SellPriceMethod   = EnumSellPriceMethod.AVERAGE_PRICE;
                public EnumPriceAddOnMethod PriceAddOnMethod = EnumPriceAddOnMethod.ADD_PRICE;
                public int SellAmount = 1000, SellThreshold = 1000;
                public double PriceAddOn = 0.0;
                
                public SellResConfig(int ResourceType)
                {
                    this.ResourceType = ResourceType;
                }
            }

            public class SellCityConfig
            {
                public int CityId;
                public string CityName;
                public bool SellFood = false, SellWoods = false, SellStone = false, SellIron = false;
            }
        } //end class
    } //end class
} //end namespace
