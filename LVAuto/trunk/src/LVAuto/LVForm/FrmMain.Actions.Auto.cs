﻿/* This file is to separate the business actions of FrmMain from the GUI's auto-generated actions
 * Contains actions for the Auto tab
 */

using System.Windows.Forms;
using System.Collections;
using System;
using System.Threading;
using System.Xml;

namespace LVAuto.LVForm
{
    public partial class FrmMain : Form
    {

        private const string NODE_ROOT = "lvauto";

        private const string NODE_CONSTRUCTIONS          = "constructions";
        private const string NODE_DESTRUCTIONS           = "destructions";
        private const string NODE_CONSTRUCTIONS_BUILDING = "building";
        private const string NODE_DESTRUCTIONS_BUILDING  = "building";
        private const string NODE_WEAPONS                = "weapons";
        private const string NODE_WEAPONS_WEAPON         = "weapon";
        private const string NODE_QUEST                  = "quest";
        private const string NODE_QUEST_GENERAL          = "general";

        private const string ATTR_TIME                       = "time";
        private const string ATTR_CITY_ID                    = "cityId";
        private const string ATTR_CITY_NAME                  = "cityName";
        private const string ATTR_BUILDING_NAME              = "buildingName";
        private const string ATTR_BUILDING_GID               = "buildingGid";
        private const string ATTR_BUILDING_PID               = "buildingPid";
        private const string ATTR_CONSTRUCT_ALL              = "constructAll";
        private const string ATTR_AUTO_RESOURCE              = "autoResource";
        private const string ATTR_GOLD_THRESHOLD             = "goldThreshold";
        private const string ATTR_NUM_WEAPONS_PER_BATCH      = "numWeaponsPerBatch";
        private const string ATTR_TOTAL_WEAPONS              = "totalWeapons";
        private const string ATTR_ARMOUR_ID                  = "armourId";
        private const string ATTR_WEAPON_ID                  = "weaponId";
        private const string ATTR_HORSE_ID                   = "horseId";
        private const string ATTR_PRODUCTION_LEVEL           = "productionLevel";
        private const string ATTR_QUEST_ID                   = "questId";
        private const string ATTR_QUEST_NAME                 = "questName";
        private const string ATTR_QUEST_ATTACK_NUM_GENERALS  = "attackNumGenerals";
        private const string ATTR_GENERAL_ID                 = "generalId";
        private const string ATTR_GENERAL_NAME               = "generalName";
        private const string ATTR_GENERAL_MIN_MORALE         = "minMorale";
        private const string ATTR_GENERAL_AUTO_MORALE        = "autoMorale";
        private const string ATTR_GENERAL_MIN_TROOPS         = "minTroops";
        private const string ATTR_GENERAL_AUTO_TROOPS        = "autoTroops";
        private const string ATTR_GENERAL_ATTACK_METHOD_ID   = "attachMethodId";
        private const string ATTR_GENERAL_ATTACK_METHOD_NAME = "attachMethodName";
        private const string ATTR_GENERAL_SELECT_TARGET_ID   = "selectTargetId";
        private const string ATTR_GENERAL_SELECT_TARGET_NAME = "selectTargetName";
        private const string ATTR_GENERAL_AUTO_FORMULA       = "autoFormula";
        private const string ATTR_GENERAL_NUM_ARCHERS        = "numArchers";
        private const string ATTR_GENERAL_NUM_INFANTRIES     = "numInfantries";
        private const string ATTR_GENERAL_NUM_CAVALRIES      = "numCavalries";
        private const string ATTR_GENERAL_NUM_CATAPULTS      = "numCatapults";

        private void Auto_checkAutoSellResources_CheckedChanged(object sender, EventArgs e)
        {
            LVToggleAutoSellResources();
        }

        private void Auto_checkAutoBuyResources_CheckedChanged(object sender, EventArgs e)
        {
            LVToggleAutoBuyResources();
        }

