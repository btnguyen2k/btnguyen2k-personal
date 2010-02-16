/* This file is to separate the business actions of FrmMain from the GUI's auto-generated actions
 * Contains actions for the Sell Resources tab
 */

using System.Windows.Forms;
using System.Collections;
using System;
using System.Threading;
using System.Data;

namespace LVAuto.LVForm
{
    public partial class FrmMain : Form
    {
        public bool SellResourceTab_Loaded = false;

        private void LVReloadCitesForSellResources()
        {
            const string COL_CITY_ID    = "CITY_ID";
            const string COL_CITY_NAME  = "CITY_NAME";
            const string COL_SELL_FOOD  = "SELL_FOOD";
            const string COL_SELL_WOODS = "SELL_WOODS";
            const string COL_SELL_STONE = "SELL_STONE";
            const string COL_SELL_IRON  = "SELL_IRON";

            DataSet tempDs = new DataSet("temp");
            DataTable tempTbl = new DataTable("temp");

            tempTbl.Columns.Add(new DataColumn(COL_CITY_ID, typeof(int)));
            tempTbl.Columns.Add(new DataColumn(COL_CITY_NAME, typeof(string)));
            tempTbl.Columns.Add(new DataColumn(COL_SELL_FOOD, typeof(bool)));
            tempTbl.Columns.Add(new DataColumn(COL_SELL_WOODS, typeof(bool)));
            tempTbl.Columns.Add(new DataColumn(COL_SELL_STONE, typeof(bool)));
            tempTbl.Columns.Add(new DataColumn(COL_SELL_IRON, typeof(bool)));

            foreach (Command.CityObj.City city in Command.CityObj.City.AllCity)
            {
                if (city.id > 0)
                {
                    DataRow tempDr = tempTbl.NewRow();
                    tempDr[COL_CITY_ID] = city.id;
                    tempDr[COL_CITY_NAME] = city.name;
                    tempDr[COL_SELL_FOOD] = false;
                    tempDr[COL_SELL_WOODS] = false;
                    tempDr[COL_SELL_STONE] = false;
                    tempDr[COL_SELL_IRON] = false;

                    if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig != null)
                    {
                        foreach (LVConfig.AutoConfig.SellResourcesConfig.SellCityConfig cityConfig in LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig)
                        {
                            if (city.id == cityConfig.CityId)
                            {
                                tempDr[COL_SELL_FOOD] = cityConfig.SellFood;
                                tempDr[COL_SELL_WOODS] = cityConfig.SellWoods;
                                tempDr[COL_SELL_STONE] = cityConfig.SellStone;
                                tempDr[COL_SELL_IRON] = cityConfig.SellIron;
                            }
                        }
                    }
                    tempTbl.Rows.Add(tempDr);
                }
            } //end foreach

            tempDs.Tables.Add(tempTbl);
            SellRes_gridCityList.DataSource = tempDs.Tables[0];
            SellRes_gridCityList.Columns[0].HeaderText = "ID";
            SellRes_gridCityList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;		//.DisplayedCells;
            SellRes_gridCityList.Columns[0].ReadOnly = true;
            SellRes_gridCityList.Columns[0].Visible = false;

            SellRes_gridCityList.Columns[1].HeaderText = "Tên";
            SellRes_gridCityList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;	//DataGridViewAutoSizeColumnMode.DisplayedCells;
            SellRes_gridCityList.Columns[1].ReadOnly = true;

            SellRes_gridCityList.Columns[2].HeaderText = "Lúa";
            SellRes_gridCityList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            SellRes_gridCityList.Columns[2].ReadOnly = false;

            SellRes_gridCityList.Columns[3].HeaderText = "Gỗ";
            SellRes_gridCityList.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            SellRes_gridCityList.Columns[3].ReadOnly = false;

            SellRes_gridCityList.Columns[4].HeaderText = "Đá";
            SellRes_gridCityList.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            SellRes_gridCityList.Columns[4].ReadOnly = false;

            SellRes_gridCityList.Columns[5].HeaderText = "Sắt";
            SellRes_gridCityList.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            SellRes_gridCityList.Columns[5].ReadOnly = false;

