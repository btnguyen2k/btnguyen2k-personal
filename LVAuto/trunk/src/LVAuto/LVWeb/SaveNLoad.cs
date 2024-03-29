using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Data;

namespace LVAuto.LVWeb
{
	public class SaveNLoad
	{
		public  struct CodeSaveTask
		{
			public const string XayNha		 = "0";
			public const string HaNha		 = "1";
			public const string VanChuyen	 = "2";
			public const string MuaVuKhi	 = "3";
			public const string LuyenSyKhi	 = "4";
			public const string BienChe		 = "5";
			public const string DieuPhai	 = "6";
			public const string ThaoPhat	 = "7";
			public const string AnUi		 = "8";
			public const string Upgrade		 = "9";
			public const string SellResource = "10";
			public const string BuyResource  = "11";
			public const string SellResourceHeader = "12";
			public const string VanChuyenVuKhi = "13";
			public const string BuyResourceHeader = "14";
			public const string XayNhaHeader = "15";
			public const string BinhMan = "16";
			public const string CallMan = "17";

		}

		private const  string spareChar = ";";
		string error = "";

		public virtual void saveConfig(string filename)
		{
			string data = "";
			string error = "";
			 try 
			 {
				 System.IO.File.Delete(filename);
            } catch (Exception ex) 
			 {            
			}
			try
			{
				//FileStream f = new FileStream(saveFileDialog1.FileName, FileMode.CreateNew);

				LVAuto.LVObj.City city;
				LVAuto.LVObj.Building building;

				string buldupdata = "";
				string bulddowndata = "";


				//luu xay nha
				if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities != null)
				{
					error = "xây nhà hoặc hạ nhà";
					for (int i = 0; i < LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
					{
						city = LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i];
						if (city.AllBuildings != null)
						{
							for (int j = 0; j < city.AllBuildings.Length; j++)
							{
								building = city.AllBuildings[j];
								// xay nha
								if (building.IsUp)
								{
									buldupdata += CodeSaveTask.XayNha + spareChar;

									buldupdata += LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_txtTimer.Text + spareChar; 				//thoigian
									buldupdata += city.Id + spareChar;
									buldupdata += city.Name + spareChar;
									buldupdata += building.Name + spareChar;
									buldupdata += building.GId + spareChar;
									buldupdata += building.PId + spareChar;
									buldupdata += "\r\n";
								}
								// ha nha
								if (building.IsDown)
								{
									bulddowndata += CodeSaveTask.HaNha + spareChar;

									bulddowndata += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtDELCHECK.Text + spareChar; 				//thoigian
									bulddowndata += city.Id			+ spareChar; 
									bulddowndata += city.Name		+ spareChar;
									bulddowndata += building.Name	+ spareChar; 
									bulddowndata += building.GId	+ spareChar; 
									bulddowndata +=  building.PId	+ spareChar ;
									bulddowndata += "\r\n";
								}


							}
							
						}

					}
				}
			


				data = buldupdata + bulddowndata;


				// lưu xây nhà header
				error = "xây nhà ";
				data += CodeSaveTask.XayNhaHeader + spareChar;
				data += LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_txtTimer.Text + spareChar; //time;
				data += LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_checkUpgradeAll.Checked.ToString() + spareChar;
				data += LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_checkAutoBuyResources.Checked.ToString() + spareChar;
				data += LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_txtGoldThreshold.Text + spareChar;
				data += "\r\n";

				// luu van chuyen			
				LVAuto.LVForm.Command.OPTObj.Vanchuyen vc;
				foreach (object obj in LVAuto.LVForm.FrmMain.LVFRMMAIN.pnVanchuyen.Controls)
				{
					error = "vận chuyển";

					vc = (LVAuto.LVForm.Command.OPTObj.Vanchuyen)obj;
					if (vc.chkOK.Checked)
					{
						data += CodeSaveTask.VanChuyen + spareChar;
						data += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKVANCHUYEN.Text + spareChar;// thoigian

						data += ((LVAuto.LVObj.City)vc.cboSource.SelectedItem).Id + spareChar;	// source id
						data += ((LVAuto.LVObj.City)vc.cboSource.SelectedItem).Name + spareChar;	// source name
						data += ((LVAuto.LVObj.City)vc.cboDesc.SelectedItem).Id + spareChar;	// desc id
						data += ((LVAuto.LVObj.City)vc.cboDesc.SelectedItem).Name + spareChar;	// desc name

						data += vc.txtFOOD.Text + spareChar;		//lua
						data += vc.txtWOOD.Text + spareChar;
						data += vc.txtIRON.Text + spareChar;
						data += vc.txtSTONE.Text + spareChar;
						data += vc.txtMONEY.Text + spareChar;
						data += "\r\n";
					}
				}


				// luu mua vu khi
				LVAuto.LVForm.Command.OPTObj.Weapon vk;
				foreach (object objvk in LVAuto.LVForm.FrmMain.LVFRMMAIN.pnWepon.Controls)
				{
					error = "mua vũ khí";

					vk = (LVAuto.LVForm.Command.OPTObj.Weapon)objvk;
					if (vk.chkOK.Checked)
					{
						data += CodeSaveTask.MuaVuKhi + spareChar;
						data += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKWEPON.Text + spareChar;// thoigian
						data += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCOUNTWEPON.Text + spareChar;// so luong mua
						data += vk.cityid + spareChar;												// 
						data += (LVAuto.LVForm.Command.City.GetCityByID(vk.cityid)).Name + spareChar;	// 
						data += vk.cboWepon.SelectedIndex	+ spareChar;
						data += vk.cboAmor.SelectedIndex	+ spareChar;
						data += vk.cboHorse.SelectedIndex	+ spareChar;
						
						data += vk.txtAmount.Text	+ spareChar;
						
						data += vk.cboLevel.SelectedIndex	+ spareChar;
						data += vk.posid_w + spareChar;
						data += vk.posid_a + spareChar;
						data += vk.posid_h + spareChar;
					
						data += "\r\n";
					}
					
				}

				// luyen sy khi  * root.Nodes.Add(onecity.id + "." + onegeneral.id,   onecity.name + " - " + onegeneral.name );
				TreeNode root = LVAuto.LVForm.FrmMain.LVFRMMAIN.tvSIKHI.Nodes["root"];
				TreeNode g;
				string[] ginfo;
				for (int i = 0; i < root.Nodes.Count; i++)
				{
					error = "luyện sĩ khí";

					g = root.Nodes[i];
					if (g.Checked)
					{
						data += CodeSaveTask.LuyenSyKhi + spareChar;
						data += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKSIKHI.Text + spareChar;		// thoi gian
						data += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtOneSiKhi.Text + spareChar;		// mooi lan nang
						
						ginfo = g.Name.Split(new char[] { '.' });

						data += ginfo[0] + spareChar;				// cityid
						data += (LVAuto.LVForm.Command.City.GetCityByID(int.Parse(ginfo[0]))).Name + spareChar;
						data += ginfo[1] + spareChar;								// Generalid
						data += g.Text +spareChar;
						data += "\r\n";
					}
				}


				// Bien che
				LVAuto.LVForm.LVCommon.BienChe oneBienChe;
				for (int i = 0; i < LVAuto.LVForm.FrmMain.ListBienChe.Count; i++)
				{
					error = "biên chế quân";

					oneBienChe = (LVAuto.LVForm.LVCommon.BienChe)LVAuto.LVForm.FrmMain.ListBienChe[i];

					data += CodeSaveTask.BienChe + spareChar;
					data += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtTabBienCheTimeCheck.Text + spareChar;		// thoi gian
					data += oneBienChe.nangsykhi.ToString() + spareChar; ;

					data += oneBienChe.cityid + spareChar;		// cityid
					data += oneBienChe.cityname + spareChar;		// cityid

					data += oneBienChe.generalid + spareChar;		// cityid
					data += oneBienChe.generalname + spareChar;		// cityid

					data += oneBienChe.bobinhamount + spareChar;		
					data += oneBienChe.kybinhamount + spareChar;		
					data += oneBienChe.cungthuamount + spareChar;
					data += oneBienChe.xemount  + spareChar;
					data += "\r\n";
				}

				// luu quan dieu phai
				LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong g1;
				error = "điều phái";
				for (int i =0; i < LVAuto.LVForm.FrmMain.DieuPhaiQuanVanVo.Count; i++)
				{
					g1 = ((LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong)LVAuto.LVForm.FrmMain.DieuPhaiQuanVanVo[i]);
					data += CodeSaveTask.DieuPhai+ spareChar;
					data += g1.cityID + spareChar;
					data += g1.cityname + spareChar;
					data += g1.desc + spareChar;
					data += g1.generalid + spareChar;
					data += g1.generalname + spareChar;
					data += g1.generaltype + spareChar;
					data += g1.ID + spareChar;
					data += g1.loidailevel + spareChar;
					data += g1.tasktype + spareChar;
					data += g1.timetoruninmilisecond + spareChar;
					data += g1.X + spareChar;
					data += g1.Y + spareChar;
					data += "\r\n";
				}

				// luu thao phat


				for (int i = 0; i < LVAuto.LVForm.FrmMain.TuongDiThaoPhatList.Count; i++ )
				{
					error = "thảo phạt";

					LVAuto.LVForm.LVCommon.GeneralThaoPhat gen = (LVAuto.LVForm.LVCommon.GeneralThaoPhat)LVAuto.LVForm.FrmMain.TuongDiThaoPhatList[i];

					data += CodeSaveTask.ThaoPhat + spareChar;
					data += gen.TimeToRun + spareChar;

					data += gen.Id + spareChar;
					data += gen.Name + spareChar;

					data += gen.CityId + spareChar;
					data += gen.QuestId + spareChar;
					data += gen.SiKhiMinToGo + spareChar;
					data += gen.TuBienCheQuan + spareChar;
					data += gen.TuUpSiKhi + spareChar;
					data += gen.SoLuongQuanMinToGo + spareChar;

					data += gen.AttachNumGenerals + spareChar;
					
					data += gen.PhuongThucTanCongID + spareChar;
					data += gen.PhuongThucTanCongName + spareChar;
					
					data += gen.PhuongThucChonMucTieuID + spareChar;
					data += gen.PhuongThucChonMucTieuName + spareChar;

					data += gen.TuDoiTranHinhKhac.ToString() + spareChar;

					//data += gen.general2 + spareChar;
					//data += gen.general3 + spareChar;
					//data += gen.general4 + spareChar;
					//data += gen.general5 + spareChar;

					data += gen.MuuKeTrongChienTranID + spareChar;
					data += gen.MuuKeTrongChienTranName + spareChar;
					data += gen.TuKhoiPhucTrangThai.ToString() + spareChar;

					data += gen.NumInfantries.ToString() + spareChar;
					data += gen.NumCavalries.ToString() + spareChar;
					data += gen.NumArchers.ToString() + spareChar;
					data += gen.NumCatapults.ToString() + spareChar;



					data += "\r\n";
				}

				// luu an ui				
				for (int i = 0; i < LVAuto.LVForm.FrmMain.AnuiForAuto.Count; i++)
				{
					error = "an ủi";

						data += CodeSaveTask.AnUi + spareChar;
						data += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKANUI.Text + spareChar;		// time
						data += LVAuto.LVForm.FrmMain.AnuiForAuto[i].ToString() + spareChar;		// cityid	
						//data += LVAuto.Command.City.GetCityByID(LVAuto.frmmain.AnuiForAuto[i]).name + spareChar;
						data += "\r\n";
				
				}

				// luu upgrade
				for (int i = 1; i < LVAuto.LVForm.FrmMain.UpgradeForAuto.Count; i++)
				{
					error = "nâng cấp trong đại học điện";

					data += CodeSaveTask.Upgrade + spareChar;
					data += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKUPDATE.Text + spareChar;		// time
					data += LVAuto.LVForm.FrmMain.UpgradeForAuto[0].ToString() + spareChar;		//city ID
					data += LVAuto.LVForm.FrmMain.UpgradeForAuto[i].ToString() + spareChar;		// itemid
					data += "\r\n";
				}


				//luu sell tai nguyen header
				error = "bán tài nguyên";
				data += CodeSaveTask.SellResourceHeader + spareChar;
				data += LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtTimer.Text + spareChar; //time
				


				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.SoLuongBan				+ spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.SoLuongAnToan				+ spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.CongThucNhan.ToString()	+ spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.LoaiBan.ToString()		+ spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.GiaTri					+ spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.BanCoDinh.ToString()		+ spareChar;

				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.SoLuongBan + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.SoLuongAnToan + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.CongThucNhan.ToString() + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.LoaiBan.ToString() + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.GiaTri + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.BanCoDinh.ToString() + spareChar;
				
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.SoLuongBan + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.SoLuongAnToan + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.CongThucNhan.ToString() + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.LoaiBan.ToString() + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.GiaTri + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.BanCoDinh.ToString() + spareChar;

				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.SoLuongBan + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.SoLuongAnToan + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.CongThucNhan.ToString() + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.LoaiBan.ToString() + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.GiaTri + spareChar;
				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.BanCoDinh.ToString() + spareChar;

				data += LVAuto.LVForm.FrmMain.BanTaiNguyen.SalesOff + spareChar; 

				data += "\r\n";


				//luu sell tai nguyen conten
				if (LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo != null)
					for (int i = 0; i < LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo.Length; i++)
					{
						data += CodeSaveTask.SellResource + spareChar;
						data += LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo[i].CityId + spareChar;
						data += LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo[i].CityName + spareChar;
						data += LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo[i].BanDa.ToString() + spareChar;
						data += LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo[i].BanGo.ToString() + spareChar;
						data += LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo[i].BanLua.ToString() + spareChar;
						data += LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo[i].BanSat.ToString() + spareChar;

						data += "\r\n";
					}

				
				//luu buy tài nguyên header
				error = "mua tài nguyên";
				data += CodeSaveTask.BuyResourceHeader + spareChar;
				data += LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtTimer.Text + spareChar; //time
				data += LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtGoldThreshold.Text + spareChar; //vàng an toàn
				data += LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_radioBuyAmountMethodPercent.Checked.ToString() + spareChar; //mua theo %
				data += LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtBuyAmount.Text + spareChar; //giới hạn mua (giá trị)
				data += "\r\n";

				//luu buy
				DataTable dtTable = (DataTable)LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_gridCityList.DataSource;
				if (dtTable != null)
					for (int i = 0; i < dtTable.Rows.Count; i++)
					{
						error = "mua tài nguyên";
						data += CodeSaveTask.BuyResource + spareChar;
						data += LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtTimer.Text + spareChar; //time
						data += LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtGoldThreshold.Text + spareChar;

						data += dtTable.Rows[i]["ID_TT"].ToString() + spareChar;

						data += (bool)dtTable.Rows[i]["BUY_LUA"] + spareChar;
						data += (bool)dtTable.Rows[i]["BUY_GO"] + spareChar;
						data += (bool)dtTable.Rows[i]["BUY_SAT"] + spareChar;
						data += (bool)dtTable.Rows[i]["BUY_DA"] + spareChar;
						data += "\r\n";
					}


				//luu van chuyen vu khi
				if (LVAuto.LVForm.FrmMain.VanChuyenVuKhi != null && LVAuto.LVForm.FrmMain.VanChuyenVuKhi.Count > 0)
				{
					LVAuto.LVForm.Command.CommonObj.VanChuyenVK objVCVK;
					for (int i = 0; i < LVAuto.LVForm.FrmMain.VanChuyenVuKhi.Count; i++)
					{
						error = "vận chuyển vũ khí";
						objVCVK = (LVAuto.LVForm.Command.CommonObj.VanChuyenVK)LVAuto.LVForm.FrmMain.VanChuyenVuKhi[i];

						data += CodeSaveTask.VanChuyenVuKhi+ spareChar;
						data += LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKVANCHUYENVUKHI.Text + spareChar; //time

						data += objVCVK.id + spareChar;
						data += objVCVK.LoaiVuKhiID + spareChar;
						data += objVCVK.ThanhChuyenDiID + spareChar;
						data += objVCVK.ThanhChuyenDenID + spareChar;
						data += objVCVK.TongSoLuongChuyen + spareChar;
						data += objVCVK.DaChuyenDuoc + spareChar;
						data += objVCVK.SoLuongChuyenMoiLan + spareChar;
						
						data += "\r\n";
					}
				}


				// lưu bình man
				error = "bình man";

				for (int i = 0; i < LVAuto.LVForm.FrmMain.BinhManList.Count; i++)
				{

					LVAuto.LVForm.Command.CommonObj.BinhManObj obj2 = (LVAuto.LVForm.Command.CommonObj.BinhManObj)LVAuto.LVForm.FrmMain.BinhManList[i];
					data += CodeSaveTask.BinhMan + spareChar;
					
					data += obj2.ManID + spareChar;
					data += obj2.ManName + spareChar;
					data += obj2.PhuongThucTanCongID + spareChar;
					data += obj2.PhuongThucTanCongName + spareChar;
					data += obj2.PhuongThucChonMucTieuID + spareChar;
					data += obj2.PhuongThucChonMucTieuName + spareChar;
					data += obj2.UuTienID + spareChar;
					data += obj2.UuTienName + spareChar;
					data += obj2.SoLuongQuanMinToGo + spareChar;
					data += obj2.SiKhiMinToGo + spareChar;
					data += obj2.MaxOToGo + spareChar;
					data += obj2.TimeToCheck + spareChar;
					data += obj2.TuUpSiKhi + spareChar;
					data += obj2.Id + spareChar;
					data += obj2.Name + spareChar;
					data += obj2.CityId + spareChar;
					data += obj2.CityName + spareChar;
					data += obj2.CityId + spareChar;
					data += obj2.CityName + spareChar;
					
					data += obj2.Military.Bobinh[0] + spareChar;
					data += obj2.Military.Bobinh[1] + spareChar;
					data += obj2.Military.Bobinh[2] + spareChar;
					data += obj2.Military.Bobinh[3] + spareChar;
					
					data += obj2.Military.KyBinh[0] + spareChar;
					data += obj2.Military.KyBinh[1] + spareChar;
					data += obj2.Military.KyBinh[2] + spareChar;
					data += obj2.Military.KyBinh[3] + spareChar;

					data += obj2.Military.CungThu[0] + spareChar;
					data += obj2.Military.CungThu[1] + spareChar;
					data += obj2.Military.CungThu[2] + spareChar;
					data += obj2.Military.CungThu[3] + spareChar;

					data += obj2.Military.Xe[0] + spareChar;
					data += obj2.Military.Xe[1] + spareChar;
					data += obj2.Military.Xe[2] + spareChar;
					data += obj2.Military.Xe[3] + spareChar;

					data += obj2.MuuKeTrongChienTranID + spareChar;
					data += obj2.MuuKeTrongChienTranName + spareChar;
					data += obj2.TuKhoiPhucTrangThai + spareChar;

                    data += obj2.ManType + spareChar;
                    data += obj2.ToaDoMoX + spareChar;
                    data += obj2.ToaDoMoY + spareChar;

					data += "\r\n";

				}

							
					// lưu call man
				error = "gọi man";

				for (int i = 0; i < LVAuto.LVForm.FrmMain.CallManList.Count; i++)
				{

					LVAuto.LVForm.Command.CommonObj.CallManObj obj2 = (LVAuto.LVForm.Command.CommonObj.CallManObj)LVAuto.LVForm.FrmMain.CallManList[i];
					data += CodeSaveTask.CallMan + spareChar;

					data += obj2.TimeToCheck + spareChar;
					data += obj2.CityId + spareChar;
					data += obj2.CityName + spareChar;
					data += obj2.Id + spareChar;
					data += obj2.Name + spareChar;

					data += obj2.GroupID+ spareChar;
					data += obj2.ToaDoCallVeX + spareChar;
					data += obj2.ToaDoCallVeY + spareChar;

					data += obj2.Military.Bobinh[0] + spareChar;
					data += obj2.Military.Bobinh[1] + spareChar;
					data += obj2.Military.Bobinh[2] + spareChar;
					data += obj2.Military.Bobinh[3] + spareChar;

					data += obj2.Military.KyBinh[0] + spareChar;
					data += obj2.Military.KyBinh[1] + spareChar;
					data += obj2.Military.KyBinh[2] + spareChar;
					data += obj2.Military.KyBinh[3] + spareChar;

					data += obj2.Military.CungThu[0] + spareChar;
					data += obj2.Military.CungThu[1] + spareChar;
					data += obj2.Military.CungThu[2] + spareChar;
					data += obj2.Military.CungThu[3] + spareChar;

					data += obj2.Military.Xe[0] + spareChar;
					data += obj2.Military.Xe[1] + spareChar;
					data += obj2.Military.Xe[2] + spareChar;
					data += obj2.Military.Xe[3] + spareChar;

					data += obj2.SoTuongMinhDanh1TuongDich + spareChar;
					data += obj2.PhuongThucTanCongID + spareChar;
					data += obj2.PhuongThucTanCongName + spareChar;
					data += obj2.PhuongThucChonMucTieuID + spareChar;
					data += obj2.PhuongThucChonMucTieuName + spareChar;
					data += obj2.MuuKeTrongChienTranID + spareChar;
					data += obj2.MuuKeTrongChienTranName + spareChar;

					data += obj2.SoLuongQuanMinToGo + spareChar;
					data += obj2.SiKhiMinToGo + spareChar;
					data += obj2.TuUpSiKhi + spareChar;
					data += obj2.TuKhoiPhucTrangThai + spareChar;
					data += obj2.TuMuaManTocLenh + spareChar;

					for (int j = 0; j < obj2.Mans.Count; j++)
					{
						LVAuto.LVForm.Command.CommonObj.ManOBJ man = (LVAuto.LVForm.Command.CommonObj.ManOBJ)obj2.Mans[j];
						data += man.ManID + spareChar;
						data += man.ManName + spareChar;

					}
					
					data += "\r\n";
				}



				wite2file(filename, data);

			}
			catch (Exception ex)
			{
				if (error != "")
					MessageBox.Show("Có lỗi khi lưu dữ liệu " + error + ". Cần kiểm tra lại.", "Lỗi khi lưu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void showLoadingMsg()
		{
			LVAuto.LVForm.FrmMain.LVFRMMAIN.lblLoadingResMessage.Visible = true;
			LVAuto.LVForm.FrmMain.LVFRMMAIN.lblLoadingResMessage.BringToFront();

		}

		private void hideLoadingMsg()
		{
			//LVAuto.frmmain.LVFRMMAIN.lblLoadingResMessage.Visible = false;
			LVAuto.LVForm.FrmMain.LVFRMMAIN.lblLoadingResMessage.BringToFront();

		}

		private void setTextLoading(string str)
		{
			LVAuto.LVForm.FrmMain.LVFRMMAIN.lblLoadingResMessage.Text = str;
		}

		public void loadConfig(string filename)
		{
            try
            {
                error = "";

                showLoadingMsg();
                LVAuto.LVForm.FrmMain.LVFRMMAIN.SetAllTabsEnable(false);
                setTextLoading("Đang đọc dữ liệu, đợi tý đê ....");
                Application.DoEvents();

                System.Collections.ArrayList ardata = new System.Collections.ArrayList();
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(filename))
                {
                    String line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        ardata.Add(line);
                        Application.DoEvents();

                    }
                }

                if (ardata == null || ardata.Count <= 0) return;

                string str = "";
                string[] dataline;
                string[] separ = new string[] { spareChar };
                string code = "";
                int i;
                int j;
                LVAuto.LVForm.Command.City.GetAllSimpleCity();
                Application.DoEvents();
                //clear data

                for (i = 0; i < LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities.Length; i++)
                {
                    Application.DoEvents();
                    if (LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings != null)
                    {
                        Application.DoEvents();
                        for (j = 0; j < LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings.Length; j++)
                        {
                            Application.DoEvents();

                            LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings[j].IsUp = false;
                            LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[i].AllBuildings[j].IsDown = false;
                        }
                    }
                }

                str = ardata[0].ToString().Trim();
                string preValue = str.Substring(0, str.IndexOf(spareChar, 0));
                System.Collections.Hashtable data = new System.Collections.Hashtable();
                System.Collections.ArrayList subdata = new System.Collections.ArrayList();
                for (i = 0; i < ardata.Count; i++)
                {
                    Application.DoEvents();

                    str = ardata[i].ToString().Trim();
                    if (str == "") continue;
                    if (preValue == str.Substring(0, str.IndexOf(spareChar, 0)))
                    {
                        subdata.Add(str);
                    }
                    else
                    {
                        data.Add(preValue, subdata);
                        subdata = new System.Collections.ArrayList();
                        subdata.Add(str);
                    }
                    preValue = str.Substring(0, str.IndexOf(spareChar, 0));
                }


                data.Add(preValue, subdata);

                string key;
                foreach (object obj in data.Keys)
                {
                    Application.DoEvents();
                    key = obj.ToString().Trim();
                    switch (key)
                    {
                        case CodeSaveTask.XayNha:
                            setTextLoading("Đọc dữ liệu xây nhà....");
                            loadBuildUpDown((ArrayList)data[key], true);
                            break;

                        case CodeSaveTask.XayNhaHeader:
                            setTextLoading("Đọc dữ liệu xây nhà....");
                            loadBuildUpHeader((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.HaNha:
                            setTextLoading("Đọc dữ liệu hạ nhà....");
                            loadBuildUpDown((ArrayList)data[key], false);
                            break;

                        case CodeSaveTask.VanChuyen:
                            setTextLoading("Đọc dữ liệu vận chuyển....");
                            loadVanChuyen((ArrayList)data[key]);
                            break;
                        case CodeSaveTask.MuaVuKhi:
                            setTextLoading("Đọc dữ liệu mua vũ khí....");
                            loadMuaVuKhi((ArrayList)data[key]);
                            break;
                        case CodeSaveTask.LuyenSyKhi:
                            setTextLoading("Đọc dữ liệu luyện sĩ khí....");
                            loadUpSyKhi((ArrayList)data[key]);
                            break;
                        case CodeSaveTask.BienChe:
                            setTextLoading("Đọc dữ liệu biên chế....");
                            loadBienChe((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.ThaoPhat:
                            setTextLoading("Đọc dữ liệu thảo phạt....");
                            loadThaoPhat((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.AnUi:
                            setTextLoading("Đọc dữ liệu an ủi....");
                            loadAnUi((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.DieuPhai:
                            setTextLoading("Đọc dữ liệu điều phái....");
                            loadDieuPhai((ArrayList)data[key]);
                            break;


                        case CodeSaveTask.Upgrade:
                            setTextLoading("Đọc dữ liệu nâng cấp trong đại học điện....");
                            loadUpgrade((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.SellResource:
                            setTextLoading("Đọc dữ liệu bán tài nguyên....");
                            loadSellResource((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.SellResourceHeader:
                            setTextLoading("Đọc dữ liệu bán tài nguyên....");
                            loadSellResourceHeader((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.BuyResource:
                            setTextLoading("Đọc dữ liệu mua tài nguyên....");
                            loadBuyResource((ArrayList)data[key]);
                            break;
                        case CodeSaveTask.VanChuyenVuKhi:
                            setTextLoading("Đọc dữ liệu vận chuyển vũ khí....");
                            loadVanChuyenVuKhi((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.BuyResourceHeader:
                            setTextLoading("Đọc dữ liệu mua tài nguyên....");
                            loadBuyResourceHeader((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.BinhMan:
                            setTextLoading("Đọc dữ liệu bình man....");
                            loadBinhMan((ArrayList)data[key]);
                            break;

                        case CodeSaveTask.CallMan:
                            setTextLoading("Đọc dữ liệu gọi man....");
                            loadCallMan((ArrayList)data[key]);
                            break;

                    }

                }
                Application.DoEvents();
                hideLoadingMsg();

                LVAuto.LVForm.FrmMain.LVFRMMAIN.SetAllTabsEnable(true);

                if (error.Trim() != "")
                    MessageBox.Show(error, "Lỗi khi load dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    MessageBox.Show("Hi " + LVAuto.LVWeb.LVClient.lvusername + "\r\n Đã load dữ liệu xong, kiểm tra lại đê", "Load dữ liệu xong - " + LVAuto.LVWeb.LVClient.lvusername, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    setTextLoading("");
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                //Console.WriteLine("The file could not be read:");
                //Console.WriteLine(e.Message);
                hideLoadingMsg();

                LVAuto.LVForm.FrmMain.LVFRMMAIN.SetAllTabsEnable(true);

                if (error.Trim() != "")
                    MessageBox.Show(error, "Lỗi khi load dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

		}

		private void loadCallMan(ArrayList ardata)
		{
			try
			{

				LVAuto.LVForm.FrmMain.CallManList.Clear();
				string[] separ = new string[] { spareChar };

				string[] data;

				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);


					LVAuto.LVForm.Command.CommonObj.CallManObj obj2 = new LVAuto.LVForm.Command.CommonObj.CallManObj();



					obj2.TimeToCheck	= int.Parse(data[1].ToString()); ;
					obj2.CityId			= int.Parse(data[2].ToString());
					obj2.CityName		= (data[3].ToString());
					obj2.Id		= int.Parse(data[4].ToString());
					obj2.Name	= (data[5].ToString());

					obj2.GroupID		= int.Parse(data[6].ToString());
					obj2.ToaDoCallVeX	= int.Parse(data[7].ToString());
					obj2.ToaDoCallVeY	= int.Parse(data[8].ToString());

					obj2.Military.Bobinh[0] = int.Parse(data[9].ToString());
					obj2.Military.Bobinh[1] = int.Parse(data[10].ToString());
					obj2.Military.Bobinh[2] = int.Parse(data[11].ToString());
					obj2.Military.Bobinh[3] = int.Parse(data[12].ToString());

					obj2.Military.KyBinh[0] = int.Parse(data[13].ToString());
					obj2.Military.KyBinh[1] = int.Parse(data[14].ToString());
					obj2.Military.KyBinh[2] = int.Parse(data[15].ToString());
					obj2.Military.KyBinh[3] = int.Parse(data[16].ToString());

					obj2.Military.CungThu[0] = int.Parse(data[17].ToString());
					obj2.Military.CungThu[1] = int.Parse(data[18].ToString());
					obj2.Military.CungThu[2] = int.Parse(data[19].ToString());
					obj2.Military.CungThu[3] = int.Parse(data[20].ToString());

					obj2.Military.Xe[0] = int.Parse(data[21].ToString());
					obj2.Military.Xe[1] = int.Parse(data[22].ToString());
					obj2.Military.Xe[2] = int.Parse(data[23].ToString());
					obj2.Military.Xe[3] = int.Parse(data[24].ToString());

					obj2.SoTuongMinhDanh1TuongDich	= int.Parse(data[25].ToString());
					obj2.PhuongThucTanCongID		= int.Parse(data[26].ToString());
					obj2.PhuongThucTanCongName		= (data[27].ToString());
					obj2.PhuongThucChonMucTieuID	= int.Parse(data[28].ToString());
					obj2.PhuongThucChonMucTieuName	= (data[29].ToString());
					obj2.MuuKeTrongChienTranID		= int.Parse(data[30].ToString());
					obj2.MuuKeTrongChienTranName	= (data[31].ToString());

					obj2.SoLuongQuanMinToGo			= int.Parse(data[32].ToString());
					obj2.SiKhiMinToGo				= int.Parse(data[33].ToString());
					obj2.TuUpSiKhi					= bool.Parse(data[34].ToString());
					obj2.TuKhoiPhucTrangThai		= bool.Parse(data[35].ToString());
					obj2.TuMuaManTocLenh			= bool.Parse(data[36].ToString());

					int jj = 37;
					while (jj < data.Length - 1)
					{

						LVAuto.LVForm.Command.CommonObj.ManOBJ man = new LVAuto.LVForm.Command.CommonObj.ManOBJ();
						man.ManID = int.Parse(data[jj].ToString());
						man.ManName = (data[jj + 1].ToString());
						jj = jj + 2;
						obj2.Mans.Add(man);
					}
					/*
					for (int j = 37; j < data.Length; j++)
					{
						LVAuto.Command.CommonObj.ManOBJ man = new LVAuto.Command.CommonObj.ManOBJ();
						man.ManID = int.Parse(data[j].ToString());
						man.ManName = (data[j+1].ToString());
						obj2.Mans.Add(man);
					}
					*/
					LVAuto.LVForm.FrmMain.CallManList.Add(obj2);
				}

			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu gọi man  \r\n";
			}
		}


		private void loadBinhMan(ArrayList ardata)
		{
			try
			{

				LVAuto.LVForm.FrmMain.BinhManList.Clear();
				string[] separ = new string[] { spareChar };

				string[] data;

				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);


					LVAuto.LVForm.Command.CommonObj.BinhManObj obj2 = new LVAuto.LVForm.Command.CommonObj.BinhManObj();
					

					obj2.ManID = int.Parse(data[1].ToString());	;
					obj2.ManName = (data[2].ToString());	;
					obj2.PhuongThucTanCongID = int.Parse(data[3].ToString());	;
					obj2.PhuongThucTanCongName = (data[4].ToString());	;
					obj2.PhuongThucChonMucTieuID = int.Parse(data[5].ToString());	;
					obj2.PhuongThucChonMucTieuName = (data[6].ToString());	;
					obj2.UuTienID = int.Parse(data[7].ToString());	;
					obj2.UuTienName = (data[8].ToString());	;
					obj2.SoLuongQuanMinToGo = int.Parse(data[9].ToString());	;
					obj2.SiKhiMinToGo = int.Parse(data[10].ToString());	;
					obj2.MaxOToGo = int.Parse(data[11].ToString());	;
					obj2.TimeToCheck = int.Parse(data[12].ToString());	;
					obj2.TuUpSiKhi = bool.Parse(data[13].ToString());	;
					obj2.Id = int.Parse(data[14].ToString());	;
					obj2.Name = (data[15].ToString());	;
					obj2.CityId = int.Parse(data[16].ToString());	;
					obj2.CityName = (data[17].ToString());	;
					obj2.CityId = int.Parse(data[18].ToString());	;
					obj2.CityName = (data[19].ToString());	;

					obj2.Military.Bobinh[0] = int.Parse(data[20].ToString());	;
					obj2.Military.Bobinh[1] = int.Parse(data[21].ToString());	;
					obj2.Military.Bobinh[2] = int.Parse(data[22].ToString());	;
					obj2.Military.Bobinh[3] = int.Parse(data[23].ToString());	;

					obj2.Military.KyBinh[0] = int.Parse(data[24].ToString());	;
					obj2.Military.KyBinh[1] = int.Parse(data[25].ToString());	;
					obj2.Military.KyBinh[2] = int.Parse(data[26].ToString());	;
					obj2.Military.KyBinh[3] = int.Parse(data[27].ToString());	;

					obj2.Military.CungThu[0] = int.Parse(data[28].ToString());	;
					obj2.Military.CungThu[1] = int.Parse(data[29].ToString());	;
					obj2.Military.CungThu[2] = int.Parse(data[30].ToString());	;
					obj2.Military.CungThu[3] = int.Parse(data[31].ToString());	;

					obj2.Military.Xe[0] = int.Parse(data[32].ToString());	;
					obj2.Military.Xe[1] = int.Parse(data[33].ToString());	;
					obj2.Military.Xe[2] = int.Parse(data[34].ToString());	;
					obj2.Military.Xe[3] = int.Parse(data[35].ToString());	;

					obj2.MuuKeTrongChienTranID = int.Parse(data[36].ToString());	;
					obj2.MuuKeTrongChienTranName = (data[37].ToString());	;
					obj2.TuKhoiPhucTrangThai = bool.Parse(data[38].ToString());	;

                    obj2.ManType = int.Parse(data[39].ToString());
                    obj2.ToaDoMoX = int.Parse(data[40].ToString());
                    obj2.ToaDoMoY = int.Parse(data[41].ToString());

					LVAuto.LVForm.FrmMain.BinhManList.Add(obj2);
				}
			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu bình man  \r\n";
			}


		}

		private void loadBuildUpHeader(ArrayList ardata)
		{
			// lưu xây nhà header
			
			try
			{			

				string[] separ = new string[] { spareChar };

				string[] data;
			
				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

					LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_txtTimer.Text = data[1].ToString();		// thoi gian
					LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_checkUpgradeAll.Checked = bool.Parse(data[2].ToString());		// 
					LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_checkAutoBuyResources.Checked =bool.Parse(data[3].ToString());
					LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_txtGoldThreshold.Text = data[4].ToString();

				}
			}
			catch (Exception ex)
			{
				error = "Không load được dữ xây nhà  \r\n";
			}
		}

		private void loadBuildUpDown(ArrayList ardata, bool isUp)
		{
			try
			{
				string time;
				int cityid;
				string buildingname;
				int gid;
				int pid;

				int citypost =-1;
				int buildingpost;
				string[] data;
				string str;
				int oldcityid =0;
				string[] separ = new string[] { spareChar };
				LVAuto.LVObj.City city =null;
				for (int i = 0; i < ardata.Count; i++)
				{
					Application.DoEvents();
					str = (string)ardata[i];
					data = str.Split(separ, StringSplitOptions.None); ;

					time = data[1];
					cityid = int.Parse(data[2]);
					buildingname = data[4];
					gid = int.Parse(data[5]);
					pid = int.Parse(data[6]);

					if (oldcityid != cityid)
					{

						city = LVAuto.LVForm.Command.City.GetCityByID(cityid);
						
						if (city == null) continue;
						
						oldcityid = cityid;
						citypost = LVAuto.LVForm.Command.City.GetCityPostByID(cityid);
						LVAuto.LVForm.Command.City.GetAllBuilding(citypost, false);
					 
					}

					if (city != null)
					{
						
						Application.DoEvents();

						buildingpost = LVAuto.LVForm.Command.City.GetBuildingPostById(citypost, gid, pid);
						if (buildingpost != -1)
						{
							if (isUp)
							{
								setTextLoading("Đọc dữ liệu xây nhà " + city.Name + " ....");
								LVAuto.LVForm.FrmMain.LVFRMMAIN.Construct_txtTimer.Text = time;
								LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].AllBuildings[buildingpost].IsUp = true;
							}
							else
							{
								setTextLoading("Đọc dữ liệu hạ nhà " + city.Name + " ...."); 

								LVAuto.LVForm.FrmMain.LVFRMMAIN.txtDELCHECK.Text = time;
								LVAuto.LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[citypost].AllBuildings[buildingpost].IsDown = true;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (isUp)
					error = "Không load được dữ liệu xây dựng \r\n";
				else
					error = "Không load được dữ liệu hạ nhà \r\n";
				
			}
		}

		private void loadVanChuyenVuKhi(ArrayList ardata)
		{
			try
			{
				string[] separ = new string[] { spareChar };
				string[] data;
				LVAuto.LVForm.Command.CommonObj.VanChuyenVK objVCVK;
				LVAuto.LVForm.FrmMain.VanChuyenVuKhi.Clear();

				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					objVCVK = new LVAuto.LVForm.Command.CommonObj.VanChuyenVK();

					LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKVANCHUYENVUKHI.Text = data[1].ToString();		// thoi gian

					objVCVK.id = long.Parse(data[2].ToString());
					objVCVK.LoaiVuKhiID = int.Parse(data[3].ToString());
					objVCVK.ThanhChuyenDiID = int.Parse(data[4].ToString());
					objVCVK.ThanhChuyenDenID = int.Parse(data[5].ToString());
					objVCVK.TongSoLuongChuyen = int.Parse(data[6].ToString());
					objVCVK.DaChuyenDuoc = int.Parse(data[7].ToString());
					objVCVK.SoLuongChuyenMoiLan = int.Parse(data[8].ToString());

					setTextLoading("Đọc dữ liệu vận chuyển vũ khí " + LVAuto.LVForm.Command.City.GetCityByID(objVCVK.ThanhChuyenDiID).Name + " ....");
					Application.DoEvents();

					LVAuto.LVForm.FrmMain.VanChuyenVuKhi.Add(objVCVK);

				}

			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu vận chuyển vũ khí \r\n";
			}		
		}

		private void loadMuaVuKhi(ArrayList ardata)
		{
			try
			{
				string[] separ = new string[] { spareChar };
				LVAuto.LVForm.Command.OPTObj.Weapon vk;
				string[] data;
				if (LVAuto.LVForm.LVCommon.common.LoadCityForBuyWepon(LVAuto.LVForm.FrmMain.LVFRMMAIN.pnWepon) ==0)
					LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyWepon_loaded = true;

				for (int j = 0; j < LVAuto.LVForm.FrmMain.LVFRMMAIN.pnWepon.Controls.Count; j++)
				{
					vk = (LVAuto.LVForm.Command.OPTObj.Weapon)LVAuto.LVForm.FrmMain.LVFRMMAIN.pnWepon.Controls[j];
					for (int i = 0; i < ardata.Count; i++)
					{
						data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

						setTextLoading("Đọc dữ liệu mua vũ khí " + data[4].ToString()  + " ....");
						Application.DoEvents();

						if (vk.cityid == int.Parse(data[3]))
						{
							vk.chkOK.Checked = true;

							LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKWEPON.Text = data[1];// thoigian
							LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCOUNTWEPON.Text = data[2];// so luong mua
							vk.cboWepon.SelectedIndex = int.Parse(data[5]);
							vk.cboAmor.SelectedIndex = int.Parse(data[6]);
							vk.cboHorse.SelectedIndex = int.Parse(data[7]);
							vk.txtAmount.Text = data[8].ToString();
							vk.cboLevel.SelectedIndex = int.Parse(data[9]);
							vk.posid_w = int.Parse(data[10]);
							vk.posid_a = int.Parse(data[11]);
							vk.posid_h = int.Parse(data[12]);
						}
					}

				}
			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu mua vũ khí \r\n";
			}
		}
		private void loadVanChuyen(ArrayList ardata)
		{
			try
			{
				string[] separ = new string[] { spareChar };
				LVAuto.LVForm.Command.OPTObj.Vanchuyen vc;
				string[] data;



				// load panel neeus chua loa
				//if (LVAuto.frmmain.LVFRMMAIN.pnVanchuyen.Controls.Count == 0)
				{
					LVAuto.LVForm.LVCommon.common.LoadCityToGridForVanchuyen(LVAuto.LVForm.FrmMain.LVFRMMAIN.pnVanchuyen); 
					LVAuto.LVForm.FrmMain.LVFRMMAIN.VanChuyen_loaded = true;
				}
				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);


					vc = (LVAuto.LVForm.Command.OPTObj.Vanchuyen)LVAuto.LVForm.FrmMain.LVFRMMAIN.pnVanchuyen.Controls[LVAuto.LVForm.FrmMain.LVFRMMAIN.pnVanchuyen.Controls.Count - i - 1];
					vc.chkOK.Checked = true;

					LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKVANCHUYEN.Text = data[1];		// thoi gian
					vc.cboSource.SelectedItem = LVAuto.LVForm.Command.City.GetCityByID(int.Parse(data[2]));		// source
					vc.cboDesc.SelectedItem = LVAuto.LVForm.Command.City.GetCityByID(int.Parse(data[4]));		// desc
					
					setTextLoading("Đọc dữ liệu vận chuyển " + data[3].ToString() + " ....");
					Application.DoEvents();

					if (data[6] != null || data[6] != "")		//food
						vc.txtFOOD.Text = data[6];

					if (data[7] != null || data[7] != "")		//wood
						vc.txtWOOD.Text = data[7];

					if (data[8] != null || data[8] != "")		//iron
						vc.txtIRON.Text = data[8];

					if (data[9] != null || data[9] != "")		//stone
						vc.txtSTONE.Text = data[9];

					if (data[10] != null || data[10] != "")		//stone
						vc.txtMONEY.Text = data[10];
				}
			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu vận chuyển \r\n";
			}
		}

		private void loadUpSyKhi(ArrayList ardata)
		{
			try
			{
				string[] separ = new string[] { spareChar };

				string[] data;

				if (LVAuto.LVForm.LVCommon.common.LoadGeneralForUpSiKhi(LVAuto.LVForm.FrmMain.LVFRMMAIN.tvSIKHI)) LVAuto.LVForm.FrmMain.LVFRMMAIN.Upsikhi_loaded = true;

				TreeNode root = LVAuto.LVForm.FrmMain.LVFRMMAIN.tvSIKHI.Nodes["root"];
				TreeNode g;

				int cityid;
				int genid;

				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

					LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKSIKHI.Text = data[1];		// thoi gian
					LVAuto.LVForm.FrmMain.LVFRMMAIN.txtOneSiKhi.Text = data[2];		// mooi lan nang

					cityid = int.Parse( data[3]);
					genid = int.Parse(data[5]);

					setTextLoading("Đọc dữ liệu luyện sĩ khí " + data[6].ToString() + " ....");
					Application.DoEvents();

					for (int j = 0; j < root.Nodes.Count; j++)
					{
						g = root.Nodes[j];
						if (g.Name == cityid + "." + genid) g.Checked = true;;
						
					}
				}
			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu nâng sĩ khí  \r\n";
			}
		}
		
		private void loadBienChe(ArrayList ardata)
		{
			try
			{
				LVAuto.LVForm.FrmMain.ListBienChe = new ArrayList();

				string[] separ = new string[] { spareChar };

				string[] data;
				LVAuto.LVForm.LVCommon.BienChe oneBienChe;
				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					oneBienChe = new LVAuto.LVForm.LVCommon.BienChe();

					LVAuto.LVForm.FrmMain.LVFRMMAIN.txtTabBienCheTimeCheck.Text = data[1].ToString();		// thoi gian
					oneBienChe.nangsykhi = bool.Parse(data[2].ToString());		// 


					oneBienChe.cityid		= int.Parse (data[3].ToString());		
					oneBienChe.cityname		= (data[4].ToString());

					oneBienChe.generalid	= int.Parse (data[5].ToString());
					oneBienChe.generalname	= (data[6].ToString());

					oneBienChe.bobinhamount = int.Parse (data[7].ToString());
					oneBienChe.kybinhamount = int.Parse (data[8].ToString());
					oneBienChe.cungthuamount = int.Parse (data[9].ToString());
					oneBienChe.xemount = int.Parse(data[10].ToString());

					setTextLoading("Đọc dữ liệu biên chế " + oneBienChe.generalname + " ....");
					Application.DoEvents();

					LVAuto.LVForm.FrmMain.ListBienChe.Add(oneBienChe); 
				
				}
			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu biên chế  \r\n";
			}
		}

		private void loadDieuPhai(ArrayList ardata)
		{
			try
			{
				LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong g;
				LVAuto.LVForm.FrmMain.DieuPhaiQuanVanVo.Clear();

				string[] separ = new string[] { spareChar };

				string[] data;
				
				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

					g = new LVAuto.LVForm.Command.CommonObj.DieuPhaiTuong();


					//data += CodeSaveTask.DieuPhai + spareChar; data[0]
					g.cityID		=  int.Parse(data[1].ToString());
					g.cityname		=  data[2].ToString();
					g.desc			=  data[3].ToString();
					g.generalid		=  int.Parse(data[4].ToString());
					g.generalname	=  data[5].ToString();

					setTextLoading("Đọc dữ liệu điều phái " + g.generalname + " ....");
					Application.DoEvents();

					
					g.generaltype	=  int.Parse(data[6].ToString());
					g.ID			=  long.Parse(data[7].ToString());
					g.loidailevel	=  int.Parse(data[8].ToString());
					g.tasktype		=  int.Parse(data[9].ToString());
					g.timetoruninmilisecond =  int.Parse(data[10].ToString());
					g.X				=  int.Parse(data[11].ToString());
					g.Y				=  int.Parse(data[12].ToString());

					LVAuto.LVForm.FrmMain.DieuPhaiQuanVanVo.Add(g);
				}
				
			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu điều phái \r\n";

			}
			
		}

		private void loadThaoPhat(ArrayList ardata)
		{

			try
			{
				string[] separ = new string[] { spareChar };

				string[] data;
				LVAuto.LVForm.FrmMain.TuongDiThaoPhatList.Clear();
				for (int i = 0; i < ardata.Count; i++)
				{
					LVAuto.LVForm.LVCommon.GeneralThaoPhat gen = new LVAuto.LVForm.LVCommon.GeneralThaoPhat();

					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					gen.TimeToRun  =  int.Parse( data[1].ToString());
					gen.Id = int.Parse(data[2].ToString());
					gen.Name = data[3].ToString();

					gen.CityId		=   int.Parse(data[4].ToString());
					gen.QuestId	=   int.Parse(data[5].ToString());
					gen.SiKhiMinToGo	=   int.Parse(data[6].ToString());
					gen.TuBienCheQuan  =  bool.Parse( data[7].ToString());
					gen.TuUpSiKhi	= bool.Parse(data[8].ToString());
					gen.SoLuongQuanMinToGo = int.Parse(data[9].ToString());


					


					gen.AttachNumGenerals = int.Parse(data[10].ToString());
					string str;
					for (int j = 0; j < LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownAttackNumGenerals.Items.Count; j++)
					{
						str = LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownAttackNumGenerals.Items[j].ToString();
						//str = str.Substring(0, str.IndexOf("."));
						if (gen.AttachNumGenerals == int.Parse(str))
						{
							LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownAttackNumGenerals.SelectedIndex = j;
							break;
						}
					}
					
					gen.PhuongThucTanCongID = int.Parse(data[11].ToString());
					for (int j = 0; j < LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownAttackMethod.Items.Count; j++)
					{
						str = LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownAttackMethod.Items[j].ToString();
						str = str.Substring(0, str.IndexOf("."));
						if (gen.PhuongThucTanCongID == int.Parse(str))
						{
							LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownAttackMethod.SelectedIndex = j;
							break;
						}
					}
					gen.PhuongThucTanCongName = data[12].ToString();


					gen.PhuongThucChonMucTieuID = int.Parse(data[13].ToString());
					for (int j = 0; j < LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownTargetMethod.Items.Count; j++)
					{
						str = LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownTargetMethod.Items[j].ToString();
						str = str.Substring(0, str.IndexOf("."));
						if (gen.PhuongThucChonMucTieuID == int.Parse(str))
						{
							LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownTargetMethod.SelectedIndex = j;
							break;
						}
					}
					gen.PhuongThucChonMucTieuName = (data[14].ToString());
					gen.TuDoiTranHinhKhac = bool.Parse(data[15].ToString());

					gen.MuuKeTrongChienTranID = int.Parse(data[16].ToString());
					for (int j = 0; j < LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownSchemeInBattle.Items.Count; j++)
					{
						str = LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownSchemeInBattle.Items[j].ToString();
						str = str.Substring(0, str.IndexOf("."));
						if (gen.MuuKeTrongChienTranID == int.Parse(str))
						{
							LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_dropdownSchemeInBattle.SelectedIndex = j;
							break;
						}
					}
					gen.MuuKeTrongChienTranName = (data[17].ToString());

					gen.TuKhoiPhucTrangThai = bool.Parse(data[18].ToString());
					LVAuto.LVForm.FrmMain.LVFRMMAIN.Quest_checkAutoRestoreStatus.Checked = gen.TuKhoiPhucTrangThai;

					gen.NumInfantries=int.Parse(data[19].ToString());
					gen.NumCavalries=int.Parse(data[20].ToString());
					gen.NumArchers=int.Parse(data[21].ToString());
					gen.NumCatapults = int.Parse(data[22].ToString());

					LVAuto.LVForm.FrmMain.TuongDiThaoPhatList.Add(gen);

					Application.DoEvents();

				} // for



				LVAuto.LVForm.LVCommon.common.LoadDataForThaoPhat(LVAuto.LVForm.FrmMain.TuongDiThaoPhatList);

			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu thảo phạt   \r\n";
			}

		}

		private void loadAnUi(ArrayList ardata)
		{

			try
			{	
				string[] separ = new string[] { spareChar };

				string[] data;
				int cityid;

				LVAuto.LVForm.FrmMain.AnuiForAuto = new ArrayList();
				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					
					cityid = int.Parse(data[2].ToString());
					
					LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKANUI.Text = data[1].ToString();
					LVAuto.LVForm.FrmMain.AnuiForAuto.Add(cityid);
					//setTextLoading("Đọc dữ liệu an ủi " + data[3].ToString()  + " ....");
					Application.DoEvents();
				}
			}

			catch (Exception ex)
			{
				error = "Không load được dữ liệu an ủi   \r\n";
			}

			
		}

		private void loadUpgrade(ArrayList ardata)
		{
				

			try
			{
				string[] separ = new string[] { spareChar };

				string[] data;				

				LVAuto.LVForm.FrmMain.UpgradeForAuto = new ArrayList();

				for (int i = 0; i < ardata.Count; i++)
				{

					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

					if (i == 0) LVAuto.LVForm.FrmMain.UpgradeForAuto.Add(data[2].ToString());

					LVAuto.LVForm.FrmMain.LVFRMMAIN.txtCHECKUPDATE.Text = data[1].ToString();
					
					LVAuto.LVForm.FrmMain.UpgradeForAuto.Add(data[3].ToString());
					LVAuto.LVForm.FrmMain.LVFRMMAIN.ReSearch_loaded= false;

					setTextLoading("Đọc dữ liệu nâng cấp trong đại học điện....");
					Application.DoEvents();
				}
			}

			catch (Exception ex)
			{
				error = "Không load được dữ liệu nâng cấp trong đại học điện   \r\n";
			}

		}

		/*private void test(ArrayList ardata)
		{
			try
			{
				string[] separ = new string[] { spareChar };
				string[] data;
				
				Common.common.LoadCityToGridForSellResource(LVAuto.frmmain.LVFRMMAIN.dtaSELL);

				DataGridView dg = LVAuto.frmmain.LVFRMMAIN.dtaSELL;

				LVAuto.frmmain.LVFRMMAIN.SellResource_loaded = true;


				int cityid;
				for (int i = 0; i < ardata.Count; i++)
				{

					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

					LVAuto.frmmain.LVFRMMAIN.txtSELLCHECK.Text = data[1]; //time
					cityid = int.Parse(data[2]);
					LVAuto.frmmain.LVFRMMAIN.txtCOUNTLUA.Text = data[7].ToString();
					LVAuto.frmmain.LVFRMMAIN.txtPRICELUA.Text = data[8].ToString();
					LVAuto.frmmain.LVFRMMAIN.txtSAFELUA.Text = data[9].ToString();

					LVAuto.frmmain.LVFRMMAIN.txtCOUNTGO.Text = data[10].ToString();
					LVAuto.frmmain.LVFRMMAIN.txtPRICEGO.Text = data[11].ToString();
					LVAuto.frmmain.LVFRMMAIN.txtSAFEGO.Text = data[12].ToString();

					LVAuto.frmmain.LVFRMMAIN.txtCOUNTSAT.Text = data[13].ToString();
					LVAuto.frmmain.LVFRMMAIN.txtPRICESAT.Text = data[14].ToString();
					LVAuto.frmmain.LVFRMMAIN.txtSAFESAT.Text = data[15].ToString();

					LVAuto.frmmain.LVFRMMAIN.txtCOUNTDA.Text = data[16].ToString();
					LVAuto.frmmain.LVFRMMAIN.txtPRICEDA.Text = data[17].ToString();
					LVAuto.frmmain.LVFRMMAIN.txtSAFEDA.Text = data[18].ToString();

					for (int j = 0; j < dg.Rows.Count; j++)
					{
						if (int.Parse(dg.Rows[j].Cells[0].Value.ToString()) == cityid)
						{
							dg.Rows[j].Cells[2].Value = bool.Parse(data[3].ToString());
							dg.Rows[j].Cells[3].Value = bool.Parse(data[4].ToString());
							dg.Rows[j].Cells[4].Value = bool.Parse(data[5].ToString());
							dg.Rows[j].Cells[5].Value = bool.Parse(data[6].ToString());

							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu bán tài nguyên   \r\n";
			}

		}
		  */
		private void loadSellResourceHeader(ArrayList ardata)
		{
			try
			{
				string[] separ = new string[] { spareChar };
				string[] data;
				data = ((string)ardata[0]).Trim().Split(separ, StringSplitOptions.None);

				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtTimer.Text = data[1].ToString();
				
				LVAuto.LVForm.FrmMain.BanTaiNguyen.timetoruninminute = int.Parse(data[1].ToString());

				LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.SoLuongBan				= int.Parse(data[2].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.SoLuongAnToan				= int.Parse(data[3].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.CongThucNhan				= bool.Parse(data[4].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.LoaiBan					= int.Parse(data[5].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.GiaTri					= double.Parse(data[6].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.BanCoDinh					= bool.Parse(data[7].ToString());


				LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.SoLuongBan					= int.Parse(data[8].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.SoLuongAnToan				= int.Parse(data[9].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.CongThucNhan				= bool.Parse(data[10].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.LoaiBan					= int.Parse(data[11].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.GiaTri						= double.Parse(data[12].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.BanCoDinh					= bool.Parse(data[13].ToString());


				LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.SoLuongBan					= int.Parse(data[14].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.SoLuongAnToan = int.Parse(data[15].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.CongThucNhan = bool.Parse(data[16].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.LoaiBan = int.Parse(data[17].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.GiaTri = double.Parse(data[18].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.BanCoDinh = bool.Parse(data[19].ToString());

				LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.SoLuongBan = int.Parse(data[20].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.SoLuongAnToan = int.Parse(data[21].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.CongThucNhan = bool.Parse(data[22].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.LoaiBan = int.Parse(data[23].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.GiaTri = double.Parse(data[24].ToString());
				LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.BanCoDinh = bool.Parse(data[25].ToString());

				LVAuto.LVForm.FrmMain.BanTaiNguyen.SalesOff = bool.Parse(data[26].ToString());

				//--------------
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtTimer.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.timetoruninminute.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_checkSelloff.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.SalesOff;

				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellAmountMethodFoodFix.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.BanCoDinh;
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellAmountMethodFoodPercent.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.BanCoDinh;
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtAmountSellFood.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.SoLuongBan.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtAmountThresholdFood.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.SoLuongAnToan.ToString();


				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_textAddOnValueFood.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.GiaTri.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_textAddOnValueFood.Enabled = true;
				if (LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.LoaiBan == 1)		// co dinh
				{
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodFoodFix.Checked = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodFoodAdd.Enabled = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodFoodMultiply.Enabled = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodFoodMin.Checked = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodFoodAvg.Checked = false;
				}
				else
				{
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodFoodAdd.Enabled = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodFoodMultiply.Enabled = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodFoodAdd.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.CongThucNhan;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodFoodMultiply.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.CongThucNhan;
					if (LVAuto.LVForm.FrmMain.BanTaiNguyen.LUA.LoaiBan == 2)		// trung binh
					{
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodFoodAvg.Checked = true;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodFoodFix.Checked = false;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodFoodMin.Checked = false;
					}
					else
					{
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodFoodMin.Checked = true;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodFoodFix.Checked = false;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodFoodAvg.Checked = false;
					}
				}

				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellAmountMethodWoodsFix.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.BanCoDinh;
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellAmountMethodWoodsPercent.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.BanCoDinh;

				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtAmountSellWoods.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.SoLuongBan.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtAmountThresholdWoods.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.SoLuongAnToan.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_textAddOnValueWoods.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.GiaTri.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_textAddOnValueWoods.Enabled = true;
				if (LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.LoaiBan == 1)		// co dinh
				{
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodWoodsFix.Checked = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodWoodsAdd.Enabled = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodWoodsMultiply.Enabled = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodWoodsMin.Checked = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodWoodsAvg.Checked = false;
				}
				else
				{
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodWoodsAdd.Enabled = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodWoodsMultiply.Enabled = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodWoodsAdd.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.CongThucNhan;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodWoodsMultiply.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.CongThucNhan;
					if (LVAuto.LVForm.FrmMain.BanTaiNguyen.GO.LoaiBan == 2)		// trung binh
					{
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodWoodsAvg.Checked = true;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodWoodsFix.Checked = false;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodWoodsMin.Checked = false;
					}
					else
					{
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodWoodsMin.Checked = true;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodWoodsFix.Checked = false;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodWoodsAvg.Checked = false;
					}
				}

				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellAmountMethodStoneFix.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.BanCoDinh;
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellAmountMethodStonePercent.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.BanCoDinh;

				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtAmountSellStone.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.SoLuongBan.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtAmountThresholdStone.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.SoLuongAnToan.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_textAddOnValueStone.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.GiaTri.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_textAddOnValueStone.Enabled = true;
				if (LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.LoaiBan == 1)		// co dinh
				{
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodStoneFix.Checked = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodStoneAdd.Enabled = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodStoneMultiply.Enabled = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodStoneMin.Checked = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodStoneAvg.Checked = false;
				}
				else
				{
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodStoneAdd.Enabled = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodStoneMultiply.Enabled = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodStoneAdd.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.CongThucNhan;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodStoneMultiply.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.CongThucNhan;
					if (LVAuto.LVForm.FrmMain.BanTaiNguyen.DA.LoaiBan == 2)		// trung binh
					{
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodStoneAvg.Checked = true;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodStoneFix.Checked = false;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodStoneMin.Checked = false;
					}
					else
					{
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodStoneMin.Checked = true;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodStoneFix.Checked = false;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodStoneAvg.Checked = false;
					}
				}

				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellAmountMethodIronFix.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.BanCoDinh;
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellAmountMethodIronPercent.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.BanCoDinh;

				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtAmountSellIron.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.SoLuongBan.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_txtAmountThresholdIron.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.SoLuongAnToan.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_textAddOnValueIron.Text = LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.GiaTri.ToString();
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_textAddOnValueIron.Enabled = true;
				if (LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.LoaiBan == 1)		// co dinh
				{
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodIronFix.Checked = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodIronAdd.Enabled = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodIronMultiply.Enabled = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodIronMin.Checked = false;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodIronAvg.Checked = false;
				}
				else
				{
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodIronAdd.Enabled = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodIronMultiply.Enabled = true;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodIronAdd.Checked = !LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.CongThucNhan;
					LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioAddOnMethodIronMultiply.Checked = LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.CongThucNhan;
					if (LVAuto.LVForm.FrmMain.BanTaiNguyen.SAT.LoaiBan == 2)		// trung binh
					{
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodIronAvg.Checked = true;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodIronFix.Checked = false;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodIronMin.Checked = false;
					}
					else
					{
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodIronMin.Checked = true;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodIronFix.Checked = false;
						LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_radioSellPriceMethodIronAvg.Checked = false;
					}
				}


							
			}
			catch (Exception ex)
			{
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellResourceTab_Loaded = false;
				error = "Không load được dữ liệu bán tài nguyên   \r\n";
			}
		}



		private void loadSellResource(ArrayList ardata)
		{
			try
			{

				string[] separ = new string[] { spareChar };
				string[] data;

				LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo = new LVAuto.LVForm.Command.CommonObj.BanTaiNguyenObj.cityInfo_[ardata.Count];

				LVAuto.LVForm.Command.CommonObj.BanTaiNguyenObj.cityInfo_ cityinfo;

				for (int i = 0; i < ardata.Count; i++)
				{
					Application.DoEvents();

					cityinfo = new LVAuto.LVForm.Command.CommonObj.BanTaiNguyenObj.cityInfo_();


					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					

					cityinfo.CityId  = int.Parse( data[1].ToString());
					cityinfo.CityName  = data[2].ToString();
					cityinfo.BanDa  = bool.Parse( data[3].ToString());
					cityinfo.BanGo  = bool.Parse(data[4].ToString());
					cityinfo.BanLua  = bool.Parse(data[5].ToString());
					cityinfo.BanSat= bool.Parse(data[6].ToString());

					LVAuto.LVForm.FrmMain.BanTaiNguyen.CityInfo[i] = cityinfo;
				}

			
				LVAuto.LVForm.LVCommon.common.LoadCityToGridForSellResource(LVAuto.LVForm.FrmMain.LVFRMMAIN.SellRes_gridCityList);
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellResourceTab_Loaded = true;
			}
			catch (Exception ex)
			{
				LVAuto.LVForm.FrmMain.LVFRMMAIN.SellResourceTab_Loaded = false;
				error = "Không load được dữ liệu bán tài nguyên   \r\n";
			}

		}

		/*private void loadSellResource(ArrayList ardata)
		{

			

				try
				{
					string[] separ = new string[] { spareChar };
					string[] data;

					Common.common.LoadCityToGridForSellResource(LVAuto.frmmain.LVFRMMAIN.dtaSELL);

					DataGridView dg = LVAuto.frmmain.LVFRMMAIN.dtaSELL;

					LVAuto.frmmain.LVFRMMAIN.SellResource_loaded = true;


					int cityid;
					for (int i = 0; i < ardata.Count; i++)
					{
						Application.DoEvents();

						data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

						LVAuto.frmmain.LVFRMMAIN.txtSELLCHECK.Text = data[1]; //time
						cityid = int.Parse(data[2]);
						LVAuto.frmmain.LVFRMMAIN.txtCOUNTLUA.Text = data[7].ToString();
						LVAuto.frmmain.LVFRMMAIN.txtPRICELUA.Text = data[8].ToString();
						LVAuto.frmmain.LVFRMMAIN.txtSAFELUA.Text = data[9].ToString();

						LVAuto.frmmain.LVFRMMAIN.txtCOUNTGO.Text = data[10].ToString();
						LVAuto.frmmain.LVFRMMAIN.txtPRICEGO.Text = data[11].ToString();
						LVAuto.frmmain.LVFRMMAIN.txtSAFEGO.Text = data[12].ToString();

						LVAuto.frmmain.LVFRMMAIN.txtCOUNTSAT.Text = data[13].ToString();
						LVAuto.frmmain.LVFRMMAIN.txtPRICESAT.Text = data[14].ToString();
						LVAuto.frmmain.LVFRMMAIN.txtSAFESAT.Text = data[15].ToString();

						LVAuto.frmmain.LVFRMMAIN.txtCOUNTDA.Text = data[16].ToString();
						LVAuto.frmmain.LVFRMMAIN.txtPRICEDA.Text = data[17].ToString();
						LVAuto.frmmain.LVFRMMAIN.txtSAFEDA.Text = data[18].ToString();

						for (int j = 0; j < dg.Rows.Count; j++)
						{
							Application.DoEvents();
							if (int.Parse(dg.Rows[j].Cells[0].Value.ToString()) == cityid)
							{
								dg.Rows[j].Cells[2].Value = bool.Parse(data[3].ToString());
								dg.Rows[j].Cells[3].Value = bool.Parse(data[4].ToString());
								dg.Rows[j].Cells[4].Value = bool.Parse(data[5].ToString());
								dg.Rows[j].Cells[5].Value = bool.Parse(data[6].ToString());
								setTextLoading("Đọc dữ liệu bán tài nguyên " + LVAuto.Command.City.GetCityByID(cityid).name  + " ....");
								Application.DoEvents();
								break;
							}
						}
					}
				}
				catch (Exception ex)
				{
					error = "Không load được dữ liệu bán tài nguyên   \r\n";
				}

		}
		*/

		private void loadBuyResourceHeader(ArrayList ardata)
		{
			
			try
			{
				string[] separ = new string[] { spareChar };
				string[] data;
				data = ((string)ardata[0]).Trim().Split(separ, StringSplitOptions.None);

				LVAuto.LVForm.FrmMain.MuaTaiNguyen.TimeToRunInMinute = int.Parse(data[1].ToString());
				LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtTimer.Text = LVAuto.LVForm.FrmMain.MuaTaiNguyen.TimeToRunInMinute.ToString();

				LVAuto.LVForm.FrmMain.MuaTaiNguyen.VangAnToan = int.Parse(data[2].ToString());
				LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtGoldThreshold.Text = LVAuto.LVForm.FrmMain.MuaTaiNguyen.VangAnToan.ToString();

				LVAuto.LVForm.FrmMain.MuaTaiNguyen.MuaTheoDonViKho = !bool.Parse(data[3].ToString());
				LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_radioBuyAmountMethodPercent.Checked = !LVAuto.LVForm.FrmMain.MuaTaiNguyen.MuaTheoDonViKho;
				LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_radioBuyAmountMethodFix.Checked = LVAuto.LVForm.FrmMain.MuaTaiNguyen.MuaTheoDonViKho;

				LVAuto.LVForm.FrmMain.MuaTaiNguyen.GiaTri = int.Parse(data[4].ToString());
				LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtBuyAmount.Text = LVAuto.LVForm.FrmMain.MuaTaiNguyen.GiaTri.ToString();
			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu mua tài nguyên   \r\n";
			}
		}

		private void loadBuyResource(ArrayList ardata)
		{
			try
			{
				string[] separ = new string[] { spareChar };
				string[] data;

				LVAuto.LVForm.LVCommon.common.LoadCityToGridForBuyResource(LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_gridCityList);

				DataGridView dg = LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_gridCityList;

				LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyResourceTab_loaded = true;
				
				int cityid;
								
				for (int i = 0; i < ardata.Count; i++)
				{

					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtTimer.Text = data[1]; //time
					LVAuto.LVForm.FrmMain.LVFRMMAIN.BuyRes_txtGoldThreshold.Text = data[2];
					cityid = int.Parse(data[3]);

					for (int j = 0; j < dg.Rows.Count; j++)
					{
						if (int.Parse(dg.Rows[j].Cells[0].Value.ToString()) == cityid)
						{
							dg.Rows[j].Cells[2].Value = bool.Parse(data[4].ToString());
							dg.Rows[j].Cells[3].Value = bool.Parse(data[5].ToString());
							dg.Rows[j].Cells[4].Value = bool.Parse(data[6].ToString());
							dg.Rows[j].Cells[5].Value = bool.Parse(data[7].ToString());
							setTextLoading("Đọc dữ liệu mua tài nguyên " + LVAuto.LVForm.Command.City.GetCityByID(cityid).Name  + " ....");
							Application.DoEvents();
							break;
						}
					}
				}
				
			}
			catch (Exception ex)
			{
				error = "Không load được dữ liệu mua tài nguyên   \r\n";
			}

		}

		public static void wite2file(string filepath, string stContent)
		{
			//string content = "---------" + System.DateTime.Now.ToString() + Environment.NewLine + stContent;

			//using (FileStream fs = new FileStream(@"C:\Temp\MyLog.Txt", FileMode.OpenOrCreate))
			using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
			{
				fs.Seek(fs.Length, SeekOrigin.Begin);
				using (StreamWriter sw = new StreamWriter(fs))
				{
					sw.WriteLine(stContent);
					sw.Close();
				}
				fs.Close();
			}

		}

	}
}
