using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace LVAuto.Common
{
	public class BienCheQuan
	{
		public static bool BienChe(int cityid, int genaralid, int bobinh, int kybinh, int cungthu, int xe)
		{
			try
			{
				// không biên chế nếu là trại
				if (cityid < 0) return false;

				// lay thong tin ve tuong
				LVAuto.Command.CityObj.MilitaryGeneral geninfo = Command.Common.GetGeneralMilitaryInfoEx(cityid, genaralid);
				if (geninfo == null) return false;

				return BienChe(geninfo, bobinh, kybinh, cungthu, xe);		
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public static bool BienChe(LVAuto.Command.CityObj.MilitaryGeneral geninfo, int bobinh, int kybinh, int cungthu, int xe)
		{
			try
			{
				int cityid = geninfo.CityID;
				if (cityid < 0) return false;  // khong bien che khi o trai

				int genaralid = geninfo.GeneralId;
				Hashtable hasTemp;


				// lay thong tin ve tuong
				//LVAuto.Command.CityObj.General geninfo = Command.Common.GetGeneralMilitaryInfoEx(cityid, genaralid);
				//if (geninfo == null) return false;
				

				//geninfo.GeneralId = genaralid; 
				int citypost = LVAuto.Command.City.GetCityPostByID(cityid);
				if (citypost == -1) return false;

				int soluonglinhcanbuild = bobinh + kybinh + cungthu + xe * 3;
				if (geninfo.Military.SoQuanDangCo >= soluonglinhcanbuild) return false;


				int bobinh2 = bobinh <= geninfo.Military.Bobinh[0] ? 0 : bobinh - geninfo.Military.Bobinh[0];
				int kybinh2 = kybinh <= geninfo.Military.KyBinh[0] ? 0 : kybinh - geninfo.Military.KyBinh[0];
				int cungthu2 = cungthu <= geninfo.Military.CungThu[0] ? 0 : cungthu - geninfo.Military.CungThu[0];
				int xe2 = xe <= geninfo.Military.Xe[0] ? 0 : xe - geninfo.Military.Xe[0]; ;

				soluonglinhcanbuild = bobinh2 + kybinh2 + cungthu2 + xe2 * 3;

				if (soluonglinhcanbuild > geninfo.Military.SoQuanCamDuoc - geninfo.Military.SoQuanDangCo) soluonglinhcanbuild = geninfo.Military.SoQuanCamDuoc - geninfo.Military.SoQuanDangCo;
				if ( geninfo.Military.SoQuanDangCo >= geninfo.Military.SoQuanCamDuoc) return false;
				
				string cookies = LVAuto.Web.LVWeb.CurrentLoginInfo.MakeCookiesString(cityid);

				//if (soluonglinhcanbuild > geninfo.Military.SoQuanCamDuoc - geninfo.Military.SoQuanDangCo)
				//	soluonglinhcanbuild = geninfo.Military.SoQuanCamDuoc - geninfo.Military.SoQuanDangCo;

				//soluonglinhcanbuild = soluonglinhcanbuild - geninfo.Military.SoQuanDangCo;

			

				//int tongsoycbuild = bobinh + kybinh + cungthu + xe*3;

				int danfree;
				int dandangco;
				int maxdan;
				int dangtang;
				double rate;
				if (geninfo.Military.TanBinh < soluonglinhcanbuild)
				{
					
					// thieu tan binh, kiem tra dan free
					hasTemp = Command.City.Execute(6, "", true, cookies);
					ArrayList population= (ArrayList) hasTemp["population"];

					 danfree = int.Parse(population[4].ToString());
					 dandangco = int.Parse(population[0].ToString());
					 maxdan = int.Parse(population[1].ToString());
					 dangtang = int.Parse(population[6].ToString());

					if (danfree + geninfo.Military.TanBinh < soluonglinhcanbuild)
					{
						if (dangtang == 1)
						{
							// thu an ui de lay them dan
							ArrayList aino = (ArrayList)hasTemp["morale"];
							if (Command.Build.SelectBuilding(cityid, 1, 8, 0, cookies))
							{
								if (int.Parse(aino[2].ToString()) > 0)
								{
									Command.OPT.QuanPhuAnUi(3, cookies);
								}
								else
								{
									Command.OPT.QuanPhuAnUi(4, cookies);
								}

								// thieu tan binh, kiem tra dan free
								hasTemp = Command.City.Execute(6, "", true, cookies);
								population = (ArrayList)hasTemp["population"];
							}
						}
					}

					

					danfree = int.Parse(population[4].ToString());
					dandangco = int.Parse(population[0].ToString());
					maxdan = int.Parse(population[1].ToString());
					dangtang = int.Parse(population[6].ToString());

					int danlamgo =0;
					int danlamda =0;
					int danlamsat =0;
					int danlamthuc =0;
					
					int laydanlamgo = 0;
					int laydanlamda = 0;
					int laydanlamsat = 0;
					int laydanlamthuc = 0;
					ArrayList temp;
					if (danfree + geninfo.Military.TanBinh < soluonglinhcanbuild)
					{
						// thieu dan, lay dan dang di lam mo						
						hasTemp = Command.Build.Execute(2, "gid=1&pid=-1&tab=1&tid=" + cityid, true, cookies);
						temp = (ArrayList) hasTemp["wood"];
                        if (temp.Count == 0)
                            danlamgo = 0;
                        else
                        {
                            for (int i = 0; i < temp.Count; i++)
                            {
                                danlamgo += int.Parse(((ArrayList)temp[i])[4].ToString());
                            }
                        }

						temp = (ArrayList)hasTemp["stone"];
						if (temp.Count == 0)
							danlamda = 0;
						else
                            for (int i = 0; i < temp.Count; i++)
                                danlamda += int.Parse(((ArrayList)temp[i])[4].ToString());
						
						temp = (ArrayList)hasTemp["iron"];
						if (temp.Count == 0)
							danlamsat = 0;
						else
                            for (int i = 0; i < temp.Count; i++)
							    danlamsat += int.Parse(((ArrayList)temp[i])[4].ToString());

						temp = (ArrayList)hasTemp["food"];
						if (temp.Count == 0)
							danlamthuc = 0;
						else
                            for (int i = 0; i < temp.Count; i++)
							    danlamthuc += int.Parse(((ArrayList)temp[i])[4].ToString());

					}

					int needmore = soluonglinhcanbuild - (danfree + geninfo.Military.TanBinh);
					int totalhave = danlamgo + danlamda + danlamsat;
					
					laydanlamgo = danlamgo;
					laydanlamda = danlamda;
					laydanlamsat = danlamsat;

					if (needmore > 0 && totalhave > 0 )
					{
						if (needmore < totalhave)
						{
							rate = (double)needmore / (double)totalhave;
							laydanlamgo = (int)(Math.Round((decimal)(rate * danlamgo)));
							laydanlamda = (int)(Math.Round((decimal)(rate * danlamda)));
							laydanlamsat = (int)(Math.Round((decimal)(rate * danlamsat))); ;

						}

						// thu lay them dan o mo
						if (laydanlamgo > 0)
							LVAuto.Command.OPT.UpdateSoluongDanCongLamMo(cityid, Command.OPT.LoaiMo.Go, danlamgo - laydanlamgo, cookies);
						
						if (laydanlamda > 0)
							LVAuto.Command.OPT.UpdateSoluongDanCongLamMo(cityid, Command.OPT.LoaiMo.Da, danlamda - laydanlamda, cookies);

						if (laydanlamsat > 0) 
							LVAuto.Command.OPT.UpdateSoluongDanCongLamMo(cityid, Command.OPT.LoaiMo.Sat, danlamsat - laydanlamsat, cookies);

						// lay dan lam tan binh
						// thieu tan binh, kiem tra dan free
						hasTemp = Command.City.Execute(6, "", true, cookies);
						population = (ArrayList)hasTemp["population"];
					
					}


					danfree = int.Parse(population[4].ToString());
					int chieumo = soluonglinhcanbuild -  geninfo.Military.TanBinh;
					if (chieumo > danfree) chieumo = danfree;
					if (chieumo > 0)
					{
						LVAuto.Command.OPT.ChieuMoTanBinh(cityid, chieumo, cookies);
					}

				} //---if (geninfo.Military.TanBinh < soluonglinhcanbuild)   Da du tan binh 

				geninfo = Command.Common.GetGeneralMilitaryInfoEx(cityid, genaralid);
				if (geninfo == null) return false;

				rate = (double)soluonglinhcanbuild / (double) (bobinh2 + kybinh2 + cungthu2 + xe2 * 3);
				bobinh2 = (int) Math.Round((decimal) ( rate * bobinh2));
				kybinh2 = (int)Math.Round((decimal)(rate * kybinh2));
				cungthu2 = (int)Math.Round((decimal)(rate * cungthu2));
				xe2 = (int)Math.Round((decimal)(rate * xe2));
				
				soluonglinhcanbuild = bobinh2 + kybinh2 + cungthu2 + xe2 * 3;
				
				// khoong du tan binh
				if (geninfo.Military.TanBinh < soluonglinhcanbuild)
				{
					rate = (double)((double)geninfo.Military.TanBinh / (double)soluonglinhcanbuild);
					bobinh2 =(int) Math.Round((decimal) (rate * bobinh2));
					kybinh2 = (int)Math.Round((decimal)(rate * kybinh2));
					cungthu2 = (int)Math.Round((decimal)(rate * cungthu2));
					xe2 = (int) Math.Round((decimal)(rate * xe2));
				}

				if (bobinh2 > 0) LVAuto.Command.OPT.UpdateSoluongLinh(cityid, genaralid, Command.OPT.LoaiQuan.Bobinh, bobinh2, cookies);
				if (kybinh2 > 0) LVAuto.Command.OPT.UpdateSoluongLinh(cityid, genaralid, Command.OPT.LoaiQuan.Kybinh, kybinh2, cookies);
				if (cungthu2 > 0) LVAuto.Command.OPT.UpdateSoluongLinh(cityid, genaralid, Command.OPT.LoaiQuan.Cungthu, cungthu2, cookies);
				if (xe2 > 0) LVAuto.Command.OPT.UpdateSoluongLinh(cityid, genaralid, Command.OPT.LoaiQuan.Xe, xe2, cookies);
				return true;
			}				
			catch (Exception ex)
			{
				return false;
			}
		}

		public static void BienChe(ArrayList danhsachtuong, int mucnangsk)
		{
			try
			{
				Common.BienChe oneBienche  ;
				string cookies;
				int mucnang = mucnangsk;
				int sk;
				for (int i=0; i < danhsachtuong.Count; i++)
				{
					oneBienche = (Common.BienChe) danhsachtuong[i];
					cookies = LVAuto.Web.LVWeb.CurrentLoginInfo.MakeCookiesString(oneBienche.cityid);
					if (oneBienche.cityid > 0)	// chi bieen che khi o thanh		
					{
						BienChe(oneBienche.cityid, oneBienche.generalid, oneBienche.bobinhamount, oneBienche.kybinhamount, oneBienche.cungthuamount, oneBienche.xemount);
						// nang sy khi
					}
					if (oneBienche.nangsykhi)
					{

						LVAuto.Command.City.SwitchCitySlow(oneBienche.cityid);

						//sk = Command.Common.GetGeneralSyKhiInLuyenBinh(oneBienche.cityid, oneBienche.generalid);

						//if (sk >= 100 || sk < 0) continue;
						

						if (oneBienche.cityid > 0 )
						{
							//if (Command.Build.SelectBuilding(oneBienche.cityid, 16, cookies))
							{
								Command.OPT.UpSiKhi(oneBienche.cityid, oneBienche.generalid, mucnang);
							}
						}
						else
						{
							//if (Command.Build.SelectBuilding(oneBienche.cityid, 19, cookies))
							{
								Command.OPT.UpSiKhi(oneBienche.cityid, oneBienche.generalid, mucnang);
							}
						}

					}
				}			

			}
			catch (Exception ex)
			{
			}
		}
	}
}