        /// <summary>
        /// Save configurations.
        /// </summary>
        private void LVSaveConfig()
        {
            DialogResult dr = dlgSaveConfigFile.ShowDialog();
            if (dr == DialogResult.OK || dr == DialogResult.Yes)
            {
                string filename = dlgSaveConfigFile.FileName;
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                XmlNode root = xmlDoc.CreateElement(NODE_ROOT);
                xmlDoc.AppendChild(root);

                LVSaveConfig_Constructions(xmlDoc, root);
                LVSaveConfig_Destructions(xmlDoc, root);
                LVSaveConfig_Weapons(xmlDoc, root);
                LVSaveConfig_Quest(xmlDoc, root);

                xmlDoc.Save(filename);
                //(new LVAuto.LVWeb.SaveNLoad()).saveConfig(filename);
            }
        }

        /// <summary>
        /// Saves weapon configurations.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="root"></param>
        private void LVSaveConfig_Quest(XmlDocument doc, XmlNode root)
        {
            XmlElement myNode = doc.CreateElement(NODE_QUEST);
            root.AppendChild(myNode);
            myNode.SetAttribute(ATTR_TIME, Quest_txtTimer.Text);
            Command.CommonObj.ThaoPhat quest = (Command.CommonObj.ThaoPhat)Quest_dropdownQuestList.SelectedItem;
            myNode.SetAttribute(ATTR_QUEST_ID, quest.id.ToString());
            myNode.SetAttribute(ATTR_QUEST_NAME, quest.name);
            myNode.SetAttribute(ATTR_QUEST_ATTACK_NUM_GENERALS, Quest_dropdownAttackNumGenerals.SelectedItem.ToString());
            
            foreach (LVAuto.LVForm.LVCommon.GeneralThaoPhat general in LVAuto.LVForm.FrmMain.TuongDiThaoPhatList)
            {
                XmlElement node = doc.CreateElement(NODE_QUEST_GENERAL);
                myNode.AppendChild(node);
                node.SetAttribute(ATTR_CITY_ID, general.CityId.ToString());
                node.SetAttribute(ATTR_CITY_NAME, general.CityName);
                node.SetAttribute(ATTR_GENERAL_ID, general.Id.ToString());
                node.SetAttribute(ATTR_GENERAL_NAME, general.Name);
                node.SetAttribute(ATTR_GENERAL_AUTO_MORALE, general.TuUpSiKhi.ToString());
                node.SetAttribute(ATTR_GENERAL_MIN_MORALE, general.SiKhiMinToGo.ToString());
                node.SetAttribute(ATTR_GENERAL_AUTO_TROOPS, general.TuBienCheQuan.ToString());
                node.SetAttribute(ATTR_GENERAL_MIN_TROOPS, general.SoLuongQuanMinToGo.ToString());
                node.SetAttribute(ATTR_GENERAL_ATTACK_METHOD_ID, general.PhuongThucTanCongID.ToString());
                node.SetAttribute(ATTR_GENERAL_ATTACK_METHOD_NAME, general.PhuongThucTanCongName);
                node.SetAttribute(ATTR_GENERAL_SELECT_TARGET_ID, general.PhuongThucChonMucTieuID.ToString());
                node.SetAttribute(ATTR_GENERAL_SELECT_TARGET_NAME, general.PhuongThucChonMucTieuName);
                node.SetAttribute(ATTR_GENERAL_AUTO_FORMULA, general.TuDoiTranHinhKhac.ToString());
                node.SetAttribute(ATTR_GENERAL_NUM_ARCHERS, general.NumArchers.ToString());
                node.SetAttribute(ATTR_GENERAL_NUM_CAVALRIES, general.NumCavalries.ToString());
                node.SetAttribute(ATTR_GENERAL_NUM_INFANTRIES, general.NumInfantries.ToString());
                node.SetAttribute(ATTR_GENERAL_NUM_CATAPULTS, general.NumCatapults.ToString());

                //data += gen.MuuKeTrongChienTranID + spareChar;
                //data += gen.MuuKeTrongChienTranName + spareChar;
                //data += gen.TuKhoiPhucTrangThai.ToString() + spareChar;
            }

        } //end LVSaveConfig_Quest

