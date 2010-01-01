/* This file is to separate the business actions of FrmMain from the GUI's auto-generated actions */

using System.Windows.Forms;
using System.Collections;
using System;

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
            if (txtUsername.Text.Trim() == "")
            {
                LVUtils.MsgBoxUtils.WarningBox("Chưa nhập Username!");
                return false;
            }
            if (txtLvPassword.Text.Trim() == "")
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
        /// Login to LV Server
        /// </summary>
        private void LVLogin()
        {
            if (!ValidateLoginForm()) return;

            int count = 0;
            Hashtable temp = null;
            Hashtable loginform;
            Hashtable logindata;
            string data;
            int i = 0;

            do
            {
                btnLogin.Enabled = false;
                ShowLoadingLabel("Logging in...");
                try
                {
                    loginform = LVAuto.LVWeb.LVClient.LoginForm();
                    //ChallengeScript":"eval(\u0027835+409\u0027)
                    //calculate antibot
                    data = loginform["DATA"].ToString();
                    i = data.IndexOf("\\u0027");
                    int j = data.IndexOf("\\u0027", i + 1);
                    string[] oper = data.Substring(i + 6, (j - i) - 6).Split(new char[] { '+' });
                    int antibot = int.Parse(oper[0].Trim()) + int.Parse(oper[1].Trim());
                    logindata = LVWeb.ParseHeader.GetDataFromForm(data);
                    logindata["NoBot1$NoBot1_NoBotExtender_ClientState"] = antibot.ToString();
                    logindata["TxtPass"] = txtLvPassword.Text;
                    logindata["TxtUserName"] = txtUsername.Text;
                    logindata.Remove("imgLoginLogOut");
                    logindata["imgLoginLogOut.x"] = 10;
                    logindata["imgLoginLogOut.y"] = 10;
                    LVAuto.LVWeb.LVClient.LoginFormData = logindata;
                    LVAuto.LVWeb.LVClient.lvusername = txtUsername.Text;
                    LVAuto.LVWeb.LVClient.lvpassword = txtLvPassword.Text;
                    temp = LVAuto.LVWeb.LVClient.Login();
                    count++;
                }
                catch (Exception ex)
                {
                    count++;
                }
            } while (temp == null && count < 3);


            if (temp == null)
            {
                i = 0;
            }
            else
            {
                data = temp["DATA"].ToString();
                i = data.IndexOf("\\u0027");
            }

            if (i > -1)
            {
                HideLoadingLabel();
                btnLogin.Enabled = true; ;
                MessageBox.Show("Sai pass hoặc đăng nhập thất bại. hê hê");
            }
            else
            {
                temp = LVAuto.LVWeb.LVClient.LoginPlay();
                LVAuto.LVWeb.LVClient.LoginHtml = temp["DATA"].ToString();
                if (!FirstLogin_())
                {
                    MessageBox.Show("Sai pass hoặc đăng nhập thất bại. hê hê");
                    btnLogin.Enabled = true; ;
                    return;
                }
                txtLvPassword.Enabled = false;
                txtUsername.Enabled = false;
                btnLogin.Enabled = false;
                //frLoading.Close();

                ShowCoDanhTuongViengTham();
                HideLoadingLabel();
                MessageBox.Show("Đã login thành công. Hãy quên phần login đi nhé. Auto thôi.");

                this.Text = strVersion + " - Welcome " + LVAuto.LVWeb.LVClient.lvusername;

                notifyIcon1.Text = strVersion + " - " + LVAuto.LVWeb.LVClient.lvusername;
                //LVAuto.Web.LVWeb.processCheckImage();



            }

        }
    }
}
