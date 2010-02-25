using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using LVAuto.LVForm.HTMLParse;
using LVAuto.LVObj;
using LVAuto.LVForm.Command.CommonObj;

namespace LVAuto.LVForm
{
    public partial class FrmMain : Form 
	{
        private const string APP_NAME = "LVAuto";
        private const string APP_VERSION = "0.1";
        private string strVersion = APP_NAME + " " + APP_VERSION;

		private bool IsLogin = false;

		public static LVAuto.LVForm.FrmMain LVFRMMAIN;

		public static Label _lblLoading;

		public static ArrayList ListBienChe = new ArrayList(); // List danh sach bien che
		//public static LVAuto.Common.PhaiQuanVanDaoMo QuanVanDaoMo = new LVAuto.Common.PhaiQuanVanDaoMo();
		//public static LVAuto.Common.GeneralThaoPhat TuongDiThaoPhat = new LVAuto.Common.GeneralThaoPhat();
		public static ArrayList TuongDiThaoPhatList = new ArrayList();//  LVAuto.Common.GeneralThaoPhat;

		public static ArrayList AnuiForAuto = new ArrayList();
		public static ArrayList UpgradeForAuto = new ArrayList();			// nang cap trong dai hoc dien [0]: cityid; [1->]: id can nang cap
		
		public static ArrayList DieuPhaiQuanVanVo = new ArrayList();
		public static Command.CommonObj.BanTaiNguyenObj BanTaiNguyen = new LVAuto.LVForm.Command.CommonObj.BanTaiNguyenObj();
		public static Command.CommonObj.MuaTaiNguyenObj MuaTaiNguyen = new LVAuto.LVForm.Command.CommonObj.MuaTaiNguyenObj();
		public static ArrayList DiChuyenTrai = new ArrayList();
		public static ArrayList VanChuyenVuKhi = new ArrayList();
		public static ArrayList BinhManList = new ArrayList();
		public static ArrayList CallManList = new ArrayList(); //LVAuto.Command.CommonObj.CallManObj


		public static bool TuLinhBongLoc = true;
		public static bool ThongBaoDanhTuongViengTham = true;
		//public static string DanhTuongDangViengTham = "";
		public static bool TuBanThuongDoDe = true;
		public static bool TuBanAnUiKhiDanNo = true;
		public static bool TuXoaNhacNhoCuaAdmin = true;

		public static  int _tuxacnhananh = 1;  //0: ko lam gi ca; 1: hien anh; 2: tu xac nhan
		public static bool _chuongbao = false;
		public static int imagecheckid;

		public bool Anui_loaded				= false;
		public bool BuildCity_loaded = false;
		public bool DelCity_loaded = false;
		public bool ThaoPhat_loaded = false;
		public bool ReSearch_loaded = false;

		public bool VanChuyen_loaded = false;
		public bool MoveDoanhTrai_loaded = false;
		public bool Upsikhi_loaded = false;
		public bool BuyWepon_loaded = false;

		private string fileSavepath = "";

		private Hashtable QuocKho_Ruong = new Hashtable();
		
		public int SoLanTuCheckAnhSaiLienTiep = 0;
		public DateTime LastTimeImageCheck = DateTime.Now;


		// for proxy

		public static string ProxyProtocol;
		public static string ProxyServer;
		public static string ProxyPort;
		public static string ProxyUser;
		public static string ProxyPass;
 
		public FrmMain() 
		{
            InitializeComponent();
            InitEventHandlers_SellRes();
            InitEventHandlers_BuyRes();
			timerDanhTuongViengTham.Start();
        }

        /// <summary>
        /// Auto-Called when form is loaded for the first time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Main_Load(object sender, EventArgs e) 
		{
			try
			{
                this.AppNotifyIcon.Text = this.strVersion;
                LVFRMMAIN = this;

                #if (!DEBUG)
                    // Disable all tabs, except for the "login" one
                    SetAllTabsEnable(false);
                    tabMainTab.TabPages[0].Enabled = true; //"Login" tab is the first one
                #endif

                Login_txtUsername.Focus();
		        //panelLoading.BringToFront();
			    //panelLoading.Visible = false;
			    picAttach.Visible = false;

                LVLoadServerList();

                LVCITYTASK = new LVAuto.LVForm.LVThread.CITYTASK();
                LVAUTOTASK = new LVAuto.LVForm.LVThread.AUTOTASK(lblLoadingResMessage);

                THREAD_SELL_RESOURCES = LVAuto.LVThread.AutoSellResources.getInstance(Auto_labelAutoSellRes);
                THREAD_BUY_RESOURCES = LVAuto.LVThread.AutoBuyResources.getInstance(Auto_labelAutoBuyRes);
                THREAD_CONSTRUCT = LVAuto.LVThread.AutoConstruct.getInstance(Auto_labelAutoConstruct);

                LVDEL = new LVAuto.LVForm.LVThread.DEL(lblDELMESSAGE);
                LVTHAOPHAT = new LVAuto.LVForm.LVThread.THAOPHAT(lblTHAOPHATMESSAGE);
                LVUPGRADE = new LVAuto.LVForm.LVThread.UPGRADE(lblUPGEADEMESSAGE);
                LVANUI = new LVAuto.LVForm.LVThread.ANUI(lblANUIMESSAGE);
                LVVANCHUYEN = new LVAuto.LVForm.LVThread.VANCHUYEN(lblVANCHUYENMESSAGE);
                LVMOVEDOANHTRAI = new LVAuto.LVForm.LVThread.MOVEDOANHTRAI(lblMOVEDOANHTRAI);
                LVSIKHI = new LVAuto.LVForm.LVThread.SIKHI(lblSIKHIMESSAGE);
                LVBUYWEPON = new LVAuto.LVForm.LVThread.BUYWEPON(lblBUYWEPONMESSAGE);
                LVBIENCHE = new LVAuto.LVForm.LVThread.BIENCHE(lblBIENCHEMESSAGE);
                LVDAOMO = new LVAuto.LVForm.LVThread.PHAIQUANVANDAOMO(lblDIEUPHAIMESSAGE);
                LVLOIDAI = new LVAuto.LVForm.LVThread.LOIDAI(lblDIEUPHAIMESSAGE);
                LVAUTOVCVK = new LVAuto.LVForm.LVThread.AUTOVANCHUYENVK(lblAUTOVCVKMESSAGE);
                LVAUTOBINHMAN = new LVAuto.LVForm.LVThread.AUTOBINHMAN(lblAUTOBINHMANMESSAGE);
                LVAUTOCALLMAN = new LVAuto.LVForm.LVThread.AUTOCALLMAN(lblAUTOCALLMANMESSAGE);


                chkTabBinhManListManDanh.Items.Clear();
                chkTabBinhManListManDanh.Items.Add("1. Uy Quốc");
                chkTabBinhManListManDanh.Items.Add("2. Tiên Ti");
                chkTabBinhManListManDanh.Items.Add("3. Ô hoàn");
                chkTabBinhManListManDanh.Items.Add("4. Sơn việt");
                chkTabBinhManListManDanh.Items.Add("5. Khương để");
                chkTabBinhManListManDanh.Items.Add("6. Mạnh hoạch");
                chkTabBinhManListManDanh.Items.Add("7. Hung nô");
                chkTabBinhManListManDanh.Items.Add("101. Quân lương");
                chkTabBinhManListManDanh.Items.Add("201. Thần thú");

                #if (DEBUG)
                    strVersion += " (Debug)";

                    //txtTabBinhManMaxODi.Text = "50";
                #else
                    //tabMainTab.Controls.Remove(tabBinhMan);
                    //chkAUTOBINHMAN.Visible = false;
				    //lblAUTOBINHMANMESSAGE.Visible = false;
                    strVersion += " (Release)";

                    //txtTabBinhManMaxODi.Text = "10";
                #endif

                this.Text = strVersion;
                this.AppNotifyIcon.Text = this.strVersion;
                Login_txtUsername.Focus();
			}
			catch (Exception ex)
			{
                LVUtils.MsgBoxUtils.ErrorBox("Unexpected error:" + ex.Message);
                this.Close();
			}
        }

		private void tabControl1_Selected(object sender, TabControlEventArgs e)
		{
            SwitchTab(tabMainTab.SelectedTab);
    	}