        /// <summary>
        /// Saves weapon configurations.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="root"></param>
        private void LVSaveConfig_Weapons(XmlDocument doc, XmlNode root)
        {
            XmlElement myNode = doc.CreateElement(NODE_WEAPONS);
            root.AppendChild(myNode);
            myNode.SetAttribute(ATTR_TIME, LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKWEPON.Text);
            myNode.SetAttribute(ATTR_NUM_WEAPONS_PER_BATCH, LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCOUNTWEPON.Text);

            foreach (LVAuto.LVForm.Command.OPTObj.Weapon weapon in LVAuto.LVForm.FrmMain.LVFRMMAIN.pnWepon.Controls)
            {
                if (weapon.chkOK.Checked)
                {
                    XmlElement node = doc.CreateElement(NODE_WEAPONS_WEAPON);
                    myNode.AppendChild(node);
                    node.SetAttribute(ATTR_CITY_ID, weapon.cityid.ToString());
                    node.SetAttribute(ATTR_CITY_NAME, (LVAuto.LVForm.Command.City.GetCityByID(weapon.cityid)).Name);
                    node.SetAttribute(ATTR_WEAPON_ID, weapon.cboWepon.SelectedIndex.ToString());
                    node.SetAttribute(ATTR_ARMOUR_ID, weapon.cboAmor.SelectedIndex.ToString());
                    node.SetAttribute(ATTR_HORSE_ID, weapon.cboHorse.SelectedIndex.ToString());
                    node.SetAttribute(ATTR_PRODUCTION_LEVEL, weapon.cboLevel.SelectedIndex.ToString());
                    node.SetAttribute(ATTR_TOTAL_WEAPONS, weapon.txtAmount.Text);

                    //data += vk.posid_w + spareChar;
                    //data += vk.posid_a + spareChar;
                    //data += vk.posid_h + spareChar;
                }
            }
        } //end LVSaveConfig_Weapons

        /// <summary>
        /// Saves construction configurations.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="root"></param>
        private void LVSaveConfig_Constructions(XmlDocument doc, XmlNode root)
        {
            XmlElement myNode = doc.CreateElement(NODE_CONSTRUCTIONS);
            root.AppendChild(myNode);
            myNode.SetAttribute(ATTR_TIME, LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_txtTimer.Text);
            myNode.SetAttribute(ATTR_CONSTRUCT_ALL, LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_checkUpgradeAll.Checked.ToString());
            myNode.SetAttribute(ATTR_AUTO_RESOURCE, LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_checkAutoBuyResources.Checked.ToString());
            myNode.SetAttribute(ATTR_GOLD_THRESHOLD, LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_txtGoldThreshold.Text);

            if ( LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null )
            {
                return;
            }

            foreach (LVAuto.LVObj.City city in LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities)
            {
                if (city.AllBuildings == null)
                {
                    continue;
                }

                foreach (LVAuto.LVObj.Building building in city.AllBuildings)
                {
                    if (building.IsUp)
                    {
                        XmlElement nodeBuilding = doc.CreateElement(NODE_CONSTRUCTIONS_BUILDING);
                        myNode.AppendChild(nodeBuilding);
                        nodeBuilding.SetAttribute(ATTR_CITY_ID, city.Id.ToString());
                        nodeBuilding.SetAttribute(ATTR_CITY_NAME, city.Name);
                        nodeBuilding.SetAttribute(ATTR_BUILDING_NAME, building.Name);
                        nodeBuilding.SetAttribute(ATTR_BUILDING_GID, building.GId.ToString());
                        nodeBuilding.SetAttribute(ATTR_BUILDING_PID, building.PId.ToString());
                    }
                }
            }
        } //end LVSaveConfig_Constructions

        /// <summary>
        /// Saves destruction configurations.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="root"></param>
        private void LVSaveConfig_Destructions(XmlDocument doc, XmlNode root)
        {
            XmlElement myNode = doc.CreateElement(NODE_DESTRUCTIONS);
            root.AppendChild(myNode);
            myNode.SetAttribute(ATTR_TIME, LVAuto.LVForm.FrmMain.LVFRMMAIN.txtDELCHECK.Text);

            if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities == null)
            {
                return;
            }

