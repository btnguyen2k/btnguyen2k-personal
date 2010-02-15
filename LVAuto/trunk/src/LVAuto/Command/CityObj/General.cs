using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LVAuto.LVForm.Command.CityObj
{
	public class General : IComparable 
	{
        public const int GENERAL_TYPE_CIVIL    = 1; //quan văn
        public const int GENERAL_TYPE_MILITARY = 2; //quan võ

        /// <summary>
        /// General's unique id.
        /// </summary>
		public int Id;
        /// <summary>
        /// General's name
        /// </summary>
		public string Name;
        /// <summary>
        /// General's historical name
        /// </summary>
		public string HistoricalName;

        /// <summary>
        /// Id of the city that the general is currently in
        /// </summary>
		public int CityId;          
        /// <summary>
        /// Name of the city that the general is currently in
        /// </summary>
		public string CityName;
		
        /// <summary>
        /// General's status (normal, busy, etc)
        /// </summary>
		public int Status;		
        /// <summary>
        /// General'type (civil or military)
        /// </summary>
		public int Type;		

        /// <summary>
        /// General's position (free, strategist, etc)
        /// </summary>
        public int Position;

		public int Level;

		public int LoyaltyLevel;

		public int CurrentHp;
		public int MaxHp;

		public int PointsLeft;

		public override string ToString()
		{
			/*
			string st = "";

			st = CityName + " - " + GeneralName;

			if (GeneralHistroryName != "")
				st += "(" + GeneralHistroryName + ")";

			if (ChucTuoc != 0)
				st += " - " + GetChucTuocName(ChucTuoc);

			if (GeneralStatus != 0)
				st  += " - " + GetTrangThaiName(GeneralStatus);




			return st;
			 * */

			return Name;
		}

		public struct GeneralStatus
		{
			public const int Binhthuong		= 0;
			public const int TrongThuong	= 1;
			public const int TriThuong		= 2;
			public const int XuatChinh		= 16;
			public const int DiChiVien		= 17;
			public const int LoiDai			= 18;
			public const int DieuPhai		= 21;
			public const int NhiemVu		= 24; 
			public const int ChiVien        = 29;

		}

		public static string GetGeneralStatusName(int status)
		{
            switch (status)
            {
                case GeneralStatus.Binhthuong:
                    return "Bình thường";

                case GeneralStatus.TrongThuong:
                    return "Trọng thương";

                case GeneralStatus.TriThuong:
                    return "Trị thương";

                case GeneralStatus.XuatChinh:
                    return "Xuất chinh";

                case GeneralStatus.DiChiVien:
                    return "Đi Chi viện";

                case GeneralStatus.LoiDai:
                    return "Lôi đài";

                case GeneralStatus.DieuPhai:
                    return "Điều phái";

                case GeneralStatus.NhiemVu:
                    return "Nhiệm vụ";

                case GeneralStatus.ChiVien:
                    return "Chi viện";

                default:
                    return "Unknow";
            }
		}

		public struct TroopStatus
		{
			public const int BinhThuong  = 0;
			public const int HonLoan     = 1;
			public const int SoHai       = 4;	
			public const int ThieuKiLuat = 3;
		}

		public static string GetTroopStatusName(int status)
		{
			switch (status)
			{
				case TroopStatus.BinhThuong:
                    return "Bình thường";

				case TroopStatus.HonLoan:
					return "Hỗn loạn";

				case TroopStatus.ThieuKiLuat:
					return "Thiếu kỉ luật";

				case TroopStatus.SoHai:
					return "Sợ hãi";

				default:
					return "Unknow";
			}
		}

		public static double GetTroopRateByStatus(int status)
		{
			switch (status)
			{
				case TroopStatus.BinhThuong:
                    return 1.0;

				case TroopStatus.HonLoan:
					return 0.1;

				case TroopStatus.ThieuKiLuat:
					return 0.8;

				case TroopStatus.SoHai:
					return 0.8;

				default:
					return 1.0;
			}
		}

		public struct CivilGeneralPosition
		{
			public const int NhanRoi = 0;
			public const int ThaiThu = 1;
			public const int QuanSu  = 2;
			public const int MuuSy   = 3;
		}

		public static string GetCivilGeneralPositionName(int position)
		{
            switch (position)
			{
                case CivilGeneralPosition.NhanRoi:
					return "Nhàn rỗi";
					
				case CivilGeneralPosition.ThaiThu:
					return "Thái thú";

				case CivilGeneralPosition.QuanSu:
					return "Quân Sư";

				case CivilGeneralPosition.MuuSy:
					return "Mưu Sỹ";

				default:
					return "Unknow";
			}
		}

		public virtual int CompareTo(object obj)
		{
			if (obj is General)
			{
				General temp = (General)obj;

                if (this.CityId != temp.CityId)
                {
                    return this.CityId.CompareTo(temp.CityId);
                }
                else
                {
                    if (this.Type != temp.Type)
                    {
                        return this.Type.CompareTo(temp.Type);
                    }
                    else
                    {
                        return this.Id.CompareTo(temp.Id);
                    }
                }
			}
			throw new ArgumentException("Argument is not an instance of General!");
		}

        /// <summary>
        /// Checks if a military general is busy.
        /// </summary>
        /// <param name="militaryGeneral"></param>
        /// <param name="attackNumGenerals">number of my generals attacking one target</param>
        /// <returns></returns>
        public static bool IsMilitaryGeneralBusy(MilitaryGeneral militaryGeneral, int attackNumGenerals)
        {
            //IList<MilitaryGeneral> tmp = new List<MilitaryGeneral>();
            ArrayList tmp = new ArrayList();
            tmp.Add(militaryGeneral);
            return AreMilitaryGeneralsBusy(tmp, attackNumGenerals);
        }

        /// <summary>
        /// Checks if all specified military generals are busy.
        /// </summary>
        /// <param name="militaryGenerals"></param>
        /// <param name="attackNumGenerals">number of my generals attacking one target</param>
        /// <returns></returns>
        public static bool AreMilitaryGeneralsBusy(ArrayList militaryGenerals, int attackNumGenerals)
		{
            int cityid = 0;
            int genid = 0;
            Command.CityObj.MilitaryGeneral callmanobj;
            ArrayList geninfo;

            bool ret = false;

            Hashtable result;
            ArrayList temp;

            try
            {
                int genIDtmp = 0;
                for (int genidx = 0; genidx < militaryGenerals.Count; genidx++)
                {

                    callmanobj = (Command.CityObj.MilitaryGeneral)militaryGenerals[genidx];

                    if (callmanobj.Id == genIDtmp) continue;
                    genIDtmp = callmanobj.Id;

                    cityid = callmanobj.CityId;
                    LVAuto.LVForm.Command.CityObj.City cityByID = LVAuto.LVForm.Command.City.GetCityByID(cityid);

                    string cookies = LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(cityid);
                    LVAuto.LVForm.Command.City.SwitchCitySlow(cityid);



                    int status;
                    int time;		//s
                    int battleid;

                    string para;
                    // check xem co đang đánh nhau khong
                    if (cityid > 0)                             // thien thanh gid=69
                        if (cityByID.size > 3)
                            para = "gid=69&pid=-1&tab=1&tid=0";
                        else
                            para = "gid=16&pid=-1&tab=1&tid=0";
                    else
                        para = "gid=19&pid=-1&tab=1&tid=" + cityid;

                    Hashtable result1 = LVAuto.LVForm.Command.Build.Execute(2, para, true, cookies);
                    if (result1 != null && result1["ret"].ToString() == "0")
                    {
                        ArrayList battle = (ArrayList)result1["battle"];
                        for (int ii = 0; ii < battle.Count; ii++)
                        {
                            temp = (ArrayList)battle[ii];
                            battleid = int.Parse(temp[0].ToString());

                            // lay thong tin chien truong
                            para = "lBattleID=" + battleid;
                            result = LVAuto.LVForm.Command.Common.Execute(4, para, true, cookies);
                            if (result != null)
                            {
                                // check xem tướng mình có đang đánh nhau không
                                ArrayList myside = (ArrayList)result["myside"];
                                for (int i = 0; i < myside.Count; i++)
                                {
                                    temp = (ArrayList)myside[0];
                                    status = int.Parse(temp[0].ToString());
                                    genid = int.Parse(temp[13].ToString());
                                    battleid = int.Parse(result["battle_id"].ToString());
                                    if (callmanobj.Id == genid)
                                    {
                                        //SetText("Tướng " + callmanobj.GeneralName + " đang đi...");
                                        //LVAuto.LVThread.AUTOFIGHTING.startBattle(0, time, 1, 250, Common.WarFunc.PHUONGTHUCTANCONG.HoaTien );
                                        //LVAuto.LVThread.AUTOFIGHTING.startBattle(battleid, 0, Callmanobj.SoTuongMinhDanh1TuongDich, 300, Callmanobj.PhuongThucTanCongID, Callmanobj.PhuongThucChonMucTieuID);
                                        LVAuto.LVForm.LVThread.AUTOFIGHTING.startBattle(battleid, 0, attackNumGenerals, militaryGenerals);
                                        ret = true;
                                        break;
                                        //return true;
                                    }

                                }
                            }// end if (result != null)						
                        } // end 					for (int i = 0; i < battle.Count; i++)

                    }


                    // kiem tra xem co dang di khong
                    para = "tid=0";
                    if (cityid > 0)
                        para = "tid=0";
                    else
                        para = "tid=" + cityid;


                    result = LVAuto.LVForm.Command.Common.Execute(40, para, true, cookies);
                    //result = LVAuto.Command.City.GetCityTask(cookies);
                    if (result != null)
                    {
                        ArrayList gotobat = (ArrayList)result["goto"];
                        for (int i = 0; i < gotobat.Count; i++)
                        {
                            ArrayList oneGotobat = (ArrayList)gotobat[i];
                            status = int.Parse(oneGotobat[1].ToString());       // sai, cos the la 2 
                            time = int.Parse(oneGotobat[13].ToString());		//s
                            genid = int.Parse(((ArrayList)(((ArrayList)oneGotobat[8])[0]))[0].ToString());
                            battleid = int.Parse(oneGotobat[4].ToString());         // nếu là đánh địa tinh thì id=0, kiểm tra [1]=14: địa tinh; =1: đánh man
                            if (callmanobj.Id != genid) continue;

                            if (int.Parse(oneGotobat[1].ToString()) == 14)      // DDiaj tinh
                            {
                                int x = int.Parse(oneGotobat[6].ToString());
                                int y = int.Parse(oneGotobat[7].ToString());
                                // laays thong tin battleID cua dia tinh
                                Hashtable tmp = Command.Common.Execute(98, "DestX=" + x + "&DestY=" + y, true);
                                if (tmp != null && tmp["ret"].ToString() == "0")
                                {
                                    battleid = int.Parse(tmp["npc_id"].ToString());
                                }

                            }


                            if (status != 0)			// Đang di hoặc đang về thao phat	 1: dang di, 2: dang danh, 3: đang trở về
                            {
                                //SetText("Tướng " + callmanobj.GeneralName + " đang đi...");

                                if (status == 2) time = 0;
                                //if (battleid != 0 && (status == 1 || status == 2))
                                if (battleid != 0)
                                {

                                    //LVAuto.LVThread.AUTOFIGHTING.startBattle(0, time, 1, 250, Common.WarFunc.PHUONGTHUCTANCONG.HoaTien );
                                    //LVAuto.LVThread.AUTOFIGHTING.startBattle(battleid, time, Callmanobj.SoTuongMinhDanh1TuongDich, 300, Callmanobj.PhuongThucTanCongID, Callmanobj.PhuongThucChonMucTieuID);
                                    LVAuto.LVForm.LVThread.AUTOFIGHTING.startBattle(battleid, time, attackNumGenerals, militaryGenerals);

                                }

                                ret = true;
                                break;
                                //return true;
                            }
                        }

                    }
                }
                return ret;

            }
            catch (Exception ex)
            {
                return ret;
            }
		}


	}
}
