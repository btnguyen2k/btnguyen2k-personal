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
        private const int RESOURCE_SELL_FIX_AMOUNT_DEFAULT = 1000;
        private const int RESOURCE_THRESHOLD_FIX_AMOUNT_DEFAULT = 500;
        private const int RESOURCE_SELL_PERCENT_AMOUNT_DEFAULT = 10;
        private const int RESOURCE_THRESHOLD_PERCENT_AMOUNT_DEFAULT = 30;

        public bool SellResourceTab_Loaded = false;
        private bool SellResFoodCheckAll = true;
        private bool SellResWoodsCheckAll = true;
        private bool SellResStoneCheckAll = true;
        private bool SellResIronCheckAll = true;

        private void InitEvent_SellRes()
        {
            this.tabBanTaiNguyen.Enter += tabSellRes_Enter;
            this.tabBanTaiNguyen.Leave += tabSellRes_Leave;

            //food
            this.SellRes_radioSellAmountMethodFoodFix.CheckedChanged += SellRes_radioSellAmountMethodFood_CheckedChanged;
            this.SellRes_radioSellAmountMethodFoodPercent.CheckedChanged += SellRes_radioSellAmountMethodFood_CheckedChanged;
            this.SellRes_radioSellPriceMethodFoodFix.CheckedChanged += SellRes_radioSellPriceMethodFood_CheckedChanged;
            this.SellRes_radioSellPriceMethodFoodAvg.CheckedChanged += SellRes_radioSellPriceMethodFood_CheckedChanged;
            this.SellRes_radioSellPriceMethodFoodMin.CheckedChanged += SellRes_radioSellPriceMethodFood_CheckedChanged;

            //woods
            this.SellRes_radioSellAmountMethodWoodsFix.CheckedChanged += SellRes_radioSellAmountMethodWoods_CheckedChanged;
            this.SellRes_radioSellAmountMethodWoodsPercent.CheckedChanged += SellRes_radioSellAmountMethodWoods_CheckedChanged;
            this.SellRes_radioSellPriceMethodWoodsFix.CheckedChanged += SellRes_radioSellPriceMethodWoods_CheckedChanged;
            this.SellRes_radioSellPriceMethodWoodsAvg.CheckedChanged += SellRes_radioSellPriceMethodWoods_CheckedChanged;
            this.SellRes_radioSellPriceMethodWoodsMin.CheckedChanged += SellRes_radioSellPriceMethodWoods_CheckedChanged;

            //stone
            this.SellRes_radioSellAmountMethodStoneFix.CheckedChanged += SellRes_radioSellAmountMethodStone_CheckedChanged;
            this.SellRes_radioSellAmountMethodStonePercent.CheckedChanged += SellRes_radioSellAmountMethodStone_CheckedChanged;
            this.SellRes_radioSellPriceMethodStoneFix.CheckedChanged += SellRes_radioSellPriceMethodStone_CheckedChanged;
            this.SellRes_radioSellPriceMethodStoneAvg.CheckedChanged += SellRes_radioSellPriceMethodStone_CheckedChanged;
            this.SellRes_radioSellPriceMethodStoneMin.CheckedChanged += SellRes_radioSellPriceMethodStone_CheckedChanged;

            //iron
            this.SellRes_radioSellAmountMethodIronFix.CheckedChanged += SellRes_radioSellAmountMethodIron_CheckedChanged;
            this.SellRes_radioSellAmountMethodIronPercent.CheckedChanged += SellRes_radioSellAmountMethodIron_CheckedChanged;
            this.SellRes_radioSellPriceMethodIronFix.CheckedChanged += SellRes_radioSellPriceMethodIron_CheckedChanged;
            this.SellRes_radioSellPriceMethodIronAvg.CheckedChanged += SellRes_radioSellPriceMethodIron_CheckedChanged;
            this.SellRes_radioSellPriceMethodIronMin.CheckedChanged += SellRes_radioSellPriceMethodIron_CheckedChanged;
        }

        private void SellRes_btnSelectAllFood_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SellRes_gridCityList.Rows.Count; i++)
            {
                SellRes_gridCityList.Rows[i].Cells[2].Value = SellResFoodCheckAll;
            }
            SellResFoodCheckAll = !SellResFoodCheckAll;
        }

        private void SellRes_btnSelectAllWoods_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SellRes_gridCityList.Rows.Count; i++)
            {
                SellRes_gridCityList.Rows[i].Cells[3].Value = SellResWoodsCheckAll;
            }
            SellResWoodsCheckAll = !SellResWoodsCheckAll;
        }

        private void SellRes_btnSelectAllStone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SellRes_gridCityList.Rows.Count; i++)
            {
                SellRes_gridCityList.Rows[i].Cells[4].Value = SellResStoneCheckAll;
            }
            SellResStoneCheckAll = !SellResStoneCheckAll;
        }

        private void SellRes_btnSelectAllIron_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SellRes_gridCityList.Rows.Count; i++)
            {
                SellRes_gridCityList.Rows[i].Cells[5].Value = SellResIronCheckAll;
            }
            SellResIronCheckAll = !SellResIronCheckAll;
        }

        private void SellRes_btnReloadCities_Click(object sender, EventArgs e)
        {
            LVReloadCitesForSellResources();
        }

        private void SellRes_btnUpdate_Click(object sender, EventArgs e)
        {
            LVUpdateConfig_SellRes();
        }

        private void SellRes_radioSellPriceMethodFood_CheckedChanged(object sender, EventArgs e)
        {
            if (SellRes_radioSellPriceMethodFoodFix.Checked)
            {
                SellRes_radioAddOnMethodFoodAdd.Enabled = false;
                SellRes_radioAddOnMethodFoodMultiply.Enabled = false;
            }
            else
            {
                SellRes_radioAddOnMethodFoodAdd.Enabled = true;
                SellRes_radioAddOnMethodFoodMultiply.Enabled = true;
            }
        }

        private void SellRes_radioSellAmountMethodFood_CheckedChanged(object sender, EventArgs e)
        {
            if (SellRes_radioSellAmountMethodFoodFix.Checked)
            {
                SellRes_labelAmountSellFood.Text = "tấn";
                SellRes_labelAmountThresholdFood.Text = "tấn";
                SellRes_txtAmountSellFood.Text = RESOURCE_SELL_FIX_AMOUNT_DEFAULT.ToString();
                SellRes_txtAmountThresholdFood.Text = RESOURCE_THRESHOLD_FIX_AMOUNT_DEFAULT.ToString();
            }
            else
            {
                SellRes_labelAmountSellFood.Text = "% kho";
                SellRes_labelAmountThresholdFood.Text = "% kho";
                SellRes_txtAmountSellFood.Text = RESOURCE_SELL_PERCENT_AMOUNT_DEFAULT.ToString();
                SellRes_txtAmountThresholdFood.Text = RESOURCE_THRESHOLD_PERCENT_AMOUNT_DEFAULT.ToString();
            }
        }

        private void SellRes_radioSellPriceMethodWoods_CheckedChanged(object sender, EventArgs e)
        {
            if (SellRes_radioSellPriceMethodWoodsFix.Checked)
            {
                SellRes_radioAddOnMethodWoodsAdd.Enabled = false;
                SellRes_radioAddOnMethodWoodsMultiply.Enabled = false;
            }
            else
            {
                SellRes_radioAddOnMethodWoodsAdd.Enabled = true;
                SellRes_radioAddOnMethodWoodsMultiply.Enabled = true;
            }
        }

        private void SellRes_radioSellAmountMethodWoods_CheckedChanged(object sender, EventArgs e)
        {
            if (SellRes_radioSellAmountMethodWoodsFix.Checked)
            {
                SellRes_labelAmountSellWoods.Text = "tấn";
                SellRes_labelAmountThresholdWoods.Text = "tấn";
                SellRes_txtAmountSellWoods.Text = RESOURCE_SELL_FIX_AMOUNT_DEFAULT.ToString();
                SellRes_txtAmountThresholdWoods.Text = RESOURCE_THRESHOLD_FIX_AMOUNT_DEFAULT.ToString();
            }
            else
            {
                SellRes_labelAmountSellWoods.Text = "% kho";
                SellRes_labelAmountThresholdWoods.Text = "% kho";
                SellRes_txtAmountSellWoods.Text = RESOURCE_SELL_PERCENT_AMOUNT_DEFAULT.ToString();
                SellRes_txtAmountThresholdWoods.Text = RESOURCE_THRESHOLD_PERCENT_AMOUNT_DEFAULT.ToString();
            }
        }

        private void SellRes_radioSellPriceMethodStone_CheckedChanged(object sender, EventArgs e)
        {
            if (SellRes_radioSellPriceMethodStoneFix.Checked)
            {
                SellRes_radioAddOnMethodStoneAdd.Enabled = false;
                SellRes_radioAddOnMethodStoneMultiply.Enabled = false;
            }
            else
            {
                SellRes_radioAddOnMethodStoneAdd.Enabled = true;
                SellRes_radioAddOnMethodStoneMultiply.Enabled = true;
            }
        }

        private void SellRes_radioSellAmountMethodStone_CheckedChanged(object sender, EventArgs e)
        {
            if (SellRes_radioSellAmountMethodStoneFix.Checked)
            {
                SellRes_labelAmountSellStone.Text = "tấn";
                SellRes_labelAmountThresholdStone.Text = "tấn";
                SellRes_txtAmountSellStone.Text = RESOURCE_SELL_FIX_AMOUNT_DEFAULT.ToString();
                SellRes_txtAmountThresholdStone.Text = RESOURCE_THRESHOLD_FIX_AMOUNT_DEFAULT.ToString();
            }
            else
            {
                SellRes_labelAmountSellStone.Text = "% kho";
                SellRes_labelAmountThresholdStone.Text = "% kho";
                SellRes_txtAmountSellStone.Text = RESOURCE_SELL_PERCENT_AMOUNT_DEFAULT.ToString();
                SellRes_txtAmountThresholdStone.Text = RESOURCE_THRESHOLD_PERCENT_AMOUNT_DEFAULT.ToString();
            }
        }

        private void SellRes_radioSellPriceMethodIron_CheckedChanged(object sender, EventArgs e)
        {
            if (SellRes_radioSellPriceMethodIronFix.Checked)
            {
                SellRes_radioAddOnMethodIronAdd.Enabled = false;
                SellRes_radioAddOnMethodIronMultiply.Enabled = false;
            }
            else
            {
                SellRes_radioAddOnMethodIronAdd.Enabled = true;
                SellRes_radioAddOnMethodIronMultiply.Enabled = true;
            }
        }

        private void SellRes_radioSellAmountMethodIron_CheckedChanged(object sender, EventArgs e)
        {
            if (SellRes_radioSellAmountMethodIronFix.Checked)
            {
                SellRes_labelAmountSellIron.Text = "tấn";
                SellRes_labelAmountThresholdIron.Text = "tấn";
                SellRes_txtAmountSellIron.Text = RESOURCE_SELL_FIX_AMOUNT_DEFAULT.ToString();
                SellRes_txtAmountThresholdIron.Text = RESOURCE_THRESHOLD_FIX_AMOUNT_DEFAULT.ToString();
            }
            else
            {
                SellRes_labelAmountSellIron.Text = "% kho";
                SellRes_labelAmountThresholdIron.Text = "% kho";
                SellRes_txtAmountSellIron.Text = RESOURCE_SELL_PERCENT_AMOUNT_DEFAULT.ToString();
                SellRes_txtAmountThresholdIron.Text = RESOURCE_THRESHOLD_PERCENT_AMOUNT_DEFAULT.ToString();
            }
        }

        private void tabSellRes_Enter(object sender, EventArgs e)
        {
            if (!SellResourceTab_Loaded)
            {
                LVReloadCitesForSellResources();
            }
        }

        private void tabSellRes_Leave(object sender, EventArgs e)
        {
            LVUpdateConfig_SellRes();
        }

        /// <summary>
        /// Updates the configuration object.
        /// </summary>
        private void LVUpdateConfig_SellRes()
        {
            LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.SaleOff = SellRes_checkSelloff.Checked;
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.SleepTimeInMinutes = int.Parse(SellRes_txtTimer.Text);
                if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.SleepTimeInMinutes < 1)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.SleepTimeInMinutes = 1;
                }
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị thời gian kiểm tra: [" + SellRes_txtTimer.Text + "] (phải là số nguyên)!");
                return;
            }
            LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig = new LVAuto.LVConfig.AutoConfig.SellResourcesConfig.SellCityConfig[SellRes_gridCityList.Rows.Count];
            for (int i = 0; i < SellRes_gridCityList.Rows.Count; i++)
            {
                LVAuto.LVConfig.AutoConfig.SellResourcesConfig.SellCityConfig cityConfig = new LVAuto.LVConfig.AutoConfig.SellResourcesConfig.SellCityConfig();
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig[i] = cityConfig;
                int cityId = int.Parse(SellRes_gridCityList.Rows[i].Cells[0].Value.ToString());
                LVAuto.LVObj.City city = LVHelper.CityCommandHelper.GetCityById(cityId);
                cityConfig.CityId = city.Id;
                cityConfig.CityName = city.Name;
                cityConfig.SellFood = bool.Parse(SellRes_gridCityList.Rows[i].Cells[2].Value.ToString());
                cityConfig.SellWoods = bool.Parse(SellRes_gridCityList.Rows[i].Cells[3].Value.ToString());
                cityConfig.SellStone = bool.Parse(SellRes_gridCityList.Rows[i].Cells[4].Value.ToString());
                cityConfig.SellIron = bool.Parse(SellRes_gridCityList.Rows[i].Cells[5].Value.ToString());
            }

            LVUpdateConfig_SellRes_Food();
            LVUpdateConfig_SellRes_Woods();
            LVUpdateConfig_SellRes_Stone();
            LVUpdateConfig_SellRes_Iron();
        }
        
        private void LVUpdateConfig_SellRes_Food() {
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellAmount = int.Parse(SellRes_txtAmountSellFood.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị bán của Lúa không hợp lệ: [" + SellRes_txtAmountSellFood.Text + "] (phải là số nguyên)!");
                return;
            }
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellThreshold = int.Parse(SellRes_txtAmountThresholdFood.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị an toàn của Lúa không hợp lệ: [" + SellRes_txtAmountThresholdFood.Text + "] (phải là số nguyên)!");
                return;
            }
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.PriceAddOn = double.Parse(SellRes_textAddOnValueFood.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị cộng/nhân giá Lúa không hợp lệ: [" + SellRes_textAddOnValueFood.Text + "] (phải là một số)!");
                return;
            }
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellAmount < 0)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellAmount = 0;
            }
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellThreshold < 0)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellThreshold = 0;
            }
            if (SellRes_radioSellAmountMethodFoodFix.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellAmountMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.FIX_AMOUNT;   
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellAmountMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.PERCENT_STORAGE;
                if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellAmount > 100)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellAmount = 100;
                }
                if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellThreshold > 100)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellThreshold = 100;
                }
            }
            if (SellRes_radioAddOnMethodFoodAdd.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.PriceAddOnMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.ADD_PRICE;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.PriceAddOnMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.MULTIPLY_PRICE;
            }
            if (SellRes_radioSellPriceMethodFoodFix.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.FIX_PRICE;
            }
            else if (SellRes_radioSellPriceMethodFoodAvg.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.AVERAGE_PRICE;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.MIN_PRICE;
            }
        }

        private void LVUpdateConfig_SellRes_Woods()
        {
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellAmount = int.Parse(SellRes_txtAmountSellWoods.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị bán của Gỗ không hợp lệ: [" + SellRes_txtAmountSellWoods.Text + "] (phải là số nguyên)!");
                return;
            }
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellThreshold = int.Parse(SellRes_txtAmountThresholdWoods.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị an toàn của Gỗ không hợp lệ: [" + SellRes_txtAmountThresholdWoods.Text + "] (phải là số nguyên)!");
                return;
            }
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.PriceAddOn = double.Parse(SellRes_textAddOnValueWoods.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị cộng/nhân giá Gỗ không hợp lệ: [" + SellRes_textAddOnValueWoods.Text + "] (phải là một số)!");
                return;
            }
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellAmount < 0)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellAmount = 0;
            }
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellThreshold < 0)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellThreshold = 0;
            }
            if (SellRes_radioSellAmountMethodWoodsFix.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellAmountMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.FIX_AMOUNT;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellAmountMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.PERCENT_STORAGE;
                if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellAmount > 100)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellAmount = 100;
                }
                if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellThreshold > 100)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellThreshold = 100;
                }
            }
            if (SellRes_radioAddOnMethodWoodsAdd.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.PriceAddOnMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.ADD_PRICE;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.PriceAddOnMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.MULTIPLY_PRICE;
            }
            if (SellRes_radioSellPriceMethodWoodsFix.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.FIX_PRICE;
            }
            else if (SellRes_radioSellPriceMethodWoodsAvg.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.AVERAGE_PRICE;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.MIN_PRICE;
            }
        }

        private void LVUpdateConfig_SellRes_Stone()
        {
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellAmount = int.Parse(SellRes_txtAmountSellStone.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị bán của Đá không hợp lệ: [" + SellRes_txtAmountSellStone.Text + "] (phải là số nguyên)!");
                return;
            }
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellThreshold = int.Parse(SellRes_txtAmountThresholdStone.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị an toàn của Đá không hợp lệ: [" + SellRes_txtAmountThresholdStone.Text + "] (phải là số nguyên)!");
                return;
            }
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.PriceAddOn = double.Parse(SellRes_textAddOnValueStone.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị cộng/nhân giá Đá không hợp lệ: [" + SellRes_textAddOnValueStone.Text + "] (phải là một số)!");
                return;
            }
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellAmount < 0)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellAmount = 0;
            }
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellThreshold < 0)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellThreshold = 0;
            }
            if (SellRes_radioSellAmountMethodStoneFix.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellAmountMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.FIX_AMOUNT;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellAmountMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.PERCENT_STORAGE;
                if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellAmount > 100)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellAmount = 100;
                }
                if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellThreshold > 100)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellThreshold = 100;
                }
            }
            if (SellRes_radioAddOnMethodStoneAdd.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.PriceAddOnMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.ADD_PRICE;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.PriceAddOnMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.MULTIPLY_PRICE;
            }
            if (SellRes_radioSellPriceMethodStoneFix.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.FIX_PRICE;
            }
            else if (SellRes_radioSellPriceMethodStoneAvg.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.AVERAGE_PRICE;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.MIN_PRICE;
            }
        }

        private void LVUpdateConfig_SellRes_Iron()
        {
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellAmount = int.Parse(SellRes_txtAmountSellIron.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị bán của Sắt không hợp lệ: [" + SellRes_txtAmountSellIron.Text + "] (phải là số nguyên)!");
                return;
            }
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellThreshold = int.Parse(SellRes_txtAmountThresholdIron.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị an toàn của Sắt không hợp lệ: [" + SellRes_txtAmountThresholdIron.Text + "] (phải là số nguyên)!");
                return;
            }
            try
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.PriceAddOn = double.Parse(SellRes_textAddOnValueIron.Text);
            }
            catch (FormatException)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Giá trị cộng/nhân giá Sắt không hợp lệ: [" + SellRes_textAddOnValueIron.Text + "] (phải là một số)!");
                return;
            }
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellAmount < 0)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellAmount = 0;
            }
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellThreshold < 0)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellThreshold = 0;
            }
            if (SellRes_radioSellAmountMethodIronFix.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellAmountMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.FIX_AMOUNT;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellAmountMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.PERCENT_STORAGE;
                if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellAmount > 100)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellAmount = 100;
                }
                if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellThreshold > 100)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellThreshold = 100;
                }
            }
            if (SellRes_radioAddOnMethodIronAdd.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.PriceAddOnMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.ADD_PRICE;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.PriceAddOnMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.MULTIPLY_PRICE;
            }
            if (SellRes_radioSellPriceMethodIronFix.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.FIX_PRICE;
            }
            else if (SellRes_radioSellPriceMethodIronAvg.Checked)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.AVERAGE_PRICE;
            }
            else
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellPriceMethod = LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.MIN_PRICE;
            }
        }

        /// <summary>
        /// Reloads cities info for SellResources tab.
        /// </summary>
        private void LVReloadCitesForSellResources()
        {
            const string COL_CITY_ID    = "CITY_ID";
            const string COL_CITY_NAME  = "CITY_NAME";
            const string COL_SELL_FOOD  = "SELL_FOOD";
            const string COL_SELL_WOODS = "SELL_WOODS";
            const string COL_SELL_STONE = "SELL_STONE";
            const string COL_SELL_IRON  = "SELL_IRON";

            string msg = "Reloading cities for selling resources...";
            ShowLoadingLabel(msg);
            WriteLog(msg);

            DataSet tempDs = new DataSet("temp");
            DataTable tempTbl = new DataTable("temp");

            tempTbl.Columns.Add(new DataColumn(COL_CITY_ID, typeof(int)));
            tempTbl.Columns.Add(new DataColumn(COL_CITY_NAME, typeof(string)));
            tempTbl.Columns.Add(new DataColumn(COL_SELL_FOOD, typeof(bool)));
            tempTbl.Columns.Add(new DataColumn(COL_SELL_WOODS, typeof(bool)));
            tempTbl.Columns.Add(new DataColumn(COL_SELL_STONE, typeof(bool)));
            tempTbl.Columns.Add(new DataColumn(COL_SELL_IRON, typeof(bool)));

            int countCities = 0;
            foreach (LVObj.City city in LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities)
            {
                if (city.Id > 0)
                {
                    countCities++;

                    DataRow tempDr = tempTbl.NewRow();
                    tempDr[COL_CITY_ID] = city.Id;
                    tempDr[COL_CITY_NAME] = city.Name;
                    tempDr[COL_SELL_FOOD] = false;
                    tempDr[COL_SELL_WOODS] = false;
                    tempDr[COL_SELL_STONE] = false;
                    tempDr[COL_SELL_IRON] = false;

                    if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig != null)
                    {
                        foreach (LVConfig.AutoConfig.SellResourcesConfig.SellCityConfig cityConfig in LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig)
                        {
                            if (city.Id == cityConfig.CityId)
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
            if (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig == null)
            {
                LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig = new LVAuto.LVConfig.AutoConfig.SellResourcesConfig.SellCityConfig[countCities];
                for (int i = 0; i < countCities; i++)
                {
                    LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.CityConfig[i] = new LVAuto.LVConfig.AutoConfig.SellResourcesConfig.SellCityConfig();
                }
            }

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

            msg = "Reloading cities for selling resources...DONE.";
            ShowLoadingLabel(msg);
            WriteLog(msg);
            SellResourceTab_Loaded = true;

            UpdateGUI_SelLRes();
        }

        private void UpdateGUI_SelLRes() {
            SellRes_txtTimer.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.SleepTimeInMinutes.ToString();

            UpdateGUI_SelLRes_Food();
            UpdateGUI_SelLRes_Woods();
            UpdateGUI_SelLRes_Stone();
            UpdateGUI_SelLRes_Iron();
        }

        private void UpdateGUI_SelLRes_Food() 
        {
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellAmountMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.FIX_AMOUNT:
                    SellRes_radioSellAmountMethodFoodFix.Checked = true;
                    SellRes_radioSellAmountMethodFoodPercent.Checked = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.PERCENT_STORAGE:
                    SellRes_radioSellAmountMethodFoodFix.Checked = false;
                    SellRes_radioSellAmountMethodFoodPercent.Checked = true;
                    break;
            } //end switch
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.PriceAddOnMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.ADD_PRICE:
                    SellRes_radioAddOnMethodFoodAdd.Checked = true;
                    SellRes_radioAddOnMethodFoodMultiply.Checked = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.MULTIPLY_PRICE:
                    SellRes_radioAddOnMethodFoodAdd.Checked = false;
                    SellRes_radioAddOnMethodFoodMultiply.Checked = true;
                    break;
            }
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellPriceMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.FIX_PRICE:
                    SellRes_radioSellPriceMethodFoodFix.Checked = true;
                    SellRes_radioSellPriceMethodFoodAvg.Checked = false;
                    SellRes_radioSellPriceMethodFoodMin.Checked = false;
                    SellRes_radioAddOnMethodFoodAdd.Enabled = false;
                    SellRes_radioAddOnMethodFoodMultiply.Enabled = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.AVERAGE_PRICE:
                    SellRes_radioSellPriceMethodFoodFix.Checked = false;
                    SellRes_radioSellPriceMethodFoodAvg.Checked = true;
                    SellRes_radioSellPriceMethodFoodMin.Checked = false;
                    SellRes_radioAddOnMethodFoodAdd.Enabled = true;
                    SellRes_radioAddOnMethodFoodMultiply.Enabled = true;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.MIN_PRICE:
                    SellRes_radioSellPriceMethodFoodFix.Checked = false;
                    SellRes_radioSellPriceMethodFoodAvg.Checked = false;
                    SellRes_radioSellPriceMethodFoodMin.Checked = true;
                    SellRes_radioAddOnMethodFoodAdd.Enabled = true;
                    SellRes_radioAddOnMethodFoodMultiply.Enabled = true;
                    break;
            } //end switch
            SellRes_txtAmountSellFood.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellAmount.ToString();
            SellRes_txtAmountThresholdFood.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.SellThreshold.ToString();
            SellRes_textAddOnValueFood.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.FoodConfig.PriceAddOn.ToString();
        }

        private void UpdateGUI_SelLRes_Woods()
        {
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellAmountMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.FIX_AMOUNT:
                    SellRes_radioSellAmountMethodWoodsFix.Checked = true;
                    SellRes_radioSellAmountMethodWoodsPercent.Checked = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.PERCENT_STORAGE:
                    SellRes_radioSellAmountMethodWoodsFix.Checked = false;
                    SellRes_radioSellAmountMethodWoodsPercent.Checked = true;
                    break;
            } //end switch
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.PriceAddOnMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.ADD_PRICE:
                    SellRes_radioAddOnMethodWoodsAdd.Checked = true;
                    SellRes_radioAddOnMethodWoodsMultiply.Checked = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.MULTIPLY_PRICE:
                    SellRes_radioAddOnMethodWoodsAdd.Checked = false;
                    SellRes_radioAddOnMethodWoodsMultiply.Checked = true;
                    break;
            }
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellPriceMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.FIX_PRICE:
                    SellRes_radioSellPriceMethodWoodsFix.Checked = true;
                    SellRes_radioSellPriceMethodWoodsAvg.Checked = false;
                    SellRes_radioSellPriceMethodWoodsMin.Checked = false;
                    SellRes_radioAddOnMethodWoodsAdd.Enabled = false;
                    SellRes_radioAddOnMethodWoodsMultiply.Enabled = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.AVERAGE_PRICE:
                    SellRes_radioSellPriceMethodWoodsFix.Checked = false;
                    SellRes_radioSellPriceMethodWoodsAvg.Checked = true;
                    SellRes_radioSellPriceMethodWoodsMin.Checked = false;
                    SellRes_radioAddOnMethodWoodsAdd.Enabled = true;
                    SellRes_radioAddOnMethodWoodsMultiply.Enabled = true;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.MIN_PRICE:
                    SellRes_radioSellPriceMethodWoodsFix.Checked = false;
                    SellRes_radioSellPriceMethodWoodsAvg.Checked = false;
                    SellRes_radioSellPriceMethodWoodsMin.Checked = true;
                    SellRes_radioAddOnMethodWoodsAdd.Enabled = true;
                    SellRes_radioAddOnMethodWoodsMultiply.Enabled = true;
                    break;
            } //end switch
            SellRes_txtAmountSellWoods.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellAmount.ToString();
            SellRes_txtAmountThresholdWoods.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.SellThreshold.ToString();
            SellRes_textAddOnValueWoods.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.WoodsConfig.PriceAddOn.ToString();
        }

        private void UpdateGUI_SelLRes_Stone()
        {
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellAmountMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.FIX_AMOUNT:
                    SellRes_radioSellAmountMethodStoneFix.Checked = true;
                    SellRes_radioSellAmountMethodStonePercent.Checked = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.PERCENT_STORAGE:
                    SellRes_radioSellAmountMethodStoneFix.Checked = false;
                    SellRes_radioSellAmountMethodStonePercent.Checked = true;
                    break;
            } //end switch
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.PriceAddOnMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.ADD_PRICE:
                    SellRes_radioAddOnMethodStoneAdd.Checked = true;
                    SellRes_radioAddOnMethodStoneMultiply.Checked = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.MULTIPLY_PRICE:
                    SellRes_radioAddOnMethodStoneAdd.Checked = false;
                    SellRes_radioAddOnMethodStoneMultiply.Checked = true;
                    break;
            }
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellPriceMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.FIX_PRICE:
                    SellRes_radioSellPriceMethodStoneFix.Checked = true;
                    SellRes_radioSellPriceMethodStoneAvg.Checked = false;
                    SellRes_radioSellPriceMethodStoneMin.Checked = false;
                    SellRes_radioAddOnMethodStoneAdd.Enabled = false;
                    SellRes_radioAddOnMethodStoneMultiply.Enabled = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.AVERAGE_PRICE:
                    SellRes_radioSellPriceMethodStoneFix.Checked = false;
                    SellRes_radioSellPriceMethodStoneAvg.Checked = true;
                    SellRes_radioSellPriceMethodStoneMin.Checked = false;
                    SellRes_radioAddOnMethodStoneAdd.Enabled = true;
                    SellRes_radioAddOnMethodStoneMultiply.Enabled = true;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.MIN_PRICE:
                    SellRes_radioSellPriceMethodStoneFix.Checked = false;
                    SellRes_radioSellPriceMethodStoneAvg.Checked = false;
                    SellRes_radioSellPriceMethodStoneMin.Checked = true;
                    SellRes_radioAddOnMethodStoneAdd.Enabled = true;
                    SellRes_radioAddOnMethodStoneMultiply.Enabled = true;
                    break;
            } //end switch
            SellRes_txtAmountSellStone.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellAmount.ToString();
            SellRes_txtAmountThresholdStone.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.SellThreshold.ToString();
            SellRes_textAddOnValueStone.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.StoneConfig.PriceAddOn.ToString();
        }

        private void UpdateGUI_SelLRes_Iron()
        {
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellAmountMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.FIX_AMOUNT:
                    SellRes_radioSellAmountMethodIronFix.Checked = true;
                    SellRes_radioSellAmountMethodIronPercent.Checked = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellAmountMethod.PERCENT_STORAGE:
                    SellRes_radioSellAmountMethodIronFix.Checked = false;
                    SellRes_radioSellAmountMethodIronPercent.Checked = true;
                    break;
            } //end switch
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.PriceAddOnMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.ADD_PRICE:
                    SellRes_radioAddOnMethodIronAdd.Checked = true;
                    SellRes_radioAddOnMethodIronMultiply.Checked = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumPriceAddOnMethod.MULTIPLY_PRICE:
                    SellRes_radioAddOnMethodIronAdd.Checked = false;
                    SellRes_radioAddOnMethodIronMultiply.Checked = true;
                    break;
            }
            switch (LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellPriceMethod)
            {
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.FIX_PRICE:
                    SellRes_radioSellPriceMethodIronFix.Checked = true;
                    SellRes_radioSellPriceMethodIronAvg.Checked = false;
                    SellRes_radioSellPriceMethodIronMin.Checked = false;
                    SellRes_radioAddOnMethodIronAdd.Enabled = false;
                    SellRes_radioAddOnMethodIronMultiply.Enabled = false;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.AVERAGE_PRICE:
                    SellRes_radioSellPriceMethodIronFix.Checked = false;
                    SellRes_radioSellPriceMethodIronAvg.Checked = true;
                    SellRes_radioSellPriceMethodIronMin.Checked = false;
                    SellRes_radioAddOnMethodIronAdd.Enabled = true;
                    SellRes_radioAddOnMethodIronMultiply.Enabled = true;
                    break;
                case LVAuto.LVConfig.AutoConfig.SellResourcesConfig.EnumSellPriceMethod.MIN_PRICE:
                    SellRes_radioSellPriceMethodIronFix.Checked = false;
                    SellRes_radioSellPriceMethodIronAvg.Checked = false;
                    SellRes_radioSellPriceMethodIronMin.Checked = true;
                    SellRes_radioAddOnMethodIronAdd.Enabled = true;
                    SellRes_radioAddOnMethodIronMultiply.Enabled = true;
                    break;
            } //end switch
            SellRes_txtAmountSellIron.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellAmount.ToString();
            SellRes_txtAmountThresholdIron.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.SellThreshold.ToString();
            SellRes_textAddOnValueIron.Text = LVConfig.AutoConfig.CONFIG_SELL_RESOURCES.IronConfig.PriceAddOn.ToString();
        }
    } //end class
} //end namespace