		private void LoadDataForBinhMan()
		{
			cbTabBinhManThanhXuatQuan.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities);


		}
		private void LoadDataForVCVK()
		{
			try
			{

				chklVCVKThanhDi.Items.Clear();
				chklVCVKThanhDen.Items.Clear();
				chklVCVKLoaiVuKhi.Items.Clear();
				for (int i = 0; i < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
				{
					if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].Id > 0)
					{
						chklVCVKThanhDi.Items.Add(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i], false);
						chklVCVKThanhDen.Items.Add(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i], false);
					}
				}
				chklVCVKLoaiVuKhi.Items.AddRange(LVCommon.Wepons.arWepon);


				/*
				cbVCVKThanhDi.Items.Clear();
				chklVCVKThanhDen.Items.Clear();
				cbVCVKLoaiVK.Items.Clear();
				for (int i = 0; i < LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
				{
					if (LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].id > 0)
					{
						cbVCVKThanhDi.Items.Add(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i]);
						chklVCVKThanhDen.Items.Add(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i], false);
					}
				}

				cbVCVKLoaiVK.Items.AddRange(Common.Wepons.arWepon);
				*/

			}
			catch (Exception ex)
			{

			}
		}


		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) 
		{
            this.Show();
			WindowState = FormWindowState.Normal;

        }

        private void frmmain_Deactivate(object sender, EventArgs e) {
            if (Auto_checkAutoAll.Checked) 
			{
               // this.Hide();
				if (WindowState == FormWindowState.Minimized) this.Hide();
            }
        }


        private void Auto_checkAutoAll_CheckedChanged(object sender, EventArgs e) 
		{
            LVToggleAutoAll();
        }

        private void cmdGet_Click(object sender, EventArgs e) {
            Command.OPT.BuyItem(1);
        }

        private void Quest_dropdownCityList_SelectedIndexChanged(object sender, EventArgs e) {
            LVReloadGeneralsForQuest();
        }

        private void cmdLoadConfig_Click(object sender, EventArgs e) {
            openFileDialog1.ShowDialog();
			
			(new LVAuto.LVWeb.SaveNLoad()).loadConfig(fileSavepath);

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e) {

			fileSavepath = openFileDialog1.FileName;
        }

        private void Auto_btnSaveConfig_Click(object sender, EventArgs e) {
            LVSaveConfig();
        }

        private void Auto_checkAutoConstruct_CheckedChanged(object sender, EventArgs e) {
            LVToggleAutoConstruct();
        }

        private void frmmain_FormClosing(object sender, FormClosingEventArgs e) 
		{

			string message =  "Hi " + LVAuto.LVWeb.LVClient.lvusername + "!!!\r\n Bạn không muốn chạy chương trình này nữa phải không?";
			string caption = "Chán quá, chuồn thôi  " + LVAuto.LVWeb.LVClient.lvusername;

			MessageBoxButtons buttons = MessageBoxButtons.YesNo;
			DialogResult result;

			// Displays the MessageBox.
			result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

			if(result == DialogResult.Yes)
			{
				StopAllAutoThreads();
				// Closes the parent form.
				//this.Close();
				e.Cancel = false;	

			}
			else
			{
				e.Cancel = true;				
			}
        }

        private void chkAutoST_CheckedChanged(object sender, EventArgs e) {
            if (Auto_checkAutoQuest.Checked) 
			{
				try
				{

					//-- phan nay chua check
				 /*
					LVTHAOPHAT.GetParameter(
                        TuongDiThaoPhat.cityid , TuongDiThaoPhat.general1, TuongDiThaoPhat.general2, TuongDiThaoPhat.general3, 
						TuongDiThaoPhat.general4, TuongDiThaoPhat.general5,
                        TuongDiThaoPhat.nhiemvuid,
                        TuongDiThaoPhat.tuupsikhi,
                        TuongDiThaoPhat.timetorun* 60 * 1000, 
						TuongDiThaoPhat.sikhimin,
						TuongDiThaoPhat.soluongquanmin,
						TuongDiThaoPhat.tubienchequan
                    );
					*/

					LVTHAOPHAT.GetParameter(TuongDiThaoPhatList, int.Parse((Quest_txtTimer.Text)) * 60 * 1000);

				 


                    pnTHAOPHAT.Enabled = false;
					LVTHAOPHAT.Auto();

				
				} 
			    catch (Exception ex) 
				{
                    lblTHAOPHATMESSAGE.Text = "Chưa chọn đúng tham số";
                    Auto_checkAutoQuest.Checked = false;
                }
            } 
		    else 
	        {
                LVTHAOPHAT.Stop();
                pnTHAOPHAT.Enabled = true;
            }
        }

        private void chkAutoUpgrade_CheckedChanged(object sender, EventArgs e) {
            if (chkAutoUpgrade.Checked) 
			{
                try 
				{
                   
					//LVUPGRADE.GetParameter(tvUpdate, ((LVAuto.LVObj.City)cboCityForUpgrade.SelectedItem).id, 
					//	int.Parse(txtCHECKUPDATE.Text) * 60 * 1000);

					int cityid = int.Parse(UpgradeForAuto[0].ToString()); ;
					int citypos = LVAuto.LVForm.Command.City.GetCityPostByID(cityid);
					if (citypos == -1)
					{
						lblUPGEADEMESSAGE.Text = "Chưa chọn thành chính";
						chkAutoUpgrade.Checked = false;
						return;
					}

					LVUPGRADE.GetParameter(UpgradeForAuto,	int.Parse(txtCHECKUPDATE.Text.Trim()) * 60 * 1000);
					string str = txtCHECKUPDATE.Text; 											  					
					pnUPGRADE.Enabled = false;
                    LVUPGRADE.Auto();
                } 
				catch (Exception ex) 
				{
                    lblUPGEADEMESSAGE.Text = "Chưa chọn thành chính";
                    chkAutoUpgrade.Checked = false;
                }
            } else {
                LVUPGRADE.Stop();
                pnUPGRADE.Enabled = true;
            }
        }

        private void chkAutoUpDanSo_CheckedChanged(object sender, EventArgs e) {
			try
			{
			if (chkAutoUpDanSo.Checked) 
			{
                //LVANUI.GetParameter(dtaAnUi, int.Parse(txtCHECKANUI.Text) * 60 * 1000);
                //dtaAnUi.Enabled = false;

				//LVANUI.GetParameter(dtaAnUi, int.Parse(txtCHECKANUI.Text) * 60 * 1000);
				//dtaAnUi.Enabled = false;
				//LVANUI.GetParameter(AnuiForAuto, int.Parse(txtCHECKANUI.Text) * 60 * 1000);
				LVANUI.GetParameter(AnuiForAuto, chkANUI_TuMuaLua.Checked, long.Parse(txtANUI_VangAnToan.Text), int.Parse(txtCHECKANUI.Text) * 60 * 1000);
				chklstAnUi.Enabled = false;
				LVANUI.Auto();

				//LVANUI.run();

            } else {
                LVANUI.Stop();
                //dtaAnUi.Enabled = true;
				chklstAnUi.Enabled = true;
            }
			}
			catch (Exception ex)
			{
				lblANUIMESSAGE.Text = "Chưa chọn đúng tham số";
				chkAutoUpDanSo.Checked = false;
			}
        }

        private void chkAutoVanchuyen_CheckedChanged(object sender, EventArgs e) {
            try {
			if (chkAutoVanchuyen.Checked) {
                LVVANCHUYEN.GetParameter(pnVanchuyen, int.Parse(txtCHECKVANCHUYEN.Text) * 60 * 1000);
                pnVanchuyen.Enabled = false;
                LVVANCHUYEN.Auto();
            } else {
                LVVANCHUYEN.Stop();
                pnVanchuyen.Enabled = true;
            }
			}
			catch (Exception ex)
			{
				lblVANCHUYENMESSAGE.Text = "Chưa chọn đúng tham số";
				chkAutoVanchuyen.Checked = false;
			}
        }

		private void chkAutoMove_CheckedChanged(object sender, EventArgs e)
		{
			try
			{

				if (chkAutoMove.Checked)
				{
					//LVMOVEDOANHTRAI.GetParameter(pnDoanhTrai, int.Parse(txtCHECKMOVE.Text) * 60 * 1000);
					if (DiChuyenTrai.Count > 0)
					{
						LVMOVEDOANHTRAI.GetParameter(DiChuyenTrai, int.Parse(txtCHECKMOVE.Text) * 60 * 1000);
						pnDoanhTrai.Enabled = false;
						LVMOVEDOANHTRAI.Auto();
					}
					else
					{
						lblMOVEDOANHTRAI.Text = "Chưa chọn đúng tham số";
						chkAutoMove.Checked = false;
					}
				}
				else
				{
					LVMOVEDOANHTRAI.Stop();
					pnDoanhTrai.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				lblMOVEDOANHTRAI.Text = "Chưa chọn đúng tham số";
				chkAutoMove.Checked = false;
			}
		}

        private void tvBUILD_AfterCheck(object sender, TreeViewEventArgs e) {
            foreach (TreeNode c in e.Node.Nodes) 
            {
                c.Checked = e.Node.Checked;
            }
        }

        private void chkAutoUpSiKhi_CheckedChanged(object sender, EventArgs e) 
		{
			try
			{
				if (Auto_checkAutoMorale.Checked) 
				{
					LVSIKHI.GetParameter(tvSIKHI,int.Parse(txtOneSiKhi.Text), int.Parse(txtCHECKSIKHI.Text) * 60 * 1000);
					tvSIKHI.Enabled = false;
					LVSIKHI.Auto();
				} else {
					LVSIKHI.Stop();
					tvSIKHI.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				lblSIKHIMESSAGE.Text = "Chưa chọn đúng tham số";
				Auto_checkAutoMorale.Checked = false;
			}
        }

        private void chkAutobuyWepon_CheckedChanged(object sender, EventArgs e) 
		{
			try
			{
				if (chkAutobuyWepon.Checked) 
				{
					LVBUYWEPON.GetParameter(int.Parse(txtCOUNTWEPON.Text),pnWepon, int.Parse(txtCHECKWEPON.Text) * 60 * 1000);
					pnWepon.Enabled = false;
					LVBUYWEPON.Auto();
				} else 
				{
					LVBUYWEPON.Stop();
					pnWepon.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				lblBUYWEPONMESSAGE.Text = "Chưa chọn đúng tham số";
				chkAutobuyWepon.Checked = false;
         
			}

        }

        private void cmdUpdatePrice_Click_1(object sender, EventArgs e) {
            //LVThread.UPDATEPRICE up = new LVAuto.LVThread.UPDATEPRICE(int.Parse(txtPRICELUA.Text), int.Parse(txtPRICEGO.Text), int.Parse(txtPRICEDA.Text), int.Parse(txtPRICESAT.Text));

			LVThread.UPDATEPRICE up = new LVAuto.LVForm.LVThread.UPDATEPRICE(int.Parse(SellRes_textAddOnValueFood.Text), int.Parse(SellRes_textAddOnValueWoods.Text), 
												int.Parse(SellRes_textAddOnValueStone.Text), int.Parse(SellRes_textAddOnValueIron.Text));
			up.Auto();
        }

        private void chkDebug_CheckedChanged(object sender, EventArgs e) {
            lock (LVAuto.LVWeb.LVClient.islock) {
                LVAuto.LVWeb.LVClient.debug = chkDebug.Checked;
            }
        }

        private void cmdBanthuong_Click(object sender, EventArgs e) 
		{
            LVThread.BANTHUONG bt = new LVAuto.LVForm.LVThread.BANTHUONG(true);

            bt.Auto();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            lock (LVAuto.LVWeb.LVClient.islock) {
                lock (LVAuto.LVWeb.LVClient.ispause) {
                    LVAuto.LVWeb.LVClient.issendsms = checkBox2.Checked;
                    LVAuto.LVWeb.LVClient.smsusername = txtPhoneSend.Text;
                    LVAuto.LVWeb.LVClient.smspass = txtPassword.Text;
                    LVAuto.LVWeb.LVClient.smsto = txtTo.Text;
                }
            }
        }

        private void cmdTestSMS_Click(object sender, EventArgs e) {
            LVAuto.LVWeb.LVClient.LoginMobi(txtPhoneSend.Text, txtPassword.Text, txtTo.Text, "Test thu tinh nang send sms");
        }

        private void Construct_btnReloadBuildings_Click(object sender, EventArgs e) 
		{
            LVReloadBuildingsForConstruct();
        }

        private void cmdReload_Click(object sender, EventArgs e) {
            lock (LVAuto.LVWeb.LVClient.islock) {
                //FirstLogin();
				LVLoadServerData();
                MessageBox.Show("Đã nạp lại thông tin thành công. Bạn tự load lại cấu hình nhé.");
            }
        }

        private void chkAutoDel_CheckedChanged(object sender, EventArgs e) 
		{
            if (Auto_checkAutoDestruct.Checked) 
			{
                LVDEL.GetParameter(tvDEL, int.Parse(txtDELCHECK.Text) * 60 * 1000);
                tvDEL.Enabled = false;
                LVDEL.Auto();
            } else {
                LVDEL.Stop();
                tvDEL.Enabled = true;
            }
        }

        /// <summary>
        /// Shows the loading message
        /// </summary>
        /// <param name="message"></param>
		public void ShowLoadingLabel(string message)
		{
            this.Text = strVersion + " | Please wait: " + message;
            labelStatus.Text = message;
			//lblPanelLoading.Text = message;
			//panelLoading.BringToFront();
			//panelLoading.Visible = true;
			//lblPanelLoading.BringToFront();
            LVFRMMAIN.Refresh();
		}

        /// <summary>
        /// Shows the default loading message
        /// </summary>
        public void ShowLoadingLabel()
		{
            ShowLoadingLabel("Loading...");
		}

        /// <summary>
        /// Hides the loading message
        /// </summary>
		public void HideLoadingLabel()
		{
            this.Text = strVersion;
            labelStatus.Text = "";
			//panelLoading.Visible = false;
			LVFRMMAIN.Refresh();
		}

		private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
		{
			_chuongbao = chkChuong.Checked;
		}

		private void Construct_dropdownCityList_SelectedIndexChanged(object sender, EventArgs e)
		{
            LVLoadBuildingsToTreeViewForConstruct();
		}

		private void tvDEL_Leave(object sender, EventArgs e)
		{
			try
			{
				int cityPost = cboTabHaNhaCity.SelectedIndex;

				//Xay dung mang de xay nha
				TreeNode root = tvDEL.Nodes[0];

				//if (root.Checked) IsBuildAll = true;
				TreeNode tt;
				for (int i = 0; i < root.Nodes.Count; i++)
				{
					LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cityPost].AllBuildings[i].IsDown = root.Nodes[i].Checked;
				}
				int ii;
			}
			catch (Exception ex)
			{
			}
		}

		private void cboTabHaNhaCity_SelectedIndexChanged(object sender, EventArgs e)
		{
            try
            {
                LVAuto.LVForm.LVCommon.common.LoadBuildingToTreeViewForbuild_(tvDEL, cboTabHaNhaCity.SelectedIndex);

                TreeNode root = tvDEL.Nodes[0];

                root.ExpandAll();
                //if (root.Checked) IsBuildAll = true;
                for (int i = 0; i < root.Nodes.Count; i++)
                {
                    root.Nodes[i].Checked = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cboTabHaNhaCity.SelectedIndex].AllBuildings[i].IsDown;
                }
            }
            catch (Exception ex)
            {

            }

		}

        private void tabMainTab_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (!e.TabPage.Enabled)
			{
				e.Cancel = true;
			}
		}

		private void tabMainTab_DrawItem(object sender, DrawItemEventArgs e)
		{
			TabPage page = tabMainTab.TabPages[e.Index];

			if (!page.Enabled)
			{
				using (SolidBrush brush = new SolidBrush(SystemColors.GrayText))
				{
					e.Graphics.DrawString(page.Text, page.Font, brush, e.Bounds);
				}
			}
			else
			{
				using (SolidBrush brush = new SolidBrush(page.ForeColor))
				{
					e.Graphics.DrawString(page.Text, page.Font, brush, e.Bounds);
				}
			}
		}

		private void chkChuong_CheckedChanged(object sender, EventArgs e)
		{
			_chuongbao = chkChuong.Checked;
		}

		private void radHinhAnhKhongLamGi_CheckedChanged(object sender, EventArgs e)
		{
			if (radHinhAnhKhongLamGi.Checked) _tuxacnhananh = 0;
		}

		private void radXacNhanAnhHienAnh_CheckedChanged(object sender, EventArgs e)
		{
			if (radXacNhanAnhHienAnh.Checked) _tuxacnhananh = 1;

		}

		private void radXacNhanAnhTuNhan_CheckedChanged(object sender, EventArgs e)
		{
			if (radXacNhanAnhTuNhan.Checked)
			{
				_tuxacnhananh = 2;
			}
			SoLanTuCheckAnhSaiLienTiep = 0;
			txtXacNhanAnhMaxCheck.Enabled = radXacNhanAnhTuNhan.Checked;
			txtXacNhanAnhMinTimeCheck.Enabled = radXacNhanAnhTuNhan.Checked;
		}

		private void btLuyenSykhiNapLai_Click(object sender, EventArgs e)
		{
			try
			{
				this.Enabled = false;
				LVAuto.LVForm.Command.Common.GetAllSimpleMilitaryGeneralInfoIntoCity();
				LVCommon.common.LoadGeneralForUpSiKhi(tvSIKHI, false);
				this.Enabled = true;
			}
			catch (Exception ex)
			{
				this.Enabled = true;
			}
		}

		
		private void cboBienCheCity_SelectedIndexChanged(object sender, EventArgs e)
		{
			chklstGeneral.Items.Clear();
            try {
				ShowLoadingLabel();
				LVAuto.LVForm.Command.Common.GetGeneral(cboBienCheCity.SelectedIndex, false);
				chklstGeneral.Items.AddRange(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cboBienCheCity.SelectedIndex].MilitaryGenerals);

				
				HideLoadingLabel();
            } catch (Exception ex) { }
        }

		private void btBiencheAccept_Click(object sender, EventArgs e)
		{
			try
			{
				LVObj.MilitaryGeneral onegenral;
				LVAuto.LVForm.LVCommon.BienChe bienche;
				//int cityid = cboBienCheCity.SelectedIndex;
				int citypost = cboBienCheCity.SelectedIndex;
				int cityid = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].Id;

				for (int i = 0; i < chklstGeneral.CheckedItems.Count; i++)
				{
					bienche = new LVAuto.LVForm.LVCommon.BienChe();
					onegenral = (LVObj.MilitaryGeneral)chklstGeneral.CheckedItems[i];
					bienche.cityid = cityid;

					bienche.generalid = onegenral.Id;
					bienche.generalname = onegenral.Name;
					bienche.cityname = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].Name;
					bienche.bobinhamount = txtBiencheBoBinh.Text.Trim().Length != 0 ? int.Parse(txtBiencheBoBinh.Text) : 0;
					bienche.kybinhamount = txtBiencheKybinh.Text.Trim() != "" ? int.Parse(txtBiencheKybinh.Text) : 0;
					bienche.cungthuamount = txtBienCheCungThu.Text.Trim().Length != 0 ? int.Parse(txtBienCheCungThu.Text) : 0;
					bienche.xemount = txtBiencheXe.Text.Trim().Length != 0 ? int.Parse(txtBiencheXe.Text) : 0;
					bienche.nangsykhi = chkBienCheTuNangSK.Checked;
					if (bienche.bobinhamount != 0 || bienche.kybinhamount != 0 || bienche.cungthuamount != 0 || bienche.xemount != 0)
						ListBienChe.Add(bienche);
				}
				
					LVAuto.LVForm.LVCommon.common.LoadGeneralForBienChe(tvBienCheList);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Sai cái gì đó, xem lại đê!");
			}

		}

		private void btBienCheBo_Click(object sender, EventArgs e)
		{

			int index;
			for (int i = 0; i < tvBienCheList.Nodes.Count; i++)
			{
				if (tvBienCheList.Nodes[i].Checked)
				{
					index = tvBienCheList.Nodes[i].Index;
					ListBienChe.RemoveAt(index);
					tvBienCheList.Nodes.RemoveAt(index);
					
				}	
			}
			LVAuto.LVForm.LVCommon.common.LoadGeneralForBienChe(tvBienCheList);

/*
			if (tvBienCheList.SelectedNode != null)
			{

				tvBienCheList.Nodes.Remove(tvBienCheList.SelectedNode);
				LVAuto.Common.common.LoadGeneralForBienChe(tvBienCheList);
			}
*/
		}

		private void chkAutoBienche_CheckedChanged(object sender, EventArgs e)
		{
			if (Auto_checkAutoTroops.Checked)
			{
				if (LVBIENCHE== null)
					LVBIENCHE = new LVAuto.LVForm.LVThread.BIENCHE(lblBIENCHEMESSAGE);

				LVBIENCHE.GetParameter(10, int.Parse(txtTabBienCheTimeCheck.Text) * 60 * 1000);
				cboBienCheCity.Enabled = false;
				chklstGeneral.Enabled = false;
				btBiencheAccept.Enabled = false;

				
				//LVBUILD.run();
				LVBIENCHE.Auto();
			}
			else
			{
				LVBIENCHE.Stop();
				cboBienCheCity.Enabled = true;
				chklstGeneral.Enabled = true;
				btBiencheAccept.Enabled = true;

			}
		}

		private void chkAutoDieuPhai_CheckedChanged(object sender, EventArgs e)
		{
			
			
			if (chkAutoDieuPhai.Checked)
			{
				bool found1 = false;
				bool found2 = false;
				LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong g;
				btTapDieuPhaiBo.Enabled = false;

				for (int i = 0; i < DieuPhaiQuanVanVo.Count; i++)
				{
					g = ((LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong) DieuPhaiQuanVanVo[i]);
					if (g.tasktype == LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong.TASKTYPE.DaoMo)
					{
						if (!found1)
						{
							LVDAOMO.GetParameter(g.cityID, g.X, g.Y, g.timetoruninmilisecond);
							grbtabDieuPhaiDaoMo.Enabled = false;
							grbTabDieuPhaiLoiDai.Enabled = false;
							LVDAOMO.Auto();
							found1 = true;
						}
						if (found1 & found2) break;
						
					}

					if (g.tasktype == LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong.TASKTYPE.LoiDai)
					{
						if (!found2)
						{
							LVLOIDAI.GetParameter(DieuPhaiQuanVanVo, g.timetoruninmilisecond);
							grbtabDieuPhaiDaoMo.Enabled = false;
							grbTabDieuPhaiLoiDai.Enabled = false;
							LVLOIDAI.Auto();
							found2 = true;
						}
						if (found1 & found2) break;
					}


				}
				if (!found1 & !found2)
				{
					chkAutoDieuPhai.Checked = false;
					btTapDieuPhaiBo.Enabled = true;
					grbtabDieuPhaiDaoMo.Enabled = true;
					grbTabDieuPhaiLoiDai.Enabled = true;

					lblDIEUPHAIMESSAGE.Text = "Không chọn thằng quan nào để phái cả, check lại đê";
						
				}
			}
			else
			{
				LVDAOMO.Stop();
				LVLOIDAI.Stop();
				grbtabDieuPhaiDaoMo.Enabled = true;
				grbTabDieuPhaiLoiDai.Enabled = true;
				btTapDieuPhaiBo.Enabled = true;
			
			}
		}

		private void chklstGeneral_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool haschecked;
			if (chklstGeneral.CheckedItems.Count > 0)
				btBiencheAccept.Enabled = true;
			else
				btBiencheAccept.Enabled = false;
		}

		private void txtBiencheBoBinh_Leave(object sender, EventArgs e)
		{
			if (txtBiencheBoBinh.Text.Trim().Length == 0) txtBiencheBoBinh.Text = "0";
		}

		private void txtBiencheKybinh_Leave(object sender, EventArgs e)
		{
			if (txtBiencheKybinh.Text.Trim().Length == 0) txtBiencheKybinh.Text = "0";
		}

		private void txtBienCheCungThu_Leave(object sender, EventArgs e)
		{
			if (txtBienCheCungThu.Text.Trim().Length == 0) txtBienCheCungThu.Text = "0";
		}

		private void txtBiencheXe_Leave(object sender, EventArgs e)
		{
			if (txtBiencheXe.Text.Trim().Length == 0) txtBiencheXe.Text = "0";
		}


		public  void PhaiQuanVan(LVAuto.LVForm.LVCommon.PhaiQuanVanDaoMo pqv)
		{
			if (pqv.check)
			{
				LVDAOMO.Auto();
			}
		}

		private void chklstAnUi_Leave(object sender, EventArgs e)
		{
			AnuiForAuto = new ArrayList();

			for (int i = 0; i < chklstAnUi.CheckedItems.Count; i++)
			{
				AnuiForAuto.Add(((LVAuto.LVObj.City)chklstAnUi.CheckedItems[i]).Id);
			}
		}

		private void pnUPGRADE_Leave(object sender, EventArgs e)
		{
			UpgradeForAuto = new ArrayList();
			if (cboCityForUpgrade.SelectedItem != null)
			{
				UpgradeForAuto.Add(((LVAuto.LVObj.City)cboCityForUpgrade.SelectedItem).Id);
				TreeNode root = tvUpdate.Nodes["root"];
				foreach (TreeNode t in root.Nodes)
				{
					foreach (TreeNode c in t.Nodes)
					{
						if (c.Checked)
						{
							UpgradeForAuto.Add( c.Name);

						}
					}
				}
			}
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			this.BringToFront();
			this.Show();
			this.WindowState = FormWindowState.Normal;
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			this.Close(); ;
		}


		private void btTabDieuPhaiPhaiQuanVan_Click(object sender, EventArgs e)
		{
			try
			{		
				/*
				QuanVanDaoMo.cityid = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cboTabDieuPhaiThanhPhaiQuanVan.SelectedIndex].id;
				QuanVanDaoMo.X = int.Parse(txtTabDieuPhaiDaoMoX.Text);
				QuanVanDaoMo.Y = int.Parse(txtTabDieuPhaiDaoMoY.Text);
				QuanVanDaoMo.Time = 60000;
				QuanVanDaoMo.check = true;
				*/
				Command.CommonObj.DieuPhaiTuong dieuphai = new LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong();
				dieuphai.cityID = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cboTabDieuPhaiThanhPhaiQuanVan.SelectedIndex].Id;
				dieuphai.X = int.Parse(txtTabDieuPhaiDaoMoX.Text);
				dieuphai.Y = int.Parse(txtTabDieuPhaiDaoMoY.Text);
				dieuphai.generaltype = Command.CommonObj.DieuPhaiTuong.GENERALTYPE.QuanVan;
				dieuphai.timetoruninmilisecond = 60000; ;
				dieuphai.desc = "Điều phái quan văn từ thành " + LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cboTabDieuPhaiThanhPhaiQuanVan.SelectedIndex].Name
									+ " đến mỏ có tọa độ (" + dieuphai.X + "," + dieuphai.Y + ")";
				dieuphai.tasktype = Command.CommonObj.DieuPhaiTuong.TASKTYPE.DaoMo;
				DieuPhaiQuanVanVo.Add(dieuphai);
				
				cboTabDieuPhaiThanhPhaiQuanVan.SelectedIndex = -1;
				txtTabDieuPhaiDaoMoX.Text = "";
				txtTabDieuPhaiDaoMoY.Text= "";
				btTabDieuPhaiPhaiQuanVan.Enabled = false;
				showDataForGridDieuPhai();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Chọn chưa đúng các tham số", "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void showDataForGridDieuPhai()
		{
			Command.CommonObj.DieuPhaiTuong dieuphai;
			lstboxTabDieuPhaiDieuPhai.DataSource = null;
			lstboxTabDieuPhaiDieuPhai.Items.Clear();

			lstboxTabDieuPhaiDieuPhai.DataSource = DieuPhaiQuanVanVo;
			lstboxTabDieuPhaiDieuPhai.DisplayMember = "GetDesc";
			lstboxTabDieuPhaiDieuPhai.ValueMember = "GetID";


		}

		private void btTapDieuPhaiBo_Click(object sender, EventArgs e)
		{
			long id ;
			for (int i = 0; i < lstboxTabDieuPhaiDieuPhai.SelectedItems.Count; i++)
			{
				DieuPhaiQuanVanVo.Remove((Command.CommonObj.DieuPhaiTuong)lstboxTabDieuPhaiDieuPhai.SelectedItems[i]);
			}
			showDataForGridDieuPhai();
		}

		private void cboTabDieuPhaiThanhPhaiQuanVan_SelectedIndexChanged(object sender, EventArgs e)
		{
			int tmp;
			if (txtTabDieuPhaiDaoMoX.Text.Trim() != ""  && txtTabDieuPhaiDaoMoY.Text.Trim() != ""  &
					cboTabDieuPhaiThanhPhaiQuanVan.SelectedItem != null && int.TryParse(txtTabDieuPhaiDaoMoX.Text, out tmp)
						&& int.TryParse(txtTabDieuPhaiDaoMoY.Text, out tmp))
				btTabDieuPhaiPhaiQuanVan.Enabled = true;
			else
				btTabDieuPhaiPhaiQuanVan.Enabled = false;
		}

		private void txtTabDieuPhaiDaoMoX_TextChanged(object sender, EventArgs e)
		{
			int tmp;
			if (txtTabDieuPhaiDaoMoX.Text.Trim() != "" && txtTabDieuPhaiDaoMoY.Text.Trim() != "" &
					cboTabDieuPhaiThanhPhaiQuanVan.SelectedItem != null && int.TryParse(txtTabDieuPhaiDaoMoX.Text, out tmp)
						&& int.TryParse(txtTabDieuPhaiDaoMoY.Text, out tmp))
				btTabDieuPhaiPhaiQuanVan.Enabled = true;
			else
				btTabDieuPhaiPhaiQuanVan.Enabled = false;
		}

		private void txtTabDieuPhaiDaoMoY_TextChanged(object sender, EventArgs e)
		{
			int tmp;
			if (txtTabDieuPhaiDaoMoX.Text.Trim() != "" && txtTabDieuPhaiDaoMoY.Text.Trim() != "" &
					cboTabDieuPhaiThanhPhaiQuanVan.SelectedItem != null && int.TryParse(txtTabDieuPhaiDaoMoX.Text, out tmp)
						&& int.TryParse(txtTabDieuPhaiDaoMoY.Text, out tmp))
				btTabDieuPhaiPhaiQuanVan.Enabled = true;
			else
				btTabDieuPhaiPhaiQuanVan.Enabled = false;
		}

		private void cbTabDieuPhaiThanhPhaiDiLoiDai_SelectedIndexChanged(object sender, EventArgs e)
		{
			int cityid;
			int count = 0;
			if (cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedItem == null)
			{
				btTabDieuPhai_PhaiDiLoiDai.Enabled = false;
				return;
			}
			cbTabDieuPhaiQuanVanVoDiLoiDai.Items.Clear();
			cityid = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedIndex].Id;

			ArrayList argen = Command.Common.GetAllGeneralVanVo(cityid);

			if (argen == null || argen.Count == 0)
			{
				btTabDieuPhai_PhaiDiLoiDai.Enabled = false;
				return;
			}

			
			cbTabDieuPhaiQuanVanVoDiLoiDai.Items.AddRange(argen.ToArray());

			if (cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedItem == null
					|| cbTabDieuPhaiQuanVanVoDiLoiDai.SelectedItem == null
					|| cboCapLoiDai.SelectedItem == null)
			
				btTabDieuPhai_PhaiDiLoiDai.Enabled = false;
			else
				btTabDieuPhai_PhaiDiLoiDai.Enabled = true;


		}

		private void cbTabDieuPhaiQuanVanVoDiLoiDai_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedItem == null
					|| cbTabDieuPhaiQuanVanVoDiLoiDai.SelectedItem == null
					|| cboCapLoiDai.SelectedItem == null)

				btTabDieuPhai_PhaiDiLoiDai.Enabled = false;
			else
				btTabDieuPhai_PhaiDiLoiDai.Enabled = true;

		}

		private void cboCapLoiDai_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedItem == null
					|| cbTabDieuPhaiQuanVanVoDiLoiDai.SelectedItem == null
					|| cboCapLoiDai.SelectedItem == null)

				btTabDieuPhai_PhaiDiLoiDai.Enabled = false;
			else
				btTabDieuPhai_PhaiDiLoiDai.Enabled = true;

		}

		private void btTabDieuPhai_PhaiDiLoiDai_Click(object sender, EventArgs e)
		{

			if (cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedItem == null
					|| cbTabDieuPhaiQuanVanVoDiLoiDai.SelectedItem == null
					|| cboCapLoiDai.SelectedItem == null)

				btTabDieuPhai_PhaiDiLoiDai.Enabled = false;
			else
				btTabDieuPhai_PhaiDiLoiDai.Enabled = true;


			try
			{
				Command.CommonObj.DieuPhaiTuong dieuphai = new LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong();
				LVObj.MilitaryGeneral g = (LVObj.MilitaryGeneral) cbTabDieuPhaiQuanVanVoDiLoiDai.SelectedItem;
				if (g == null)
				{
					btTabDieuPhai_PhaiDiLoiDai.Enabled = false;
					return;
				}

				dieuphai.cityID = LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedIndex].Id;
				dieuphai.generalid = g.Id;
				dieuphai.generalname = g.Name;
				dieuphai.generaltype = g.Type;
				string str = cboCapLoiDai.SelectedItem.ToString();
				dieuphai.loidailevel = int.Parse( str.Substring(0, str.IndexOf(".")));
				dieuphai.timetoruninmilisecond = 60000; ;

				if (dieuphai.generaltype == Command.CommonObj.DieuPhaiTuong.GENERALTYPE.QuanVan)
				{
					dieuphai.desc = "Điều phái quan văn " + dieuphai.generalname + " từ thành " + LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedIndex].Name
										+ " đi lôi đài cấp " + dieuphai.loidailevel;
				}
				else
				{
					dieuphai.desc = "Điều phái quan võ " + dieuphai.generalname + " từ thành " + LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedIndex].Name
										+ " đi lôi đài cấp " + dieuphai.loidailevel;

				}
					
				dieuphai.tasktype = Command.CommonObj.DieuPhaiTuong.TASKTYPE.LoiDai;
				
				DieuPhaiQuanVanVo.Add(dieuphai);

				//cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedIndex = -1;
				cbTabDieuPhaiQuanVanVoDiLoiDai.SelectedIndex = -1;
				cboCapLoiDai.SelectedIndex					  = -1;
				btTabDieuPhai_PhaiDiLoiDai.Enabled = false;
				showDataForGridDieuPhai();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Chọn chưa đúng các tham số", "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void pnDoanhTrai_Leave(object sender, EventArgs e)
		{
			Command.CommonObj.DoiTraiObj objDoiTrai = null ;
			DiChuyenTrai = new ArrayList();
			LVAuto.LVForm.Command.CommonObj.doitrai vc;
			bool found = false;
 
			foreach (object obj in pnDoanhTrai.Controls)
			{
				found = false;
				vc = (LVAuto.LVForm.Command.CommonObj.doitrai)obj;
				if (vc.chkOK.Checked)
				{
					try
					{
						objDoiTrai = new LVAuto.LVForm.Command.CommonObj.DoiTraiObj();				
						objDoiTrai.TraiID = vc.id;
						objDoiTrai.TraiName = vc.txtTen.Text;
						objDoiTrai.targetX = int.Parse(vc.X.Text);
						objDoiTrai.targetY = int.Parse(vc.Y.Text);
						found = true;
						if (vc.X.Text.Trim() == "" || vc.Y.Text.Trim() == "") found = false;
					}
					catch (Exception ex)
					{
						vc.chkOK.Checked = false;
					}

					if (found) DiChuyenTrai.Add(objDoiTrai);
				}
			}
			

		}

		private void chkHaNhaDelAll_CheckedChanged(object sender, EventArgs e)
		{
            LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.IsDowndAll = chkHaNhaDelAll.Checked;
			cboTabHaNhaCity.Enabled = !chkHaNhaDelAll.Checked;
			tvDEL.Enabled = !chkHaNhaDelAll.Checked;			
		}

		private void chkXayNhaAll_CheckedChanged(object sender, EventArgs e)
		{
			Construct_dropdownCityList.Enabled = !chkXayNhaAll.Checked;
			Construct_treeBuilding.Enabled = !chkXayNhaAll.Checked;
			Construct_btnReloadBuildings.Enabled = !chkXayNhaAll.Checked;
            LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.IsBuildAll = chkXayNhaAll.Checked;	
		}

		private void chkXayNha_TuMuaTaiNguyen_CheckedChanged(object sender, EventArgs e)
		{
			txtXayNha_VangAnToan.Enabled = chkXayNha_TuMuaTaiNguyen.Checked;
		}

		private void pnXayNha_Leave(object sender, EventArgs e)
		{
			try
			{

                LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AutoBuyResources = chkXayNha_TuMuaTaiNguyen.Checked;
				if (chkXayNha_TuMuaTaiNguyen.Checked) LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.GoldThreshold = long.Parse(txtXayNha_VangAnToan.Text);

				if (chkXayNhaAll.Checked) return;

				int cityPost = Construct_dropdownCityList.SelectedIndex;
				if (cityPost < 0) return;

				try
				{
					//Xay dung mang de xay nha
					TreeNode root = Construct_treeBuilding.Nodes[0];

					//if (root.Checked) IsBuildAll = true;
					TreeNode tt;
					for (int i = 0; i < root.Nodes.Count; i++)
					{
						LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cityPost].AllBuildings[i].IsUp = root.Nodes[i].Checked;
					}
				}
				catch (Exception ex)
				{
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi gì đó trong phần Xây dựng");
			}
		}


		private void chkANUI_TuMuaLua_CheckedChanged(object sender, EventArgs e)
		{
			txtANUI_VangAnToan.Enabled = chkANUI_TuMuaLua.Checked;
		}

		private void tvBUILD_Leave_1(object sender, EventArgs e)
		{
			try
			{

                LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AutoBuyResources = chkXayNha_TuMuaTaiNguyen.Checked;
                if (chkXayNha_TuMuaTaiNguyen.Checked) LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.GoldThreshold = long.Parse(txtXayNha_VangAnToan.Text);

				if (chkXayNhaAll.Checked) return;

				int cityPost = Construct_dropdownCityList.SelectedIndex;
				if (cityPost < 0) return;
				try
				{
					//Xay dung mang de xay nha
					TreeNode root = Construct_treeBuilding.Nodes[0];

					//if (root.Checked) IsBuildAll = true;
					TreeNode tt;
					for (int i = 0; i < root.Nodes.Count; i++)
					{
						LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[cityPost].AllBuildings[i].IsUp = root.Nodes[i].Checked;
					}
				}
				catch (Exception ex)
				{
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi gì đó trong phần Xây dựng");
			}
		}

		private void Quest_btnQuestReload_Click(object sender, EventArgs e)
		{
            LVReloadGeneralsForQuest();
		}

		private void btTienIchRuongGetData_Click(object sender, EventArgs e)
		{
			try
			{
				int count = 0;
				btTienIchRuongGetData.Enabled = false;

				do
				{
					QuocKho_Ruong = Command.Common.GetGem(0);
					count++;
				} while (QuocKho_Ruong == null && count < 10);

				if (QuocKho_Ruong == null)
				{
					MessageBox.Show(" Lỗi rồi, thử làm lại xem sao nha");
					return;
				}


				
				cbTienIchChonRuong.Items.Clear();
				cbTienIchMoRuongRuongChonThanh.Items.Clear();
				//Common.Ruong ruong = new LVAuto.Common.Ruong();

				int[] arGemID = new int[QuocKho_Ruong.Count];
			

				QuocKho_Ruong.Keys.CopyTo(arGemID, 0);
				Array.Sort(arGemID);

				for (int i = 0; i < arGemID.Length; i++)
				{
					cbTienIchChonRuong.Items.Add(arGemID[i] + ". " + LVCommon.Ruong.GetRuongName(arGemID[i]));
				}

				
				cbTienIchChonRuong.Enabled = true;
				cbTienIchMoRuongRuongChonThanh.Enabled = true;
				txtTienIchRuongSoluong.Enabled = true;
				LVAuto.LVObj.City city;
				for (int i = 0; i < LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
				{
					city = LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i];
					if (city.Id > 0)
					{

						cbTienIchMoRuongRuongChonThanh.Items.Add(city);
					}
				}
				EnableMoRuong();

			}
			catch (Exception ex)
			{
				cbTienIchChonRuong.Enabled = false;			  
				cbTienIchMoRuongRuongChonThanh.Enabled = false;
				txtTienIchRuongSoluong.Enabled = false;
			}

			finally
			{
				btTienIchRuongGetData.Enabled = true;
			}
		}

		private void cbTienIchChonRuong_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				string value = cbTienIchChonRuong.SelectedItem.ToString();
				if (value == "") return;
				value = value.Substring(0, value.IndexOf("."));
				string amount = QuocKho_Ruong[int.Parse(value)].ToString();
				lblTienIchSoLuongCo.Text = amount;
				EnableMoRuong();

			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi gì đó rồi :( ");
			}
		}

		private void EnableMoRuong()
		{
			if (cbTienIchChonRuong.SelectedItem != null && cbTienIchMoRuongRuongChonThanh.SelectedItem != null)
			{
				string sl = txtTienIchRuongSoluong.Text;
				int vl;
				if (int.TryParse(sl, out vl))
				{
					if (vl > 0)
						btTienIchRuongOpen.Enabled = true;
					else
						btTienIchRuongOpen.Enabled = false;
				}

			}
			else
			{
				btTienIchRuongOpen.Enabled = false;
			}
			
		}

		private void cbTienIchMoRuongRuongChonThanh_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnableMoRuong();
		}

		private void txtTienIchRuongSoluong_TextChanged(object sender, EventArgs e)
		{
			EnableMoRuong();
		}

		private void btTienIchRuongOpen_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbTienIchChonRuong.SelectedItem == null || cbTienIchMoRuongRuongChonThanh == null)
				{
					MessageBox.Show("Chưa chọn đúng tham số");
					EnableMoRuong();
					return;
				}

				lblTienIchMoRuong_waitmsg.Enabled = true;
				lblTienIchMoRuong_waitmsg.Visible = true;
				grbTienIchMoRuong_group.Enabled = false;

				string str = cbTienIchChonRuong.SelectedItem.ToString();
				int ruongid = int.Parse(str.Substring(0, str.IndexOf(".")));
				LVAuto.LVObj.City city = (LVAuto.LVObj.City)cbTienIchMoRuongRuongChonThanh.SelectedItem;
				int cityid = city.Id;
				int amount = int.Parse(txtTienIchRuongSoluong.Text);

				string cookies = cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityid);
				LVAuto.LVForm.Command.City.SwitchCitySlow(cityid);
				// mo ruong
				string para = "lBoxID=" + ruongid;
				//string para = "lBoxID=" + ruongid + "&lcid=" + cityid + "&lCityCampID=" + cityid + "&cityid=" + cityid;
				// para += "&lcityid=" +  cityid  + "&tid=" +  cityid + "&ttid=" +  cityid;
				// para += "&lcid=" + cityid + "&cid=" + cityid; ;

				int success = 0;
				Hashtable result;
				int havenow = int.Parse(lblTienIchSoLuongCo.Text);
				for (int i = 0; i < amount; i++)
				{
					result = Command.Common.Execute(48, para, true, cookies);
					if (result != null && result["ret"].ToString() == "0")
					{
						success++;
						havenow--;
						lblTienIchSoLuongCo.Text = havenow.ToString() ;
						Application.DoEvents();
					}
				}
				/*
				if (success > 0)
				{
					int remain = int.Parse(lblTienIchSoLuongCo.Text) - success;
					lblTienIchSoLuongCo.Text = remain.ToString();
				}
				 * */
			}
			catch (Exception ex)
			{
				MessageBox.Show("Hic, có lỗi gì rùi, không thực hiện được");

			}

			finally
			{
				lblTienIchMoRuong_waitmsg.Enabled = false;
				lblTienIchMoRuong_waitmsg.Visible = false;

				grbTienIchMoRuong_group.Enabled = true; ;

			}
		}

		

		private void btVCVKOK_Click(object sender, EventArgs e)
		{
			try
			{
				


				btVCVKOK.Enabled = false;
				int sourceCityID;		// = ((LVObj.City)cbVCVKThanhDi.SelectedItem).id;
				int destCityID;		
				int loaivkid;	// = ((Common.Wepons)cbVCVKLoaiVK.SelectedItem).id;
				int tongsoluong = int.Parse(txtVCVKTongSoLuong.Text);
				int soluong = int.Parse(txtVCVKSoLuong.Text);

				LVObj.City destCity;
				for (int i = 0; i < chklVCVKThanhDi.CheckedItems.Count; i++)
				{
					sourceCityID = ((LVObj.City)chklVCVKThanhDi.CheckedItems[i]).Id;
					for (int j = 0; j < chklVCVKThanhDen.CheckedItems.Count; j++)
					{
						destCityID = ((LVObj.City)chklVCVKThanhDen.CheckedItems[j]).Id;
						for (int k = 0; k < chklVCVKLoaiVuKhi.CheckedItems.Count; k++)
						{
							loaivkid = ((LVCommon.Wepons)chklVCVKLoaiVuKhi.CheckedItems[k]).id;
							VanChuyenVuKhi.Add(new Command.CommonObj.VanChuyenVK(sourceCityID, destCityID, loaivkid, tongsoluong,soluong));
						}

					}
					//destCity = (LVObj.City)chklVCVKThanhDen.CheckedItems[i];
					//VanChuyenVuKhi.Add(new Command.CommonObj.VanChuyenVK(sourceCityID, destCity.id, loaivkid, soluong));
				}

				for (int i = 0; i < chklVCVKThanhDi.Items.Count; i++)
					chklVCVKThanhDi.SetItemChecked(i, false);

				for (int i = 0; i < chklVCVKThanhDen.Items.Count; i++)
					chklVCVKThanhDen.SetItemChecked(i, false);

				for (int i = 0; i < chklVCVKLoaiVuKhi.Items.Count; i++)
					chklVCVKLoaiVuKhi.SetItemChecked(i, false);

	
				chklVCVKThanhDen.ClearSelected();
				chklVCVKLoaiVuKhi.ClearSelected();

				chklVCVKThanhDi.EndUpdate();

				LVCommon.common.LoadDataResultForVCVK(lstVCVKResult);

			}
			catch (Exception ex)
			{
				MessageBox.Show("Sai/thiếu tham số gì đó");
			}
			finally
			{
				btVCVKOK.Enabled = true;
			}
		}

		private void btVCVKCancel_Click(object sender, EventArgs e)
		{
			Command.CommonObj.VanChuyenVK vcvk;
			for (int i = 0; i < lstVCVKResult.SelectedItems.Count; i++)
			{
				vcvk = ((Command.CommonObj.VanChuyenVK)lstVCVKResult.SelectedItems[i]);
				VanChuyenVuKhi.Remove(vcvk);
			}
			LVCommon.common.LoadDataResultForVCVK(lstVCVKResult);

		}

		private void chkAUTOVCVK_CheckedChanged(object sender, EventArgs e)
		{
			try
			{

				if (chkAUTOVCVK.Checked)
				{
					if (VanChuyenVuKhi.Count > 0)
					{
						LVAUTOVCVK.GetParameter(VanChuyenVuKhi, int.Parse(txtCHECKVANCHUYENVUKHI.Text) * 60 * 1000);
						pnVanChuyenVK.Enabled = false;
						LVAUTOVCVK.Auto();
					}
					else
					{
						lblAUTOVCVKMESSAGE.Text = "Chưa chọn đúng tham số";
						chkAUTOVCVK.Checked = false;
					}
				}
				else
				{
					LVAUTOVCVK.Stop();
					pnVanChuyenVK.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				lblAUTOVCVKMESSAGE.Text = "Chưa chọn đúng tham số";
				chkAUTOVCVK.Checked = false;
			}
		}
		//}


		public void DeleteNhacNhoAdmin()
		{
			try
			{
				int count = 0;
				int page = 1;
				while (count < 5)
				{
					count++;

					while (true && page < 100)
					{
						Application.DoEvents();

						Hashtable result;

						//string header = "GET http://s" + LVAuto.Web.LVWeb.Server + ".linhvuong.zooz.vn/message HTTP/1.1\n";

						string header = "GET http://s" + LVAuto.LVWeb.LVClient.Server + ".linhvuong.zooz.vn/Interfaces/message_list.aspx?n=n&page=" + page + " HTTP/1.1\n";

						header += "Host: s" + LVAuto.LVWeb.LVClient.Server + ".linhvuong.zooz.vn\n";
						header += "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)\n";
						header += "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\n";
						header += "Accept-Language: en-us,en;q=0.5\n";
						header += "Accept-Encoding: gzip,deflate\n";
						header += "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7\n";
						header += "Keep-Alive: 300\n";
						header += "Referer: http://s" + LVAuto.LVWeb.LVClient.Server + ".linhvuong.zooz.vn/city\n";
						header += "Cookie: " + LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString() + "\n";
						header += "Content-Type: application/x-www-form-urlencoded\n";
						header += "\n";
						result = LVAuto.LVWeb.LVClient.SendAndReceive(header, "s" + LVAuto.LVWeb.LVClient.Server + ".linhvuong.zooz.vn", 80, true);
						if (result == null)
						{
							page = 1;
							break;
						}
						string data = result["DATA"].ToString();
						if (data.Contains("Object moved"))		// het trang
						{
							page = 1;
							break;
						}
						ParseHTML parse = new ParseHTML();
						parse.Source = data;

						//parse.Source = "<tr class=\"tr yellow6\" id=\"tr_4506176\" onmouseover=\"onmover('4506176');\" onmouseout=\"onmout('4506176');\"><td width=\"28\" height=\"26px\"><input type=\"checkbox\" name=\"IsChek\" onclick=\"check_click('4506176');\" id=\"check_4506176\" value=\"4506176\" /></td><td><a href=\"javascript:void(0)\" onclick=\"get_message('4506176',1,0);\" class=\"but_12\">Nh?c nh?</a><label id=\"lb_4506176\">(Chua d?c)</label><input id=\"noread_4506176\" value=\"1\" type=\"hidden\" /></td><td><a onclick=\"Dipan.SanGuo.Common.CloseDialog('_float');Dipan.SanGuo.Common.GetUserInfo(0); return false;\" href=\"#\" class=\"but_12\">Admin</a></td><td class=\"yellow1\">2009-08-28 07:12:20</td></tr>";
						//	 <tr class="tr yellow6" id="tr_4506176" onmouseover="onmover('4506176');" onmouseout="onmout('4506176');"><td width="28" height="26px"><input type="checkbox" name="IsChek" onclick="check_click('4506176');" id="check_4506176" value="4506176" /></td><td><a href="javascript:void(0)" onclick="get_message('4506176',1,0);" class="but_12">Nh?c nh?</a><label id="lb_4506176">(Chua d?c)</label><input id="noread_4506176" value="1" type="hidden" /></td><td><a onclick="Dipan.SanGuo.Common.CloseDialog('_float');Dipan.SanGuo.Common.GetUserInfo(0); return false;" href="#" class="but_12">Admin</a></td><td class="yellow1">2009-08-28 07:12:20</td></tr>

						string checkID = "";
						ArrayList removeID = new ArrayList();
						while (!parse.Eof())
						{
							char ch = parse.Parse();
							if (ch == 0)
							{

								AttributeList tag = parse.GetTag();
								if (tag.Name.ToLower() == "a")
								{
									if (parse.InnerHTML == "Nhắc nhở")
									{
										LVAuto.LVForm.HTMLParse.Attribute arr = (LVAuto.LVForm.HTMLParse.Attribute)tag.List[1];
										checkID = arr.Value;
										checkID = checkID.Substring(checkID.IndexOf("(") + 2);
										checkID = checkID.Substring(0, checkID.IndexOf("'"));
									}
									if (parse.InnerHTML == "Admin")
									{
										removeID.Add(checkID);
									}
								}
							}
						}

						if (removeID.Count <= 0)
						{
							page++;
							continue;
						}
						string str = "<?xml version='1.0' encoding='utf-8'?>";
						str += "<root><delete><seqnos>";

						for (int i = 0; i < removeID.Count; i++)
						{
							str += removeID[i].ToString() + ",";
						}
						str = str.Substring(0, str.Length - 1);

						str += "</seqnos><del_type>1</del_type><noread_num>1</noread_num></delete></root>";

						header = "POST http://s" + LVAuto.LVWeb.LVClient.Server + ".linhvuong.zooz.vn/Interfaces/read_message.aspx HTTP/1.1\n";

						header += "Host: s" + LVAuto.LVWeb.LVClient.Server + ".linhvuong.zooz.vn\n";
						header += "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)\n";
						header += "Accept: */*";
						header += "method: POST /Interfaces/read_message.aspx HTTP/1.1\n";
						header += "x-requested-with: XMLHttpRequest\n";
						header += "Accept-Language: en-us,en;q=0.5\n";
						header += "Accept-Encoding: gzip, deflatelin\n";
						header += "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7\n";
						header += "Keep-Alive: 300\n";
						header += "Pragma: no-cache\n";
						header += "Referer: http://s" + LVAuto.LVWeb.LVClient.Server + ".linhvuong.zooz.vn/message\n";
						header += "Content-Type: application/x-www-form-urlencoded\n";
						header += "Cookie: " + LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString() + "\n";
						header += "Content-Length: " + (str.Length) + "\n";
						header += "\n";

						Hashtable res = LVAuto.LVWeb.LVClient.SendAndReceive(header + str, "s" + LVAuto.LVWeb.LVClient.Server + ".linhvuong.zooz.vn", 80, true);
						page++;
					} // end while true
				} // end while (count < 10)
			} //end try
			catch (Exception ex)
			{
			}

		}

		private void timerForAuto_Tick(object sender, EventArgs e)
		{
			try
			{
				if (LVAUTOVCVK.IsRun && VanChuyenVuKhi.Count > 0)
				{
					LVCommon.common.LoadDataResultForVCVK(lstVCVKResult);
				}
			}
			catch (Exception ex)
			{

			}
		}

		private void chkAutoLinhBongLoc_CheckedChanged(object sender, EventArgs e)
		{
			TuLinhBongLoc = chkAutoLinhBongLoc.Checked;
		}

		private void cmdTienIchXoaNhacNhoAdmin_Click(object sender, EventArgs e)
		{
			cmdTienIchXoaNhacNhoAdmin.Enabled = false;
			DeleteNhacNhoAdmin();
			cmdTienIchXoaNhacNhoAdmin.Enabled = true;

		}

		private void chkAutoThongBaoDanhTuongViengTham_CheckedChanged(object sender, EventArgs e)
		{
			ThongBaoDanhTuongViengTham = chkAutoThongBaoDanhTuongViengTham.Checked;
		}

		private void timerForTBDanhTuongViengTham_Tick(object sender, EventArgs e)
		{
			try
			{
				lblLoadingResMessage.Text = "Kiểm tra danh tướng viếng thăm ...";
			}
			catch (Exception ex) { }
			ShowCoDanhTuongViengTham();

			lblLoadingResMessage.Text = "";
		}

		private void ShowCoDanhTuongViengTham()
		{
			if (ThongBaoDanhTuongViengTham)
			{
                string g = "";
                g=Command.Build.GetGeneralViengTham();
				if (g != "")
				{
					string message = "Hi " + LVAuto.LVWeb.LVClient.lvusername + "!!!\r\n" + g;
					string title = "Hi " + LVAuto.LVWeb.LVClient.lvusername + "!!!";
					AppNotifyIcon.ShowBalloonTip(3000, "", message, ToolTipIcon.None);
				}
			}			

		}

		private void btAutoHienAnhDeCheck_Click(object sender, EventArgs e)
		{
			btAutoHienAnhDeCheck.Enabled = false;
			LVAuto.LVWeb.LVClient.processCheckImage();
			btAutoHienAnhDeCheck.Enabled = true;

		}
		private bool AnuiCheckAll = false;
		private void btAnUiCheckAll_Click(object sender, EventArgs e)
		{
			AnuiCheckAll = !AnuiCheckAll;
			for (int i = 0; i < chklstAnUi.Items.Count; i++)
			{
				chklstAnUi.SetItemChecked(i, AnuiCheckAll);
			}
		}

		private void txtXacNhanAnhMaxCheck_Leave(object sender, EventArgs e)
		{
			string str = txtXacNhanAnhMaxCheck.Text;

			int value;
			if (!int.TryParse(str, out value))
			{
				MessageBox.Show("Số lần tự check sai tối đa phải là số nguyên");
				txtXacNhanAnhMaxCheck.Focus();
			}
			if (value <= 0)
			{
				MessageBox.Show("Số lần tự check sai tối đa phải là số nguyên");
				txtXacNhanAnhMaxCheck.Focus();
			}
		}

		private void txtXacNhanAnhMinTimeCheck_Leave(object sender, EventArgs e)
		{
			string str = txtXacNhanAnhMinTimeCheck.Text;

			int value;
			if (!int.TryParse(str, out value))
			{
				MessageBox.Show("Thời gian check tiếp theo phải là số nguyên");
				txtXacNhanAnhMinTimeCheck.Focus();
			}

			if (value <= 0)
			{
				MessageBox.Show("Thời gian check tiếp theo phải là số nguyên");
				txtXacNhanAnhMinTimeCheck.Focus();
			}
		}

		private void chkAutoBanThuong_CheckedChanged(object sender, EventArgs e)
		{
			TuBanThuongDoDe = chkAutoBanThuong.Checked;
		}

		private void chkAutoAnUiKhiDanNo_CheckedChanged(object sender, EventArgs e)
		{
			TuBanAnUiKhiDanNo = chkAutoAnUiKhiDanNo.Checked;
		}

		private void chkAutoTuXoaNhacNho_CheckedChanged(object sender, EventArgs e)
		{
			TuXoaNhacNhoCuaAdmin =  chkAutoTuXoaNhacNho.Checked;
		}

		private void cbProxyProtocol_SelectedIndexChanged(object sender, EventArgs e)
		{
			string protocol = cbProxyProtocol.SelectedItem.ToString();

			switch (protocol)
			{

				case "NONE":
					txtProxyServer.Enabled = false;
					txtProxyPort.Enabled = false;
					txtProxyUser.Enabled = false;
					txtProxyPassword.Enabled = false;
					break;

				case "HTTP":
					txtProxyServer.Enabled = true;
					txtProxyPort.Enabled = true;
					txtProxyUser.Enabled = false;
					txtProxyPassword.Enabled = false;
					break;
				case "SOCKS4":
					txtProxyServer.Enabled = true;
					txtProxyPort.Enabled = true;
					txtProxyUser.Enabled = true;
					txtProxyPassword.Enabled = false;
					break;

				case "SOCKS4a":
					txtProxyServer.Enabled = true;
					txtProxyPort.Enabled = true;
					txtProxyUser.Enabled = true;
					txtProxyPassword.Enabled = false;
					break;

				case "SOCKS5":
					txtProxyServer.Enabled = true;
					txtProxyPort.Enabled = true;
					txtProxyUser.Enabled = true;
					txtProxyPassword.Enabled = false;
					break;


			}

		}

        private void cbTabBinhManThanhXuatQuan_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbTabBinhManThanhXuatQuan.SelectedItem == null) return;

			chkTabBinhManTuongXuatTran.Items.Clear();
			LVAuto.LVObj.City city = (LVAuto.LVObj.City ) cbTabBinhManThanhXuatQuan.SelectedItem ;
			
			
			if (city.MilitaryGenerals == null) city.MilitaryGenerals = LVAuto.LVForm.Command.Common.GetAllSimpleMilitaryGeneralInfo(city.Id);

			if (city.MilitaryGenerals == null)
			{
				MessageBox.Show("Lỗi rồi, mạng lởm quá, chờ tý rồi thử lại đê.", "Mạng lởm quá");
				return;
			}

			chkTabBinhManTuongXuatTran.Items.AddRange(city.MilitaryGenerals);
		}

		private void btTabBinhManXacNhan_Click(object sender, EventArgs e)
		{

			try
			{
				int num10;
				int selectedIndex = this.cbTabBinhManThanhXuatQuan.SelectedIndex;
				int id = LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[selectedIndex].Id;

				string str = this.cbTabBinhManPhuongThucTanCong.SelectedItem.ToString();
				int num3 = int.Parse(str.Substring(0, str.IndexOf(".")));
				string str2 = str.Substring(str.IndexOf(".") + 2);

				str = this.cbTabBinhManPhuongThucChonMucTieu.SelectedItem.ToString();
				int num4 = int.Parse(str.Substring(0, str.IndexOf(".")));
				string str3 = str.Substring(str.IndexOf(".") + 2);

				str = this.cbTabBinhManMuuKeTrongChienTruong.SelectedItem.ToString();
				int muuKeTrongChienTruongID = int.Parse(str.Substring(0, str.IndexOf(".")));
				string muuKeTrongChienTruongName = str.Substring(str.IndexOf(".") + 2);

				bool tuKhoiPhucTrangThai = chkTabBinhManTuKhoiPhucTrangThai.Checked;

				

				str = this.cbTabBinhManPhuongThucUuTienMan.SelectedItem.ToString();
				int num5 = int.Parse(str.Substring(0, str.IndexOf(".")));
				string str4 = str.Substring(str.IndexOf(".") + 2);

				int num6 = int.Parse(this.txtTabBinhManQuanSoMin.Text);
				int num7 = int.Parse(this.txtTabBinhManMinSyKhi.Text);
				int num8 = int.Parse(this.txtTabBinhManMaxODi.Text);
				int num9 = int.Parse(this.txtTabBinhManTimeRun.Text);
				bool flag = this.chkTabBinhManTuNangSyKhi.Checked;

				int[] numArray5 = new int[4];
				int[] numArray = numArray5;
				numArray5 = new int[4];
				int[] numArray2 = numArray5;
				numArray5 = new int[4];
				int[] numArray3 = numArray5;
				numArray5 = new int[4];
				int[] numArray4 = numArray5;
				numArray[0] = int.Parse(this.txtTabBinhManQuanSoBoBinh.Text);
				numArray2[0] = int.Parse(this.txtTabBinhManQuanSoKyBinh.Text);
				numArray3[0] = int.Parse(this.txtTabBinhManQuanSoCungThu.Text);
				numArray4[0] = int.Parse(this.txtTabBinhManQuanSoXe.Text);


				str = this.cbTabBinhManQuanGioiBoBinh1.SelectedItem.ToString();
				numArray[1] = int.Parse(str.Substring(0, str.IndexOf(".")));
				str = this.cbTabBinhManQuanGioiBoBinh2.SelectedItem.ToString();
				numArray[2] = int.Parse(str.Substring(0, str.IndexOf(".")));
				str = this.cbTabBinhManQuanGioiBoBinh3.SelectedItem.ToString();
				numArray[3] = int.Parse(str.Substring(0, str.IndexOf(".")));

				str = this.cbTabBinhManQuanGioiKyBinh1.SelectedItem.ToString();
				numArray2[1] = int.Parse(str.Substring(0, str.IndexOf(".")));
				str = this.cbTabBinhManQuanGioiKyBinh2.SelectedItem.ToString();
				numArray2[2] = int.Parse(str.Substring(0, str.IndexOf(".")));
				str = this.cbTabBinhManQuanGioiKyBinh3.SelectedItem.ToString();
				numArray2[3] = int.Parse(str.Substring(0, str.IndexOf(".")));

				str = this.cbTabBinhManQuanGioiCungThu1.SelectedItem.ToString();
				numArray3[1] = int.Parse(str.Substring(0, str.IndexOf(".")));
				str = this.cbTabBinhManQuanGioiCungThu2.SelectedItem.ToString();
				numArray3[2] = int.Parse(str.Substring(0, str.IndexOf(".")));
				str = this.cbTabBinhManQuanGioiCungThu3.SelectedItem.ToString();
				numArray3[3] = int.Parse(str.Substring(0, str.IndexOf(".")));

				str = this.cbTabBinhManQuanGioiXe1.SelectedItem.ToString();
				numArray4[1] = int.Parse(str.Substring(0, str.IndexOf(".")));

                int mantype;
                if (rdBinhManDanhMan.Checked)       // ddanhs man
                    mantype = 1;
                else
                    mantype = 14;                   // ddanhs ddia tinh



                for (num10 = 0; num10 < this.chkTabBinhManTuongXuatTran.CheckedItems.Count; num10++)
                {
                    MilitaryGeneral general = (MilitaryGeneral)this.chkTabBinhManTuongXuatTran.CheckedItems[num10];

                    if (mantype == 1)
                    {
                        for (int i = 0; i < this.chkTabBinhManListManDanh.CheckedItems.Count; i++)
                        {
                            BinhManObj obj2 = new BinhManObj();
                            str = this.chkTabBinhManListManDanh.CheckedItems[i].ToString();

                            obj2.ManType = mantype;


                            obj2.ManID = int.Parse(str.Substring(0, str.IndexOf(".")));
                            obj2.ManName = str.Substring(str.IndexOf(".") + 2);
                            obj2.PhuongThucTanCongID = num3;
                            obj2.PhuongThucTanCongName = str2;
                            obj2.PhuongThucChonMucTieuID = num4;
                            obj2.PhuongThucChonMucTieuName = str3;
                            obj2.UuTienID = num5;
                            obj2.UuTienName = str4;
                            obj2.SoLuongQuanMinToGo = num6;
                            obj2.SiKhiMinToGo = num7;
                            obj2.MaxOToGo = num8;
                            obj2.TimeToCheck = num9;
                            obj2.TuUpSiKhi = flag;
                            obj2.Id = general.Id;
                            obj2.Name = general.Name;
                            obj2.CityId = general.CityId;
                            obj2.CityName = general.CityName;
                            obj2.CityId = id;
                            obj2.CityName = LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[selectedIndex].Name;
                            obj2.Military.Bobinh = numArray;
                            obj2.Military.KyBinh = numArray2;
                            obj2.Military.CungThu = numArray3;
                            obj2.Military.Xe = numArray4;

                            obj2.MuuKeTrongChienTranID = muuKeTrongChienTruongID;
                            obj2.MuuKeTrongChienTranName = muuKeTrongChienTruongName;
                            obj2.TuKhoiPhucTrangThai = tuKhoiPhucTrangThai;


                            BinhManList.Add(obj2);


                        } // end for chkTabBinhManListManDanh
                    } // end manty=1
                    else                // Danh dia tinh trong mo
                    {
                        BinhManObj obj2 = new BinhManObj();

                        obj2.ManType = mantype;
                        obj2.ToaDoMoX = int.Parse(txtBinhManToaDoMoX.Text);
                        obj2.ToaDoMoY = int.Parse(txtBinhManToaDoMoY.Text);


                        obj2.PhuongThucTanCongID = num3;
                        obj2.PhuongThucTanCongName = str2;
                        obj2.PhuongThucChonMucTieuID = num4;
                        obj2.PhuongThucChonMucTieuName = str3;
                        obj2.UuTienID = num5;
                        obj2.UuTienName = str4;
                        obj2.SoLuongQuanMinToGo = num6;
                        obj2.SiKhiMinToGo = num7;
                        obj2.MaxOToGo = num8;
                        obj2.TimeToCheck = num9;
                        obj2.TuUpSiKhi = flag;
                        obj2.Id = general.Id;
                        obj2.Name = general.Name;
                        obj2.CityId = general.CityId;
                        obj2.CityName = general.CityName;
                        obj2.CityId = id;
                        obj2.CityName = LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[selectedIndex].Name;
                        obj2.Military.Bobinh = numArray;
                        obj2.Military.KyBinh = numArray2;
                        obj2.Military.CungThu = numArray3;
                        obj2.Military.Xe = numArray4;

                        obj2.MuuKeTrongChienTranID = muuKeTrongChienTruongID;
                        obj2.MuuKeTrongChienTranName = muuKeTrongChienTruongName;
                        obj2.TuKhoiPhucTrangThai = tuKhoiPhucTrangThai;


                        BinhManList.Add(obj2);
                    }
                } //end for chkTabBinhManTuongXuatTran

				for (num10 = 0; num10 < this.chkTabBinhManTuongXuatTran.Items.Count; num10++)
				{
					this.chkTabBinhManTuongXuatTran.SetItemChecked(num10, false);
				}
				for (num10 = 0; num10 < this.chkTabBinhManListManDanh.Items.Count; num10++)
				{
					this.chkTabBinhManListManDanh.SetItemChecked(num10, false);
				}
				LVCommon.common.LoadDataResultBinhMan(chkTabBinhManList);
			}
			catch (Exception)
			{
				MessageBox.Show("Sai cái gì đố rồi, xem lại đi");
			}
		}

		private void btTabBinhManHuyBo_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.chkTabBinhManList.CheckedItems.Count; i++)
			{
				BinhManList.Remove(this.chkTabBinhManList.CheckedItems[i]);
			}
			LVCommon.common.LoadDataResultBinhMan(chkTabBinhManList);
		}

		private void chkAUTOBINHMAN_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.chkAUTOBINHMAN.Checked)
				{
                    if (BinhManList.Count > 0)
                    {
                        if (LVAUTOBINHMAN == null)
                        {
                            LVAUTOBINHMAN = new LVAuto.LVForm.LVThread.AUTOBINHMAN(this.lblAUTOBINHMANMESSAGE);
                        }

#if (!DEBUG)
                        // chan neu release
                        int count = 0;
                        while (count < BinhManList.Count)
                        {
                           

                            bool found = false;
                            BinhManObj bm = (BinhManObj)BinhManList[count];
                            for (int i = 0; i < chkTabBinhManListManDanh.Items.Count; i++)
                            {
                                string str = chkTabBinhManListManDanh.Items[i].ToString();

                                int ManID = int.Parse(str.Substring(0, str.IndexOf(".")));
                                if (ManID == bm.ManID)
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                count = 0;
                                BinhManList.Remove(bm);
                            }
                            count++;
                        }
#endif
                        if (BinhManList.Count > 0)
                        {
                            LVAUTOBINHMAN.GetParameter(BinhManList, (int.Parse(this.txtTabBinhManTimeRun.Text) * 60) * 0x3e8);
                            this.pnBinhMan.Enabled = false;
                            LVAUTOBINHMAN.Auto();
                        }
                        else
                        {
                            this.lblAUTOBINHMANMESSAGE.Text = "Chưa chọn đúng tham số";
                            this.chkAUTOBINHMAN.Checked = false;
                        }
                    }
                    else
                    {
                        this.lblAUTOBINHMANMESSAGE.Text = "Chưa chọn đúng tham số";
                        this.chkAUTOBINHMAN.Checked = false;
                    }
				}
				else
				{
					if (LVAUTOBINHMAN != null)
					{
						LVAUTOBINHMAN.Stop();
					}
					this.pnBinhMan.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				this.lblAUTOBINHMANMESSAGE.Text = "Chưa chọn đúng tham số";
				this.chkAUTOBINHMAN.Checked = false;
			}
		}

		private void chkThaoPhatBienCheThemQuan_CheckedChanged(object sender, EventArgs e)
		{
			Quest_txtNumInfantries.Enabled = Quest_checkAutoTroop.Checked;
			Quest_txtNumCavalries.Enabled = Quest_checkAutoTroop.Checked; 
			Quest_txtNumArchers.Enabled = Quest_checkAutoTroop.Checked;
			Quest_txtNumCatapults.Enabled = Quest_checkAutoTroop.Checked;
		}

		private void Quest_btnQuestAddGeneral_Click(object sender, EventArgs e)
		{
            LVAddGeneralToQuest();
		}

		private void Quest_btnRemoveQuestGeneral_Click(object sender, EventArgs e)
		{
            LVRemoveGeneralFromQuest();			
		}

		private void cboCallManThanhTraiXuatQuan_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboCallManThanhTraiXuatQuan.SelectedItem == null) return;

			chklbCallManTuongXuatTran.Items.Clear();
			LVAuto.LVObj.City city = (LVAuto.LVObj.City)cboCallManThanhTraiXuatQuan.SelectedItem;


			if (city.MilitaryGenerals == null) city.MilitaryGenerals = LVAuto.LVForm.Command.Common.GetAllSimpleMilitaryGeneralInfo(city.Id);

			if (city.MilitaryGenerals == null)
			{
				MessageBox.Show("Lỗi rồi, mạng lởm quá, chờ tý rồi thử lại đê.", "Mạng lởm quá");
				return;
			}

			chklbCallManTuongXuatTran.Items.AddRange(city.MilitaryGenerals);
		}

		private void btCallManCancel_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.chklbCallManListResult.CheckedItems.Count; i++)
			{
				CallManList.Remove(this.chklbCallManListResult.CheckedItems[i]);
			}
			chklbCallManListResult.Items.Clear();
			chklbCallManListResult.Items.AddRange(CallManList.ToArray());
		}

		private void btCallManXacNhan_Click(object sender, EventArgs e)
		{
			try
			{
				string str;
				//TuongDiThaoPhatList.Clear();
				int cityid = 0;

				for (int idx = 0; idx < chklbCallManTuongXuatTran.CheckedItems.Count; idx++)
				{
					LVAuto.LVForm.Command.CommonObj.CallManObj TuongDiDanhMan = new LVAuto.LVForm.Command.CommonObj.CallManObj();

					TuongDiDanhMan.GroupID = int.Parse(cbCallManGroup.SelectedItem.ToString());

					TuongDiDanhMan.CityId = ((LVAuto.LVObj.City)cboCallManThanhTraiXuatQuan.SelectedItem).Id;
					cityid = TuongDiDanhMan.CityId;

					TuongDiDanhMan.TimeToCheck = int.Parse(txtCallManTimeToCheck.Text);
					TuongDiDanhMan.ToaDoCallVeX = int.Parse(txtCallManToaDoX.Text);
					TuongDiDanhMan.ToaDoCallVeY = int.Parse(txtCallManToaDoY.Text);

					TuongDiDanhMan.SiKhiMinToGo = int.Parse(txtCallManSyKhiMin.Text);
					TuongDiDanhMan.TuUpSiKhi = chkCallManTuNangSyKhi.Checked;
					TuongDiDanhMan.TuBienCheQuan = chkCallManCoBienCheThemQuan.Checked;
					TuongDiDanhMan.SoLuongQuanMinToGo = int.Parse(txtCallManQuanSoMin.Text);

					TuongDiDanhMan.Military.Bobinh[0] = int.Parse(txtCallManBienCheQuanBoBinh.Text);
					TuongDiDanhMan.Military.KyBinh[0] = int.Parse(txtCallManBienCheQuanKyBinh.Text);
					TuongDiDanhMan.Military.CungThu[0] = int.Parse(txtCallManBienCheQuanCungThu.Text);
					TuongDiDanhMan.Military.Xe[0] = int.Parse(txtCallManBienCheQuanXe.Text);


					str = this.cbCallManBienCheQuanGioiBoBinh1.SelectedItem.ToString();
					TuongDiDanhMan.Military.Bobinh[1] = int.Parse(str.Substring(0, str.IndexOf(".")));
					str = this.cbCallManBienCheQuanGioiBoBinh2.SelectedItem.ToString();
					TuongDiDanhMan.Military.Bobinh[2] = int.Parse(str.Substring(0, str.IndexOf(".")));
					str = this.cbCallManBienCheQuanGioiBoBinh3.SelectedItem.ToString();
					TuongDiDanhMan.Military.Bobinh[3] = int.Parse(str.Substring(0, str.IndexOf(".")));

					str = this.cbCallManBienCheQuanGioiKyBinh1.SelectedItem.ToString();
					TuongDiDanhMan.Military.KyBinh[1] = int.Parse(str.Substring(0, str.IndexOf(".")));
					str = this.cbCallManBienCheQuanGioiKyBinh2.SelectedItem.ToString();
					TuongDiDanhMan.Military.KyBinh[2] = int.Parse(str.Substring(0, str.IndexOf(".")));
					str = this.cbCallManBienCheQuanGioiKyBinh2.SelectedItem.ToString();
					TuongDiDanhMan.Military.KyBinh[3] = int.Parse(str.Substring(0, str.IndexOf(".")));

					str = this.cbCallManBienCheQuanGioiCungThu1.SelectedItem.ToString();
					TuongDiDanhMan.Military.CungThu[1] = int.Parse(str.Substring(0, str.IndexOf(".")));
					str = this.cbCallManBienCheQuanGioiCungThu2.SelectedItem.ToString();
					TuongDiDanhMan.Military.CungThu[2] = int.Parse(str.Substring(0, str.IndexOf(".")));
					str = this.cbCallManBienCheQuanGioiCungThu3.SelectedItem.ToString();
					TuongDiDanhMan.Military.CungThu[3] = int.Parse(str.Substring(0, str.IndexOf(".")));

					str = this.cbCallManBienCheQuanGioiXe11.SelectedItem.ToString();
					TuongDiDanhMan.Military.Xe[1] = int.Parse(str.Substring(0, str.IndexOf(".")));

					
					
					TuongDiDanhMan.SoTuongMinhDanh1TuongDich = int.Parse(cbCallManSLTuongMinhDanh1TuongDich.SelectedItem.ToString());


					str = cbCallManPhuongThucTanCong.SelectedItem.ToString();
					//str = str.Substring(0, str.IndexOf("."));
					TuongDiDanhMan.PhuongThucTanCongID = int.Parse(str.Substring(0, str.IndexOf(".")));
					TuongDiDanhMan.PhuongThucTanCongName = str.Substring(str.IndexOf(".") + 2);

					str = cbCallManPhuongThucChonMucTieu.SelectedItem.ToString();
					//str = str.Substring(0, str.IndexOf("."));
					TuongDiDanhMan.PhuongThucChonMucTieuID = int.Parse(str.Substring(0, str.IndexOf(".")));
					TuongDiDanhMan.PhuongThucChonMucTieuName = str.Substring(str.IndexOf(".") + 2);

					str = cbCallManMuuKeTrongChienTruong.SelectedItem.ToString();
					//str = str.Substring(0, str.IndexOf("."));
					TuongDiDanhMan.MuuKeTrongChienTranID = int.Parse(str.Substring(0, str.IndexOf(".")));
					TuongDiDanhMan.MuuKeTrongChienTranName = str.Substring(str.IndexOf(".") + 2);

					//
					TuongDiDanhMan.TuDoiTranHinhKhac = true; //chkCallManTuDoiTranHinh.Checked;
					TuongDiDanhMan.TuKhoiPhucTrangThai = chkCallManTuKhoiPhucTrangThai.Checked;
					TuongDiDanhMan.TuMuaManTocLenh = chkCallManTuMuaThemManTocLenh.Checked;

					LVObj.MilitaryGeneral gSelect = (LVObj.MilitaryGeneral)chklbCallManTuongXuatTran.CheckedItems[idx];

					TuongDiDanhMan.Id = gSelect.Id;
					TuongDiDanhMan.Name = gSelect.Name;

					

					if (cboCallManManCanCall.CheckedItems.Count == 0) return;



					LVAuto.LVForm.Command.CommonObj.ManOBJ manobj;
					for (int man = 0; man < cboCallManManCanCall.CheckedItems.Count; man++)
					{
						manobj = new ManOBJ();

						str = cboCallManManCanCall.CheckedItems[man].ToString();

						manobj.ManID = int.Parse(str.Substring(0, str.IndexOf(".")));
						manobj.ManName = str.Substring(str.IndexOf(".") + 2);

						TuongDiDanhMan.Mans.Add(manobj);
					}

					CallManList.Add(TuongDiDanhMan);

				} //for (int idx = 0; idx < cboGeneral.CheckedItems.Count; idx++)



				if (CallManList.Count > 0)
				{
					chklbCallManListResult.Items.Clear();
					chklbCallManListResult.Items.AddRange(CallManList.ToArray());
				}


			}
			catch (Exception ex)
			{
				MessageBox.Show("Chưa cài đặt đúng, xem lại đê", "Gọi man", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void chkCallManCoBienCheThemQuan_CheckedChanged(object sender, EventArgs e)
		{
			txtCallManBienCheQuanBoBinh.Enabled = chkCallManCoBienCheThemQuan.Checked;
			txtCallManBienCheQuanKyBinh.Enabled = chkCallManCoBienCheThemQuan.Checked; 
			txtCallManBienCheQuanCungThu.Enabled = chkCallManCoBienCheThemQuan.Checked; 
			txtCallManBienCheQuanXe.Enabled = chkCallManCoBienCheThemQuan.Checked;
		}

		private void chkAUTOCALLMAN_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.chkAUTOCALLMAN.Checked)
				{
					if (CallManList.Count > 0)
					{
						LVAUTOCALLMAN.GetParameter(CallManList, int.Parse(this.txtCallManTimeToCheck.Text) * 60 * 1000);
						this.pnCallMan.Enabled = false;
						LVAUTOCALLMAN.Auto();
					}
					else
					{
						this.lblAUTOCALLMANMESSAGE.Text = "Chưa chọn đúng tham số";
						this.chkAUTOCALLMAN.Checked = false;
					}
				}
				else
				{
					if (LVAUTOCALLMAN != null)
					{
						LVAUTOCALLMAN.Stop();
					}
					this.pnCallMan.Enabled = true;
				}
			}
			catch (Exception)
			{
				this.lblAUTOCALLMANMESSAGE.Text = "Chưa chọn đúng tham số";
				this.chkAUTOCALLMAN.Checked = false;
			}
		}

        private void tvDEL_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode c in e.Node.Nodes)
            {
                c.Checked = e.Node.Checked;
            }

        }

        private void rdBinhManDanhMan_CheckedChanged(object sender, EventArgs e)
        {
            chkTabBinhManListManDanh.Visible = rdBinhManDanhMan.Checked;
            grBinhManToaDoMo.Visible = !rdBinhManDanhMan.Checked;
        }

        private void rdBinhManDanhDiaTinh_CheckedChanged(object sender, EventArgs e)
        {
            chkTabBinhManListManDanh.Visible = !rdBinhManDanhDiaTinh.Checked;
            grBinhManToaDoMo.Visible = rdBinhManDanhDiaTinh.Checked;
        }
	} //end class
} //end namespace
