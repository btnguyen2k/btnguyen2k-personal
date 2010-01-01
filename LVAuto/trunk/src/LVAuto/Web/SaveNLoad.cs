using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Data;

namespace LVAuto.Web
{
	public class SaveNLoad
	{
		public  struct CodeSaveTask
		{
			public  const string XayNha		= "0";
			public  const string HaNha		= "1";
			public const string VanChuyen	= "2";
			public const string MuaVuKhi	= "3";
			public const string LuyenSyKhi	= "4";
			public const string BienChe		= "5";
			public const string DieuPhai	= "6";
			public const string ThaoPhat	= "7";
			public const string AnUi		= "8";
			public const string Upgrade		= "9";
			public const string SellResource = "10";
			public const string BuyResource = "11";
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

				LVAuto.Command.CityObj.City city;
				LVAuto.Command.CityObj.Building building;

				string buldupdata = "";
				string bulddowndata = "";


				//luu xay nha
				if (LVAuto.Command.CityObj.City.AllCity != null)
				{
					error = "xây nhà hoặc hạ nhà";
					for (int i = 0; i < LVAuto.Command.CityObj.City.AllCity.Length; i++)
					{
						city = LVAuto.Command.CityObj.City.AllCity[i];
						if (city.AllBuilding != null)
						{
							for (int j = 0; j < city.AllBuilding.Length; j++)
							{
								building = city.AllBuilding[j];
								// xay nha
								if (building.isUp)
								{
									buldupdata += CodeSaveTask.XayNha + spareChar;

									buldupdata += LVAuto.frmmain.LVFRMMAIN.txtBUILDCHECK.Text + spareChar; 				//thoigian
									buldupdata += city.id + spareChar;
									buldupdata += city.name + spareChar;
									buldupdata += building.name + spareChar;
									buldupdata += building.gid + spareChar;
									buldupdata += building.pid + spareChar;
									buldupdata += "\r\n";
								}
								// ha nha
								if (building.isDown)
								{
									bulddowndata += CodeSaveTask.HaNha + spareChar;

									bulddowndata += LVAuto.frmmain.LVFRMMAIN.txtDELCHECK.Text + spareChar; 				//thoigian
									bulddowndata += city.id			+ spareChar; 
									bulddowndata += city.name		+ spareChar;
									bulddowndata += building.name	+ spareChar; 
									bulddowndata += building.gid	+ spareChar; 
									bulddowndata +=  building.pid	+ spareChar ;
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
				data += LVAuto.frmmain.LVFRMMAIN.txtBUILDCHECK.Text + spareChar; //time;
				data += LVAuto.frmmain.LVFRMMAIN.chkXayNhaAll.Checked.ToString() + spareChar;
				data += LVAuto.frmmain.LVFRMMAIN.chkXayNha_TuMuaTaiNguyen.Checked.ToString() + spareChar;
				data += LVAuto.frmmain.LVFRMMAIN.txtXayNha_VangAnToan.Text + spareChar;
				data += "\r\n";

				// luu van chuyen			
				LVAuto.Command.OPTObj.Vanchuyen vc;
				foreach (object obj in LVAuto.frmmain.LVFRMMAIN.pnVanchuyen.Controls)
				{
					error = "vận chuyển";

					vc = (LVAuto.Command.OPTObj.Vanchuyen)obj;
					if (vc.chkOK.Checked)
					{
						data += CodeSaveTask.VanChuyen + spareChar;
						data += LVAuto.frmmain.LVFRMMAIN.txtCHECKVANCHUYEN.Text + spareChar;// thoigian

						data += ((LVAuto.Command.CityObj.City)vc.cboSource.SelectedItem).id + spareChar;	// source id
						data += ((LVAuto.Command.CityObj.City)vc.cboSource.SelectedItem).name + spareChar;	// source name
						data += ((LVAuto.Command.CityObj.City)vc.cboDesc.SelectedItem).id + spareChar;	// desc id
						data += ((LVAuto.Command.CityObj.City)vc.cboDesc.SelectedItem).name + spareChar;	// desc name

						data += vc.txtFOOD.Text + spareChar;		//lua
						data += vc.txtWOOD.Text + spareChar;
						data += vc.txtIRON.Text + spareChar;
						data += vc.txtSTONE.Text + spareChar;
						data += vc.txtMONEY.Text + spareChar;
						data += "\r\n";
					}
				}


				// luu mua vu khi
				LVAuto.Command.OPTObj.wepon vk;
				foreach (object objvk in LVAuto.frmmain.LVFRMMAIN.pnWepon.Controls)
				{
					error = "mua vũ khí";

					vk = (LVAuto.Command.OPTObj.wepon)objvk;
					if (vk.chkOK.Checked)
					{
						data += CodeSaveTask.MuaVuKhi + spareChar;
						data += LVAuto.frmmain.LVFRMMAIN.txtCHECKWEPON.Text + spareChar;// thoigian
						data += LVAuto.frmmain.LVFRMMAIN.txtCOUNTWEPON.Text + spareChar;// so luong mua
						data += vk.cityid + spareChar;												// 
						data += (LVAuto.Command.City.GetCityByID(vk.cityid)).name + spareChar;	// 
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
				TreeNode root = LVAuto.frmmain.LVFRMMAIN.tvSIKHI.Nodes["root"];
				TreeNode g;
				string[] ginfo;
				for (int i = 0; i < root.Nodes.Count; i++)
				{
					error = "luyện sĩ khí";

					g = root.Nodes[i];
					if (g.Checked)
					{
						data += CodeSaveTask.LuyenSyKhi + spareChar;
						data += LVAuto.frmmain.LVFRMMAIN.txtCHECKSIKHI.Text + spareChar;		// thoi gian
						data += LVAuto.frmmain.LVFRMMAIN.txtOneSiKhi.Text + spareChar;		// mooi lan nang
						
						ginfo = g.Name.Split(new char[] { '.' });

						data += ginfo[0] + spareChar;				// cityid
						data += (LVAuto.Command.City.GetCityByID(int.Parse(ginfo[0]))).name + spareChar;
						data += ginfo[1] + spareChar;								// Generalid
						data += g.Text +spareChar;
						data += "\r\n";
					}
				}


				// Bien che
				LVAuto.Common.BienChe oneBienChe;
				for (int i = 0; i < LVAuto.frmmain.ListBienChe.Count; i++)
				{
					error = "biên chế quân";

					oneBienChe = (LVAuto.Common.BienChe)LVAuto.frmmain.ListBienChe[i];

					data += CodeSaveTask.BienChe + spareChar;
					data += LVAuto.frmmain.LVFRMMAIN.txtTabBienCheTimeCheck.Text + spareChar;		// thoi gian
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
				LVAuto.Command.CommonObj.DieuPhaiTuong g1;
				error = "điều phái";
				for (int i =0; i < LVAuto.frmmain.DieuPhaiQuanVanVo.Count; i++)
				{
					g1 = ((LVAuto.Command.CommonObj.DieuPhaiTuong)LVAuto.frmmain.DieuPhaiQuanVanVo[i]);
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


				for (int i = 0; i < LVAuto.frmmain.TuongDiThaoPhatList.Count; i++ )
				{
					error = "thảo phạt";

					Common.GeneralThaoPhat gen = (Common.GeneralThaoPhat)LVAuto.frmmain.TuongDiThaoPhatList[i];

					data += CodeSaveTask.ThaoPhat + spareChar;
					data += gen.timetorun + spareChar;

					data += gen.GeneralId + spareChar;
					data += gen.GeneralName + spareChar;

					data += gen.CityID + spareChar;
					data += gen.NhiemVuThaoPhatID + spareChar;
					data += gen.SiKhiMinToGo + spareChar;
					data += gen.TuBienCheQuan + spareChar;
					data += gen.TuUpSiKhi + spareChar;
					data += gen.SoLuongQuanMinToGo + spareChar;

					data += gen.soluongtuongdanh1tuongdich + spareChar;
					
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

					data += gen.BienCheBoBinhAmount.ToString() + spareChar;
					data += gen.BienCheKyBinhAmount.ToString() + spareChar;
					data += gen.BienCheCungThuAmount.ToString() + spareChar;
					data += gen.BienCheXeAmount.ToString() + spareChar;



					data += "\r\n";
				}

				// luu an ui				
				for (int i = 0; i < LVAuto.frmmain.AnuiForAuto.Count; i++)
				{
					error = "an ủi";

						data += CodeSaveTask.AnUi + spareChar;
						data += LVAuto.frmmain.LVFRMMAIN.txtCHECKANUI.Text + spareChar;		// time
						data += LVAuto.frmmain.AnuiForAuto[i].ToString() + spareChar;		// cityid	
						//data += LVAuto.Command.City.GetCityByID(LVAuto.frmmain.AnuiForAuto[i]).name + spareChar;
						data += "\r\n";
				
				}

				// luu upgrade
				for (int i = 1; i < LVAuto.frmmain.UpgradeForAuto.Count; i++)
				{
					error = "nâng cấp trong đại học điện";

					data += CodeSaveTask.Upgrade + spareChar;
					data += LVAuto.frmmain.LVFRMMAIN.txtCHECKUPDATE.Text + spareChar;		// time
					data += LVAuto.frmmain.UpgradeForAuto[0].ToString() + spareChar;		//city ID
					data += LVAuto.frmmain.UpgradeForAuto[i].ToString() + spareChar;		// itemid
					data += "\r\n";
				}


				//luu sell tai nguyen header
				error = "bán tài nguyên";
				data += CodeSaveTask.SellResourceHeader + spareChar;
				data += LVAuto.frmmain.LVFRMMAIN.txtSELLCHECK.Text + spareChar; //time
				


				data += LVAuto.frmmain.BanTaiNguyen.LUA.SoLuongBan				+ spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.LUA.SoLuongAnToan				+ spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.LUA.CongThucNhan.ToString()	+ spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.LUA.LoaiBan.ToString()		+ spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.LUA.GiaTri					+ spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.LUA.BanCoDinh.ToString()		+ spareChar;

				data += LVAuto.frmmain.BanTaiNguyen.GO.SoLuongBan + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.GO.SoLuongAnToan + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.GO.CongThucNhan.ToString() + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.GO.LoaiBan.ToString() + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.GO.GiaTri + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.GO.BanCoDinh.ToString() + spareChar;
				
				data += LVAuto.frmmain.BanTaiNguyen.DA.SoLuongBan + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.DA.SoLuongAnToan + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.DA.CongThucNhan.ToString() + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.DA.LoaiBan.ToString() + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.DA.GiaTri + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.DA.BanCoDinh.ToString() + spareChar;

				data += LVAuto.frmmain.BanTaiNguyen.SAT.SoLuongBan + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.SAT.SoLuongAnToan + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.SAT.CongThucNhan.ToString() + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.SAT.LoaiBan.ToString() + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.SAT.GiaTri + spareChar;
				data += LVAuto.frmmain.BanTaiNguyen.SAT.BanCoDinh.ToString() + spareChar;

				data += LVAuto.frmmain.BanTaiNguyen.SalesOff + spareChar; 

				data += "\r\n";


				//luu sell tai nguyen conten
				if (LVAuto.frmmain.BanTaiNguyen.CityInfo != null)
					for (int i = 0; i < LVAuto.frmmain.BanTaiNguyen.CityInfo.Length; i++)
					{
						data += CodeSaveTask.SellResource + spareChar;
						data += LVAuto.frmmain.BanTaiNguyen.CityInfo[i].CityId + spareChar;
						data += LVAuto.frmmain.BanTaiNguyen.CityInfo[i].CityName + spareChar;
						data += LVAuto.frmmain.BanTaiNguyen.CityInfo[i].BanDa.ToString() + spareChar;
						data += LVAuto.frmmain.BanTaiNguyen.CityInfo[i].BanGo.ToString() + spareChar;
						data += LVAuto.frmmain.BanTaiNguyen.CityInfo[i].BanLua.ToString() + spareChar;
						data += LVAuto.frmmain.BanTaiNguyen.CityInfo[i].BanSat.ToString() + spareChar;

						data += "\r\n";
					}

				
				//luu buy tài nguyên header
				error = "mua tài nguyên";
				data += CodeSaveTask.BuyResourceHeader + spareChar;
				data += LVAuto.frmmain.LVFRMMAIN.txtBUYRESOURCECHECK.Text + spareChar; //time
				data += LVAuto.frmmain.LVFRMMAIN.txtSAFEGOLD.Text + spareChar; //vàng an toàn
				data += LVAuto.frmmain.LVFRMMAIN.rdMuaTNMuaTheoPhanTram.Checked.ToString() + spareChar; //mua theo %
				data += LVAuto.frmmain.LVFRMMAIN.txtMuaTNGioiHanMua.Text + spareChar; //giới hạn mua (giá trị)
				data += "\r\n";

				//luu buy
				DataTable dtTable = (DataTable)LVAuto.frmmain.LVFRMMAIN.dtaBUYRESOURCE.DataSource;
				if (dtTable != null)
					for (int i = 0; i < dtTable.Rows.Count; i++)
					{
						error = "mua tài nguyên";
						data += CodeSaveTask.BuyResource + spareChar;
						data += LVAuto.frmmain.LVFRMMAIN.txtBUYRESOURCECHECK.Text + spareChar; //time
						data += LVAuto.frmmain.LVFRMMAIN.txtSAFEGOLD.Text + spareChar;

						data += dtTable.Rows[i]["ID_TT"].ToString() + spareChar;

						data += (bool)dtTable.Rows[i]["BUY_LUA"] + spareChar;
						data += (bool)dtTable.Rows[i]["BUY_GO"] + spareChar;
						data += (bool)dtTable.Rows[i]["BUY_SAT"] + spareChar;
						data += (bool)dtTable.Rows[i]["BUY_DA"] + spareChar;
						data += "\r\n";
					}


				//luu van chuyen vu khi
				if (LVAuto.frmmain.VanChuyenVuKhi != null && LVAuto.frmmain.VanChuyenVuKhi.Count > 0)
				{
					Command.CommonObj.VanChuyenVK objVCVK;
					for (int i = 0; i < LVAuto.frmmain.VanChuyenVuKhi.Count; i++)
					{
						error = "vận chuyển vũ khí";
						objVCVK = (Command.CommonObj.VanChuyenVK)LVAuto.frmmain.VanChuyenVuKhi[i];

						data += CodeSaveTask.VanChuyenVuKhi+ spareChar;
						data += LVAuto.frmmain.LVFRMMAIN.txtCHECKVANCHUYENVUKHI.Text + spareChar; //time

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

				for (int i = 0; i < LVAuto.frmmain.BinhManList.Count; i++)
				{

					LVAuto.Command.CommonObj.BinhManObj obj2 = (LVAuto.Command.CommonObj.BinhManObj)LVAuto.frmmain.BinhManList[i];
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
					data += obj2.GeneralId + spareChar;
					data += obj2.GeneralName + spareChar;
					data += obj2.CityID + spareChar;
					data += obj2.CityName + spareChar;
					data += obj2.CityID + spareChar;
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

				for (int i = 0; i < LVAuto.frmmain.CallManList.Count; i++)
				{

					LVAuto.Command.CommonObj.CallManObj obj2 = (LVAuto.Command.CommonObj.CallManObj)LVAuto.frmmain.CallManList[i];
					data += CodeSaveTask.CallMan + spareChar;

					data += obj2.TimeToCheck + spareChar;
					data += obj2.CityID + spareChar;
					data += obj2.CityName + spareChar;
					data += obj2.GeneralId + spareChar;
					data += obj2.GeneralName + spareChar;

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
						LVAuto.Command.CommonObj.ManOBJ man = (LVAuto.Command.CommonObj.ManOBJ)obj2.Mans[j];
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
			LVAuto.frmmain.LVFRMMAIN.lblLoadingResMessage.Visible = true;
			LVAuto.frmmain.LVFRMMAIN.lblLoadingResMessage.BringToFront();

		}

		private void hideLoadingMsg()
		{
			//LVAuto.frmmain.LVFRMMAIN.lblLoadingResMessage.Visible = false;
			LVAuto.frmmain.LVFRMMAIN.lblLoadingResMessage.BringToFront();

		}

		private void setTextLoading(string str)
		{
			LVAuto.frmmain.LVFRMMAIN.lblLoadingResMessage.Text = str;
		}

		public void loadConfig(string filename)
		{
            try
            {
                error = "";

                showLoadingMsg();
                LVAuto.frmmain.LVFRMMAIN.SetAllMainTabEnable(false);
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
                Command.City.GetAllSimpleCity();
                Application.DoEvents();
                //clear data

                for (i = 0; i < LVAuto.Command.CityObj.City.AllCity.Length; i++)
                {
                    Application.DoEvents();
                    if (LVAuto.Command.CityObj.City.AllCity[i].AllBuilding != null)
                    {
                        Application.DoEvents();
                        for (j = 0; j < LVAuto.Command.CityObj.City.AllCity[i].AllBuilding.Length; j++)
                        {
                            Application.DoEvents();

                            LVAuto.Command.CityObj.City.AllCity[i].AllBuilding[j].isUp = false;
                            LVAuto.Command.CityObj.City.AllCity[i].AllBuilding[j].isDown = false;
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

                LVAuto.frmmain.LVFRMMAIN.SetAllMainTabEnable(true);

                if (error.Trim() != "")
                    MessageBox.Show(error, "Lỗi khi load dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    MessageBox.Show("Hi " + LVAuto.Web.LVWeb.lvusername + "\r\n Đã load dữ liệu xong, kiểm tra lại đê", "Load dữ liệu xong - " + LVAuto.Web.LVWeb.lvusername, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    setTextLoading("");
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                //Console.WriteLine("The file could not be read:");
                //Console.WriteLine(e.Message);
                hideLoadingMsg();

                LVAuto.frmmain.LVFRMMAIN.SetAllMainTabEnable(true);

                if (error.Trim() != "")
                    MessageBox.Show(error, "Lỗi khi load dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

		}

		private void loadCallMan(ArrayList ardata)
		{
			try
			{

				LVAuto.frmmain.CallManList.Clear();
				string[] separ = new string[] { spareChar };

				string[] data;

				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);


					LVAuto.Command.CommonObj.CallManObj obj2 = new LVAuto.Command.CommonObj.CallManObj();



					obj2.TimeToCheck	= int.Parse(data[1].ToString()); ;
					obj2.CityID			= int.Parse(data[2].ToString());
					obj2.CityName		= (data[3].ToString());
					obj2.GeneralId		= int.Parse(data[4].ToString());
					obj2.GeneralName	= (data[5].ToString());

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

						LVAuto.Command.CommonObj.ManOBJ man = new LVAuto.Command.CommonObj.ManOBJ();
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
					LVAuto.frmmain.CallManList.Add(obj2);
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

				LVAuto.frmmain.BinhManList.Clear();
				string[] separ = new string[] { spareChar };

				string[] data;

				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);


					LVAuto.Command.CommonObj.BinhManObj obj2 = new LVAuto.Command.CommonObj.BinhManObj();
					

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
					obj2.GeneralId = int.Parse(data[14].ToString());	;
					obj2.GeneralName = (data[15].ToString());	;
					obj2.CityID = int.Parse(data[16].ToString());	;
					obj2.CityName = (data[17].ToString());	;
					obj2.CityID = int.Parse(data[18].ToString());	;
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

					LVAuto.frmmain.BinhManList.Add(obj2);
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

					LVAuto.frmmain.LVFRMMAIN.txtBUILDCHECK.Text = data[1].ToString();		// thoi gian
					LVAuto.frmmain.LVFRMMAIN.chkXayNhaAll.Checked = bool.Parse(data[2].ToString());		// 
					LVAuto.frmmain.LVFRMMAIN.chkXayNha_TuMuaTaiNguyen.Checked =bool.Parse(data[3].ToString());
					LVAuto.frmmain.LVFRMMAIN.txtXayNha_VangAnToan.Text = data[4].ToString();

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
				LVAuto.Command.CityObj.City city =null;
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

						city = LVAuto.Command.City.GetCityByID(cityid);
						
						if (city == null) continue;
						
						oldcityid = cityid;
						citypost = LVAuto.Command.City.GetCityPostByID(cityid);
						LVAuto.Command.City.GetAllBuilding(citypost, false);
					 
					}

					if (city != null)
					{
						
						Application.DoEvents();

						buildingpost = LVAuto.Command.City.GetBuildingPostById(citypost, gid, pid);
						if (buildingpost != -1)
						{
							if (isUp)
							{
								setTextLoading("Đọc dữ liệu xây nhà " + city.name + " ....");
								LVAuto.frmmain.LVFRMMAIN.txtBUILDCHECK.Text = time;
								LVAuto.Command.CityObj.City.AllCity[citypost].AllBuilding[buildingpost].isUp = true;
							}
							else
							{
								setTextLoading("Đọc dữ liệu hạ nhà " + city.name + " ...."); 

								LVAuto.frmmain.LVFRMMAIN.txtDELCHECK.Text = time;
								LVAuto.Command.CityObj.City.AllCity[citypost].AllBuilding[buildingpost].isDown = true;
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
				Command.CommonObj.VanChuyenVK objVCVK;
				LVAuto.frmmain.VanChuyenVuKhi.Clear();

				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					objVCVK = new LVAuto.Command.CommonObj.VanChuyenVK();

					LVAuto.frmmain.LVFRMMAIN.txtCHECKVANCHUYENVUKHI.Text = data[1].ToString();		// thoi gian

					objVCVK.id = long.Parse(data[2].ToString());
					objVCVK.LoaiVuKhiID = int.Parse(data[3].ToString());
					objVCVK.ThanhChuyenDiID = int.Parse(data[4].ToString());
					objVCVK.ThanhChuyenDenID = int.Parse(data[5].ToString());
					objVCVK.TongSoLuongChuyen = int.Parse(data[6].ToString());
					objVCVK.DaChuyenDuoc = int.Parse(data[7].ToString());
					objVCVK.SoLuongChuyenMoiLan = int.Parse(data[8].ToString());

					setTextLoading("Đọc dữ liệu vận chuyển vũ khí " + LVAuto.Command.City.GetCityByID(objVCVK.ThanhChuyenDiID).name + " ....");
					Application.DoEvents();

					LVAuto.frmmain.VanChuyenVuKhi.Add(objVCVK);

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
				LVAuto.Command.OPTObj.wepon vk;
				string[] data;
				if (Common.common.LoadCityForBuyWepon(LVAuto.frmmain.LVFRMMAIN.pnWepon) ==0)
					LVAuto.frmmain.LVFRMMAIN.BuyWepon_loaded = true;

				for (int j = 0; j < LVAuto.frmmain.LVFRMMAIN.pnWepon.Controls.Count; j++)
				{
					vk = (LVAuto.Command.OPTObj.wepon)LVAuto.frmmain.LVFRMMAIN.pnWepon.Controls[j];
					for (int i = 0; i < ardata.Count; i++)
					{
						data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

						setTextLoading("Đọc dữ liệu mua vũ khí " + data[4].ToString()  + " ....");
						Application.DoEvents();

						if (vk.cityid == int.Parse(data[3]))
						{
							vk.chkOK.Checked = true;

							LVAuto.frmmain.LVFRMMAIN.txtCHECKWEPON.Text = data[1];// thoigian
							LVAuto.frmmain.LVFRMMAIN.txtCOUNTWEPON.Text = data[2];// so luong mua
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
				LVAuto.Command.OPTObj.Vanchuyen vc;
				string[] data;



				// load panel neeus chua loa
				//if (LVAuto.frmmain.LVFRMMAIN.pnVanchuyen.Controls.Count == 0)
				{
					Common.common.LoadCityToGridForVanchuyen(LVAuto.frmmain.LVFRMMAIN.pnVanchuyen); 
					LVAuto.frmmain.LVFRMMAIN.VanChuyen_loaded = true;
				}
				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);


					vc = (LVAuto.Command.OPTObj.Vanchuyen)LVAuto.frmmain.LVFRMMAIN.pnVanchuyen.Controls[LVAuto.frmmain.LVFRMMAIN.pnVanchuyen.Controls.Count - i - 1];
					vc.chkOK.Checked = true;

					LVAuto.frmmain.LVFRMMAIN.txtCHECKVANCHUYEN.Text = data[1];		// thoi gian
					vc.cboSource.SelectedItem = LVAuto.Command.City.GetCityByID(int.Parse(data[2]));		// source
					vc.cboDesc.SelectedItem = LVAuto.Command.City.GetCityByID(int.Parse(data[4]));		// desc
					
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

				if (Common.common.LoadGeneralForUpSiKhi(LVAuto.frmmain.LVFRMMAIN.tvSIKHI)) LVAuto.frmmain.LVFRMMAIN.Upsikhi_loaded = true;

				TreeNode root = LVAuto.frmmain.LVFRMMAIN.tvSIKHI.Nodes["root"];
				TreeNode g;

				int cityid;
				int genid;

				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

					LVAuto.frmmain.LVFRMMAIN.txtCHECKSIKHI.Text = data[1];		// thoi gian
					LVAuto.frmmain.LVFRMMAIN.txtOneSiKhi.Text = data[2];		// mooi lan nang

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
				LVAuto.frmmain.ListBienChe = new ArrayList();

				string[] separ = new string[] { spareChar };

				string[] data;
				LVAuto.Common.BienChe oneBienChe;
				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					oneBienChe = new LVAuto.Common.BienChe();

					LVAuto.frmmain.LVFRMMAIN.txtTabBienCheTimeCheck.Text = data[1].ToString();		// thoi gian
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

					LVAuto.frmmain.ListBienChe.Add(oneBienChe); 
				
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
				LVAuto.Command.CommonObj.DieuPhaiTuong g;
				LVAuto.frmmain.DieuPhaiQuanVanVo.Clear();

				string[] separ = new string[] { spareChar };

				string[] data;
				
				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

					g = new LVAuto.Command.CommonObj.DieuPhaiTuong();


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

					LVAuto.frmmain.DieuPhaiQuanVanVo.Add(g);
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
				LVAuto.frmmain.TuongDiThaoPhatList.Clear();
				for (int i = 0; i < ardata.Count; i++)
				{
					Common.GeneralThaoPhat gen = new LVAuto.Common.GeneralThaoPhat();

					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					gen.timetorun  =  int.Parse( data[1].ToString());
					gen.GeneralId = int.Parse(data[2].ToString());
					gen.GeneralName = data[3].ToString();

					gen.CityID		=   int.Parse(data[4].ToString());
					gen.NhiemVuThaoPhatID	=   int.Parse(data[5].ToString());
					gen.SiKhiMinToGo	=   int.Parse(data[6].ToString());
					gen.TuBienCheQuan  =  bool.Parse( data[7].ToString());
					gen.TuUpSiKhi	= bool.Parse(data[8].ToString());
					gen.SoLuongQuanMinToGo = int.Parse(data[9].ToString());


					


					gen.soluongtuongdanh1tuongdich = int.Parse(data[10].ToString());
					string str;
					for (int j = 0; j < LVAuto.frmmain.LVFRMMAIN.cboThaoPhatSLTuongDanh1Dich.Items.Count; j++)
					{
						str = LVAuto.frmmain.LVFRMMAIN.cboThaoPhatSLTuongDanh1Dich.Items[j].ToString();
						//str = str.Substring(0, str.IndexOf("."));
						if (gen.soluongtuongdanh1tuongdich == int.Parse(str))
						{
							LVAuto.frmmain.LVFRMMAIN.cboThaoPhatSLTuongDanh1Dich.SelectedIndex = j;
							break;
						}
					}
					
					gen.PhuongThucTanCongID = int.Parse(data[11].ToString());
					for (int j = 0; j < LVAuto.frmmain.LVFRMMAIN.cboThaoPhatPhuongThucTanCong.Items.Count; j++)
					{
						str = LVAuto.frmmain.LVFRMMAIN.cboThaoPhatPhuongThucTanCong.Items[j].ToString();
						str = str.Substring(0, str.IndexOf("."));
						if (gen.PhuongThucTanCongID == int.Parse(str))
						{
							LVAuto.frmmain.LVFRMMAIN.cboThaoPhatPhuongThucTanCong.SelectedIndex = j;
							break;
						}
					}
					gen.PhuongThucTanCongName = data[12].ToString();


					gen.PhuongThucChonMucTieuID = int.Parse(data[13].ToString());
					for (int j = 0; j < LVAuto.frmmain.LVFRMMAIN.cboThaoPhatPhuongThucChonMucTieu.Items.Count; j++)
					{
						str = LVAuto.frmmain.LVFRMMAIN.cboThaoPhatPhuongThucChonMucTieu.Items[j].ToString();
						str = str.Substring(0, str.IndexOf("."));
						if (gen.PhuongThucChonMucTieuID == int.Parse(str))
						{
							LVAuto.frmmain.LVFRMMAIN.cboThaoPhatPhuongThucChonMucTieu.SelectedIndex = j;
							break;
						}
					}
					gen.PhuongThucChonMucTieuName = (data[14].ToString());
					gen.TuDoiTranHinhKhac = bool.Parse(data[15].ToString());

					gen.MuuKeTrongChienTranID = int.Parse(data[16].ToString());
					for (int j = 0; j < LVAuto.frmmain.LVFRMMAIN.cboThaoPhatMuuKeTrongChienTruong.Items.Count; j++)
					{
						str = LVAuto.frmmain.LVFRMMAIN.cboThaoPhatMuuKeTrongChienTruong.Items[j].ToString();
						str = str.Substring(0, str.IndexOf("."));
						if (gen.MuuKeTrongChienTranID == int.Parse(str))
						{
							LVAuto.frmmain.LVFRMMAIN.cboThaoPhatMuuKeTrongChienTruong.SelectedIndex = j;
							break;
						}
					}
					gen.MuuKeTrongChienTranName = (data[17].ToString());

					gen.TuKhoiPhucTrangThai = bool.Parse(data[18].ToString());
					LVAuto.frmmain.LVFRMMAIN.chkThaoPhatTuKhoiPhucTrangThai.Checked = gen.TuKhoiPhucTrangThai;

					gen.BienCheBoBinhAmount=int.Parse(data[19].ToString());
					gen.BienCheKyBinhAmount=int.Parse(data[20].ToString());
					gen.BienCheCungThuAmount=int.Parse(data[21].ToString());
					gen.BienCheXeAmount = int.Parse(data[22].ToString());

					LVAuto.frmmain.TuongDiThaoPhatList.Add(gen);

					Application.DoEvents();

				} // for



				LVAuto.Common.common.LoadDataForThaoPhat(LVAuto.frmmain.TuongDiThaoPhatList);

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

				LVAuto.frmmain.AnuiForAuto = new ArrayList();
				for (int i = 0; i < ardata.Count; i++)
				{
					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					
					cityid = int.Parse(data[2].ToString());
					
					LVAuto.frmmain.LVFRMMAIN.txtCHECKANUI.Text = data[1].ToString();
					LVAuto.frmmain.AnuiForAuto.Add(cityid);
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

				LVAuto.frmmain.UpgradeForAuto = new ArrayList();

				for (int i = 0; i < ardata.Count; i++)
				{

					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);

					if (i == 0) LVAuto.frmmain.UpgradeForAuto.Add(data[2].ToString());

					LVAuto.frmmain.LVFRMMAIN.txtCHECKUPDATE.Text = data[1].ToString();
					
					LVAuto.frmmain.UpgradeForAuto.Add(data[3].ToString());
					LVAuto.frmmain.LVFRMMAIN.ReSearch_loaded= false;

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

				LVAuto.frmmain.LVFRMMAIN.txtSELLCHECK.Text = data[1].ToString();
				
				LVAuto.frmmain.BanTaiNguyen.timetoruninminute = int.Parse(data[1].ToString());

				LVAuto.frmmain.BanTaiNguyen.LUA.SoLuongBan				= int.Parse(data[2].ToString());
				LVAuto.frmmain.BanTaiNguyen.LUA.SoLuongAnToan				= int.Parse(data[3].ToString());
				LVAuto.frmmain.BanTaiNguyen.LUA.CongThucNhan				= bool.Parse(data[4].ToString());
				LVAuto.frmmain.BanTaiNguyen.LUA.LoaiBan					= int.Parse(data[5].ToString());
				LVAuto.frmmain.BanTaiNguyen.LUA.GiaTri					= double.Parse(data[6].ToString());
				LVAuto.frmmain.BanTaiNguyen.LUA.BanCoDinh					= bool.Parse(data[7].ToString());


				LVAuto.frmmain.BanTaiNguyen.GO.SoLuongBan					= int.Parse(data[8].ToString());
				LVAuto.frmmain.BanTaiNguyen.GO.SoLuongAnToan				= int.Parse(data[9].ToString());
				LVAuto.frmmain.BanTaiNguyen.GO.CongThucNhan				= bool.Parse(data[10].ToString());
				LVAuto.frmmain.BanTaiNguyen.GO.LoaiBan					= int.Parse(data[11].ToString());
				LVAuto.frmmain.BanTaiNguyen.GO.GiaTri						= double.Parse(data[12].ToString());
				LVAuto.frmmain.BanTaiNguyen.GO.BanCoDinh					= bool.Parse(data[13].ToString());


				LVAuto.frmmain.BanTaiNguyen.DA.SoLuongBan					= int.Parse(data[14].ToString());
				LVAuto.frmmain.BanTaiNguyen.DA.SoLuongAnToan = int.Parse(data[15].ToString());
				LVAuto.frmmain.BanTaiNguyen.DA.CongThucNhan = bool.Parse(data[16].ToString());
				LVAuto.frmmain.BanTaiNguyen.DA.LoaiBan = int.Parse(data[17].ToString());
				LVAuto.frmmain.BanTaiNguyen.DA.GiaTri = double.Parse(data[18].ToString());
				LVAuto.frmmain.BanTaiNguyen.DA.BanCoDinh = bool.Parse(data[19].ToString());

				LVAuto.frmmain.BanTaiNguyen.SAT.SoLuongBan = int.Parse(data[20].ToString());
				LVAuto.frmmain.BanTaiNguyen.SAT.SoLuongAnToan = int.Parse(data[21].ToString());
				LVAuto.frmmain.BanTaiNguyen.SAT.CongThucNhan = bool.Parse(data[22].ToString());
				LVAuto.frmmain.BanTaiNguyen.SAT.LoaiBan = int.Parse(data[23].ToString());
				LVAuto.frmmain.BanTaiNguyen.SAT.GiaTri = double.Parse(data[24].ToString());
				LVAuto.frmmain.BanTaiNguyen.SAT.BanCoDinh = bool.Parse(data[25].ToString());

				LVAuto.frmmain.BanTaiNguyen.SalesOff = bool.Parse(data[26].ToString());

				//--------------
				LVAuto.frmmain.LVFRMMAIN.txtSELLCHECK.Text = LVAuto.frmmain.BanTaiNguyen.timetoruninminute.ToString();
				LVAuto.frmmain.LVFRMMAIN.chkBanTN_SalesOff.Checked = LVAuto.frmmain.BanTaiNguyen.SalesOff;

				LVAuto.frmmain.LVFRMMAIN.rdBTNLuaAnToanCoDinh.Checked = LVAuto.frmmain.BanTaiNguyen.LUA.BanCoDinh;
				LVAuto.frmmain.LVFRMMAIN.rdBTNLuaPercentCoDinh.Checked = !LVAuto.frmmain.BanTaiNguyen.LUA.BanCoDinh;
				LVAuto.frmmain.LVFRMMAIN.txtCOUNTLUA.Text = LVAuto.frmmain.BanTaiNguyen.LUA.SoLuongBan.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtSAFELUA.Text = LVAuto.frmmain.BanTaiNguyen.LUA.SoLuongAnToan.ToString();


				LVAuto.frmmain.LVFRMMAIN.txtBanTN_LUA_TB_Heso.Text = LVAuto.frmmain.BanTaiNguyen.LUA.GiaTri.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtBanTN_LUA_TB_Heso.Enabled = true;
				if (LVAuto.frmmain.BanTaiNguyen.LUA.LoaiBan == 1)		// co dinh
				{
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_CODINH.Checked = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_TB_Cong.Enabled = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_TB_Nhan.Enabled = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_THAPNHAT.Checked = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_TRUNGBINH.Checked = false;
				}
				else
				{
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_TB_Cong.Enabled = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_TB_Nhan.Enabled = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_TB_Cong.Checked = !LVAuto.frmmain.BanTaiNguyen.LUA.CongThucNhan;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_TB_Nhan.Checked = LVAuto.frmmain.BanTaiNguyen.LUA.CongThucNhan;
					if (LVAuto.frmmain.BanTaiNguyen.LUA.LoaiBan == 2)		// trung binh
					{
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_TRUNGBINH.Checked = true;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_CODINH.Checked = false;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_THAPNHAT.Checked = false;
					}
					else
					{
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_THAPNHAT.Checked = true;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_CODINH.Checked = false;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_LUA_TRUNGBINH.Checked = false;
					}
				}

				LVAuto.frmmain.LVFRMMAIN.rdBTNGoAnToanCoDinh.Checked = LVAuto.frmmain.BanTaiNguyen.GO.BanCoDinh;
				LVAuto.frmmain.LVFRMMAIN.rdBTNGoPercentCoDinh.Checked = !LVAuto.frmmain.BanTaiNguyen.GO.BanCoDinh;

				LVAuto.frmmain.LVFRMMAIN.txtCOUNTGO.Text = LVAuto.frmmain.BanTaiNguyen.GO.SoLuongBan.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtSAFEGO.Text = LVAuto.frmmain.BanTaiNguyen.GO.SoLuongAnToan.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtBanTN_GO_TB_Heso.Text = LVAuto.frmmain.BanTaiNguyen.GO.GiaTri.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtBanTN_GO_TB_Heso.Enabled = true;
				if (LVAuto.frmmain.BanTaiNguyen.GO.LoaiBan == 1)		// co dinh
				{
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_CODINH.Checked = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_TB_Cong.Enabled = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_TB_Nhan.Enabled = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_THAPNHAT.Checked = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_TRUNGBINH.Checked = false;
				}
				else
				{
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_TB_Cong.Enabled = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_TB_Nhan.Enabled = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_TB_Cong.Checked = !LVAuto.frmmain.BanTaiNguyen.GO.CongThucNhan;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_TB_Nhan.Checked = LVAuto.frmmain.BanTaiNguyen.GO.CongThucNhan;
					if (LVAuto.frmmain.BanTaiNguyen.GO.LoaiBan == 2)		// trung binh
					{
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_TRUNGBINH.Checked = true;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_CODINH.Checked = false;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_THAPNHAT.Checked = false;
					}
					else
					{
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_THAPNHAT.Checked = true;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_CODINH.Checked = false;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_GO_TRUNGBINH.Checked = false;
					}
				}

				LVAuto.frmmain.LVFRMMAIN.rdBTNDaAnToanCoDinh.Checked = LVAuto.frmmain.BanTaiNguyen.DA.BanCoDinh;
				LVAuto.frmmain.LVFRMMAIN.rdBTNDaPercentCoDinh.Checked = !LVAuto.frmmain.BanTaiNguyen.DA.BanCoDinh;

				LVAuto.frmmain.LVFRMMAIN.txtCOUNTDA.Text = LVAuto.frmmain.BanTaiNguyen.DA.SoLuongBan.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtSAFEDA.Text = LVAuto.frmmain.BanTaiNguyen.DA.SoLuongAnToan.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtBanTN_DA_TB_Heso.Text = LVAuto.frmmain.BanTaiNguyen.DA.GiaTri.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtBanTN_DA_TB_Heso.Enabled = true;
				if (LVAuto.frmmain.BanTaiNguyen.DA.LoaiBan == 1)		// co dinh
				{
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_CODINH.Checked = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_TB_Cong.Enabled = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_TB_Nhan.Enabled = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_THAPNHAT.Checked = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_TRUNGBINH.Checked = false;
				}
				else
				{
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_TB_Cong.Enabled = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_TB_Nhan.Enabled = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_TB_Cong.Checked = !LVAuto.frmmain.BanTaiNguyen.DA.CongThucNhan;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_TB_Nhan.Checked = LVAuto.frmmain.BanTaiNguyen.DA.CongThucNhan;
					if (LVAuto.frmmain.BanTaiNguyen.DA.LoaiBan == 2)		// trung binh
					{
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_TRUNGBINH.Checked = true;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_CODINH.Checked = false;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_THAPNHAT.Checked = false;
					}
					else
					{
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_THAPNHAT.Checked = true;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_CODINH.Checked = false;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_DA_TRUNGBINH.Checked = false;
					}
				}

				LVAuto.frmmain.LVFRMMAIN.rdBTNSatAnToanCoDinh.Checked = LVAuto.frmmain.BanTaiNguyen.SAT.BanCoDinh;
				LVAuto.frmmain.LVFRMMAIN.rdBTNSatPercentCoDinh.Checked = !LVAuto.frmmain.BanTaiNguyen.SAT.BanCoDinh;

				LVAuto.frmmain.LVFRMMAIN.txtCOUNTSAT.Text = LVAuto.frmmain.BanTaiNguyen.SAT.SoLuongBan.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtSAFESAT.Text = LVAuto.frmmain.BanTaiNguyen.SAT.SoLuongAnToan.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtBanTN_SAT_TB_Heso.Text = LVAuto.frmmain.BanTaiNguyen.SAT.GiaTri.ToString();
				LVAuto.frmmain.LVFRMMAIN.txtBanTN_SAT_TB_Heso.Enabled = true;
				if (LVAuto.frmmain.BanTaiNguyen.SAT.LoaiBan == 1)		// co dinh
				{
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_CODINH.Checked = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_TB_Cong.Enabled = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_TB_Nhan.Enabled = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_THAPNHAT.Checked = false;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_TRUNGBINH.Checked = false;
				}
				else
				{
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_TB_Cong.Enabled = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_TB_Nhan.Enabled = true;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_TB_Cong.Checked = !LVAuto.frmmain.BanTaiNguyen.SAT.CongThucNhan;
					LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_TB_Nhan.Checked = LVAuto.frmmain.BanTaiNguyen.SAT.CongThucNhan;
					if (LVAuto.frmmain.BanTaiNguyen.SAT.LoaiBan == 2)		// trung binh
					{
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_TRUNGBINH.Checked = true;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_CODINH.Checked = false;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_THAPNHAT.Checked = false;
					}
					else
					{
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_THAPNHAT.Checked = true;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_CODINH.Checked = false;
						LVAuto.frmmain.LVFRMMAIN.rdBanTN_SAT_TRUNGBINH.Checked = false;
					}
				}


							
			}
			catch (Exception ex)
			{
				LVAuto.frmmain.LVFRMMAIN.SellResource_loaded = false;
				error = "Không load được dữ liệu bán tài nguyên   \r\n";
			}
		}



		private void loadSellResource(ArrayList ardata)
		{
			try
			{

				string[] separ = new string[] { spareChar };
				string[] data;

				LVAuto.frmmain.BanTaiNguyen.CityInfo = new LVAuto.Command.CommonObj.BanTaiNguyenObj.cityInfo_[ardata.Count];

				LVAuto.Command.CommonObj.BanTaiNguyenObj.cityInfo_ cityinfo;

				for (int i = 0; i < ardata.Count; i++)
				{
					Application.DoEvents();

					cityinfo = new LVAuto.Command.CommonObj.BanTaiNguyenObj.cityInfo_();


					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					

					cityinfo.CityId  = int.Parse( data[1].ToString());
					cityinfo.CityName  = data[2].ToString();
					cityinfo.BanDa  = bool.Parse( data[3].ToString());
					cityinfo.BanGo  = bool.Parse(data[4].ToString());
					cityinfo.BanLua  = bool.Parse(data[5].ToString());
					cityinfo.BanSat= bool.Parse(data[6].ToString());

					LVAuto.frmmain.BanTaiNguyen.CityInfo[i] = cityinfo;
				}

			
				Common.common.LoadCityToGridForSellResource(LVAuto.frmmain.LVFRMMAIN.dtaSELL);
				LVAuto.frmmain.LVFRMMAIN.SellResource_loaded = true;
			}
			catch (Exception ex)
			{
				LVAuto.frmmain.LVFRMMAIN.SellResource_loaded = false;
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

				LVAuto.frmmain.MuaTaiNguyen.TimeToRunInMinute = int.Parse(data[1].ToString());
				LVAuto.frmmain.LVFRMMAIN.txtBUYRESOURCECHECK.Text = LVAuto.frmmain.MuaTaiNguyen.TimeToRunInMinute.ToString();

				LVAuto.frmmain.MuaTaiNguyen.VangAnToan = int.Parse(data[2].ToString());
				LVAuto.frmmain.LVFRMMAIN.txtSAFEGOLD.Text = LVAuto.frmmain.MuaTaiNguyen.VangAnToan.ToString();

				LVAuto.frmmain.MuaTaiNguyen.MuaTheoDonViKho = !bool.Parse(data[3].ToString());
				LVAuto.frmmain.LVFRMMAIN.rdMuaTNMuaTheoPhanTram.Checked = !LVAuto.frmmain.MuaTaiNguyen.MuaTheoDonViKho;
				LVAuto.frmmain.LVFRMMAIN.rdMuaTNMuaTheoDonVi.Checked = LVAuto.frmmain.MuaTaiNguyen.MuaTheoDonViKho;

				LVAuto.frmmain.MuaTaiNguyen.GiaTri = int.Parse(data[4].ToString());
				LVAuto.frmmain.LVFRMMAIN.txtMuaTNGioiHanMua.Text = LVAuto.frmmain.MuaTaiNguyen.GiaTri.ToString();
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

				Common.common.LoadCityToGridForBuyResource(LVAuto.frmmain.LVFRMMAIN.dtaBUYRESOURCE);

				DataGridView dg = LVAuto.frmmain.LVFRMMAIN.dtaBUYRESOURCE;

				LVAuto.frmmain.LVFRMMAIN.BuyResource_loaded = true;
				
				int cityid;
								
				for (int i = 0; i < ardata.Count; i++)
				{

					data = ((string)ardata[i]).Trim().Split(separ, StringSplitOptions.None);
					LVAuto.frmmain.LVFRMMAIN.txtBUYRESOURCECHECK.Text = data[1]; //time
					LVAuto.frmmain.LVFRMMAIN.txtSAFEGOLD.Text = data[2];
					cityid = int.Parse(data[3]);

					for (int j = 0; j < dg.Rows.Count; j++)
					{
						if (int.Parse(dg.Rows[j].Cells[0].Value.ToString()) == cityid)
						{
							dg.Rows[j].Cells[2].Value = bool.Parse(data[4].ToString());
							dg.Rows[j].Cells[3].Value = bool.Parse(data[5].ToString());
							dg.Rows[j].Cells[4].Value = bool.Parse(data[6].ToString());
							dg.Rows[j].Cells[5].Value = bool.Parse(data[7].ToString());
							setTextLoading("Đọc dữ liệu mua tài nguyên " + LVAuto.Command.City.GetCityByID(cityid).name  + " ....");
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
