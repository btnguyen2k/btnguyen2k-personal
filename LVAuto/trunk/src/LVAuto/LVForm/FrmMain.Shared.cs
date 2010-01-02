/* This file contains shared private methods */

using System.Collections;
using System.Threading;
using System;
using System.Windows.Forms;

using LVAuto.LVWeb;

namespace LVAuto.LVForm
{
    public partial class FrmMain : System.Windows.Forms.Form
    {
        /// <summary>
        /// Loads LV Data from server
        /// </summary>
        /// <returns></returns>
        private bool LVLoadServerData()
        {
            AppNotifyIcon.Text = "Đang load dữ liệu từ server, vui lòng chờ. Máy có thể đứng đó!";

            if (LVClient.LoginHtml == null)
            {
                LVUtils.MsgBoxUtils.WarningBox("LVClient.LoginHtml is null!");
                return false;
            }

            try
            {
                //Enables all tabs except for the "login" one
                for (int i = 1; i < tabMainTab.TabPages.Count; i++)
                {
                    tabMainTab.TabPages[i].Enabled = true;
                }

                Hashtable lastlogindata = LVWeb.ParseHeader.GetDataFromForm(LVAuto.LVWeb.LVClient.LoginHtml);
                Hashtable lastlogin = LVWeb.LVClient.LoginPartner(lastlogindata["uid"].ToString(), lastlogindata["uname"].ToString(), lastlogindata["ulgtime"].ToString(), lastlogindata["pid"].ToString(), lastlogindata["sign"].ToString());
                LVWeb.LVClient.CurrentLoginInfo = new LVWeb.LoginInfo((string[])lastlogin["Set-Cookie"]);

                //Command.City.GetAllSimpleCity();

                //Command.City.UpdateAllSimpleCity();
                //Command.City.GetAllCity();

                chkAutoAll.Enabled = true;
                IsLogin = true;
                AppNotifyIcon.Text = strVersion;

                // bao bi tan cong, hungtv rem
                LVCITYTASK = new LVAuto.LVForm.LVThread.CITYTASK();
                LVCITYTASK.Auto();

                LVAUTOTASK = new LVAuto.LVForm.LVThread.AUTOTASK(lblLoadingResMessage);
                LVAUTOTASK.Auto();

                LVBUILD = new LVAuto.LVForm.LVThread.BUILD(lblBUILDMESSAGE);

                LVDEL = new LVAuto.LVForm.LVThread.DEL(lblDELMESSAGE);
                LVSELL = new LVAuto.LVForm.LVThread.SELL(lblSELLMESSAGE);
                LVBUYRES = new LVAuto.LVForm.LVThread.BUYRES(lblBUYRESMESSAGE);
                LVTHAOPHAT = new LVAuto.LVForm.LVThread.THAOPHAT(lblTHAOPHATMESSAGE);
                LVUPGRADE = new LVAuto.LVForm.LVThread.UPGRADE(lblUPGEADEMESSAGE);
                LVANUI = new LVAuto.LVForm.LVThread.ANUI(lblANUIMESSAGE);
                LVVANCHUYEN = new LVAuto.LVForm.LVThread.VANCHUYEN(lblVANCHUYENMESSAGE);
                LVMOVEDOANHTRAI = new LVAuto.LVForm.LVThread.MOVEDOANHTRAI(lblMOVEDOANHTRAI);
                LVSIKHI = new LVAuto.LVForm.LVThread.SIKHI(lblSIKHIMESSAGE);
                LVBUYWEPON = new LVAuto.LVForm.LVThread.BUYWEPON(lblBUYWEPONMESSAGE);

                LVBIENCHE = new LVAuto.LVForm.LVThread.BIENCHE(lblBIENCHEMESSAGE);
                LVDAOMO = new LVAuto.LVForm.LVThread.PHAIQUANVANDAOMO(lblDIEUPHAIMESSAGE);
                LVLOIDAI = new LVAuto.LVForm.LVThread.LOIDAI(lblDIEUPHAIMESSAGE);

                LVAUTOVCVK = new LVAuto.LVForm.LVThread.AUTOVANCHUYENVK(lblAUTOVCVKMESSAGE);
                //LVCITYTASK.IsRun = true;
                LVAUTOBINHMAN = new LVAuto.LVForm.LVThread.AUTOBINHMAN(lblAUTOBINHMANMESSAGE);
                LVAUTOCALLMAN = new LVAuto.LVForm.LVThread.AUTOCALLMAN(lblAUTOCALLMANMESSAGE);

                LVAuto.LVWeb.LVClient.debug = false;
                LVAuto.LVWeb.LVClient.firstlogin = false;

                tabLogin.Select();
                tabLogin.Focus();

                //Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                LVUtils.MsgBoxUtils.WarningBox("Unexpected error:\r\n"+ex.Message);
                return false;
            }
            return true;
        }
    }
}
