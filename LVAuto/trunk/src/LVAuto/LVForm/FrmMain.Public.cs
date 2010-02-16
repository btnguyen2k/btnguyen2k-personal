/* This file contains methods that are shared to the public */

using System;
namespace LVAuto.LVForm
{
    public partial class FrmMain : System.Windows.Forms.Form
    {
        private static LVAuto.LVThread.AutoSellResources THREAD_SELL_RESOURCES;
        private static LVAuto.LVThread.AutoConstruct THREAD_CONSTRUCT;
        private static LVThread.DEL LVDEL;
        private static LVThread.BUYRES LVBUYRES;
        private static LVThread.THAOPHAT LVTHAOPHAT;
        private static LVThread.UPGRADE LVUPGRADE;
        private static LVThread.ANUI LVANUI;
        private static LVThread.VANCHUYEN LVVANCHUYEN;
        private static LVThread.MOVEDOANHTRAI LVMOVEDOANHTRAI;
        private static LVThread.SIKHI LVSIKHI;
        private static LVThread.BUYWEPON LVBUYWEPON;
        private static LVThread.CITYTASK LVCITYTASK;
        private static LVThread.BIENCHE LVBIENCHE;
        private static LVThread.PHAIQUANVANDAOMO LVDAOMO;
        private static LVThread.LOIDAI LVLOIDAI;
        private static LVThread.AUTOTASK LVAUTOTASK;
        private static LVThread.AUTOVANCHUYENVK LVAUTOVCVK;
        private static LVThread.AUTOBINHMAN LVAUTOBINHMAN;
        private static LVThread.AUTOCALLMAN LVAUTOCALLMAN;

        delegate void SetTextCallback(string text);

        /// <summary>
        /// Writes a log message
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLog(string msg)
        {
            if (this.Log_txtLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(WriteLog);
                this.Log_txtLog.Invoke(d, new object[] { msg });
            }
            else
            {
                Log_txtLog.AppendText(msg + "\r\n");
                //scroll to end
                Log_txtLog.SelectionStart = Log_txtLog.TextLength;
                Log_txtLog.ScrollToCaret();
            }            
        }

        /// <summary>
        /// Sets "enable" attributes of all tabs
        /// </summary>
        /// <param name="enable"></param>
        public void SetAllTabsEnable(bool enable)
        {
            for (int i = 0; i < tabMainTab.TabPages.Count; i++)
            {
                if (!tabMainTab.TabPages[i].Text.Equals("Log"))
                {
                    tabMainTab.TabPages[i].Enabled = enable;
                }
                else
                {
                    //always enable the Log tab
                    tabMainTab.TabPages[i].Enabled = true;
                }
            }
        }

        /// <summary>
        /// Stops all the Auto threads
        /// </summary>
        private void StopAllAutoThreads()
        {
            if (THREAD_CONSTRUCT.IsRunning)
            {
                THREAD_CONSTRUCT.Stop();
            }

            if (LVDEL.IsRun) 
            { 
                LVDEL.Stop(); 
            }

            if (THREAD_SELL_RESOURCES.IsRunning)
            {
                THREAD_SELL_RESOURCES.Stop();
            }

            if (LVBUYRES.IsRun)
            {
                LVBUYRES.Stop();
            }
                
            if (LVTHAOPHAT.IsRun)
            {
                LVTHAOPHAT.Stop();
            }

            if (LVUPGRADE.IsRun)
            {
                LVUPGRADE.Stop();
            }

            if (LVANUI.IsRun)
            {
                LVANUI.Stop();
            }

            if (LVVANCHUYEN.IsRun)
            {
                LVVANCHUYEN.Stop();
            }

            if (LVMOVEDOANHTRAI.IsRun)
            {
                LVMOVEDOANHTRAI.Stop();
            }

            if (LVSIKHI.IsRun)
            {
                LVSIKHI.Stop();
            }

            if (LVBUYWEPON.IsRun)
            {
                LVBUYWEPON.Stop();
            }

            if (LVCITYTASK.IsRun)
            {
                LVCITYTASK.Stop();
            }

            if (LVBIENCHE.IsRun)
            {
                LVBIENCHE.Stop();
            }

            if (LVDAOMO.IsRun)
            {
                LVDAOMO.Stop();
            }

            if (LVAUTOTASK.IsRun)
            {
                LVAUTOTASK.Stop();
            }

            if (LVLOIDAI.IsRun)
            {
                LVLOIDAI.Stop();
            }

            if (LVAUTOVCVK.IsRun)
            {
                LVAUTOVCVK.Stop();
            }

            if (LVAUTOBINHMAN.IsRun)
            {
                LVAUTOBINHMAN.Stop();
            }

            if (LVAUTOCALLMAN.IsRun)
            {
                LVAUTOCALLMAN.Stop();
            }

            if (hook.thpause.IsRun)
            {
                hook.thpause.Stop();
            }
        } //end StopAllAutoThreads

