using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace LVAuto.LVForm.LVThread {
    class UPDATEPRICE {
        public Thread InThread; 
		public string threadID;
        public bool IsRun = false;
        public int pricelua = 0;
        public int pricego = 0;
        public int priceda = 0;
        public int pricesat = 0;
        public UPDATEPRICE(int pricelua,int pricego,int priceda,int pricesat) {
            this.priceda = priceda;
            this.pricego = pricego;
            this.pricesat = pricesat;
            this.pricelua = pricelua;
        }
        public void GetParameter() {
        }

		private void mainprocess()
		{
            try
            {
                //lock (LVAuto.Web.LVWeb.islock)
                {
                    if (Command.Build.SelectBuilding(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[0].Id, 11, LVAuto.LVWeb.LVClient.CurrentLoginInfo.MakeCookiesString(LVConfig.AutoConfig.CONFIG_CITY_CONSTRUCT.AllCities[0].Id)))
                    {
                        Command.OPT.UpdateAllPrice(pricelua, 1);
                        Command.OPT.UpdateAllPrice(pricego, 2);
                        Command.OPT.UpdateAllPrice(priceda, 3);
                        Command.OPT.UpdateAllPrice(pricesat, 4);
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                try
                {
                    LVCommon.ThreadManager.RemoveThread(threadID);
                }
                catch (Exception ex) { }
            }
		}
        public void run() 
		{
            IsRun = true;
			threadID = "UPDATEPRICE_" + DateTime.Now.Ticks;
			LVCommon.ThreadManager.TakeResourceAndRun(threadID, mainprocess);
			LVCommon.ThreadManager.RemoveThread(threadID);
			IsRun = false;
        }
		public void Auto()
		{
			if (!IsRun)
			{
				InThread = new Thread(new ThreadStart(run));
				IsRun = true;  InThread.Start();
			}
		}
        public void Stop() 
		{
            if (IsRun) 
			{
                InThread.Abort();
                InThread.Join();
				LVCommon.ThreadManager.RemoveThread(threadID);
				IsRun = false;
            }
        }
    }
}
