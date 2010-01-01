using System;
using System.Collections.Generic;
using System.Text;

namespace LVAuto.Common
{
	public class GeneralThaoPhat : Command.CityObj.MilitaryGeneral
	{

		public int NhiemVuThaoPhatID = 0;
		
		
		
		public int soluongtuongdanh1tuongdich = 1;

		public int BienCheBoBinhAmount;
		public int BienCheKyBinhAmount;
		public int BienCheCungThuAmount;
		public int BienCheXeAmount;
		/*
		public int cityid = 0;
        public int general1 = 0;
		public int general2 = 0;
        public int general3 = 0;
        public int general4 = 0;
        public int general5 = 0;
      
        
		
		
	
		public int phuonthuctancong = 0;
		public bool tudoitranhinh = true;
		public int phuongthucchonmuctieu = LVAuto.LVThread.AUTOFIGHTING.PHUONGTHUCCHONMUCTIEU.QuanDongNhat;
		public int  muuketrongchientruong = 0;
		public bool tukhoiphuctrangthai = false;
		*/
		public int timetorun = 1;	// phut

		public override string ToString()
		{
			string str = "Cử tướng " + this.GeneralName + " đi thảo phạt " + LVAuto.Command.CommonObj.ThaoPhat.GetNhiemVuName(this.NhiemVuThaoPhatID);
			str += ", sỹ khí min " + this.SiKhiMinToGo + ", quân min " + this.SoLuongQuanMinToGo;
			if (this.TuUpSiKhi) str += ", có nâng sk";
			else str += ", không nâng sk";

			if (!this.TuBienCheQuan)
				str += ", không biên chế quân";
			else
			{
				str += ". Biên chế: ";
				str += "Bộ: " + this.BienCheBoBinhAmount;
				str += ", Kỵ: " + this.BienCheKyBinhAmount;
				str += ", Cung: " + this.BienCheCungThuAmount;
				str += ", Xe: " + this.BienCheXeAmount;
			}

			str += ". " + this.soluongtuongdanh1tuongdich + " đánh 1 địch";
			str += ", tấn công " + this.PhuongThucTanCongName;
			str += ", chọn mục tiêu " + this.PhuongThucChonMucTieuName;
			str += ", mưu kế " + this.MuuKeTrongChienTranName;


			return str;
		}
	}
}
