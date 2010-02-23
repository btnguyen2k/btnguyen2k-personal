/* This file is to separate the business actions of FrmMain from the GUI's auto-generated actions
 * Contains actions for the Construct tab
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
        /// Reloads buildings for constructing.
        /// </summary>
        private void LVReloadBuildingsForConstruct()
        {
            if (Construct_dropdownCityList.SelectedIndex < 0)
            {
                //reload all cities
                LVHelper.CityCommandHelper.UpdateSimpleCityInfo();
                Construct_dropdownCityList.Items.Clear();
                Construct_dropdownCityList.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);
            }
            else
            {
                //reload one city
                int cityPos = Construct_dropdownCityList.SelectedIndex;
                LVObj.City city = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cityPos];
                LVHelper.CityCommandHelper.GetAndPopulateBuildings(city, true);
                LVLoadBuildingsToTreeViewForConstruct();
            }
        }

        /// <summary>
        /// Loads buildings into the tree view
        /// </summary>
        private void LVLoadBuildingsToTreeViewForConstruct()
        {
            Construct_treeBuilding.Nodes.Clear();

            int cityPos = Construct_dropdownCityList.SelectedIndex;
            LVObj.City city = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cityPos];

            WriteLog("Construct: Reloading tree view for city [" + city.Name + "]...");

            LVHelper.CityCommandHelper.GetAndPopulateBuildings(city, false);

            TreeNode rootNode = Construct_treeBuilding.Nodes.Add(city.Id.ToString(), city.Name);
            foreach (LVObj.Building building in city.AllBuildings)
            {
                int pid = building.PId;
                int gid = building.GId;
                int cityId = city.Id;
                string name = building.Name;
                int level = building.Level;
                string key = pid + "." + gid + "." + cityId;
                string text = name + " (" + level + ")";
                rootNode.Nodes.Add(key, text);
                WriteLog("    " + text + " {city:" + cityId + "; gid:" + gid + "; pid:" + pid + "}");
            }

            //rootNode = Construct_treeBuilding.Nodes[0];
            rootNode.ExpandAll();
            for (int i = 0; i < rootNode.Nodes.Count; i++)
            {
                rootNode.Nodes[i].Checked = city.AllBuildings[i].IsUp;
            }
            Construct_btnReloadBuildings.Visible = true;
            Construct_btnReloadBuildings.Enabled = true;
        }
    } //end class
} //end namespace
