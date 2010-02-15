/* This file is to separate the business actions of FrmMain from the GUI's auto-generated actions
 * Contains actions for the Login tab
 */

using System.Windows.Forms;
using System.Collections;
using System;
using System.Threading;

namespace LVAuto.LVForm
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Validates the login form
        /// </summary>
        /// <returns></returns>
        private bool ValidateLoginForm()
        {
            if (Login_txtUsername.Text.Trim() == "")
            {
                LVUtils.MsgBoxUtils.WarningBox("Chưa nhập Username!");
                return false;
            }
            if (Login_txtPassword.Text.Trim() == "")
            {
                LVUtils.MsgBoxUtils.WarningBox("Chưa nhập password!");
                return false;
            }

            ProxyProtocol = cbProxyProtocol.SelectedItem.ToString().Trim();

            ProxyServer = txtProxyServer.Text.Trim();
            ProxyPort = txtProxyPort.Text.Trim();
            ProxyUser = txtProxyUser.Text.Trim();
            ProxyPass = txtProxyPassword.Text.Trim();

            if (ProxyProtocol != "NONE")
            {
                if (ProxyServer == "")
                {
                    LVUtils.MsgBoxUtils.WarningBox("Chưa nhập proxy server!");
                    return false;
                }

                if (ProxyPort == "")
                {
                    LVUtils.MsgBoxUtils.WarningBox("Chưa nhập proxy port!");
                    return false;
                }
            }

            switch (ProxyProtocol)
            {
                case "NONE":
                    ProxyServer = "";
                    ProxyPort = "";
                    ProxyUser = "";
                    ProxyPass = "";
                    break;
                case "HTTP":
                    ProxyUser = "";
                    ProxyPass = "";
                    break;
                case "SOCKS4":
                    ProxyPass = "";
                    break;
                case "SOCKS4a":
                    ProxyPass = "";
                    break;
                case "SOCKS5":
                    break;
            }
            return true;
        }

        /// <summary>
        /// Loads list of LV Servers and populates the dropdown list on the login form.
        /// </summary>
        private void LVLoadServerList()
        {
            const string CONFIG_FILE = "ServerList.xml";
            try
            {
                WriteLog("Loading list of servers from [" + CONFIG_FILE + "]...");

                const string pathServer = "/LVAuto/server";

                //load server list
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load(CONFIG_FILE);
                
                System.Xml.XmlNodeList nodes = xmlDoc.SelectNodes(pathServer);
                System.Collections.ArrayList servers = new System.Collections.ArrayList();
                for (int i = 0; i < nodes.Count; i++)
                {
                    servers.Add(nodes[i].InnerText);
                }
                if (servers.Count > 0)
                {
                    Login_dropdownServerList.Items.Clear();
                    Login_dropdownServerList.Items.AddRange(servers.ToArray());
                    Login_dropdownServerList.SelectedIndex = 0;
                }

                WriteLog("Number of servers: " + servers.Count);
            }
            catch (Exception ex)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Không load được danh sách server từ file [" + CONFIG_FILE + "]:\r\n" + ex.Message);
                this.Close();
            }
        }

        /// <summary>
        /// Login to LV Server
        /// </summary>
        private void LVLogin()
        {
            if (!ValidateLoginForm()) return;

            int loginCount = 0, tokenIndex = 0;
            Hashtable temp = null;
            do
            {
                Login_btnLogin.Enabled = false;
                ShowLoadingLabel("Logging in...");
                try
                {
                    Hashtable webclientLoginForm = LVAuto.LVWeb.LVClient.LoginForm();
                    //ChallengeScript":"eval(\u0027835+409\u0027)
                    //calculate antibot
                    string data = webclientLoginForm["DATA"].ToString();
                    tokenIndex = data.IndexOf("\\u0027");
                    int j = data.IndexOf("\\u0027", tokenIndex + 1);
                    string[] oper = data.Substring(tokenIndex + 6, (j - tokenIndex) - 6).Split(new char[] { '+' });
                    int antibot = int.Parse(oper[0].Trim()) + int.Parse(oper[1].Trim());
                    Hashtable logindata = LVWeb.ParseHeader.GetDataFromForm(data);
                    logindata["NoBot1$NoBot1_NoBotExtender_ClientState"] = antibot.ToString();
                    logindata["TxtPass"] = Login_txtPassword.Text;
                    logindata["TxtUserName"] = Login_txtUsername.Text;
                    logindata.Remove("imgLoginLogOut");
                    logindata["imgLoginLogOut.x"] = 10;
                    logindata["imgLoginLogOut.y"] = 10;
                    LVAuto.LVWeb.LVClient.LoginFormData = logindata;
                    LVAuto.LVWeb.LVClient.lvusername = Login_txtUsername.Text;
                    LVAuto.LVWeb.LVClient.lvpassword = Login_txtPassword.Text;
                    temp = LVAuto.LVWeb.LVClient.Login();
                    loginCount++;
                }
                catch (Exception)
                {
                    loginCount++;
                }
            } while (temp == null && loginCount < 3);

            if (temp == null)
            {
                tokenIndex = 0;
            }
            else
            {
                string data = temp["DATA"].ToString();
                tokenIndex = data.IndexOf("\\u0027");
            }

            if (tokenIndex > -1)
            {
                HideLoadingLabel();
                Login_btnLogin.Enabled = true;
                LVUtils.MsgBoxUtils.WarningBox("Đăng nhập thật bại!");
            }
            else
            {
                temp = LVAuto.LVWeb.LVClient.LoginPlay();
                LVAuto.LVWeb.LVClient.LoginHtml = temp["DATA"].ToString();
                if (!LVLoadServerData())
                {
                    LVUtils.MsgBoxUtils.WarningBox("Không load được server data!");
                    Login_btnLogin.Enabled = true;
                    return;
                }
                Login_txtPassword.Enabled = false;
                Login_txtUsername.Enabled = false;
                Login_btnLogin.Enabled = false;
                //frLoading.Close();

                ShowCoDanhTuongViengTham();
                HideLoadingLabel();
                LVUtils.MsgBoxUtils.InfoBox("Đã login thành công. Hãy quên phần login đi nhé. Auto thôi.");
                this.strVersion += " - Welcome " + LVAuto.LVWeb.LVClient.lvusername;
                this.Text = strVersion;
                this.AppNotifyIcon.Text = strVersion;
                //LVAuto.Web.LVWeb.processCheckImage();
            }
        }
    }
}