        /// <summary>
        /// Starts all Auto threads
        /// </summary>
        private void StartAllAutoThreads()
        {
            if (Auto_checkAutoSellResources.Checked && !THREAD_SELL_RESOURCES.IsRunning)
            {
                //THREAD_SELL_RESOURCES.GetParameter(BanTaiNguyen, int.Parse(SellRes_txtTimer.Text) * 60 * 1000);
                THREAD_SELL_RESOURCES.SetUp(int.Parse(SellRes_txtTimer.Text) * 60 * 1000);
                pnSELL.Enabled = false;
                THREAD_SELL_RESOURCES.Start();
            }

            if (Auto_checkAutoBuyResources.Checked && !LVBUYRES.IsRun)
            {
                LVBUYRES.GetParameter(MuaTaiNguyen);
                pnLVBUYRES.Enabled = false;
                LVBUYRES.Auto();
            }

            if (Auto_checkAutoQuest.Checked && !LVTHAOPHAT.IsRun)
            {
                int g1 = 0, g2 = 0, g3 = 0, g4 = 0, g5 = 0;
                try
                {
                    g1 = ((Command.CityObj.MilitaryGeneral)Quest_GeneralsInCity.CheckedItems[0]).Id;
                    g2 = ((Command.CityObj.MilitaryGeneral)Quest_GeneralsInCity.CheckedItems[1]).Id;
                    g3 = ((Command.CityObj.MilitaryGeneral)Quest_GeneralsInCity.CheckedItems[2]).Id;
                    g4 = ((Command.CityObj.MilitaryGeneral)Quest_GeneralsInCity.CheckedItems[3]).Id;
                    g5 = ((Command.CityObj.MilitaryGeneral)Quest_GeneralsInCity.CheckedItems[4]).Id;
                }
                catch (Exception ex)
                {
                }
                try
                {
                    int skmin = Quest_txtMinMorale.Text.Trim().Length != 0 ? int.Parse(Quest_txtMinMorale.Text.Trim()) : 50;
                    bool tubienche = Quest_checkAutoTroop.Checked;

                    LVTHAOPHAT.GetParameter(TuongDiThaoPhatList, int.Parse(Quest_txtTimer.Text) * 60 * 1000);
                    pnTHAOPHAT.Enabled = false;
                    LVTHAOPHAT.Auto();
                }
                catch (Exception ex)
                {
                    lblTHAOPHATMESSAGE.Text = "Chưa chọn đúng tham số";
                    Auto_checkAutoQuest.Checked = false;
                }
            }

            if (Auto_checkAutoConstruct.Checked && !THREAD_CONSTRUCT.IsRunning)
            {
                THREAD_CONSTRUCT.SetUp(int.Parse(txtBUILDCHECK.Text) * 60 * 1000);
                Construct_treeBuilding.Enabled = false;
                Construct_btnReloadBuildings.Enabled = false;
                THREAD_CONSTRUCT.Start();
            }

            if (Auto_checkAutoDestruct.Checked && !LVDEL.IsRun)
            {
                LVDEL.GetParameter(tvDEL, int.Parse(txtDELCHECK.Text) * 60 * 1000);
                tvDEL.Enabled = false;
                LVDEL.Auto();
            }

            if (chkAutoUpDanSo.Checked && !LVANUI.IsRun)
            {
                LVANUI.GetParameter(AnuiForAuto, chkANUI_TuMuaLua.Checked, long.Parse(txtANUI_VangAnToan.Text), int.Parse(txtCHECKANUI.Text) * 60 * 1000);
                chklstAnUi.Enabled = false;
                LVANUI.Auto();
            }

            if (chkAutoVanchuyen.Checked && !LVVANCHUYEN.IsRun)
            {
                LVVANCHUYEN.GetParameter(pnVanchuyen, int.Parse(txtCHECKVANCHUYEN.Text) * 60 * 1000);
                pnVanchuyen.Enabled = false;
                LVVANCHUYEN.Auto();
            }

            if (chkAutoMove.Checked && !LVMOVEDOANHTRAI.IsRun)
            {
                LVMOVEDOANHTRAI.GetParameter(DiChuyenTrai, int.Parse(txtCHECKMOVE.Text) * 60 * 1000);
                pnDoanhTrai.Enabled = false;
                LVMOVEDOANHTRAI.Auto();
            }

            if (Auto_checkAutoMorale.Checked && !LVSIKHI.IsRun)
            {
                LVSIKHI.GetParameter(tvSIKHI, int.Parse(txtOneSiKhi.Text), int.Parse(txtCHECKSIKHI.Text) * 60 * 1000);
                tvSIKHI.Enabled = false;
                LVSIKHI.Auto();
            }

            if (chkAutobuyWepon.Checked && !LVBUYWEPON.IsRun)
            {
                LVBUYWEPON.GetParameter(int.Parse(txtCOUNTWEPON.Text), pnWepon, int.Parse(txtCHECKSIKHI.Text) * 60 * 1000);
                pnWepon.Enabled = false;
                LVBUYWEPON.Auto();
            }

            if (Auto_checkAutoTroops.Checked && !LVBIENCHE.IsRun)
            {
                if (LVBIENCHE == null)
                {
                    LVBIENCHE = new LVAuto.LVForm.LVThread.BIENCHE(lblBIENCHEMESSAGE);
                }
                LVBIENCHE.GetParameter(10, int.Parse(txtTabBienCheTimeCheck.Text) * 60 * 1000);
                cboBienCheCity.Enabled = false;
                chklstGeneral.Enabled = false;
                btBiencheAccept.Enabled = false;
                LVBIENCHE.Auto();
            }

            if (chkAUTOVCVK.Checked && !LVAUTOVCVK.IsRun)
            {
                LVAUTOVCVK.GetParameter(VanChuyenVuKhi, int.Parse(txtCHECKVANCHUYENVUKHI.Text) * 60 * 1000);
                pnVanChuyenVK.Enabled = false;
                LVAUTOVCVK.Auto();
            }

            if (!LVAUTOTASK.IsRun) LVAUTOTASK.Auto();
            if (!LVCITYTASK.IsRun) LVCITYTASK.Auto();
        } //end StartAllAutoThreads
    }
}
