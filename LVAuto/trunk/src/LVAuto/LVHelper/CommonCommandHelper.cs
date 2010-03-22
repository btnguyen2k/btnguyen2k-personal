using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LVAuto.LVHelper
{
    /// <summary>
    /// Helper class for Common commands.
    /// </summary>
    public class CommonCommandHelper : BaseHelper
    {
        private static object LocalLock = new object();

        public const int COMMAND_GET_MARKET_INFO = 38;
        public const int COMMAND_GET_NATIONAL_TREASURE_INFO = 24;

        private static CommonCommandHelper instance;

        /// <summary>
        /// Singleton implementation.
        /// </summary>
        /// <returns></returns>
        public static CommonCommandHelper GetInstance()
        {
            if (instance == null)
            {
                instance = new CommonCommandHelper();
            }
            return instance;
        }

        /// <summary>
        /// Constructs a new CommonCommandHelper object.
        /// </summary>
        protected CommonCommandHelper()
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

            string header = "POST http://s" + LVAuto.LVWeb.LVClient.Server + "." + LV_DOMAIN + "/GateWay/Common.ashx?id=" + commandId + "&0.05861361440438828  HTTP/1.1\n";
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
        /// Gets information about helmet items in national treasure.
        /// </summary>
        /// <returns>result["list"] is list of helmets, each item is an array where [0] is the itemId, and [1] is the itemName</returns>
        public static Hashtable GetNationalTreasureHelmets()
        {
            string parameter = "lType=1";
            return GetInstance().ExecuteCommand(COMMAND_GET_NATIONAL_TREASURE_INFO, parameter, true);
        }

        /// <summary>
        /// Gets information about weapon items in national treasure.
        /// </summary>
        /// <returns>result["list"] is list of weapons, each item is an array where [0] is the itemId, and [1] is the itemName</returns>
        public static Hashtable GetNationalTreasureWeapons()
        {
            string parameter = "lType=2";
            return GetInstance().ExecuteCommand(COMMAND_GET_NATIONAL_TREASURE_INFO, parameter, true);
        }

        /// <summary>
        /// Gets information about armour items in national treasure.
        /// </summary>
        /// <returns>result["list"] is list of armours, each item is an array where [0] is the itemId, and [1] is the itemName</returns>
        public static Hashtable GetNationalTreasureArmours()
        {
            string parameter = "lType=3";
            return GetInstance().ExecuteCommand(COMMAND_GET_NATIONAL_TREASURE_INFO, parameter, true);
        }

        /// <summary>
        /// Gets information about mount items in national treasure.
        /// </summary>
        /// <returns>result["list"] is list of mounts, each item is an array where [0] is the itemId, and [1] is the itemName</returns>
        public static Hashtable GetNationalTreasureMounts()
        {
            string parameter = "lType=4";
            return GetInstance().ExecuteCommand(COMMAND_GET_NATIONAL_TREASURE_INFO, parameter, true);
        }

        /// <summary>
        /// Gets information about resource market
        /// </summary>
        /// <param name="resourceType">1=food, 2=wood, 3=stone, 4=iron</param>
        /// <returns></returns>
        public static Hashtable GetMarketSellers(int resourceType)
        {
            string parameter = "";
            parameter += "stab=1";
            parameter += "&tid=0";
            parameter += "&type=" + resourceType;
            return GetInstance().ExecuteCommand(COMMAND_GET_MARKET_INFO, parameter, true);
        }

        /// <summary>
        /// Gets my market queue info
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetMyMarketQueue()
        {
            Hashtable result = GetInstance().ExecuteCommand(COMMAND_GET_MARKET_INFO, "stab=2", true);
            return result != null ? (ArrayList)result["infos"] : null;
        }
    } //end class
} //end namespace
