using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.OpenXml4Net;

namespace OStock_Simulation
{
    class OrderLog
    {
        private IWorkbook m_OrderLogWorkBook = null;
        private ISheet m_OrderLogSheet = null;
        private string m_sFileName;

        public OrderLog(string sFileName)
        {
            this.m_sFileName = sFileName;
            LoadExcel();
        }

        public void Log(string sDate, string sStrategyName, int nStrategySG, int nStrategySL, StrategyDirection eStrategyDirection, int nQT, double dPrice, double dProfit)
        {
            IRow Row = m_OrderLogSheet.CreateRow(m_OrderLogSheet.LastRowNum + 1);
            Row.CreateCell(0).SetCellValue(sDate);
            Row.CreateCell(1).SetCellValue(sStrategyName);
            Row.CreateCell(2).SetCellValue(nStrategySG);
            Row.CreateCell(3).SetCellValue(nStrategySL);
            Row.CreateCell(4).SetCellValue((int)eStrategyDirection);
            Row.CreateCell(5).SetCellValue(nQT);           
            Row.CreateCell(6).SetCellValue(dPrice);
            Row.CreateCell(7).SetCellValue(dProfit);
        }

        public void LogResult(int nTotalTriggerCount, int nTotalSuccessCount, int nBothBullBearCount, double dSR, double dEV, double dTotalProfit)
        {
            IRow Row = m_OrderLogSheet.CreateRow(m_OrderLogSheet.LastRowNum + 1);
            Row.CreateCell(0).SetCellValue("總次數");
            Row.CreateCell(1).SetCellValue("成功次數");
            Row.CreateCell(2).SetCellValue("多空皆觸發");
            Row.CreateCell(3).SetCellValue("成功率");
            Row.CreateCell(4).SetCellValue("期望值");
            Row.CreateCell(5).SetCellValue("總獲利");
            Row = m_OrderLogSheet.CreateRow(m_OrderLogSheet.LastRowNum + 1);
            Row.CreateCell(0).SetCellValue(nTotalTriggerCount);
            Row.CreateCell(1).SetCellValue(nTotalSuccessCount);
            Row.CreateCell(2).SetCellValue(nBothBullBearCount);
            Row.CreateCell(3).SetCellValue(dSR);
            Row.CreateCell(4).SetCellValue(dEV);
            Row.CreateCell(5).SetCellValue(dTotalProfit);
        }

        public void Save()
        {
            FileMode eMode = FileMode.OpenOrCreate;

            using (FileStream fs = new FileStream(m_sFileName, eMode, FileAccess.ReadWrite))
            {
                if (m_OrderLogWorkBook != null)
                {
                    m_OrderLogWorkBook.Write(fs);
                    m_OrderLogWorkBook = null;
                    m_OrderLogSheet = null;
                }
            }
        }

        public void LoadExcel()
        {            
            m_OrderLogWorkBook = new XSSFWorkbook();
            m_OrderLogSheet = (ISheet)m_OrderLogWorkBook.CreateSheet("Sheet1");

            IRow Row = m_OrderLogSheet.CreateRow(0);
            Row.CreateCell(0).SetCellValue("日期");
            Row.CreateCell(1).SetCellValue("策略");
            Row.CreateCell(2).SetCellValue("停利");
            Row.CreateCell(3).SetCellValue("停損");
            Row.CreateCell(4).SetCellValue("多空");
            Row.CreateCell(5).SetCellValue("口數");
            Row.CreateCell(6).SetCellValue("成交價");
            Row.CreateCell(7).SetCellValue("獲利");
            Row.CreateCell(8).SetCellValue("結果");
            Row.CreateCell(9).SetCellValue("其他");            
        }
    }
}