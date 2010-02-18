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

                Auto_checkAutoAll.Enabled = true;
                IsLogin = true;
                AppNotifyIcon.Text = strVersion;

                // bao bi tan cong, hungtv rem
                LVCITYTASK.Auto();
                LVAUTOTASK.Auto();

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

        /// <summary>
        /// Switches between tabs
        /// </summary>
        private void SwitchTab(TabPage selectedTab)
        {
            switch (selectedTab.Name.ToLower().Trim())
            {
                case "tabbantainguyen":
                    {
                        if (!SellResourceTab_Loaded)
                        {
                            LVReloadCitesForSellResources();
                        }
                        break;
                    }

                case "tabmuatainguyen":
                    {
                        if (!BuyResource_loaded)
                        {
                            LVCommon.common.LoadCityToGridForBuyResource(this.dtaBUYRESOURCE);
                            BuyResource_loaded = true;
                        }
                        break;
                    }

                case "tabxaydung":
                    {
                        if (!BuildCity_loaded)
                        {
                            ShowLoadingLabel();
                            Construct_dropdownCityList.Items.Clear();
                            Construct_dropdownCityList.Items.AddRange(LVObj.City.AllCity);
                            this.Construct_treeBuilding.Nodes.Clear();
                            BuildCity_loaded = true;
                            HideLoadingLabel();
                        }
                        break;
                    }

                case "tabhanha":
                    {
                        ShowLoadingLabel();
                        cboTabHaNhaCity.Items.Clear();
                        cboTabHaNhaCity.Items.AddRange(LVObj.City.AllCity);
                        this.tvDEL.Nodes.Clear();
                        DelCity_loaded = true;
                        HideLoadingLabel();
                        break;
                    }

                case "tabthaophat":
                    {
                        if (!ThaoPhat_loaded)
                        {
                            ShowLoadingLabel();
                            LVAuto.LVForm.LVCommon.common.LoadDataForThaoPhat(LVAuto.LVForm.FrmMain.TuongDiThaoPhatList);
                            ThaoPhat_loaded = true;
                            HideLoadingLabel();
                        }
                        break;
                    }

                case "tabnghiencuu":
                    {
                        if (!ReSearch_loaded)
                        {
                            cboCityForUpgrade.Items.Clear();
                            cboCityForUpgrade.Items.AddRange(LVObj.City.AllCity);
                            LVCommon.common.LoadUpgradeToTreeViewForUpdate(tvUpdate);

                            ReSearch_loaded = true;
                        }
                        break;
                    }

                case "tabanui":
                    {
                        LVCommon.common.LoadCityToGridForAnUi(chklstAnUi);
                        Anui_loaded = true;
                        break;
                    }

                case "tabvanchuyen":
                    {
                        if (!VanChuyen_loaded)
                        {
                            LVCommon.common.LoadCityToGridForVanchuyen(pnVanchuyen);
                            VanChuyen_loaded = true;
                        }
                        break;
                    }

                case "tabdoitrai":
                    {
                        if (!MoveDoanhTrai_loaded)
                        {
                            LVCommon.common.LoadDoanhTraiForMove(pnDoanhTrai);
                            MoveDoanhTrai_loaded = true;
                        }
                        break;
                    }

                case "tabluyensikhi":
                    {
                        if (!Upsikhi_loaded)
                        {
                            //cboLuyenSKCity.Items.Clear();
                            //cboLuyenSKCity.Items.AddRange(LVObj.City.AllCity);

                            if (LVCommon.common.LoadGeneralForUpSiKhi(tvSIKHI)) Upsikhi_loaded = true;
                        }
                        break;
                    }

                case "tabmuavukhi":
                    {
                        if (!BuyWepon_loaded)
                        {
                            ShowLoadingLabel();
                            if (LVCommon.common.LoadCityForBuyWepon(pnWepon) == 0)
                                BuyWepon_loaded = true;
                            else
                                BuyWepon_loaded = false;

                            HideLoadingLabel();
                        }
                        break;
                    }

                case "tabbienche":
                    {
                        cboBienCheCity.Items.Clear();
                        cboBienCheCity.Items.AddRange(LVObj.City.AllCity);
                        chklstGeneral.Items.Clear();
                        LVAuto.LVForm.LVCommon.common.LoadGeneralForBienChe(tvBienCheList);
                        btBiencheAccept.Enabled = false;
                        break;
                    }

                case "tabdieuphai":
                    {
                        cboTabDieuPhaiThanhPhaiQuanVan.Items.Clear();
                        cboTabDieuPhaiThanhPhaiQuanVan.Items.AddRange(LVObj.City.AllCity);
                        cbTabDieuPhaiThanhPhaiDiLoiDai.Items.Clear();
                        cbTabDieuPhaiThanhPhaiDiLoiDai.Items.AddRange(LVObj.City.AllCity);

                        btTabDieuPhaiPhaiQuanVan.Enabled = false;
                        showDataForGridDieuPhai();
                        break;
                    }

                case "tabvanchuyenvukhi":
                    {
                        LoadDataForVCVK();
                        LVCommon.common.LoadDataResultForVCVK(lstVCVKResult);
                        break;
                    }

                case "tabbinhman":
                    {
                        this.cbTabBinhManThanhXuatQuan.Items.Clear();
                        if (LVAuto.LVObj.City.AllCity == null)
                        {
                            LVAuto.LVForm.Command.City.UpdateAllSimpleCity();
                        }

                        if (LVAuto.LVObj.City.AllCity == null) return;


                        this.cbTabBinhManThanhXuatQuan.Items.AddRange(LVAuto.LVObj.City.AllCity);
                        if (this.cbTabBinhManThanhXuatQuan.SelectedItem == null)
                        {
                            this.chkTabBinhManTuongXuatTran.Items.Clear();
                        }
                        LVCommon.common.LoadDataResultBinhMan(chkTabBinhManList);
                        break;
                    }

                case "tabcallman":
                    {
                        this.cboCallManThanhTraiXuatQuan.Items.Clear();
                        if (LVAuto.LVObj.City.AllCity == null)
                        {
                            LVAuto.LVForm.Command.City.UpdateAllSimpleCity();
                        }

                        if (LVAuto.LVObj.City.AllCity == null) return;

                        this.cboCallManThanhTraiXuatQuan.Items.AddRange(LVAuto.LVObj.City.AllCity);
                        if (this.cboCallManThanhTraiXuatQuan.SelectedItem == null)
                        {
                            this.chklbCallManTuongXuatTran.Items.Clear();
                        }
                        chklbCallManListResult.Items.Clear();
                        chklbCallManListResult.Items.AddRange(CallManList.ToArray());
                        break;
                    }
            } //end switch
        }
    }
}
