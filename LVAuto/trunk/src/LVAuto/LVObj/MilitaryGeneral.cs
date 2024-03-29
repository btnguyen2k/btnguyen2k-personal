using System;
using System.Collections.Generic;
using System.Text;

namespace LVAuto.LVObj
{
	public class MilitaryGeneral : General
	{
		public cMilitary Military = new cMilitary();

		public int ChiSoSucManh;
		public int ChiSoNhanhNhen;
		public int ChiSoThongLinh;

		public int MuuKeTrongChienTranID = 0;
		public string MuuKeTrongChienTranName = "";
		
		public int MuuKeTruocKhiDiID = 0;
		public string MuuKeTruocKhiDiName = "";

		public int PhuongThucTanCongID = 0;
		public string PhuongThucTanCongName = "";

		public int PhuongThucChonMucTieuID = 0;
		public string PhuongThucChonMucTieuName = "";

		public bool TuDoiTranHinhKhac = true;
		public bool TuKhoiPhucTrangThai = false;

		public int TimeDoiTranHinhMinutes = 300;

		public int SiKhiMinToGo = 80;
		public int SoLuongQuanMinToGo = 20000;
		public bool TuBienCheQuan = true;
		public bool TuUpSiKhi = true;

		public MilitaryGeneral()
		{
			
		}


		public class cMilitary
		{
			private int _tranHinh = 0;			// trận hình BatleArray
			
			/// <summary>
			/// Trận hình khắc với trận hình quân đội đang có (dùng đối với địch). Ví dụ: trận hình đang có là hạng dực thì TranHinhKhac là câu hình.
			/// </summary>
			public int TranHinhKhac = 0;			// Trận hình khắc với trận hình của quân đội đang có, dùng đối với địch. 
													// Ví Hiện dụ: hiện là Truỳ hình thì khắc là hạng dực
			/// <summary>
			/// Trạng thái quân đội hiện tại: hoảng sợ, hỗi loạn ...
			/// </summary>
			public int TrangThaiQuanDoi = LVAuto.LVObj.MilitaryGeneral.TroopStatus.BinhThuong;
			
			// Thiết lập tự động tác chiến
			public int RatioAttack;		//Binh thường
			public int RatioPK;			// Đơn đấu	20
			public int RatioStratagem;			// Mưu kế	70
			
			// Thiết lập tự động rút quân
			public int WithdrawLoss;		//Thiệt hại tối thiểu	30
			public int WithdrawMorale;		//Sỹ khí tối thiểu	40


			public int SoQuanCamDuoc;
			public int SoQuanDangCo;
			public int SyKhi =-1;
			public int TanBinh;
			public int[] Bobinh = new int[4]{0,0,0,0};	//[0]: so luong quan dang co, [1]: loai vu khi thu 1; [2]: loai vu khi thu 2; [3]: loai vu khi thu 3 
			public int[] KyBinh = new int[4] {0, 0, 0, 0 };	//[0]: so luong quan dang co, [1]: loai vu khi thu 1; [2]: loai vu khi thu 2; [3]: loai vu khi thu 3 
			public int[] CungThu = new int[4] {0, 0, 0, 0 };	//[0]: so luong quan dang co, [1]: loai vu khi thu 1; [2]: loai vu khi thu 2; [3]: loai vu khi thu 3 
			public int[] Xe = new int[4] {0, 0, 0, 0 };	//[0]: so luong quan dang co, [1]: loai vu khi thu 1; [2]: loai vu khi thu 2; [3]: loai vu khi thu 3 

			public int TranHinh
			{
				get
				{
					return _tranHinh;
				}
				set
				{
					_tranHinh = value;
					TranHinhKhac = LVAuto.LVForm.LVCommon.WarFunc.GetTranHinhKhac(_tranHinh);
				}

			}
		}

		// 
		/// <summary>
		/// sắp xếp theo tổng số quân giảm dần
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override int CompareTo(object obj)
		{
			if (obj is MilitaryGeneral)
			{
				MilitaryGeneral temp = (MilitaryGeneral)obj;

				return temp.Military.SoQuanDangCo.CompareTo(this.Military.SoQuanDangCo);
			}
			throw new ArgumentException("object is not a Temperature");
		}

	}
}
