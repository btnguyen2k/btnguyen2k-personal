using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LVAuto.LVHelper
{
    /// <summary>
    /// Helper class for City commands.
    /// </summary>
    public class CityCommandHelper : BaseHelper
    {
        private static object LocalLock = new object();

        public const int COMMAND_GET_BUILDING_INFO = 5;
        public const int COMMAND_GET_CITY_INFO = 6;
        public const int COMMAND_GET_ALL_CITIES_INFO = 7;
        public const int COMMAND_GET_RESOURCE_INFO = 20;
        public const int COMMAND_GET_TASKS_INFO = 37;

        private static CityCommandHelper instance;

        /// <summary>
        /// Singleton implementation.
        /// </summary>
        /// <returns></returns>
        public static CityCommandHelper GetInstance()
        {
            if (instance == null)
            {
                instance = new CityCommandHelper();
            }
            return instance;
        }

        /// <summary>
        /// Constructs a new CityCommandHelper object.
        /// </summary>
        protected CityCommandHelper()
        {
        }

        /// <summary>
        /// Executes a city command.
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

            string header = "POST http://s" + LVAuto.LVWeb.LVClient.Server + "." + LV_DOMAIN + "/GateWay/City.ashx?id=" + commandId + "&0.05861361440438828  HTTP/1.1\n";
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
        /// Gets buiding info and populates to city.
        /// </summary>
        /// <param name="city"></param>
        /// <param name="reload"></param>
        public static void GetAndPopulateBuildings(LVAuto.LVObj.City city, bool reload)
        {
            string cookies = LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(city.Id);
            lock (LocalLock)
            {
                if (!reload && city.AllBuildings != null)
                {
                    //return if city already has building info and not force reloading
                    return;
                }

                Hashtable temp = GetInstance().ExecuteCommandRetry(5, COMMAND_GET_BUILDING_INFO, "", true, cookies);
                if (temp == null)
                {
                    return;
                }

                ArrayList arrBuildingInfo = (ArrayList)temp["infos"];
                city.AllBuildings = new LVAuto.LVObj.Building[arrBuildingInfo.Count];

                for (int j = 0; j < arrBuildingInfo.Count; j++)
                {
                    ArrayList buildingInfo = (ArrayList)arrBuildingInfo[j];
                    int pid = int.Parse(buildingInfo[0].ToString());
                    int gid = int.Parse(buildingInfo[1].ToString());
                    string name = buildingInfo[2].ToString();
                    int level = int.Parse(buildingInfo[3].ToString());
                    city.AllBuildings[j] = new LVAuto.LVObj.Building(pid, gid, name, level);
                }
                Array.Sort(city.AllBuildings);
            }
        }

        /// <summary>
        /// Retrieves a city by id.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static LVAuto.LVObj.City GetCityById(int cityId)
        {
            foreach (LVAuto.LVObj.City city in LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities)
            {
                if (city.Id == cityId)
                {
                    return city;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets city's current tasks.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static Hashtable GetCityTasks(int cityId)
        {
            return GetInstance().ExecuteCommand(COMMAND_GET_TASKS_INFO, "ttid=0", true, cityId);
        }

        /// <summary>
        /// Gets simple city information.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static Hashtable GetSimpleCityInfo(int cityId)
        {
            return LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(COMMAND_GET_CITY_INFO, "", true, cityId);
        }

        /// <summary>
        /// Quickly updates info for all cities.
        /// </summary>
        public static void UpdateSimpleCityInfo()
        {
            Hashtable result = GetInstance().ExecuteCommand(COMMAND_GET_ALL_CITIES_INFO, "", true);
            if (result == null)
            {
                return;
            }

            ArrayList allCitiesInfo = (ArrayList)result["infos"];
            if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null || LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length != allCitiesInfo.Count)
            {
                LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities = new LVAuto.LVObj.City[allCitiesInfo.Count];
            }
            for (int i = 0; i < allCitiesInfo.Count; i++)
            {
                ArrayList cityInfo = (ArrayList)allCitiesInfo[i];
                int cityId = int.Parse(cityInfo[1].ToString());
                string name = cityInfo[2].ToString();
                int x = int.Parse(cityInfo[4].ToString());
                int y = int.Parse(cityInfo[5].ToString());
                int size = int.Parse(cityInfo[7].ToString());
                int parentId = int.Parse(cityInfo[6].ToString());
                LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i] = new LVAuto.LVObj.City(cityId, name, x, y, size, parentId);
                GetAndPopulateBuildings(LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i], false);
            }
        }

        /// <summary>
        /// Gets current resource information.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static Hashtable GetCurentResourceInCity(int cityId)
        {
            Hashtable temp = LVHelper.CityCommandHelper.GetInstance().ExecuteCommand(COMMAND_GET_RESOURCE_INFO, "", true, cityId);
            if (temp == null)
            {
                return null;
            }
            Hashtable result = new Hashtable();
            result.Add(RESOURCE_FOOD, int.Parse(((ArrayList)temp["food"])[0].ToString()));
            result.Add(RESOURCE_WOODS, int.Parse(((ArrayList)temp["wood"])[0].ToString()));
            result.Add(RESOURCE_STONE, int.Parse(((ArrayList)temp["stone"])[0].ToString()));
            result.Add(RESOURCE_IRON, int.Parse(((ArrayList)temp["iron"])[0].ToString()));
            result.Add(RESOURCE_MAX, int.Parse(temp["max_storage"].ToString()));

            temp = GetSimpleCityInfo(cityId);
            ArrayList goldarr = (ArrayList)temp["money"];
            long gold = long.Parse(goldarr[0].ToString());
            result.Add(RESOURCE_GOLD, gold);
    
            return result;
        }
    }
}
