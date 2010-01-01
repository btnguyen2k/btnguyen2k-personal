/* This file contains methods that are shared to the public */

using System.Windows.Forms;
using System.Collections;
using System;

namespace LVAuto.LVForm
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Sets "enable" attributes of all tabs
        /// </summary>
        /// <param name="enable"></param>
        public void SetAllTabsEnable(bool enable)
        {
            for (int i = 0; i < tabMainTab.TabPages.Count; i++)
            {
                tabMainTab.TabPages[i].Enabled = enable;
            }
        }
    }
}
