using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LVAuto.LVHelper
{
    /// <summary>
    /// Helper class for Opt commands.
    /// </summary>
    public class OptCommandHelper : BaseHelper
    {

        public const int COMMAND_BUY_RESOURCE = 9;
        public const int COMMAND_GET_RESOURCE_AVG_PRICE = 29;
        public const int COMMAND_SELL_RESOURCE = 49;
        public const int COMMAND_BUILD_BUILDING = 65;
        public const int COMMAND_UPDATE_MY_MARKET_PRICE = 68;

        private static object LocalLock = new object();

        private static OptCommandHelper instance;

        /// <summary>
        /// Singleton implementation.
        /// </summary>
        /// <returns></returns>
        public static OptCommandHelper GetInstance()
        {
            if (instance == null)
            {
                instance = new OptCommandHelper();
            }
            return instance;
        }

        /// <summary>
        /// Constructs a new OptCommandHelper object.
        /// </summary>
        protected OptCommandHelper()
        {
        }

        /// <summary>
        /// Executes an opt command.
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="parameters"></param>
        /// <param name="waitForResult"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public override Hashtable ExecuteCommand(int commandId, string parameters, bool waitForResult, string cookies)
        {
            //string data = parameters + "&num=" + LVAuto.LVWeb.LVClient.idimage;
            string data = parameters != null ? parameters : "";

            string header = "POST http://s" + LVAuto.LVWeb.LVClient.Server + "." + LV_DOMAIN + "/GateWay/OPT.ashx?id=" + commandId + "&0.05861361440438828  HTTP/1.1\n";
            header += "Host: s" + LVAuto.LVWeb.LVClient.Server + "." + LV_DOMAIN + "\n";
            header += HEADER_USER_AGENT;
            header += HEADER_ACCEPT_TYPE;
            header += HEADER_ACCEPT_LANGUAGE;
            // header += "Accept-Encoding: gzip,deflate\n";
            header += HEADER_ACCEPT_CHARSET;
            header += HEADER_KEEP_ALIVE;
            header += HEADER_CONNECTION;
            header += HEADER_X;
            header += "Referer: http://s" + LVAuto.LVWeb.LVClient.Server + "." + LV_DOMAIN + "/city?login\n";
            header += HEADER_CONTENT_TYPE;
            header += "Content-Length: " + (data.Length) + "\n";
            header += HEADER_CACHE_CONTROL;
            header += "Cookie: " + cookies + "\n";
            header += "\n";
            Hashtable response = LVWeb.LVClient.SendAndReceive(header + data, "s" + LVAuto.LVWeb.LVClient.Server + "." + LV_DOMAIN, 80, waitForResult);
            if (waitForResult)
            {
                if (response != null && response["DATA"] != null)
                {
                    return (Hashtable)LVAuto.LVForm.JSON.JSON.JsonDecode(response["DATA"].ToString());
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Constructs a building.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="building"></param>
        /// <returns></returns>
        public static Hashtable ConstructBuilding(int cityId, LVAuto.LVObj.Building building)
        {
            string parameters = "";
            parameters += "gid=" + building.GId;
            parameters += "&pid=" + building.PId;
            parameters += "&tid=" + cityId;
            parameters += "&type=1";
            string cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityId);
            return GetInstance().ExecuteCommand(COMMAND_BUILD_BUILDING, parameters, true, cookies);
        }

        /// <summary>
        /// Buy resources.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="tonsToBuy"></param>
        /// <param name="price"></param>
        /// <param name="seqNo"></param>
        /// <param name="seller"></param>
        /// <param name="resourceType">1: food, 2: woods, 3: stone, 4: iron</param>
        /// <param name="buyinCount"></param>
        /// <param name="buyinPrice"></param>
        private static void BuyResource(int cityId, int tonsToBuy, int price, int seqNo, int seller, int resourceType, int buyinCount, int buyinPrice)
        {
            string parameter = "";
            parameter += "buyincount=" + buyinCount;
            parameter += "&buyinprice=" + buyinPrice;
            parameter += "&res_jsondata={floatid:\"res1\",desc:\"Lương thực\",value:0,onchange:\"Dipan.SanGuo.Build.Market.DropDown3()\",data:[{text:\"Lương thực\",value:0},{text:\"Gỗ\",value:1},{text:\"Đá\",value:2},{text:\"Sắt\",value:3},]}";
            parameter += "&count=" + tonsToBuy;
            parameter += "&countprice=" + price;
            parameter += "&res_hidden=" + (resourceType - 1);
            parameter += "&seller=" + seller;
            parameter += "&seqno=" + seqNo;
            parameter += "&tid=0";
            parameter += "&type3=" + (resourceType);
            GetInstance().ExecuteCommand(COMMAND_BUY_RESOURCE, parameter, false, cityId);
        }

        /// <summary>
        /// Buys specified resource in a specified city.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="resourceType">1: food, 2: woods, 3: stone, 4: iron</param>
        /// <param name="buyAmount">amount to buy (in raw unit)</param>
        /// <param name="gold">gold quota</param>
        public static void BuyResource(int cityId, int resourceType, int buyAmount, ref long gold)
        {
            string cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityId);
            Hashtable market = LVHelper.CommonCommandHelper.GetMarketSellers(resourceType);
            if (market == null) 
            {
                return;
            }
            ArrayList marketInfo = (ArrayList)market["infos"];
            int tonsToBuy = (buyAmount / 1000)+1;
            for (int j = 0; j < marketInfo.Count; j++)
            {
                Hashtable marketInfoEntry = (Hashtable)marketInfo[j];
                int entryPrice  = int.Parse(marketInfoEntry["price"].ToString());
                int entryCount  = int.Parse(marketInfoEntry["count"].ToString());
                int entrySeller = int.Parse(marketInfoEntry["seller"].ToString());
                int entrySeqNo  = int.Parse(marketInfoEntry["seqno"].ToString());

                if (gold < Math.Min(entryCount * entryPrice, tonsToBuy * entryPrice))
                {
                    //out of gold
                    break;
                }
                if (tonsToBuy >= entryCount)
                {
                    BuyResource(cityId, entryCount, entryPrice, entrySeqNo, entrySeller, 1, entryCount, entryPrice);
                    tonsToBuy -= entryCount;
                    gold -= entryCount * entryPrice;
                }
                else
                {
                    BuyResource(cityId, tonsToBuy, entryPrice, entrySeqNo, entrySeller, 1, entryCount, entryPrice);
                    gold -= tonsToBuy * entryPrice;
                    tonsToBuy = 0;
                }
                if (tonsToBuy <= 0) break;
            }
        } //end BuyResource

        /// <summary>
        /// Gets market average price for a resource.
        /// </summary>
        /// <param name="resourceType">1: food, 2: woods, 3: stone, 4: iron</param>
        /// <returns></returns>
        public static int GetAvgPrice(int resourceType)
        {
            string parameter = "";
            parameter += "type=" + resourceType;

            Hashtable result = GetInstance().ExecuteCommand(COMMAND_GET_RESOURCE_AVG_PRICE, parameter, true);
            return int.Parse(result["DATA"].ToString());
        }

        /// <summary>
        /// Gets market minimum price for a resource.
        /// </summary>
        /// <param name="resourceType">1: food, 2: woods, 3: stone, 4: iron</param>
        /// <returns></returns>
        public static int GetMinPrice(int resourceType)
        {
            Hashtable market = CommonCommandHelper.GetMarketSellers(resourceType);
            if (market == null)
            {
                return -1;
            }

            ArrayList infos = (ArrayList)market["infos"];
            if (infos == null || infos.Count == 0)
            {
                return -1;
            }

            Hashtable infoitem = (Hashtable)infos[0]; //get the first item, it's always the lowest price listed
            if (infoitem == null || infoitem["price"] == null)
            {
                return -1;
            }

            return int.Parse(infoitem["price"].ToString());
        }

        /// <summary>
        /// Sells resource.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="amountInTon"></param>
        /// <param name="price"></param>
        /// <param name="resourceType">1: food, 2: woods, 3: stone, 4: iron</param>
        /// <returns></returns>
        public static Hashtable SellResource(int cityId, int amountInTon, int price, int resourceType)
        {
            string parameter = "";
            parameter += "countprice1=" + amountInTon * price;
            parameter += "&res1_hidden=" + (resourceType - 1);
            parameter += "&res1_jsondata={floatid:\"res1\",desc:\"Lương thực\",value:0,onchange:\"Dipan.SanGuo.Build.Market.DropDown3()\",data:[{text:\"Lương thực\",value:0},{text:\"Gỗ\",value:1},{text:\"Đá\",value:2},{text:\"Sắt\",value:3},]}";
            parameter += "&sellcount1=" + amountInTon;
            parameter += "&sellprice1=" + price;
            parameter += "&tid=0";
            parameter += "&type1=" + resourceType;
            return GetInstance().ExecuteCommand(COMMAND_SELL_RESOURCE, parameter, true, cityId);
        }

        /// <summary>
        /// Updates my market listing price.
        /// </summary>
        /// <param name="seqNo"></param>
        /// <param name="price"></param>
        public static void UpdateMyMarketPrice(int seqNo, int price)
        {
            GetInstance().ExecuteCommand(COMMAND_UPDATE_MY_MARKET_PRICE, "price=" + price + "&seqno=" + seqNo + "&tid=0", true);
        }
    } //end class
} //end namespace
