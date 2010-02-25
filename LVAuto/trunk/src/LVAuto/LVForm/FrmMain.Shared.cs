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
                string msg = "Loading data from server...";
                ShowLoadingLabel(msg);
                WriteLog(msg);

                SetAllTabsEnable(false);

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

                int retries = 0;
                do
                {
                    LVHelper.CityCommandHelper.UpdateSimpleCityInfo();
                    retries++;
                } while (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null && retries < 3);
                if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null)
                {
                    WriteLog("Loading data from server...No City!");
                    return false;
                }

                tabLogin.Select();
                tabLogin.Focus();

                HideLoadingLabel();
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
                //case "tabbantainguyen":
                //    {
                //        if (!SellResourceTab_Loaded)
                //        {
                //            LVReloadCitesForSellResources();
                //        }
                //        break;
                //    }

                //case "tabmuatainguyen":
                //    {
                //        if (!BuyResource_loaded)
                //        {
                //            LVCommon.common.LoadCityToGridForBuyResource(this.BuyRes_gridCityList);
                //            BuyResource_loaded = true;
                //        }
                //        break;
                //    }

                case "tabxaydung":
                    {
                        if (!BuildCity_loaded)
                        {
                            ShowLoadingLabel();
                            Construct_dropdownCityList.Items.Clear();
                            Construct_dropdownCityList.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);
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
                        cboTabHaNhaCity.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);
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
                            cboCityForUpgrade.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);
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
                            //cboLuyenSKCity.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);

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
                        cboBienCheCity.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);
                        chklstGeneral.Items.Clear();
                        LVAuto.LVForm.LVCommon.common.LoadGeneralForBienChe(tvBienCheList);
                        btBiencheAccept.Enabled = false;
                        break;
                    }

                case "tabdieuphai":
                    {
                        cboTabDieuPhaiThanhPhaiQuanVan.Items.Clear();
                        cboTabDieuPhaiThanhPhaiQuanVan.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);
                        cbTabDieuPhaiThanhPhaiDiLoiDai.Items.Clear();
                        cbTabDieuPhaiThanhPhaiDiLoiDai.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);

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
                        if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null)
                        {
                            LVAuto.LVForm.Command.City.UpdateAllSimpleCity();
                        }

                        if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null) return;


                        this.cbTabBinhManThanhXuatQuan.Items.AddRange(LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);
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
                        if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null)
                        {
                            LVAuto.LVForm.Command.City.UpdateAllSimpleCity();
                        }

                        if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null) return;

                        this.cboCallManThanhTraiXuatQuan.Items.AddRange(LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);
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
    } //end class
} //end namespace
