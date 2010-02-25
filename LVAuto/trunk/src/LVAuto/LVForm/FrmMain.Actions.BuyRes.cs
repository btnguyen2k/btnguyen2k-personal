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
        private const int RESOURCE_BUY_FIX_AMOUNT_DEFAULT = 5000000;
        private const int RESOURCE_BUY_PERCENT_AMOUNT_DEFAULT = 30;

        public bool BuyResourceTab_loaded = false;
        private bool BuyResFoodCheckAll = true;
        private bool BuyResWoodsCheckAll = true;
        private bool BuyResStoneCheckAll = true;
        private bool BuyResIronCheckAll = true;

        private void InitEventHandlers_BuyRes()
        {
            this.tabMuaTaiNguyen.Enter += tabBuyRes_Enter;
            this.tabMuaTaiNguyen.Leave += tabBuyRes_Leave;

            this.BuyRes_radioBuyAmountMethodFix.CheckedChanged += BuyRes_radioBuyAmountMethod_CheckedChanged;
            this.BuyRes_radioBuyAmountMethodPercent.CheckedChanged += BuyRes_radioBuyAmountMethod_CheckedChanged;
        }

        private void BuyRes_radioBuyAmountMethod_CheckedChanged(object sender, EventArgs e)
        {
            if (BuyRes_radioBuyAmountMethodFix.Checked)
            {
                BuyRes_txtBuyAmount.Text = RESOURCE_BUY_FIX_AMOUNT_DEFAULT.ToString();
            }
            else
            {
                BuyRes_txtBuyAmount.Text = RESOURCE_BUY_PERCENT_AMOUNT_DEFAULT.ToString();
            }
        }

        private void BuyRes_btnSelectAllFood_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < BuyRes_gridCityList.Rows.Count; i++)
            {
                BuyRes_gridCityList.Rows[i].Cells[2].Value = BuyResFoodCheckAll;
            }
            BuyResFoodCheckAll = !BuyResFoodCheckAll;
        }

        private void BuyRes_btnSelectAllWoods_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < BuyRes_gridCityList.Rows.Count; i++)
            {
                BuyRes_gridCityList.Rows[i].Cells[3].Value = BuyResWoodsCheckAll;
            }
            BuyResWoodsCheckAll = !BuyResWoodsCheckAll;
        }

        private void BuyRes_btnSelectAllStone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < BuyRes_gridCityList.Rows.Count; i++)
            {
                BuyRes_gridCityList.Rows[i].Cells[4].Value = BuyResStoneCheckAll;
            }
            BuyResStoneCheckAll = !BuyResStoneCheckAll;
        }

        private void BuyRes_btnSelectAllIron_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < BuyRes_gridCityList.Rows.Count; i++)
            {
                BuyRes_gridCityList.Rows[i].Cells[5].Value = BuyResIronCheckAll;
            }
            BuyResIronCheckAll = !BuyResIronCheckAll;
        }

        private void tabBuyRes_Enter(object sender, EventArgs e)
        {
            if (!BuyResourceTab_loaded)
            {
                LVReloadCitesForBuyResources();
            }
        }

        private void tabBuyRes_Leave(object sender, EventArgs e)
        {
            LVUpdateConfig_BuyRes();
        }

        /// <summary>
        /// Reloads cities info for BuyResources tab.
        /// </summary>
        private void LVReloadCitesForBuyResources()
        {
            const string COL_CITY_ID = "CITY_ID";
            const string COL_CITY_NAME = "CITY_NAME";
            const string COL_BUY_FOOD = "BUY_FOOD";
            const string COL_BUY_WOODS = "BUY_WOODS";
            const string COL_BUY_STONE = "BUY_STONE";
            const string COL_BUY_IRON = "BUY_IRON";

            string msg = "Reloading cities for buying resources...";
            ShowLoadingLabel(msg);
            WriteLog(msg);

            DataSet tempDs = new DataSet("temp");
            DataTable tempTbl = new DataTable("temp");

            tempTbl.Columns.Add(new DataColumn(COL_CITY_ID, typeof(int)));
            tempTbl.Columns.Add(new DataColumn(COL_CITY_NAME, typeof(string)));
            tempTbl.Columns.Add(new DataColumn(COL_BUY_FOOD, typeof(bool)));
            tempTbl.Columns.Add(new DataColumn(COL_BUY_WOODS, typeof(bool)));
            tempTbl.Columns.Add(new DataColumn(COL_BUY_STONE, typeof(bool)));
            tempTbl.Columns.Add(new DataColumn(COL_BUY_IRON, typeof(bool)));

            int countCities = 0;
            foreach (LVObj.City city in LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities)
            {
                if (city.Id > 0)
                {
                    countCities++;

                    DataRow tempDr = tempTbl.NewRow();
                    tempDr[COL_CITY_ID] = city.Id;
                    tempDr[COL_CITY_NAME] = city.Name;
                    tempDr[COL_BUY_FOOD] = false;
                    tempDr[COL_BUY_WOODS] = false;
                    tempDr[COL_BUY_STONE] = false;
                    tempDr[COL_BUY_IRON] = false;
                    tempTbl.Rows.Add(tempDr);

                    if (LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig != null)
                    {
                        foreach (LVConfig.AutoConfig.BuyResourcesConfig.BuyResCityConfig cityConfig in LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig)
                        {
                            if (city.Id == cityConfig.CityId)
                            {
                                tempDr[COL_BUY_FOOD] = cityConfig.BuyFood;
                                tempDr[COL_BUY_WOODS] = cityConfig.BuyWoods;
                                tempDr[COL_BUY_STONE] = cityConfig.BuyStone;
                                tempDr[COL_BUY_IRON] = cityConfig.BuyIron;
                            }
                        }
                    }
                }
            } //end foreach
            if (LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig == null)
            {
                LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig = new LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.BuyResCityConfig[countCities];
                for (int i = 0; i < countCities; i++)
                {
                    LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig[i] = new LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.BuyResCityConfig();
                }
            }

            tempDs.Tables.Add(tempTbl);
            BuyRes_gridCityList.DataSource = tempDs.Tables[0];
            BuyRes_gridCityList.Columns[0].HeaderText = "ID";
            BuyRes_gridCityList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            BuyRes_gridCityList.Columns[0].ReadOnly = true;
            BuyRes_gridCityList.Columns[0].Visible = false;

            BuyRes_gridCityList.Columns[1].HeaderText = "Tên";
            BuyRes_gridCityList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; //DataGridViewAutoSizeColumnMode.DisplayedCells;
            BuyRes_gridCityList.Columns[1].ReadOnly = true;

            BuyRes_gridCityList.Columns[2].HeaderText = "Lúa";
            BuyRes_gridCityList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            BuyRes_gridCityList.Columns[2].ReadOnly = false;

            BuyRes_gridCityList.Columns[3].HeaderText = "Gỗ";
            BuyRes_gridCityList.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            BuyRes_gridCityList.Columns[3].ReadOnly = false;

            BuyRes_gridCityList.Columns[4].HeaderText = "Đá";
            BuyRes_gridCityList.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            BuyRes_gridCityList.Columns[4].ReadOnly = false;

            BuyRes_gridCityList.Columns[5].HeaderText = "Sắt";
            BuyRes_gridCityList.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            BuyRes_gridCityList.Columns[5].ReadOnly = false;

            msg = "Reloading cities for buying resources...DONE.";
            ShowLoadingLabel(msg);
            WriteLog(msg);
            BuyResourceTab_loaded = true;

            UpdateGUI_BuyRes();
        }

        private void UpdateGUI_BuyRes()
        {
            switch (LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmountMethod)
            {
                case LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.EnumBuyAmountMethod.FIX_AMOUNT:
                    BuyRes_radioBuyAmountMethodFix.Checked = true;
                    BuyRes_radioBuyAmountMethodPercent.Checked = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.EnumBuyAmountMethod.PERCENT_STORAGE:
                    BuyRes_radioBuyAmountMethodFix.Checked = false;
                    BuyRes_radioBuyAmountMethodPercent.Checked = true;
                    break;
            }
            BuyRes_txtGoldThreshold.Text = LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.GoldThreshold.ToString();
            BuyRes_txtBuyAmount.Text = LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmount.ToString();
            BuyRes_txtTimer.Text = LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.SleepTimeInMinutes.ToString();
        }

        /// <summary>
        /// Updates the configuration object.
        /// </summary>
        private void LVUpdateConfig_BuyRes()
        {
            try
            {
                LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.SleepTimeInMinutes = int.Parse(BuyRes_txtTimer.Text);
                if (LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.SleepTimeInMinutes < 1)
                {
                    LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.SleepTimeInMinutes = 1;
                }
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị thời gian kiểm tra: [" + BuyRes_txtTimer.Text + "] (phải là số nguyên)!");
                return;
            }

            try
            {
                LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmount = int.Parse(BuyRes_txtBuyAmount.Text);
                if (LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmount < 0)
                {
                    LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmount = 0;
                }
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá mua không hợp lệ: [" + BuyRes_txtBuyAmount.Text + "] (phải là số nguyên)!");
                return;
            }

            try
            {
                LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.GoldThreshold = long.Parse(BuyRes_txtGoldThreshold.Text);
                if (LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.GoldThreshold < 0)
                {
                    LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.GoldThreshold = 0;
                }
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị vàng an toàn không hợp lệ: [" + BuyRes_txtGoldThreshold.Text + "] (phải là số nguyên)!");
                return;
            }

            if (BuyRes_radioBuyAmountMethodFix.Checked)
            {
                LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmountMethod = LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.EnumBuyAmountMethod.FIX_AMOUNT;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmountMethod = LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.EnumBuyAmountMethod.PERCENT_STORAGE;
                if (LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmount > 100)
                {
                    LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.BuyAmount = 100;
                }
            }

            LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig = new LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.BuyResCityConfig[BuyRes_gridCityList.Rows.Count];
            for (int i = 0; i < BuyRes_gridCityList.Rows.Count; i++)
            {
                LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.BuyResCityConfig cityConfig = new LVAuto.LVConfig.AutoConfig.BuyResourcesConfig.BuyResCityConfig();
                LVConfig.AutoConfig.CONFIG_BUY_RESOURCES.CityConfig[i] = cityConfig;
                int cityId = int.Parse(BuyRes_gridCityList.Rows[i].Cells[0].Value.ToString());
                LVAuto.LVObj.City city = LVHelper.CityCommandHelper.GetCityById(cityId);
                cityConfig.CityId = city.Id;
                cityConfig.CityName = city.Name;
                cityConfig.BuyFood = bool.Parse(BuyRes_gridCityList.Rows[i].Cells[2].Value.ToString());
                cityConfig.BuyWoods = bool.Parse(BuyRes_gridCityList.Rows[i].Cells[3].Value.ToString());
                cityConfig.BuyStone = bool.Parse(BuyRes_gridCityList.Rows[i].Cells[4].Value.ToString());
                cityConfig.BuyIron = bool.Parse(BuyRes_gridCityList.Rows[i].Cells[5].Value.ToString());
            }
        }
    } //end class
} //end namespace