            foreach (LVAuto.LVObj.City city in LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities)
            {
                if (city.AllBuildings == null)
                {
                    continue;
                }

                foreach (LVAuto.LVObj.Building building in city.AllBuildings)
                {
                    if (building.IsDown)
                    {
                        XmlElement nodeBuilding = doc.CreateElement(NODE_DESTRUCTIONS_BUILDING);
                        myNode.AppendChild(nodeBuilding);
                        nodeBuilding.SetAttribute(ATTR_CITY_ID, city.Id.ToString());
                        nodeBuilding.SetAttribute(ATTR_CITY_NAME, city.Name);
                        nodeBuilding.SetAttribute(ATTR_BUILDING_NAME, building.Name);
                        nodeBuilding.SetAttribute(ATTR_BUILDING_GID, building.GId.ToString());
                        nodeBuilding.SetAttribute(ATTR_BUILDING_PID, building.PId.ToString());
                    }
                }
            }
        } //end LVSaveConfig_Destructions

        /// <summary>
        /// Toggles the "Auto All" check box.
        /// </summary>
        private void LVToggleAutoAll()
        {
            Auto_checkAutoConstruct.Enabled = Auto_checkAutoAll.Checked;
            Auto_checkAutoBuyResources.Enabled = Auto_checkAutoAll.Checked;
            Auto_checkAutoSellResources.Enabled = Auto_checkAutoAll.Checked;
            Auto_checkAutoQuest.Enabled = Auto_checkAutoAll.Checked;
            chkAutoUpgrade.Enabled = Auto_checkAutoAll.Checked;
            chkAutoUpDanSo.Enabled = Auto_checkAutoAll.Checked;
            chkAutoVanchuyen.Enabled = Auto_checkAutoAll.Checked;
            chkAutoMove.Enabled = Auto_checkAutoAll.Checked;
            Auto_checkAutoMorale.Enabled = Auto_checkAutoAll.Checked;
            chkAutobuyWepon.Enabled = Auto_checkAutoAll.Checked;
            Auto_checkAutoDestruct.Enabled = Auto_checkAutoAll.Checked;
            Auto_checkAutoTroops.Enabled = Auto_checkAutoAll.Checked;
            chkAutoDieuPhai.Enabled = Auto_checkAutoAll.Checked;
            chkAUTOVCVK.Enabled = Auto_checkAutoAll.Checked;
            chkAUTOBINHMAN.Enabled = Auto_checkAutoAll.Checked;
            chkAUTOCALLMAN.Enabled = Auto_checkAutoAll.Checked;

            if (!Auto_checkAutoAll.Checked)
            {
                StopAllAutoThreads();
            }
            else
            {
                if (!LVAUTOTASK.IsRun) LVAUTOTASK.Auto();
                if (!LVCITYTASK.IsRun) LVCITYTASK.Auto();
            }
        } //end LVToggleAutoAll

        /// <summary>
        /// Toggles the "Auto Construct" check box.
        /// </summary>
        private void LVToggleAutoConstruct()
        {
            if (Auto_checkAutoConstruct.Checked)
            {
                THREAD_CONSTRUCT.SetUp(int.Parse(Construct_txtTimer.Text) * 60 * 1000);
                Construct_treeBuilding.Enabled = false;
                pnXayNha.Enabled = false;
                Construct_btnReloadBuildings.Enabled = false;
                THREAD_CONSTRUCT.Start();
            }
            else
            {
                Construct_treeBuilding.Enabled = true;
                pnXayNha.Enabled = true; ;
                THREAD_CONSTRUCT.Stop();
            }
        }

        /// <summary>
        /// Toggles the "Auto Sell Resources" check box.
        /// </summary>
        private void LVToggleAutoSellResources()
        {
            if (Auto_checkAutoSellResources.Checked)
            {
                THREAD_SELL_RESOURCES.SetUp(int.Parse(SellRes_txtTimer.Text) * 60);
                pnSELL.Enabled = false;
                THREAD_SELL_RESOURCES.Start();
            }
            else
            {
                THREAD_SELL_RESOURCES.Stop();
                pnSELL.Enabled = true;
            }
        }

        /// <summary>
        /// Toggles the "Auto Buy Resources" check box.
        /// </summary>
        private void LVToggleAutoBuyResources()
        {
            if (Auto_checkAutoBuyResources.Checked)
            {
                THREAD_BUY_RESOURCES.SetUp(int.Parse(BuyRes_txtTimer.Text) * 60);
                pnLVBUYRES.Enabled = false;
                THREAD_BUY_RESOURCES.Start();
            }
            else
            {
                THREAD_BUY_RESOURCES.Stop();
                pnLVBUYRES.Enabled = true;
            }
        }
    } //end class
} //end namespace
