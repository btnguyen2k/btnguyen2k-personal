/* This file is to separate the business actions of FrmMain from the GUI's auto-generated actions
 * Contains actions for the Quest tab
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
        /// <summary>
        /// Removes general from quest list
        /// </summary>
        private void LVRemoveGeneralFromQuest()
        {
            foreach (object obj in Quest_QuestGeneralList.CheckedItems)
            {
                TuongDiThaoPhatList.Remove(obj);
            }
            Quest_QuestGeneralList.Items.Clear();
            Quest_QuestGeneralList.Items.AddRange(TuongDiThaoPhatList.ToArray());
        }

        /// <summary>
        /// Adds general to quest list
        /// </summary>
        private void LVAddGeneralToQuest()
        {
            try
            {
                //TuongDiThaoPhatList.Clear();
                foreach (Command.CityObj.MilitaryGeneral selectedGeneral in Quest_GeneralsInCity.CheckedItems)
                {
                    LVCommon.GeneralThaoPhat questGeneral = new LVCommon.GeneralThaoPhat();

                    questGeneral.CityId = ((LVAuto.LVForm.Command.CityObj.City)Quest_dropdownCityList.SelectedItem).id;
                    questGeneral.QuestId = ((Command.CommonObj.ThaoPhat)Quest_dropdownQuestList.SelectedItem).id;
                    questGeneral.TimeToRun = int.Parse(Quest_txtTimer.Text);

                    questGeneral.SiKhiMinToGo = int.Parse(Quest_txtMinMorale.Text);
                    questGeneral.TuUpSiKhi = Quest_checkAutoMorale.Checked;
                    questGeneral.TuBienCheQuan = Quest_checkAutoTroop.Checked;
                    questGeneral.SoLuongQuanMinToGo = int.Parse(Quest_txtMinTroops.Text);
                    questGeneral.AttachNumGenerals = int.Parse(Quest_dropdownAttackNumGenerals.SelectedItem.ToString());

                    string str = Quest_dropdownAttackMethod.SelectedItem.ToString();
                    questGeneral.PhuongThucTanCongID = int.Parse(str.Substring(0, str.IndexOf(".")));
                    questGeneral.PhuongThucTanCongName = str.Substring(str.IndexOf(".") + 2);

                    str = Quest_dropdownTargetMethod.SelectedItem.ToString();
                    questGeneral.PhuongThucChonMucTieuID = int.Parse(str.Substring(0, str.IndexOf(".")));
                    questGeneral.PhuongThucChonMucTieuName = str.Substring(str.IndexOf(".") + 2);

                    str = Quest_dropdownSchemeInBattle.SelectedItem.ToString();
                    questGeneral.MuuKeTrongChienTranID = int.Parse(str.Substring(0, str.IndexOf(".")));
                    questGeneral.MuuKeTrongChienTranName = str.Substring(str.IndexOf(".") + 2);

                    questGeneral.TuDoiTranHinhKhac = Quest_checkAutoFormula.Checked;
                    questGeneral.TuKhoiPhucTrangThai = Quest_checkAutoRestoreStatus.Checked;

                    questGeneral.Id = selectedGeneral.Id;
                    questGeneral.Name = selectedGeneral.Name;

                    questGeneral.NumInfantries = int.Parse(Quest_txtNumInfantries.Text);
                    questGeneral.NumCavalries = int.Parse(Quest_txtNumCavalries.Text);
                    questGeneral.NumArchers = int.Parse(Quest_txtNumArchers.Text);
                    questGeneral.NumCatapults = int.Parse(Quest_txtNumCatapults.Text);

                    TuongDiThaoPhatList.Add(questGeneral);
                } //for 

                if (TuongDiThaoPhatList.Count <= 0) 
                { 
                    return; 
                }
                int cityId = 0;
                foreach (LVCommon.GeneralThaoPhat questGeneral in TuongDiThaoPhatList)
                {
                    if (cityId == 0)
                    {
                        cityId = questGeneral.CityId;
                    }
                    else if ( cityId != questGeneral.CityId)
                    {
                        LVUtils.MsgBoxUtils.WarningBox("Các tướng thảo phạt phải cùng 1 thành/trại, xem lại đê");
                        TuongDiThaoPhatList.Clear();
                        break;
                    }
                }
                Quest_QuestGeneralList.Items.Clear();
                Quest_QuestGeneralList.Items.AddRange(TuongDiThaoPhatList.ToArray());
            }
            catch (Exception)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Chưa cài đặt đúng cho thảo phạt, xem lại đê");
            }
        }

        /// <summary>
        /// Reloads general list for the Quest tab.
        /// </summary>
        private void LVReloadGeneralsForQuest()
        {
            if (Quest_dropdownCityList.SelectedIndex < 0)
            {
                LVUtils.MsgBoxUtils.ErrorBox("Chưa chọn thành");
                return;
            }

            Quest_btnQuestReload.Enabled = false;
            try
            {
                ShowLoadingLabel();
                Command.CityObj.City city = (Command.CityObj.City)Quest_dropdownCityList.SelectedItem;
                LVAuto.LVForm.Command.Common.GetAllSimpleMilitaryGeneralInfoIntoCity();
                if (Command.CityObj.City.AllCity[Quest_dropdownCityList.SelectedIndex].MilitaryGeneral == null)
                {
                    LVUtils.MsgBoxUtils.WarningBox("Lỗi rồi, có thể mạng lởm, đợi tý làm lại đê");
                    return;
                }
                Quest_GeneralsInCity.Items.Clear();
                Quest_GeneralsInCity.Items.AddRange(Command.CityObj.City.AllCity[Quest_dropdownCityList.SelectedIndex].MilitaryGeneral);
            }
            finally
            {
                HideLoadingLabel();
                Quest_btnQuestReload.Enabled = true;
            }
        }
    }
}
