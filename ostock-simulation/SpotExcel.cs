using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace OStock_Simulation
{
    class SpotExcel
    {
        private IWorkbook m_StrategyWorkBook = null;
        private ISheet m_StrategySheet = null;
        private string m_StrategyName;

        public SpotExcel(string sStrategyName, params object[] arg)
        {
            sStrategyName = string.Format(sStrategyName, arg);
            this.m_StrategyName = sStrategyName;
        }

        public bool Init()
        {
            if (!File.Exists(m_StrategyName))
            {
                m_StrategyWorkBook = new XSSFWorkbook();
                m_StrategySheet = (ISheet)m_StrategyWorkBook.CreateSheet("Sheet1");

                IRow Row = m_StrategySheet.CreateRow(0);
                Row.CreateCell(0).SetCellValue("-500");
                Row.CreateCell(1).SetCellValue("-450");
                Row.CreateCell(2).SetCellValue("-400");
                Row.CreateCell(3).SetCellValue("-350");
                Row.CreateCell(4).SetCellValue("-300");
                Row.CreateCell(5).SetCellValue("-250");
                Row.CreateCell(6).SetCellValue("-200");
                Row.CreateCell(7).SetCellValue("-150");
                Row.CreateCell(8).SetCellValue("-100");
                Row.CreateCell(9).SetCellValue("-50");
                Row.CreateCell(10).SetCellValue("0");
                Row.CreateCell(11).SetCellValue("50");
                Row.CreateCell(12).SetCellValue("100");
                Row.CreateCell(13).SetCellValue("150");
                Row.CreateCell(14).SetCellValue("200");
                Row.CreateCell(15).SetCellValue("250");
                Row.CreateCell(16).SetCellValue("300");
                Row.CreateCell(17).SetCellValue("350");
                Row.CreateCell(18).SetCellValue("400");
                Row.CreateCell(19).SetCellValue("450");
                Row.CreateCell(20).SetCellValue("500");               
                return true;
            }
            return false;
        }

        public void Write(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11)
        {
            IRow row = m_StrategySheet.CreateRow(m_StrategySheet.LastRowNum + 1);
            row.CreateCell(0).SetCellValue(a0);
            row.CreateCell(1).SetCellValue(a1);
            row.CreateCell(2).SetCellValue(a2);
            row.CreateCell(3).SetCellValue(a3);
            row.CreateCell(4).SetCellValue(a4);
            row.CreateCell(5).SetCellValue(a5);
            row.CreateCell(6).SetCellValue(a6);
            row.CreateCell(7).SetCellValue(a7);
            row.CreateCell(8).SetCellValue(a8);
            row.CreateCell(9).SetCellValue(a9);
            row.CreateCell(10).SetCellValue(a10);
            row.CreateCell(11).SetCellValue(a11);
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