            /*
            LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtTimer.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.timetoruninminute.ToString();


            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCOUNTLUA.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.SoLuongBan.ToString();
            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtSAFELUA.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.SoLuongAnToan.ToString();

            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtBanTN_LUA_TB_Heso.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.GiaTri.ToString();
            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtBanTN_LUA_TB_Heso.Enabled = true;
            if (LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.LoaiBan == 1)		// co dinh
            {
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_CODINH.Checked = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_TB_Cong.Enabled = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_TB_Nhan.Enabled = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_THAPNHAT.Checked = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_TRUNGBINH.Checked = false;
            }
            else
            {
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_TB_Cong.Enabled = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_TB_Nhan.Enabled = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_TB_Cong.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.CongThucNhan;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_TB_Nhan.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.CongThucNhan;
                if (LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.LoaiBan == 2)		// trung binh
                {
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_TRUNGBINH.Checked = true;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_CODINH.Checked = false;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_THAPNHAT.Checked = false;
                }
                else
                {
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_THAPNHAT.Checked = true;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_CODINH.Checked = false;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_LUA_TRUNGBINH.Checked = false;
                }
            }

            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtBanTN_GO_TB_Heso.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.GiaTri.ToString();
            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtBanTN_GO_TB_Heso.Enabled = true;
            if (LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.LoaiBan == 1)		// co dinh
            {
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_CODINH.Checked = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_TB_Cong.Enabled = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_TB_Nhan.Enabled = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_THAPNHAT.Checked = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_TRUNGBINH.Checked = false;
            }
            else
            {
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_TB_Cong.Enabled = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_TB_Nhan.Enabled = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_TB_Cong.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.CongThucNhan;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_TB_Nhan.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.CongThucNhan;
                if (LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.LoaiBan == 2)		// trung binh
                {
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_TRUNGBINH.Checked = true;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_CODINH.Checked = false;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_THAPNHAT.Checked = false;
                }
                else
                {
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_THAPNHAT.Checked = true;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_CODINH.Checked = false;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_GO_TRUNGBINH.Checked = false;
                }
            }


            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtBanTN_DA_TB_Heso.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.GiaTri.ToString();
            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtBanTN_DA_TB_Heso.Enabled = true;
            if (LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.LoaiBan == 1)		// co dinh
            {
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_CODINH.Checked = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_TB_Cong.Enabled = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_TB_Nhan.Enabled = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_THAPNHAT.Checked = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_TRUNGBINH.Checked = false;
            }
            else
            {
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_TB_Cong.Enabled = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_TB_Nhan.Enabled = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_TB_Cong.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.CongThucNhan;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_TB_Nhan.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.CongThucNhan;
                if (LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.LoaiBan == 2)		// trung binh
                {
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_TRUNGBINH.Checked = true;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_CODINH.Checked = false;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_THAPNHAT.Checked = false;
                }
                else
                {
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_THAPNHAT.Checked = true;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_CODINH.Checked = false;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_DA_TRUNGBINH.Checked = false;
                }
            }


            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtBanTN_SAT_TB_Heso.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.GiaTri.ToString();
            LVAuto.LVForm.FrmMain.LVFRMMAIN.txtBanTN_SAT_TB_Heso.Enabled = true;
            if (LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.LoaiBan == 1)		// co dinh
            {
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_CODINH.Checked = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_TB_Cong.Enabled = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_TB_Nhan.Enabled = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_THAPNHAT.Checked = false;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_TRUNGBINH.Checked = false;
            }
            else
            {
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_TB_Cong.Enabled = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_TB_Nhan.Enabled = true;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_TB_Cong.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.CongThucNhan;
                LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_TB_Nhan.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.CongThucNhan;
                if (LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.LoaiBan == 2)		// trung binh
                {
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_TRUNGBINH.Checked = true;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_CODINH.Checked = false;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_THAPNHAT.Checked = false;
                }
                else
                {
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_THAPNHAT.Checked = true;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_CODINH.Checked = false;
                    LVAuto.LVForm.FrmMain.LVFRMMAIN.rdBanTN_SAT_TRUNGBINH.Checked = false;
                }
            }
            */
            SellResourceTab_Loaded = true;
        }
    } //end class
} //end namespace
