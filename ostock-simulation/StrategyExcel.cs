using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace OStock_Simulation
{
    public enum StrategyDirection : int
    {
        Bull = 1,
        Bear = 2
    }

    class StrategyExcel
    {
        private Config m_Config = null;
        private IWorkbook m_StrategyWorkBook = null;
        private ISheet m_StrategySheet = null;
        private string m_StrategyName;

        public StrategyExcel(Config config, string sStrategyName, params object[] arg)
        {
            m_Config = config;
            sStrategyName = string.Format(sStrategyName, arg);
            m_StrategyName = string.Format("{0}\\{1}", m_Config.OutputDirectory, sStrategyName);
        }

        public bool Init()
        {
            if (!File.Exists(m_StrategyName))
            {
                m_StrategyWorkBook = new XSSFWorkbook();
                m_StrategySheet = (ISheet)m_StrategyWorkBook.CreateSheet("Sheet1");

                IRow Row = m_StrategySheet.CreateRow(0);
                Row.CreateCell(0).SetCellValue("策略名稱");
                Row.CreateCell(1).SetCellValue("停利");
                Row.CreateCell(2).SetCellValue("停損");
                Row.CreateCell(3).SetCellValue("總次數");
                Row.CreateCell(4).SetCellValue("成功機率");
                Row.CreateCell(5).SetCellValue("期望值");
                Row.CreateCell(6).SetCellValue("近2年次數");
                Row.CreateCell(7).SetCellValue("近2年成功率");
                Row.CreateCell(8).SetCellValue("近2年期望值");
                Row.CreateCell(9).SetCellValue("多/空");
                return true;
            }
            return false;
        }

        public void Write(string sStrategyName, int nSG, int nSL, int nAT, double dSR, double dEV, int n2AT, double d2SR, double d2EV, int nDirection) 
        {
            IRow row = m_StrategySheet.CreateRow(m_StrategySheet.LastRowNum + 1);
            row.CreateCell(0).SetCellValue(sStrategyName);
            row.CreateCell(1).SetCellValue(nSG);
            row.CreateCell(2).SetCellValue(nSL);
            row.CreateCell(3).SetCellValue(nAT);
            row.CreateCell(4).SetCellValue(dSR);
            row.CreateCell(5).SetCellValue(dEV);
            row.CreateCell(6).SetCellValue(n2AT);
            row.CreateCell(7).SetCellValue(d2SR);
            row.CreateCell(8).SetCellValue(d2EV);
            row.CreateCell(9).SetCellValue(nDirection);
        }

        public void Close() 
        {
            using (FileStream fs = new FileStream(m_StrategyName, FileMode.Create, FileAccess.ReadWrite))
            {
                if (m_StrategyWorkBook != null)
                {
                    m_StrategyWorkBook.Write(fs);
                    //m_StrategyWorkBook = null;
                    //m_StrategySheet = null;
                }
            }
        }        
    }
}
