using System;
using System.Collections.Generic;
using System.Text;



namespace LVAuto.Command.OPTObj 
{
	public class BattleField 
	{
        public int Battleid = 0;
        public int Timeleft = 0;		// giây
		public int MapID;
		public bool PheTanCong;			// true: thuộc phe tấn công, false: thuộc phe phòng thủ

		//public int SoTuongDanh1TuongDich= 1;		//0: mục tiêu tùy chọn, 1: 1&1, 2: 2&1: 3: 3&1 ............. 
		
		public GeneralInCombat[] allattacktroops;
		public GeneralInCombat[] alldefendtroops;

		public GeneralInCombat[] myattacktroops;
		public GeneralInCombat[] mydefendtroops;

		public GeneralInCombat[] mytroops;
		public GeneralInCombat[] enemytroops;

    }
}
