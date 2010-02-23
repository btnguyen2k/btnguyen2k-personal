using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LVAuto.LVHelper
{
    /// <summary>
    /// Helper class for Building commands.
    /// </summary>
    public class BuildingCommandHelper : BaseHelper
    {
        private static object LocalLock = new object();

        public const int COMMAND_GET_BUILDING_INFO = 2;

        private static BuildingCommandHelper instance;

        /// <summary>
        /// Singleton implementation.
        /// </summary>
        /// <returns></returns>
        public static BuildingCommandHelper GetInstance()
        {
            if (instance == null)
            {
                instance = new BuildingCommandHelper();
            }
            return instance;
        }

        /// <summary>
        /// Constructs a new BuildingCommandHelper object.
        /// </summary>
        protected BuildingCommandHelper()
        {
        }

        /// <summary>
        /// Executes a building command.
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

            string header = "POST http://s" + LVAuto.LVWeb.LVClient.Server + "." + LV_DOMAIN + "/GateWay/build.ashx?id=" + commandId + "&0.05861361440438828  HTTP/1.1\n";
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
        /// Gets building's general information.
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="gid"></param>
        /// <param name="pid"></param>
        /// <param name="tab"></param>
        /// <returns></returns>
        public static Hashtable GetBuildingInfo(int cityId, int gid, int pid, int tab)
        {
            string cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityId);
            return GetInstance().ExecuteCommand(COMMAND_GET_BUILDING_INFO, "gid=" + gid + "&pid=-1" + "&tab=" + tab + "&tid=" + cityId, true, cookies);
        }

        /// <summary>
        /// Gets resources needed for upgrade building.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="building"></param>
        /// <returns>resources needed for upgrading as a hash, null indicates that upgrading condition does not meet</returns>
        public static Hashtable GetResourcesForUpgrade(int cityId, LVAuto.LVObj.Building building)
        {
            string cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityId);
            Hashtable temp = GetInstance().ExecuteCommand(COMMAND_GET_BUILDING_INFO, "gid=" + building.GId + "&pid=" + building.PId + "&tab=-1&tid=" + cityId, true, cookies);
            if (temp == null)
            {
                return null;
            }
            ArrayList upgradeResources = (ArrayList)temp["update"];
            if (upgradeResources == null)
            {
                //upgrading condition has not met?
                return null;
            }
            Hashtable result = new Hashtable();
            result.Add(RESOURCE_FOOD, int.Parse(upgradeResources[5].ToString()));
            result.Add(RESOURCE_WOODS, int.Parse(upgradeResources[6].ToString()));
            result.Add(RESOURCE_STONE, int.Parse(upgradeResources[7].ToString()));
            result.Add(RESOURCE_IRON, int.Parse(upgradeResources[8].ToString()));
            result.Add(RESOURCE_GOLD, int.Parse(upgradeResources[9].ToString()));
            result.Add(RESOURCE_TIME, int.Parse(upgradeResources[10].ToString()));
            return result;
        }

        /// <summary>
        /// Selects a building.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="building"></param>
        /// <returns></returns>
        public static bool SelectBuilding(int cityId, LVAuto.LVObj.Building building)
        {
            int gid = building.GId;
            int pid = building.PId;
            int tab = building.Tab;
            string cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityId);
            Hashtable result = GetInstance().ExecuteCommand(COMMAND_GET_BUILDING_INFO, "gid=" + gid + "&pid=-1" + "&tab=" + tab + "&tid=" + cityId, true, cookies);
            return (result["ret"].ToString() == "0");
        }

        /// <summary>
        /// Selects a building.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="building"></param>
        /// <param name="tab"></param>
        /// <returns></returns>
        public static bool SelectBuilding(int cityId, LVAuto.LVObj.Building building, int tab)
        {
            int gid = building.GId;
            int pid = building.PId;
            string cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityId);
            Hashtable result = GetInstance().ExecuteCommand(COMMAND_GET_BUILDING_INFO, "gid=" + gid + "&pid=-1" + "&tab=" + tab + "&tid=" + cityId, true, cookies);
            return (result["ret"].ToString() == "0");
        }

        /// <summary>
        /// Gets number of buildings we can build simultaneously.
        /// </summary>
        /// <returns></returns>
        public static int GetSimultaneousBuildings()
        {
            foreach (LVAuto.LVObj.City city in LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities)
            {
                Hashtable result = GetBuildingInfo(city.Id, 1, 8, 2);
                if (result != null & result["plus_left"].ToString().Trim() != "0")
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }
            return 1;
        }
    } //end class
} //end namespace
