using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LVAuto.LVHelper
{
    /// <summary>
    /// Base class for other helpers
    /// </summary>
    public abstract class BaseHelper
    {
        public const string RESOURCE_FOOD = "FOOD";
        public const string RESOURCE_WOODS = "WOODS";
        public const string RESOURCE_STONE = "STONE";
        public const string RESOURCE_IRON = "IRON";
        public const string RESOURCE_GOLD = "GOLD";
        public const string RESOURCE_TIME = "TIME";
        public const string RESOURCE_MAX = "MAX_RESOURCE";

        public const string LV_DOMAIN = "linhvuong.zooz.vn";
        public const string HEADER_USER_AGENT = "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.5) Gecko/2008120122 Firefox/3.0.5\n";
        public const string HEADER_ACCEPT_TYPE = "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\n";
        public const string HEADER_ACCEPT_LANGUAGE = "Accept-Language: en-us,en;q=0.5\n";
        public const string HEADER_ACCEPT_CHARSET = "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7\n";
        public const string HEADER_KEEP_ALIVE = "Keep-Alive: 300\n";
        public const string HEADER_CONNECTION = "Connection: keep-alive\n";
        public const string HEADER_X = "X-Requested-With: XMLHttpRequest\nX-Prototype-Version: 1.5.0\n";
        public const string HEADER_CONTENT_TYPE = "Content-Type: application/x-www-form-urlencoded\n";
        public const string HEADER_CACHE_CONTROL = "Pragma: no-cache\nCache-Control: no-cache\n";

        /// <summary>
        /// Constructs a new BaseHelper object.
        /// </summary>
        protected BaseHelper()
        {
        }

        /// <summary>
        /// Executes a command with default cookie string.
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="parameters"></param>
        /// <param name="waitForResult"></param>
        /// <returns></returns>
        public Hashtable ExecuteCommand(int commandId, string parameters, bool waitForResult)
        {
            return ExecuteCommand(commandId, parameters, waitForResult, LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString());
        }

        /// <summary>
        /// Executes a command against a city (specified by cityId) with default cookie string.
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="parameters"></param>
        /// <param name="waitForResult"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public Hashtable ExecuteCommand(int commandId, string parameters, bool waitForResult, int cityId)
        {
            return ExecuteCommand(commandId, parameters, waitForResult, LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityId));
        }

        /// <summary>
        /// Executes a command with supplied cookie string. Sub-class to implement this method.
        /// </summary>
        /// <param name="commandid"></param>
        /// <param name="parameters"></param>
        /// <param name="waitForResult"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public abstract Hashtable ExecuteCommand(int commandid, string parameters, bool waitForResult, string cookies);

        /// <summary>
        /// Executes a command with default cookie string. Retres if failed.
        /// </summary>
        /// <param name="numRetries"></param>
        /// <param name="commandId"></param>
        /// <param name="parameters"></param>
        /// <param name="waitForResult"></param>
        /// <returns></returns>
        public Hashtable ExecuteCommandRetry(int numRetries, int commandId, string parameters, bool waitForResult)
        {
            return ExecuteCommandRetry(numRetries, commandId, parameters, waitForResult, LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString());
        }

        /// <summary>
        /// Executes a command against a city (specified by cityId) with default cookie string. Retries if failed.
        /// </summary>
        /// <param name="numRetries"></param>
        /// <param name="commandId"></param>
        /// <param name="parameters"></param>
        /// <param name="waitForResult"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public Hashtable ExecuteCommandRetry(int numRetries, int commandId, string parameters, bool waitForResult, int cityId)
        {
            return ExecuteCommandRetry(numRetries, commandId, parameters, waitForResult, LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityId));
        }

        /// <summary>
        /// Executes a command with supplied cookie string. Retries if failed.
        /// </summary>
        /// <param name="numRetries"></param>
        /// <param name="commandId"></param>
        /// <param name="parameters"></param>
        /// <param name="waitForResult"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public Hashtable ExecuteCommandRetry(int numRetries, int commandId, string parameters, bool waitForResult, string cookies)
        {
            for (int i = 0; i < numRetries; i++)
            {
                Hashtable result = ExecuteCommand(commandId, parameters, waitForResult, cookies);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    } //end class
} //end namespace
