using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;

namespace OStock_Simulation
{
    class TechPointer
    {
        public static string OPEN = "open";
        public static string HIGH = "high";
        public static string LOW = "low";
        public static string CLOSE = "close";
        
        private ISheet m_BiasSheet;

        public TechPointer()
        {
            
        }

        public void LoadExcel(ISheet BiasSheet)
        {
            this.m_BiasSheet = BiasSheet;
        }        

        // 取期貨MA, 假設DateStrategy[nKey]是2015/04/22, 算出的MA5為2015/04/21收盤時的值
        //
        public double GetMA(int nDay, int nKey, Dictionary<int, DateStrategyMap> DateStrategy)
        {
            double dResult = 0;

            if (nKey <= nDay)
                return 0;

            for (int i = 0; i < nDay; i++)
            {
                DateStrategyMap dsMap = DateStrategy[nKey - nDay + i];
                dResult += dsMap.FuturesClose;
            }

            dResult = dResult / nDay;
            return dResult;
        }

        public double Get5KMA(int nBar, DateStrategyMap DateStrategy, DateTime Time)
        {
            if(nBar > 60)
                return 0;

            double dResult = 0;
            Time = Time.AddMinutes(-5);
            for (int i = 0; i < nBar; i++) 
            {
                double dClose = DateStrategy.Get5KClose(Time);
                if (dClose == 0)
                    return 0;
                dResult += dClose;
                Time = Time.AddMinutes(-5);
            }

            dResult = dResult / nBar;
            return dResult;
        }
    }
}
