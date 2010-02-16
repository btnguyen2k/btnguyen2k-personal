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
using LVAuto.LVForm.Command.CityObj;
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

		public bool BuyResource_loaded = false;
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

		private bool bantaiNguyenLuaCheckAll = true;
		private bool bantaiNguyenGoCheckAll = true;
		private bool bantaiNguyenSatCheckAll = true;
		private bool bantaiNguyenDaCheckAll = true;

		private bool muataiNguyenLuaCheckAll = true;
		private bool muataiNguyenGoCheckAll = true;
		private bool muataiNguyenSatCheckAll = true;
		private bool muataiNguyenDaCheckAll = true;

		private Hashtable QuocKho_Ruong = new Hashtable();
		
		
		// ban tai nguyen
		private int BanTNSoLuongBanCoDinhDefault = 1000;
		private int BanTNSoLuongBanPercentDefault = 30;
		private int BanTNAnToanCoDinhDefault = 500;
		private int BanTNAnToanPercentDefault = 60;

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
		        panelLoading.BringToFront();
			    panelLoading.Visible = false;
			    picAttach.Visible = false;

                LVLoadServerList();

                LVCITYTASK = new LVAuto.LVForm.LVThread.CITYTASK();
                LVAUTOTASK = new LVAuto.LVForm.LVThread.AUTOTASK(lblLoadingResMessage);

                THREAD_SELL_RESOURCES = LVAuto.LVThread.AutoSellResources.getInstance(Auto_labelAutoSellRes);
                THREAD_CONSTRUCT = LVAuto.LVThread.AutoConstruct.getInstance(Auto_labelAutoConstruct);
                LVDEL = new LVAuto.LVForm.LVThread.DEL(lblDELMESSAGE);
                LVBUYRES = new LVAuto.LVForm.LVThread.BUYRES(Auto_labelAutoBuyRes);
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
			cbTabBinhManThanhXuatQuan.Items.AddRange(Command.CityObj.City.AllCity);


		}
		private void LoadDataForVCVK()
		{
			try
			{

				chklVCVKThanhDi.Items.Clear();
				chklVCVKThanhDen.Items.Clear();
				chklVCVKLoaiVuKhi.Items.Clear();
				for (int i = 0; i < Command.CityObj.City.AllCity.Length; i++)
				{
					if (Command.CityObj.City.AllCity[i].id > 0)
					{
						chklVCVKThanhDi.Items.Add(Command.CityObj.City.AllCity[i], false);
						chklVCVKThanhDen.Items.Add(Command.CityObj.City.AllCity[i], false);
					}
				}
				chklVCVKLoaiVuKhi.Items.AddRange(LVCommon.Wepons.arWepon);


				/*
				cbVCVKThanhDi.Items.Clear();
				chklVCVKThanhDen.Items.Clear();
				cbVCVKLoaiVK.Items.Clear();
				for (int i = 0; i < Command.CityObj.City.AllCity.Length; i++)
				{
					if (Command.CityObj.City.AllCity[i].id > 0)
					{
						cbVCVKThanhDi.Items.Add(Command.CityObj.City.AllCity[i]);
						chklVCVKThanhDen.Items.Add(Command.CityObj.City.AllCity[i], false);
					}
				}

				cbVCVKLoaiVK.Items.AddRange(Common.Wepons.arWepon);
				*/

			}
			catch (Exception ex)
			{

			}
		}


		/* public void FirstLogin() {
			 notifyIcon1.Text = "Đang load dữ liệu, vui lòng chờ. Máy có thể đứng đó.";
            
			 if (LVAuto.Web.LVWeb.LoginHtml != null) {
				 while (true) {
					 try {
						 Hashtable lastlogindata = Web.ParseHeader.GetDataFromForm(LVAuto.Web.LVWeb.LoginHtml);
						 Hashtable lastlogin = Web.LVWeb.LoginPartner(lastlogindata["uid"].ToString(), lastlogindata["uname"].ToString(), lastlogindata["ulgtime"].ToString(), lastlogindata["pid"].ToString(), lastlogindata["sign"].ToString());
						 Web.LVWeb.CurrentLoginInfo = new Web.LoginInfo((string[])lastlogin["Set-Cookie"]);
                        
						 Command.City.GetAllCity();
							
						 Common.common.LoadCityToGridForSellResource(this.dtaSELL);
						 while (true) {
							 try {
								 LoadAvgPrice();
								 break;
							 } catch (Exception ex) {
							 }
						 }
						 Common.common.LoadCityToGridForBuyResource(this.dtaBUYRESOURCE);
						 Common.common.LoadCityToGridForAnUi(dtaAnUi);
						 Common.common.LoadBuildingToTreeViewForbuild(this.tvBUILD);
						 Common.common.LoadBuildingToTreeViewForbuild(this.tvDEL);
						 while (true) {
							 try {
								 Command.Common.GetMaxNhiemVu();
								 break;
							 } catch (Exception ex) {
							 }
						 }
						 Common.common.LoadUpgradeToTreeViewForUpdate(tvUpdate);
						 Common.common.LoadCityToGridForVanchuyen(pnVanchuyen);
						 Common.common.LoadDoanhTraiForMove(pnDoanhTrai);
						 Common.common.LoadGeneralForUpSiKhi(tvSIKHI);
						 Common.common.LoadCityForBuyWepon(pnWepon);



						 chkAutoAll.Enabled = true;
						 IsLogin = true;
						 cboCity.Items.Clear();
						 cboCity.Items.AddRange(Command.CityObj.City.AllCity);
						 cboCityForUpgrade.Items.Clear();
						 cboCityForUpgrade.Items.AddRange(Command.CityObj.City.AllCity);
						 cboNhiemVu.Items.Clear();
						 cboNhiemVu.Items.AddRange(Command.CommonObj.ThaoPhat.AllNhiemVu);
						 notifyIcon1.Text = "LVAuto";
						 LVBUILD = new LVAuto.LVThread.BUILD(lblBUILDMESSAGE);
						 LVDEL = new LVAuto.LVThread.DEL(lblDELMESSAGE);
						 LVSELL = new LVAuto.LVThread.SELL(lblSELLMESSAGE);
						 LVBUYRES = new LVAuto.LVThread.BUYRES(lblBUYRESMESSAGE);
						 LVTHAOPHAT = new LVAuto.LVThread.THAOPHAT(lblTHAOPHATMESSAGE);
						 LVUPGRADE = new LVAuto.LVThread.UPGRADE(lblUPGEADEMESSAGE);
						 LVANUI = new LVAuto.LVThread.ANUI(lblANUIMESSAGE);
						 LVVANCHUYEN = new LVAuto.LVThread.VANCHUYEN(lblVANCHUYENMESSAGE);
						 LVMOVEDOANHTRAI = new LVAuto.LVThread.MOVEDOANHTRAI(lblMOVEDOANHTRAI);
						 LVSIKHI = new LVAuto.LVThread.SIKHI(lblSIKHIMESSAGE);
						 LVBUYWEPON = new LVAuto.LVThread.BUYWEPON(lblBUYWEPONMESSAGE);
						 LVCITYTASK = new LVAuto.LVThread.CITYTASK();
						 //LVCITYTASK.IsRun = true;
                        
						 LVCITYTASK.Auto();
                        
						 LVAuto.Web.LVWeb.debug = false;
						 LVAuto.Web.LVWeb.firstlogin = false;
						 break;
					 } catch (Exception ex) {
						 //MessageBox.Show(ex.ToString());
					 }
				 }
			 } else {
				 Application.Exit();
			 }
		 }

		 */
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

        private void deltuong()
        {

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
            /*
            Quest_GeneralsInCity.Items.Clear();
            try {
				ShowLoadingLabel();
				LVAuto.LVForm.Command.Common.GetGeneral(Quest_dropdownCityList.SelectedIndex, false); 
				Quest_GeneralsInCity.Items.AddRange(Command.CityObj.City.AllCity[Quest_dropdownCityList.SelectedIndex].MilitaryGeneral);

				int g;
				for (int i = 0; i < Quest_GeneralsInCity.Items.Count; i++)
				{
					g = ((Command.CityObj.MilitaryGeneral)Quest_GeneralsInCity.Items[i]).Id;
					for (int k = 0; k < TuongDiThaoPhatList.Count; k++)
					{
						if (g == ((Common.GeneralThaoPhat)TuongDiThaoPhatList[k]).Id )
							Quest_GeneralsInCity.SetItemChecked(i, true);
					}
				}

				HideLoadingLabel();
            } catch (Exception ex) { }
            */
        }

        private void LoadAvgPrice() 
		{
            /* 1. Lúa
             2. Gỗ
             3. Đá
             4. Sắt
             * */
            //txtPRICELUA.Text = "" + (int.Parse(Command.OPT.AvgPrice(1).ToString())+10);
            //txtPRICEGO.Text = "" + (int.Parse(Command.OPT.AvgPrice(2).ToString()) + 10);
            //txtPRICEDA.Text = "" + (int.Parse(Command.OPT.AvgPrice(3).ToString()) + 10);
            //txtPRICESAT.Text = "" + (int.Parse(Command.OPT.AvgPrice(4).ToString()) + 10);
        }

        private void cmdLoadConfig_Click(object sender, EventArgs e) {
            openFileDialog1.ShowDialog();
			
			(new LVAuto.LVWeb.SaveNLoad()).loadConfig(fileSavepath);

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e) {

			fileSavepath = openFileDialog1.FileName;
			return;
			 /*
			Common.config.load(openFileDialog1.FileName);
            try {
                //Load ban tai nguyen
                if (Common.config.SELLRESOURCE != null) {
                    txtSELLCHECK.Text = Common.config.SELLRESOURCE[1];
                    //nap lua
                    txtCOUNTLUA.Text = Common.config.SELLRESOURCE[2];
                    txtPRICELUA.Text = Common.config.SELLRESOURCE[3];
                    txtSAFELUA.Text = Common.config.SELLRESOURCE[4];
                    //nap go
                    txtCOUNTGO.Text = Common.config.SELLRESOURCE[5];
                    txtPRICEGO.Text = Common.config.SELLRESOURCE[6];
                    txtSAFEGO.Text = Common.config.SELLRESOURCE[7];
                    //nap sat
                    txtCOUNTSAT.Text = Common.config.SELLRESOURCE[8];
                    txtPRICESAT.Text = Common.config.SELLRESOURCE[9];
                    txtSAFESAT.Text = Common.config.SELLRESOURCE[10];
                    //nap da
                    txtCOUNTDA.Text = Common.config.SELLRESOURCE[11];
                    txtPRICEDA.Text = Common.config.SELLRESOURCE[12];
                    txtSAFEDA.Text = Common.config.SELLRESOURCE[13];
                    //nap thanh
                    DataTable data = (DataTable)dtaSELL.DataSource;
                    for (int i = 0; i < data.Rows.Count; i++) {
                        for (int j = 14; j < Common.config.SELLRESOURCE.Length; j++) {
                            if (Common.config.SELLRESOURCE[j] == data.Rows[i]["ID_TT"].ToString()) {
                                data.Rows[i]["SELL_LUA"] = Common.config.SELLRESOURCE[j + 1] == "1" ? true : false;
                                data.Rows[i]["SELL_GO"] = Common.config.SELLRESOURCE[j + 2] == "1" ? true : false;
                                data.Rows[i]["SELL_SAT"] = Common.config.SELLRESOURCE[j + 3] == "1" ? true : false;
                                data.Rows[i]["SELL_DA"] = Common.config.SELLRESOURCE[j + 4] == "1" ? true : false;
                            }
                            j += 4;
                        }
                    }
                }
                //Load mua tai nguyen
                if (Common.config.BUYRESOURCE != null) {
                    txtBUYRESOURCECHECK.Text = Common.config.BUYRESOURCE[1];
                    txtSAFEGOLD.Text = Common.config.BUYRESOURCE[2];
                    DataTable data = (DataTable)dtaBUYRESOURCE.DataSource;
                    for (int i = 0; i < data.Rows.Count; i++) {
                        for (int j = 3; j < Common.config.BUYRESOURCE.Length; j++) {
                            if (Common.config.BUYRESOURCE[j] == data.Rows[i]["ID_TT"].ToString()) {
                                data.Rows[i]["BUY_LUA"] = Common.config.BUYRESOURCE[j + 1] == "1" ? true : false;
                                data.Rows[i]["BUY_GO"] = Common.config.BUYRESOURCE[j + 2] == "1" ? true : false;
                                data.Rows[i]["BUY_SAT"] = Common.config.BUYRESOURCE[j + 3] == "1" ? true : false;
                                data.Rows[i]["BUY_DA"] = Common.config.BUYRESOURCE[j + 4] == "1" ? true : false;
                            }
                            j += 4;
                        }
                    }
                }
                //load xay nha
                if (Common.config.UPDATEBUILDING != null) {
                    txtBUILDCHECK.Text = Common.config.UPDATEBUILDING[1];
                    TreeNode root = tvBUILD.Nodes["root"];
                    for (int i = 2; i < Common.config.UPDATEBUILDING.Length; i++) {
                        if (Common.config.UPDATEBUILDING[i] == root.Name) root.Checked = true;
                    }
                    foreach (TreeNode tt in root.Nodes) {
                        for (int i = 2; i < Common.config.UPDATEBUILDING.Length; i++) {
                            if (Common.config.UPDATEBUILDING[i] == tt.Name) tt.Checked = true;
                        }
                        foreach (TreeNode b in tt.Nodes) {
                            for (int i = 2; i < Common.config.UPDATEBUILDING.Length; i++) {
                                if (Common.config.UPDATEBUILDING[i] == b.Name) b.Checked = true;
                            }
                        }
                    }
                }
                //load ha nha
                if (Common.config.DELBUILDING != null) {
                    txtDELCHECK.Text = Common.config.DELBUILDING[1];
                    TreeNode root = tvDEL.Nodes["root"];
                    for (int i = 2; i < Common.config.DELBUILDING.Length; i++) {
                        if (Common.config.DELBUILDING[i] == root.Name) root.Checked = true;
                    }
                    foreach (TreeNode tt in root.Nodes) {
                        for (int i = 2; i < Common.config.DELBUILDING.Length; i++) {
                            if (Common.config.DELBUILDING[i] == tt.Name) tt.Checked = true;
                        }
                        foreach (TreeNode b in tt.Nodes) {
                            for (int i = 2; i < Common.config.DELBUILDING.Length; i++) {
                                if (Common.config.DELBUILDING[i] == b.Name) b.Checked = true;
                            }
                        }
                    }
                }
                //load vu khi
                if (Common.config.WEPON != null) {
                    txtCHECKWEPON.Text = Common.config.WEPON[1];
                    txtCOUNTWEPON.Text = Common.config.WEPON[2];
                    for (int i = 3; i < Common.config.WEPON.Length; i++) {
                        foreach (object o in pnWepon.Controls) {
                            Command.OPTObj.wepon w = (Command.OPTObj.wepon)o;
                            if (w.cityid.ToString() == Common.config.WEPON[i].ToString()) {
                                w.cboWepon.Text = Common.config.WEPON[i + 1];
                                w.cboAmor.Text = Common.config.WEPON[i + 2];
                                w.cboHorse.Text = Common.config.WEPON[i + 3];
                                w.cboLevel.Text = Common.config.WEPON[i + 4];
                                w.chkOK.Checked = bool.Parse(Common.config.WEPON[i + 5]);
                                i = i + 5;
                            }
                        }
                    }
                }
            } catch (Exception ex) {
            }
			  * */
        }

        private void Auto_btnSaveConfig_Click(object sender, EventArgs e) {
            LVSaveConfig();
        }

        private void cmdUpdatePrice_Click(object sender, EventArgs e) {
            
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

        private void Auto_checkAutoSellResources_CheckedChanged(object sender, EventArgs e) {
            LVToggleAutoSellResources();
        }

        private void Auto_checkAutoBuyResources_CheckedChanged(object sender, EventArgs e) {
            try
			{
			if (Auto_checkAutoBuyResources.Checked) 
			{
                //LVBUYRES.GetParameter(
                //    int.Parse(txtSAFEGOLD.Text),
                //    dtaBUYRESOURCE, int.Parse(txtBUYRESOURCECHECK.Text) * 60 * 1000);

				LVBUYRES.GetParameter(MuaTaiNguyen);
				pnLVBUYRES.Enabled = false;
				
                LVBUYRES.Auto();
            } else {
                LVBUYRES.Stop();
                pnLVBUYRES.Enabled = true;
            }
			}
			catch (Exception ex)
			{
				Auto_labelAutoBuyRes.Text = "Chưa chọn đúng tham số";
				Auto_checkAutoBuyResources.Checked = false;
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
                   
					//LVUPGRADE.GetParameter(tvUpdate, ((LVAuto.Command.CityObj.City)cboCityForUpgrade.SelectedItem).id, 
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

        private void tvBUILD_AfterSelect(object sender, TreeViewEventArgs e) {

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

			LVThread.UPDATEPRICE up = new LVAuto.LVForm.LVThread.UPDATEPRICE(int.Parse(txtBanTN_LUA_TB_Heso.Text), int.Parse(txtBanTN_GO_TB_Heso.Text), 
												int.Parse(txtBanTN_DA_TB_Heso.Text), int.Parse(txtBanTN_SAT_TB_Heso.Text));
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {

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

        private void timer1_Tick(object sender, EventArgs e) 
		{
			//System.Collections.ArrayList arTemp = LVAuto.Common.ThreadManager.s_alRegister;
			//System.Collections.ArrayList arTempRunning = LVAuto.Common.ThreadManager.s_funcRunning;
        }

        private void cboServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LVAuto.LVWeb.LVClient.Server = int.Parse(Login_dropdownServerList.Text.Substring(0,1));
        }

        /// <summary>
        /// Auto-Called when the "Login" button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
			try
			{
                LVLogin();
			}
			catch
			{
				MessageBox.Show("Hic hic, không login được mà không biết tại sao, có thể do mạng lởm. Cố thử lại lần nữa xem sao.", "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Login_txtPassword.Enabled = true;
				Login_txtUsername.Enabled = true;
				Login_btnLogin.Enabled = true;
				//frLoading.Close();
				HideLoadingLabel();
			}
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
			lblPanelLoading.Text = message;
			panelLoading.BringToFront();
			panelLoading.Visible = true;
			lblPanelLoading.BringToFront();
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
			panelLoading.Visible = false;
			LVFRMMAIN.Refresh();
		}

		private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
		{
			_chuongbao = chkChuong.Checked;
		}

		private void tabLogin_Click(object sender, EventArgs e)
		{

		}

		private void Construct_dropdownCityList_SelectedIndexChanged(object sender, EventArgs e)
		{
            LVLoadBuildingsToTreeViewForConstruct();
		}

		private void tvBUILD_Leave(object sender, EventArgs e)
		{
			
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
					Command.CityObj.City.AllCity[cityPost].AllBuilding[i].isDown = root.Nodes[i].Checked;
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
                    root.Nodes[i].Checked = Command.CityObj.City.AllCity[cboTabHaNhaCity.SelectedIndex].AllBuilding[i].isDown;
                }
            }
            catch (Exception ex)
            {

            }

		}

		private void label61_Click(object sender, EventArgs e)
		{

		}

		private void txtDELCHECK_TextChanged(object sender, EventArgs e)
		{

		}

		private void cboGeneral_Leave(object sender, EventArgs e)
		{
			/*
			int cityPost = cboCity.SelectedIndex;
			int task = cboNhiemVu.SelectedIndex;

			//Xay dung mang de xay nha
			TreeNode root = tvDEL.Nodes[0];

			//if (root.Checked) IsBuildAll = true;
			TreeNode tt;
			for (int i = 0; i < root.Nodes.Count; i++)
			{
				Command.CityObj.City.AllCity[cityPost].AllBuilding[i].isDown = root.Nodes[i].Checked;
			}
			 */ 
		}

		private void txtCHECKWEPON_TextChanged(object sender, EventArgs e)
		{

		}

		private void cboNhiemVu_SelectedIndexChanged(object sender, EventArgs e)
		{

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

		private void grbXacNhanHinhAnh_Enter(object sender, EventArgs e)
		{

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

		private void cboLuyenSKCity_SelectedIndexChanged(object sender, EventArgs e)
		{


			/*
			 * 
			  cboGeneral.Items.Clear();
            try {
				showLoadingLabel();
				LVAuto.Command.Common.GetGeneral(cboCity.SelectedIndex, false); 
				cboGeneral.Items.AddRange(Command.CityObj.City.AllCity[cboCity.SelectedIndex].AllGeneral);


				hideLoadingLabel();
            } catch (Exception ex) { }
			 * 
			 * 
			*/
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
				chklstGeneral.Items.AddRange(Command.CityObj.City.AllCity[cboBienCheCity.SelectedIndex].MilitaryGeneral);

				
				HideLoadingLabel();
            } catch (Exception ex) { }
        }

		private void btBiencheAccept_Click(object sender, EventArgs e)
		{
			try
			{
				Command.CityObj.MilitaryGeneral onegenral;
				LVAuto.LVForm.LVCommon.BienChe bienche;
				//int cityid = cboBienCheCity.SelectedIndex;
				int citypost = cboBienCheCity.SelectedIndex;
				int cityid = Command.CityObj.City.AllCity[citypost].id;

				for (int i = 0; i < chklstGeneral.CheckedItems.Count; i++)
				{
					bienche = new LVAuto.LVForm.LVCommon.BienChe();
					onegenral = (Command.CityObj.MilitaryGeneral)chklstGeneral.CheckedItems[i];
					bienche.cityid = cityid;

					bienche.generalid = onegenral.Id;
					bienche.generalname = onegenral.Name;
					bienche.cityname = Command.CityObj.City.AllCity[citypost].name;
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

		private void btTest_Click(object sender, EventArgs e)
		{
            int i;
           
           
            
            DeleteNhacNhoAdmin();

			LVAuto.LVForm.Control.War w = new LVAuto.LVForm.Control.War();
			w.Show();

			return;

			int generalid;
			int cityid;
			int sk;


			
			generalid = 412227;
			// -16702 "5.3"
			cityid = -16702;


			//cityid = 13139; //iwunu5
			//generalid = 426324;			//,"Lão Nạp Thạch

			LVAuto.LVForm.Command.CityObj.MilitaryGeneral[] g = Command.Common.GetGeneralInforInLuyenBinh(cityid);

			//sk = LVAuto.Command.Common.GetGeneralSyKhiInLuyenBinh(cityid, generalid);

			cityid = 13139;	// iwunu5
			generalid = 4426324;  //Lam nap thach

			 g = Command.Common.GetGeneralInforInLuyenBinh(cityid);

			//sk = LVAuto.Command.Common.GetGeneralSyKhiInLuyenBinh(cityid, generalid);
			return;






			// test di loi dai

			 cityid = 13139;	// iwunu5

			int genid = 426324;  //Lam nap thach
			string cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityid);
			string para;
			LVAuto.LVForm.Command.City.SwitchCitySlow(cityid);
			para = "lCityTentID=" + cityid + "&lGeneralID=" + genid + "&lLevel=1";
			Hashtable r= Command.OPT.Execute(21, para, false);
			if (r != null && r["DATA"].ToString() == "{\"ret\":0}")
			{
				bool ok = true;

			}

			//Command.Common.GetGeneral(0, true);
			//Command.Common.GetGeneralMilitary(10718,405711);
			//Common.BienCheQuan.BienChe(10718, 405711, 100, 100, 100);
			//Common.BienCheQuan.BienChe(ListBienChe );
			int g1 = 277645, g2 = 275162, g3 = 313841, g4 = 0, g5 = 0, idnhiemvu = 4;
		
			Command.Common.GetMaxNhiemVu();
			Command.OPT.DanhSonTac(idnhiemvu, g1, g2, g3, g4, g5, cookies);

			i = 0;
			
			Hashtable ret;
			//405723,"Quân Lạc Xuyên"
			 generalid = 405723;
			//[10718,"iwunu7(Đô thành)
			 cityid = 10718;




			// tu ddi dao mo
			int X = 299;
			int Y = -111;
			cityid = 13139;
			int mapid;
			//http://s3.linhvuong.zooz.vn/GateWay/Common.ashx?id=72&0.24057125736018958&DestX=299&DestY=-111
			para = "DestX=" + X + "&DestY=" + Y;
			ret = LVAuto.LVForm.Command.Common.Execute(72, para, true);

			if (ret == null) 
				return;
			mapid = int.Parse(ret["map_id"].ToString());

			return;


			

			
			sk = LVAuto.LVForm.Command.Common.GetGeneralSyKhiInLuyenBinh(cityid, generalid);


			//412227,"Tề Hậu"
			generalid = 412227;
			// -16702 "5.3"
			cityid = -16702;

			sk = LVAuto.LVForm.Command.Common.GetGeneralSyKhiInLuyenBinh(cityid, generalid);

			return;

			
			
			// tu ddi dao mo
			 X = 299;
			Y = -111;
			 cityid = 13139;
						//http://s3.linhvuong.zooz.vn/GateWay/Common.ashx?id=72&0.24057125736018958&DestX=299&DestY=-111
			para = "DestX=" + X + "&DestY=" + Y;
			ret = LVAuto.LVForm.Command.Common.Execute(72, para, true);

			if (ret == null) return;
			mapid = int.Parse(ret["map_id"].ToString());

			//get danh sach quan van
			//http://s3.linhvuong.zooz.vn/GateWay/Common.ashx?id=15&0.31696323126782144&lType=1&tid=13139

			 cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityid);

			para = "lType=1&tid=" + cityid;
			ret = LVAuto.LVForm.Command.Common.Execute(15, para, true, cookies);

			ArrayList gen = (ArrayList) ret["generals"];  // 3=0, 5=0, 13=0
			 genid = int.Parse(((ArrayList)gen[0])[0].ToString());

			// cu quan van di
			//http://s3.linhvuong.zooz.vn/GateWay/OPT.ashx?id=100&0.4721581634738514&DestMapID=1004086&HeroID=412545&tid=11697
			para = "DestMapID=" + mapid + "&HeroID=" + genid + "&tid=" + cityid;
			ret = LVAuto.LVForm.Command.OPT.Execute(100, para, true, cookies);
			int iii;
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

		private void tabBienChe_Click(object sender, EventArgs e)
		{

		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void groupBox5_Enter(object sender, EventArgs e)
		{

		}


		public  void PhaiQuanVan(LVAuto.LVForm.LVCommon.PhaiQuanVanDaoMo pqv)
		{
			if (pqv.check)
			{
				LVDAOMO.Auto();
			}
		}
/*		private void gbTuPhaiQuanVan_Leave(object sender, EventArgs e)
		{
			try
			{
				QuanVanDaoMo.check = chkTienIchTuphaiQuanVan.Checked;
				if (QuanVanDaoMo.check)
				{
					QuanVanDaoMo.cityid = Command.CityObj.City.AllCity[cboTuphaiquanvanCity.SelectedIndex].id;
					QuanVanDaoMo.X = int.Parse(txtTuphaiQuanVanX.Text);
					QuanVanDaoMo.Y = int.Parse(txtTuphaiQuanVanY.Text);
					QuanVanDaoMo.Time = 60000;
					//PhaiQuanVan(QuanVanDaoMo);
				}
				else
				{
					//LVTIENICH.Stop();
				}

			}
			catch (Exception ex)
			{
			}
		}
*/
		private void pnTHAOPHAT_Leave(object sender, EventArgs e)
		{
			

		}

		

		private void chklstAnUi_Leave(object sender, EventArgs e)
		{
			AnuiForAuto = new ArrayList();

			for (int i = 0; i < chklstAnUi.CheckedItems.Count; i++)
			{
				AnuiForAuto.Add(((LVAuto.LVForm.Command.CityObj.City)chklstAnUi.CheckedItems[i]).id);
			}
		}

		private void pnUPGRADE_Leave(object sender, EventArgs e)
		{
			UpgradeForAuto = new ArrayList();
			if (cboCityForUpgrade.SelectedItem != null)
			{
				UpgradeForAuto.Add(((LVAuto.LVForm.Command.CityObj.City)cboCityForUpgrade.SelectedItem).id);
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

		private void cboCityForUpgrade_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void cboTuphaiquanvanCity_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void btTabDieuPhaiPhaiQuanVan_Click(object sender, EventArgs e)
		{
			try
			{		
				/*
				QuanVanDaoMo.cityid = Command.CityObj.City.AllCity[cboTabDieuPhaiThanhPhaiQuanVan.SelectedIndex].id;
				QuanVanDaoMo.X = int.Parse(txtTabDieuPhaiDaoMoX.Text);
				QuanVanDaoMo.Y = int.Parse(txtTabDieuPhaiDaoMoY.Text);
				QuanVanDaoMo.Time = 60000;
				QuanVanDaoMo.check = true;
				*/
				Command.CommonObj.DieuPhaiTuong dieuphai = new LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong();
				dieuphai.cityID = Command.CityObj.City.AllCity[cboTabDieuPhaiThanhPhaiQuanVan.SelectedIndex].id;
				dieuphai.X = int.Parse(txtTabDieuPhaiDaoMoX.Text);
				dieuphai.Y = int.Parse(txtTabDieuPhaiDaoMoY.Text);
				dieuphai.generaltype = Command.CommonObj.DieuPhaiTuong.GENERALTYPE.QuanVan;
				dieuphai.timetoruninmilisecond = 60000; ;
				dieuphai.desc = "Điều phái quan văn từ thành " + Command.CityObj.City.AllCity[cboTabDieuPhaiThanhPhaiQuanVan.SelectedIndex].name
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
			cityid = Command.CityObj.City.AllCity[cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedIndex].id;

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
				Command.CityObj.MilitaryGeneral g = (Command.CityObj.MilitaryGeneral) cbTabDieuPhaiQuanVanVoDiLoiDai.SelectedItem;
				if (g == null)
				{
					btTabDieuPhai_PhaiDiLoiDai.Enabled = false;
					return;
				}

				dieuphai.cityID = Command.CityObj.City.AllCity[cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedIndex].id;
				dieuphai.generalid = g.Id;
				dieuphai.generalname = g.Name;
				dieuphai.generaltype = g.Type;
				string str = cboCapLoiDai.SelectedItem.ToString();
				dieuphai.loidailevel = int.Parse( str.Substring(0, str.IndexOf(".")));
				dieuphai.timetoruninmilisecond = 60000; ;

				if (dieuphai.generaltype == Command.CommonObj.DieuPhaiTuong.GENERALTYPE.QuanVan)
				{
					dieuphai.desc = "Điều phái quan văn " + dieuphai.generalname + " từ thành " + Command.CityObj.City.AllCity[cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedIndex].name
										+ " đi lôi đài cấp " + dieuphai.loidailevel;
				}
				else
				{
					dieuphai.desc = "Điều phái quan võ " + dieuphai.generalname + " từ thành " + Command.CityObj.City.AllCity[cbTabDieuPhaiThanhPhaiDiLoiDai.SelectedIndex].name
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

		
		private void tabBanTaiNguyen_Leave(object sender, EventArgs e)
		{
			
		}

		private void chkBanTN_LUA_TB_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void chkBanTN_GO_TB_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void chkBanTN_SAT_TB_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void chkBanTN_DA_TB_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void pnSELL_Paint(object sender, PaintEventArgs e)
		{

		}

		private void SellRes_btnSelectAllFood_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < SellRes_gridCityList.Rows.Count; i++)
			{
				SellRes_gridCityList.Rows[i].Cells[2].Value = bantaiNguyenLuaCheckAll;
			}
			bantaiNguyenLuaCheckAll = !bantaiNguyenLuaCheckAll;
		}

		private void SellRes_btnSelectAllWoods_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < SellRes_gridCityList.Rows.Count; i++)
			{
				SellRes_gridCityList.Rows[i].Cells[3].Value = bantaiNguyenGoCheckAll;				
			}
			bantaiNguyenGoCheckAll = !bantaiNguyenGoCheckAll;
		}

		private void SellRes_btnSelectAllIron_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < SellRes_gridCityList.Rows.Count; i++)
			{
				SellRes_gridCityList.Rows[i].Cells[4].Value = bantaiNguyenSatCheckAll;
			}
			bantaiNguyenSatCheckAll = !bantaiNguyenSatCheckAll;
		}

		private void SellRes_btnSelectAllStone_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < SellRes_gridCityList.Rows.Count; i++)
			{
				SellRes_gridCityList.Rows[i].Cells[5].Value = bantaiNguyenDaCheckAll;
			}
			bantaiNguyenDaCheckAll = !bantaiNguyenDaCheckAll;
		}

		private void btMuaTNSelectAllLua_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < dtaBUYRESOURCE.Rows.Count; i++)
			{
				dtaBUYRESOURCE.Rows[i].Cells[2].Value = muataiNguyenLuaCheckAll;
			}
			muataiNguyenLuaCheckAll = !muataiNguyenLuaCheckAll;
		}

		private void btMuaTNSelectAllGo_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < dtaBUYRESOURCE.Rows.Count; i++)
			{
				dtaBUYRESOURCE.Rows[i].Cells[3].Value = muataiNguyenGoCheckAll;
			}
			muataiNguyenGoCheckAll = !muataiNguyenGoCheckAll;
		}

		private void btMuaTNSelectAllSat_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < dtaBUYRESOURCE.Rows.Count; i++)
			{
				dtaBUYRESOURCE.Rows[i].Cells[4].Value = muataiNguyenSatCheckAll;
			}
			muataiNguyenSatCheckAll = !muataiNguyenSatCheckAll;
		}

		private void btMuaTNSelectAllDa_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < dtaBUYRESOURCE.Rows.Count; i++)
			{
				dtaBUYRESOURCE.Rows[i].Cells[5].Value = muataiNguyenDaCheckAll;
			}
			muataiNguyenDaCheckAll = !muataiNguyenDaCheckAll;
		}

		private void tabMainTab_Leave(object sender, EventArgs e)
		{

		}

		private void tabMuaTaiNguyen_Leave(object sender, EventArgs e)
		{
			try
			{

				MuaTaiNguyen.VangAnToan = long.Parse(txtSAFEGOLD.Text);
				MuaTaiNguyen.TimeToRunInMinute = double.Parse(txtBUYRESOURCECHECK.Text);
				MuaTaiNguyen.MuaTheoDonViKho = (rdMuaTNMuaTheoDonVi.Checked);
				MuaTaiNguyen.GiaTri = double.Parse(txtMuaTNGioiHanMua.Text);

				DataTable temp = (DataTable)dtaBUYRESOURCE.DataSource;

				LVAuto.LVForm.Command.CommonObj.MuaTaiNguyenObj.cityInfo_ ctinfo;

				ArrayList arCity = new ArrayList();
				if (temp != null)
				{
					for (int i = 0; i < temp.Rows.Count; i++)
					{
						ctinfo = new LVAuto.LVForm.Command.CommonObj.MuaTaiNguyenObj.cityInfo_();
						ctinfo.CityId = int.Parse((temp.Rows[i]["ID_TT"]).ToString());
						ctinfo.CityName = (temp.Rows[i]["NAME_TT"]).ToString();

						ctinfo.MuaLua = (bool)temp.Rows[i]["BUY_LUA"];
						ctinfo.MuaGo = (bool)temp.Rows[i]["BUY_GO"];
						ctinfo.MuaSat = (bool)temp.Rows[i]["BUY_SAT"];
						ctinfo.MuaDa = (bool)temp.Rows[i]["BUY_DA"];

						if (ctinfo.MuaLua || ctinfo.MuaGo || ctinfo.MuaDa || ctinfo.MuaSat)
							arCity.Add(ctinfo);
					}

					if (arCity.Count > 0)
					{
						MuaTaiNguyen.newCityInfo(arCity.Count);
						for (int i = 0; i < arCity.Count; i++)
							MuaTaiNguyen.CityInfo[i] = (LVAuto.LVForm.Command.CommonObj.MuaTaiNguyenObj.cityInfo_)arCity[i];
						
					}
				}
				
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi sai gì đó ở mua tài nguyên, kiểm tra lại đê", "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Question);

			}
		}

		private void rdMuaTNMuaTheoPhanTram_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void rdMuaTNMuaTheoDonVi_CheckedChanged(object sender, EventArgs e)
		{

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

	

		private void pnSELL_Leave(object sender, EventArgs e)
		{
			try
			{
				BanTaiNguyen.SalesOff = SellRes_checkSelloff.Checked;


				BanTaiNguyen.LUA.BanCoDinh = rdBTNLuaAnToanCoDinh.Checked;
				BanTaiNguyen.LUA.SoLuongBan = int.Parse(txtCOUNTLUA.Text);
				BanTaiNguyen.LUA.SoLuongAnToan = int.Parse(txtSAFELUA.Text);
				
				if (rdBanTN_LUA_CODINH.Checked) BanTaiNguyen.LUA.LoaiBan = 1; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				if (rdBanTN_LUA_TRUNGBINH.Checked) BanTaiNguyen.LUA.LoaiBan = 2; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				if (rdBanTN_LUA_THAPNHAT.Checked) BanTaiNguyen.LUA.LoaiBan = 3; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				BanTaiNguyen.LUA.CongThucNhan = rdBanTN_LUA_TB_Nhan.Checked;
				BanTaiNguyen.LUA.GiaTri = double.Parse(txtBanTN_LUA_TB_Heso.Text);

				BanTaiNguyen.GO.BanCoDinh = rdBTNGoAnToanCoDinh.Checked;
				BanTaiNguyen.GO.SoLuongBan = int.Parse(txtCOUNTGO.Text);
				BanTaiNguyen.GO.SoLuongAnToan = int.Parse(txtSAFEGO.Text);
				if (rdBanTN_GO_CODINH.Checked) BanTaiNguyen.GO.LoaiBan = 1; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				if (rdBanTN_GO_TRUNGBINH.Checked) BanTaiNguyen.GO.LoaiBan = 2; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				if (rdBanTN_GO_THAPNHAT.Checked) BanTaiNguyen.GO.LoaiBan = 3; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				BanTaiNguyen.GO.CongThucNhan = rdBanTN_GO_TB_Nhan.Checked;
				BanTaiNguyen.GO.GiaTri = double.Parse(txtBanTN_GO_TB_Heso.Text);

				BanTaiNguyen.SAT.BanCoDinh = rdBTNSatAnToanCoDinh.Checked;
				BanTaiNguyen.SAT.SoLuongBan = int.Parse(txtCOUNTSAT.Text);
				BanTaiNguyen.SAT.SoLuongAnToan = int.Parse(txtSAFESAT.Text);
				if (rdBanTN_SAT_CODINH.Checked) BanTaiNguyen.SAT.LoaiBan = 1; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				if (rdBanTN_SAT_TRUNGBINH.Checked) BanTaiNguyen.SAT.LoaiBan = 2; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				if (rdBanTN_SAT_THAPNHAT.Checked) BanTaiNguyen.SAT.LoaiBan = 3; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				BanTaiNguyen.SAT.CongThucNhan = rdBanTN_SAT_TB_Nhan.Checked;
				BanTaiNguyen.SAT.GiaTri = double.Parse(txtBanTN_SAT_TB_Heso.Text);

				BanTaiNguyen.DA.BanCoDinh = rdBTNDaAnToanCoDinh.Checked;
				BanTaiNguyen.DA.SoLuongBan = int.Parse(txtCOUNTDA.Text);
				BanTaiNguyen.DA.SoLuongAnToan = int.Parse(txtSAFEDA.Text);
				if (rdBanTN_DA_CODINH.Checked) BanTaiNguyen.DA.LoaiBan = 1; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				if (rdBanTN_DA_TRUNGBINH.Checked) BanTaiNguyen.DA.LoaiBan = 2; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				if (rdBanTN_DA_THAPNHAT.Checked) BanTaiNguyen.DA.LoaiBan = 3; // BanTaiNguyen.LOAIBAN.THEOGIACODINH;
				BanTaiNguyen.DA.CongThucNhan = rdBanTN_DA_TB_Nhan.Checked;
				BanTaiNguyen.DA.GiaTri = double.Parse(txtBanTN_DA_TB_Heso.Text);


				DataTable temp = (DataTable)SellRes_gridCityList.DataSource;

				LVAuto.LVForm.Command.CommonObj.BanTaiNguyenObj.cityInfo_ ctinfo;

				ArrayList arCity = new ArrayList();

				for (int i = 0; i < temp.Rows.Count; i++)
				{
					ctinfo = new LVAuto.LVForm.Command.CommonObj.BanTaiNguyenObj.cityInfo_();
					ctinfo.CityId = int.Parse((temp.Rows[i]["ID_TT"]).ToString());
					ctinfo.CityName = (temp.Rows[i]["NAME_TT"]).ToString();

					ctinfo.BanLua = (bool)temp.Rows[i]["SELL_LUA"];
					ctinfo.BanGo = (bool)temp.Rows[i]["SELL_GO"];
					ctinfo.BanDa = (bool)temp.Rows[i]["SELL_DA"];
					ctinfo.BanSat = (bool)temp.Rows[i]["SELL_SAT"];

					if (ctinfo.BanLua || ctinfo.BanGo || ctinfo.BanDa || ctinfo.BanSat)
						arCity.Add(ctinfo);
					//BanTaiNguyen.CityInfo[i] = ctinfo;
				}

				if (arCity.Count > 0)
				{
					BanTaiNguyen.NewCityInfo(arCity.Count);
					for (int i = 0; i < arCity.Count; i++)
						BanTaiNguyen.CityInfo[i] = (LVAuto.LVForm.Command.CommonObj.BanTaiNguyenObj.cityInfo_)arCity[i];

					//BanTaiNguyen.CityInfo = (LVAuto.Command.CommonObj.BanTaiNguyenObj.cityInfo_[]) arCity.ToArray();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi sai gì đó ở bán tài nguyên, kiểm tra lại đê", "Lỗi rồi", MessageBoxButtons.OK, MessageBoxIcon.Question);
				
			}
		}

		private void txtBanTN_DA_TB_Heso_TextChanged(object sender, EventArgs e)
		{

		}

		private void rdBanTN_DA_TB_Nhan_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void rdBanTN_DA_TB_Cong_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void rdBanTN_SAT_TB_Nhan_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void rdBanTN_SAT_TB_Cong_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void rdBanTN_GO_TB_Nhan_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void rdBanTN_GO_TB_Cong_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void rdBanTN_LUA_TB_Nhan_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void rdBanTN_LUA_TB_Cong_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void txtBanTN_LUA_TB_Heso_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtBanTN_GO_TB_Heso_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtBanTN_SAT_TB_Heso_TextChanged(object sender, EventArgs e)
		{

		}

		private void rdBanTN_GO_CODINH_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_GO_CODINH.Checked)
			{
				rdBanTN_GO_TB_Cong.Enabled = false;
				rdBanTN_GO_TB_Nhan.Enabled = false;
			}
			else
			{
				rdBanTN_GO_TB_Cong.Enabled = true;
				rdBanTN_GO_TB_Nhan.Enabled = true;
			}
		}

		private void rdBanTN_LUA_CODINH_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_LUA_CODINH.Checked)
			{
				rdBanTN_LUA_TB_Cong.Enabled = false;
				rdBanTN_LUA_TB_Nhan.Enabled = false;
			}
			else
			{
				rdBanTN_LUA_TB_Cong.Enabled = true;
				rdBanTN_LUA_TB_Nhan.Enabled = true;
			}
		}

		private void rdBanTN_LUA_TRUNGBINH_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_LUA_TRUNGBINH.Checked)
			{
				rdBanTN_LUA_TB_Cong.Enabled = true;
				rdBanTN_LUA_TB_Nhan.Enabled = true;
			}
			else
			{
				rdBanTN_LUA_TB_Cong.Enabled = false;
				rdBanTN_LUA_TB_Nhan.Enabled = false;
			}
		}

		private void rdBanTN_LUA_THAPNHAT_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_LUA_THAPNHAT.Checked)
			{
				rdBanTN_LUA_TB_Cong.Enabled = true;
				rdBanTN_LUA_TB_Nhan.Enabled = true;
			}
			else
			{
				rdBanTN_LUA_TB_Cong.Enabled = false;
				rdBanTN_LUA_TB_Nhan.Enabled = false;
			}
		}

		private void rdBanTN_GO_TRUNGBINH_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_GO_TRUNGBINH.Checked)
			{
				rdBanTN_GO_TB_Cong.Enabled = true;
				rdBanTN_GO_TB_Nhan.Enabled = true;
			}
			else
			{
				rdBanTN_GO_TB_Cong.Enabled = false;
				rdBanTN_GO_TB_Nhan.Enabled = false;
			}
		}

		private void rdBanTN_GO_THAPNHAT_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_GO_THAPNHAT.Checked)
			{
				rdBanTN_GO_TB_Cong.Enabled = true;
				rdBanTN_GO_TB_Nhan.Enabled = true;
			}
			else
			{
				rdBanTN_GO_TB_Cong.Enabled = false;
				rdBanTN_GO_TB_Nhan.Enabled = false;
			}
		}

		private void rdBanTN_SAT_CODINH_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_SAT_CODINH.Checked)
			{
				rdBanTN_SAT_TB_Cong.Enabled = false;
				rdBanTN_SAT_TB_Nhan.Enabled = false;
			}
			else
			{
				rdBanTN_SAT_TB_Cong.Enabled = true;
				rdBanTN_SAT_TB_Nhan.Enabled = true;
			}
		}

		private void rdBanTN_SAT_TRUNGBINH_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_SAT_TRUNGBINH.Checked)
			{
				rdBanTN_SAT_TB_Cong.Enabled = true;
				rdBanTN_SAT_TB_Nhan.Enabled = true;
			}
			else
			{
				rdBanTN_SAT_TB_Cong.Enabled = false;
				rdBanTN_SAT_TB_Nhan.Enabled = false;
			}
		}

		private void rdBanTN_SAT_THAPNHAT_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_SAT_THAPNHAT.Checked)
			{
				rdBanTN_SAT_TB_Cong.Enabled = true;
				rdBanTN_SAT_TB_Nhan.Enabled = true;
			}
			else
			{
				rdBanTN_SAT_TB_Cong.Enabled = false;
				rdBanTN_SAT_TB_Nhan.Enabled = false;
			}
		}

		private void rdBanTN_DA_CODINH_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_DA_CODINH.Checked)
			{
				rdBanTN_DA_TB_Cong.Enabled = false;
				rdBanTN_DA_TB_Nhan.Enabled = false;
			}
			else
			{
				rdBanTN_DA_TB_Cong.Enabled = true;
				rdBanTN_DA_TB_Nhan.Enabled = true;
			}
		}

		private void rdBanTN_DA_TRUNGBINH_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_DA_TRUNGBINH.Checked)
			{
				rdBanTN_DA_TB_Cong.Enabled = true;
				rdBanTN_DA_TB_Nhan.Enabled = true;
			}
			else
			{
				rdBanTN_DA_TB_Cong.Enabled = false;
				rdBanTN_DA_TB_Nhan.Enabled = false;
			}
		}

		private void rdBanTN_DA_THAPNHAT_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBanTN_DA_THAPNHAT.Checked)
			{
				rdBanTN_DA_TB_Cong.Enabled = true;
				rdBanTN_DA_TB_Nhan.Enabled = true;
			}
			else
			{
				rdBanTN_DA_TB_Cong.Enabled = false;
				rdBanTN_DA_TB_Nhan.Enabled = false;
			}
		}

		

		private void chkHaNhaDelAll_CheckedChanged(object sender, EventArgs e)
		{
			Command.CityObj.City.isDowndAll = chkHaNhaDelAll.Checked;
			cboTabHaNhaCity.Enabled = !chkHaNhaDelAll.Checked;
			tvDEL.Enabled = !chkHaNhaDelAll.Checked;			
		}

		private void pnXayNha_Paint(object sender, PaintEventArgs e)
		{

		}

		private void chkXayNhaAll_CheckedChanged(object sender, EventArgs e)
		{
			Construct_dropdownCityList.Enabled = !chkXayNhaAll.Checked;
			Construct_treeBuilding.Enabled = !chkXayNhaAll.Checked;
			Construct_btnReloadBuildings.Enabled = !chkXayNhaAll.Checked;

			Command.CityObj.City.isBuildAll = chkXayNhaAll.Checked;	
		}

		private void chkXayNha_TuMuaTaiNguyen_CheckedChanged(object sender, EventArgs e)
		{
			txtXayNha_VangAnToan.Enabled = chkXayNha_TuMuaTaiNguyen.Checked;
		}

		private void pnXayNha_Leave(object sender, EventArgs e)
		{
			try
			{

				Command.CityObj.City.isBuyRes = chkXayNha_TuMuaTaiNguyen.Checked;
				if (chkXayNha_TuMuaTaiNguyen.Checked) Command.CityObj.City.goldSafe = long.Parse(txtXayNha_VangAnToan.Text);

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
						Command.CityObj.City.AllCity[cityPost].AllBuilding[i].isUp = root.Nodes[i].Checked;
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

		private void label21_Click(object sender, EventArgs e)
		{

		}

		private void chkANUI_TuMuaLua_CheckedChanged(object sender, EventArgs e)
		{
			txtANUI_VangAnToan.Enabled = chkANUI_TuMuaLua.Checked;
		}

		private void tvBUILD_Leave_1(object sender, EventArgs e)
		{
			try
			{

				Command.CityObj.City.isBuyRes = chkXayNha_TuMuaTaiNguyen.Checked;
				if (chkXayNha_TuMuaTaiNguyen.Checked) Command.CityObj.City.goldSafe = long.Parse(txtXayNha_VangAnToan.Text);

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
						Command.CityObj.City.AllCity[cityPost].AllBuilding[i].isUp = root.Nodes[i].Checked;
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
				LVAuto.LVForm.Command.CityObj.City city;
				for (int i = 0; i < LVAuto.LVForm.Command.CityObj.City.AllCity.Length; i++)
				{
					city = LVAuto.LVForm.Command.CityObj.City.AllCity[i];
					if (city.id > 0)
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

		/*private void btTienIchRuongGetData_Click(object sender, EventArgs e)
		{
			try
			{
				btTienIchRuongGetData.Enabled = false;

				//http://s3.linhvuong.zooz.vn/GateWay/Common.ashx?id=2&0.6633130316506091&nid=0

				Hashtable result = Command.Common.Execute(2, "nid=0", false);

				//string str = "{Table:[{gem_id:1,counts:259},{gem_id:2,counts:265},{gem_id:3,counts:84},{gem_id:6,counts:93},{gem_id:7,counts:6},{gem_id:8,counts:10},{gem_id:11,counts:8},{gem_id:12,counts:15},{gem_id:13,counts:3},{gem_id:16,counts:261},{gem_id:17,counts:196},{gem_id:21,counts:54},{gem_id:24,counts:141},{gem_id:25,counts:162},{gem_id:35,counts:6},{gem_id:50,counts:2}],ret:0,nid:0}";

				if (result == null)
				{
					MessageBox.Show(" Lỗi rồi, thử làm lại xem sao nha");
					return;
				}


				string str = result["DATA"].ToString().Trim();
				if (!str.Contains("ret:0"))
				{
					MessageBox.Show(" Lỗi rồi, thử làm lại xem sao nha");
					return;
				}


				string s1 = str.Substring(str.IndexOf("[") + 1);
				s1 = s1.Substring(0, s1.IndexOf("]"));
				string[] ar = s1.Split(new char[] { ',' }, StringSplitOptions.None);

				QuocKho_Ruong.Clear();
				cbTienIchChonRuong.Items.Clear();
				cbTienIchMoRuongRuongChonThanh.Items.Clear();
				int i = 0;
				int key;
				string value;
				Common.Ruong ruong = new LVAuto.Common.Ruong();
				while (i < ar.Length)
				{
					key = int.Parse(ar[i].Substring(ar[i].IndexOf(":") + 1).Trim());
					i++;
					value = ar[i].Substring(ar[i].IndexOf(":") + 1).Trim();
					value = value.Substring(0, value.Length - 1);
					i++;
					QuocKho_Ruong.Add(key, value);
					cbTienIchChonRuong.Items.Add(key + ". " + ruong.GetRuongName(key));
				}

				cbTienIchChonRuong.Enabled = true;
				cbTienIchMoRuongRuongChonThanh.Enabled = true;
				txtTienIchRuongSoluong.Enabled = true;
				LVAuto.Command.CityObj.City city;
				for (i = 0; i < LVAuto.Command.CityObj.City.AllCity.Length; i++)
				{
					city = LVAuto.Command.CityObj.City.AllCity[i];
					if (city.id > 0)
					{

						cbTienIchMoRuongRuongChonThanh.Items.Add(city);
					}
				}


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
		*/


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
				LVAuto.LVForm.Command.CityObj.City city = (LVAuto.LVForm.Command.CityObj.City)cbTienIchMoRuongRuongChonThanh.SelectedItem;
				int cityid = city.id;
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
				int sourceCityID;		// = ((Command.CityObj.City)cbVCVKThanhDi.SelectedItem).id;
				int destCityID;		
				int loaivkid;	// = ((Common.Wepons)cbVCVKLoaiVK.SelectedItem).id;
				int tongsoluong = int.Parse(txtVCVKTongSoLuong.Text);
				int soluong = int.Parse(txtVCVKSoLuong.Text);

				Command.CityObj.City destCity;
				for (int i = 0; i < chklVCVKThanhDi.CheckedItems.Count; i++)
				{
					sourceCityID = ((Command.CityObj.City)chklVCVKThanhDi.CheckedItems[i]).id;
					for (int j = 0; j < chklVCVKThanhDen.CheckedItems.Count; j++)
					{
						destCityID = ((Command.CityObj.City)chklVCVKThanhDen.CheckedItems[j]).id;
						for (int k = 0; k < chklVCVKLoaiVuKhi.CheckedItems.Count; k++)
						{
							loaivkid = ((LVCommon.Wepons)chklVCVKLoaiVuKhi.CheckedItems[k]).id;
							VanChuyenVuKhi.Add(new Command.CommonObj.VanChuyenVK(sourceCityID, destCityID, loaivkid, tongsoluong,soluong));
						}

					}
					//destCity = (Command.CityObj.City)chklVCVKThanhDen.CheckedItems[i];
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

		private void label106_Click(object sender, EventArgs e)
		{

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

		private void cboThaoPhatSLTuongDanh1Dich_SelectedIndexChanged(object sender, EventArgs e)
		{
			/*
			int sotuongdanh1dich= cboThaoPhatSLTuongDanh1Dich.SelectedIndex;
			if (sotuongdanh1dich > 0)
			{
				cboThaoPhatPhuongThucTanCong.Enabled = true;
			}
			else
			{
				cboThaoPhatPhuongThucTanCong.Enabled = false;
			}
			 */ 
		}

		private void label87_Click(object sender, EventArgs e)
		{

		}

	

		private void rdBTNLuaAnToanCoDinh_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBTNLuaAnToanCoDinh.Checked)
			{
				lblBTNLuaDVSoluong.Text = "tấn";
				lblBTNLuaDVAnToan.Text = "tấn";
				txtCOUNTLUA.Text = BanTNSoLuongBanCoDinhDefault.ToString(); ;
				txtSAFELUA.Text = BanTNAnToanCoDinhDefault.ToString();
			}
			else
			{
				lblBTNLuaDVSoluong.Text = "% kho";
				lblBTNLuaDVAnToan.Text = "% kho";
				txtCOUNTLUA.Text = BanTNSoLuongBanPercentDefault.ToString(); ;
				txtSAFELUA.Text = BanTNAnToanPercentDefault.ToString();

			}
		}

		private void rdBTNLuaPercentCoDinh_CheckedChanged(object sender, EventArgs e)
		{
			if (!rdBTNLuaPercentCoDinh.Checked)
			{
				lblBTNLuaDVSoluong.Text = "tấn";
				lblBTNLuaDVAnToan.Text = "tấn";
			}
			else
			{
				lblBTNLuaDVSoluong.Text = "% kho";
				lblBTNLuaDVAnToan.Text = "% kho";

			}
		}

		private void rdBTNGoAnToanCoDinh_CheckedChanged(object sender, EventArgs e)
		{
			if (rdBTNGoAnToanCoDinh.Checked)
			{
				lblBTNGoDVSoluong.Text = "tấn";
				lblBTNGoDVAnToan.Text = "tấn";
				txtCOUNTGO.Text = BanTNSoLuongBanCoDinhDefault.ToString(); ;
				txtSAFEGO.Text = BanTNAnToanCoDinhDefault.ToString();
			}
			else
			{
				lblBTNGoDVSoluong.Text = "% kho";
				lblBTNGoDVAnToan.Text = "% kho";
				txtCOUNTGO.Text = BanTNSoLuongBanPercentDefault.ToString(); ;
				txtSAFEGO.Text = BanTNAnToanPercentDefault.ToString();

			}
		}

		private void rdBTNGoPercentCoDinh_CheckedChanged(object sender, EventArgs e)
		{
			if (!rdBTNGoPercentCoDinh.Checked)
			{
				lblBTNGoDVSoluong.Text = "tấn";
				lblBTNGoDVAnToan.Text = "tấn";
			}
			else
			{
				lblBTNGoDVSoluong.Text = "% kho";
				lblBTNGoDVAnToan.Text = "% kho";

			}
		}

		private void rdBTNSatAnToanCoDinh_CheckedChanged(object sender, EventArgs e)
		{

			if (rdBTNSatAnToanCoDinh.Checked)
			{
				lblBTNSatDVSoluong.Text = "tấn";
				lblBTNSatDVAnToan.Text = "tấn";
				txtCOUNTSAT.Text = BanTNSoLuongBanCoDinhDefault.ToString(); ;
				txtSAFESAT.Text = BanTNAnToanCoDinhDefault.ToString();
			}
			else
			{
				lblBTNSatDVSoluong.Text = "% kho";
				lblBTNSatDVAnToan.Text = "% kho";
				txtCOUNTSAT.Text = BanTNSoLuongBanPercentDefault.ToString(); ;
				txtSAFESAT.Text = BanTNAnToanPercentDefault.ToString();

			}
		}

		private void rdBTNDaAnToanCoDinh_CheckedChanged(object sender, EventArgs e)
		{

			if (rdBTNDaAnToanCoDinh.Checked)
			{
				lblBTNDaDVSoluong.Text = "tấn";
				lblBTNDaDVAnToan.Text = "tấn";
				txtCOUNTDA.Text = BanTNSoLuongBanCoDinhDefault.ToString(); ;
				txtSAFEDA.Text = BanTNAnToanCoDinhDefault.ToString();
			}
			else
			{
				lblBTNDaDVSoluong.Text = "% kho";
				lblBTNDaDVAnToan.Text = "% kho";
				txtCOUNTDA.Text = BanTNSoLuongBanPercentDefault.ToString(); ;
				txtSAFEDA.Text = BanTNAnToanPercentDefault.ToString();

			}
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

		private void lblPanelLoading_Click(object sender, EventArgs e)
		{

		}

		private void txtXacNhanAnhMaxCheck_TextChanged(object sender, EventArgs e)
		{

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

		private void txtXacNhanAnhMinTimeCheck_TextChanged(object sender, EventArgs e)
		{

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

		private void txtThaoPhatTongQuanMin_TextChanged(object sender, EventArgs e)
		{
			
		}

		private void chkAutoBanThuong_CheckedChanged(object sender, EventArgs e)
		{
			TuBanThuongDoDe = chkAutoBanThuong.Checked;
		}

		private void pnTHAOPHAT_Paint(object sender, PaintEventArgs e)
		{

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

		private void tabBinhMan_Click(object sender, EventArgs e)
		{

		}

		private void cbTabBinhManThanhXuatQuan_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbTabBinhManThanhXuatQuan.SelectedItem == null) return;

			chkTabBinhManTuongXuatTran.Items.Clear();
			LVAuto.LVForm.Command.CityObj.City city = (LVAuto.LVForm.Command.CityObj.City ) cbTabBinhManThanhXuatQuan.SelectedItem ;
			
			
			if (city.MilitaryGeneral == null) city.MilitaryGeneral = LVAuto.LVForm.Command.Common.GetAllSimpleMilitaryGeneralInfo(city.id);

			if (city.MilitaryGeneral == null)
			{
				MessageBox.Show("Lỗi rồi, mạng lởm quá, chờ tý rồi thử lại đê.", "Mạng lởm quá");
				return;
			}

			chkTabBinhManTuongXuatTran.Items.AddRange(city.MilitaryGeneral);
		}

		private void btTabBinhManXacNhan_Click(object sender, EventArgs e)
		{

			try
			{
				int num10;
				int selectedIndex = this.cbTabBinhManThanhXuatQuan.SelectedIndex;
				int id = LVAuto.LVForm.Command.CityObj.City.AllCity[selectedIndex].id;

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
                            obj2.CityName = LVAuto.LVForm.Command.CityObj.City.AllCity[selectedIndex].name;
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
                        obj2.CityName = LVAuto.LVForm.Command.CityObj.City.AllCity[selectedIndex].name;
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

		private void chkSKST_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void groupBox19_Enter(object sender, EventArgs e)
		{

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

		private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void textBox4_TextChanged(object sender, EventArgs e)
		{

		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void chkTabBinhManTuongXuatTran_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		

		private void cboCallManThanhTraiXuatQuan_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboCallManThanhTraiXuatQuan.SelectedItem == null) return;

			chklbCallManTuongXuatTran.Items.Clear();
			LVAuto.LVForm.Command.CityObj.City city = (LVAuto.LVForm.Command.CityObj.City)cboCallManThanhTraiXuatQuan.SelectedItem;


			if (city.MilitaryGeneral == null) city.MilitaryGeneral = LVAuto.LVForm.Command.Common.GetAllSimpleMilitaryGeneralInfo(city.id);

			if (city.MilitaryGeneral == null)
			{
				MessageBox.Show("Lỗi rồi, mạng lởm quá, chờ tý rồi thử lại đê.", "Mạng lởm quá");
				return;
			}

			chklbCallManTuongXuatTran.Items.AddRange(city.MilitaryGeneral);
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

		private void cbCallManBienCheQuanGioiCungThu3_SelectedIndexChanged(object sender, EventArgs e)
		{

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

					TuongDiDanhMan.CityId = ((LVAuto.LVForm.Command.CityObj.City)cboCallManThanhTraiXuatQuan.SelectedItem).id;
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

					Command.CityObj.MilitaryGeneral gSelect = (Command.CityObj.MilitaryGeneral)chklbCallManTuongXuatTran.CheckedItems[idx];

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

		private void tabMainTab_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void label103_Click(object sender, EventArgs e)
		{

		}

		private void textBox10_TextChanged(object sender, EventArgs e)
		{

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

        private void SellRes_btnReloadCities_Click(object sender, EventArgs e)
        {
            LVReloadCitesForSellResources();
        }		
	} //end class
} //end namespace

	


