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
                Construct_dropdownCityList.Items.AddRange(Command.CityObj.City.AllCity);
            }
            else
            {
                //reload one city
                int cityPos = Construct_dropdownCityList.SelectedIndex;
                Command.CityObj.City city = Command.CityObj.City.AllCity[cityPos];
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
            Command.CityObj.City city = Command.CityObj.City.AllCity[cityPos];

            WriteLog("Construct: Reloading tree view for city [" + city.name + "]...");

            LVHelper.CityCommandHelper.GetAndPopulateBuildings(city, false);

            TreeNode rootNode = Construct_treeBuilding.Nodes.Add(city.id.ToString(), city.name);
            foreach (Command.CityObj.Building building in city.AllBuilding)
            {
                int pid = building.pid;
                int gid = building.gid;
                int cityId = city.id;
                string name = building.name;
                int level = building.level;
                string key = pid + "." + gid + "." + cityId;
                string text = name + " (" + level + ")";
                rootNode.Nodes.Add(key, text);
                WriteLog("    " + text + " {city:" + cityId + "; gid:" + gid + "; pid:" + pid + "}");
            }

            //rootNode = Construct_treeBuilding.Nodes[0];
            rootNode.ExpandAll();
            for (int i = 0; i < rootNode.Nodes.Count; i++)
            {
                rootNode.Nodes[i].Checked = city.AllBuilding[i].isUp;
            }
            Construct_btnReloadBuildings.Visible = true;
            Construct_btnReloadBuildings.Enabled = true;
        }
    } //end class
} //end namespace
