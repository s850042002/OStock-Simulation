using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;

namespace OStock_Simulation
{    
    class ApplyStrategy
    {
        private Config m_Config = null;
        private IWorkbook m_StrategyWorkBook = null;
        private ISheet m_StrategySheet = null;
        private Dictionary<int, DateStrategyMap> m_DateStrategy;
        private OrderLog orderLog;
        private DateTime m_StartDate;
        private DateTime m_EndDate;
        private bool bBull = false, bBear = false;

        public ApplyStrategy(Config config, Dictionary<int, DateStrategyMap> DateStrategy, string sFilePath, DateTime StartDate, DateTime EndDate, bool bBull, bool bBear)
        {
            m_Config = config;
            m_DateStrategy = DateStrategy;
            LoadExcelFile(sFilePath);
            string sStrategyFileName = Path.GetFileName(sFilePath).Replace(".xlsx", "");
            m_StartDate = StartDate;
            m_EndDate = EndDate;
            string sStartDate = StartDate.ToString("yyyyMMdd");
            string sEndDate = EndDate.ToString("yyyyMMdd");
            string sOrderLogFileName = string.Format("{0}\\{1}_ApplyTo_{2}-{3}.xlsx", m_Config.OutputDirectory, sStrategyFileName, sStartDate, sEndDate);
            orderLog = new OrderLog(sOrderLogFileName);
            this.bBull = bBull;
            this.bBear = bBear;
        }

        public void ExecApply()
        {
            Form1.g_UI_ShowInfo("Applying Strategy back...");            

            if (m_StrategySheet != null)
            {
                double dTotalProfit = 0;
                int nTotalSuccessCount = 0;
                int nTotalTriggerCount = 0;
                int nBothBullBearCount = 0;

                foreach (KeyValuePair<int, DateStrategyMap> kp in m_DateStrategy)
                {
                    DateStrategyMap dsMap = kp.Value;
                    if (dsMap.Date.CompareTo(m_StartDate) < 0 || dsMap.Date.CompareTo(m_EndDate) > 0)
                        continue;

                    bool bIsTrigger = false;
                    ArrayList triggerList = new ArrayList();
                    double dFirstStrategyProfit = 0;

                    for (int i = 0; i < m_StrategySheet.LastRowNum; i++)
                    {
                        IRow row = m_StrategySheet.GetRow(i);
                        if (row == null)
                            continue;
                        string sStrategyName = row.GetCell(0).StringCellValue;                        
                        string sStrategyName1 = sStrategyName.Substring(0, 4);
                        string sStrategyName2 = sStrategyName.Substring(4, 4);
                        int nSG = 0;
                        if(row.GetCell(1) != null)
                            nSG = (int)row.GetCell(1).NumericCellValue;                        
                        int nSL = (int)row.GetCell(2).NumericCellValue;
                        StrategyDirection nDirection = (StrategyDirection)row.GetCell(9).NumericCellValue;
                        if (!bBull && nDirection == StrategyDirection.Bull)
                            continue;
                        if (!bBear && nDirection == StrategyDirection.Bear)
                            continue;

                        if (dsMap.IsContainStrategy(sStrategyName1) && dsMap.IsContainStrategy(sStrategyName2))
                        {
                            bIsTrigger = true;
                            triggerList.Add(nDirection);
                            double dIn;
                            bool bHit = false;
                            string sDate = dsMap.Date.ToString("yyyy/MM/dd");
                            DateTime Start = Convert.ToDateTime("08:45");
                            DateTime End = Convert.ToDateTime("13:25");
                            double dProfit = 0;

                            if (m_Config.IsStrategy0900(sStrategyName1) || m_Config.IsStrategy0900(sStrategyName2))
                            {
                                Start = Convert.ToDateTime("09:00");
                                dIn = dsMap.Get5KOpen(Start);
                            }
                            else
                                dIn = dsMap.FuturesOpen;

                            double dOut = dsMap.Get5KClose(End);

                            while (Start.CompareTo(End) < 1)
                            {
                                // 做多
                                //
                                if (nDirection == StrategyDirection.Bull)
                                {
                                    // 打到停損
                                    //
                                    if (dIn - dsMap.Get5KLow(Start) >= nSL)
                                    {
                                        bHit = true;
                                        dProfit = -nSL;
                                        break;
                                    }                                    
                                    // 不停利
                                    //
                                    if (nSG == 0)
                                    {
                                        Start = Start.AddMinutes(5);
                                        continue; 
                                    }
                                    else if (dsMap.Get5KHigh(Start) - dIn >= nSG)
                                    {
                                        bHit = true;
                                        dProfit = nSG;
                                        break;
                                    }
                                }
                                // 做空
                                // 
                                else if (nDirection == StrategyDirection.Bear)
                                {
                                    // 打到停損
                                    //
                                    if (dsMap.Get5KHigh(Start) - dIn >= nSL)
                                    {
                                        bHit = true;
                                        dProfit = -nSL;
                                        break;
                                    }
                                    // 不停利
                                    //
                                    if (nSG == 0)
                                    {
                                        Start = Start.AddMinutes(5);
                                        continue;                                        
                                    }
                                    else if (dIn - dsMap.Get5KLow(Start) >= nSG)
                                    {
                                        bHit = true;
                                        dProfit = nSG;
                                        break;
                                    }
                                }                                
                                Start = Start.AddMinutes(5);
                            }                           
                            
                            if (!bHit)
                            {
                                if (nDirection == StrategyDirection.Bull)
                                    dProfit = dOut - dIn;
                                else if (nDirection == StrategyDirection.Bear)
                                    dProfit = dIn - dOut;
                            }
                            orderLog.Log(sDate, sStrategyName, nSG, nSL, nDirection, 1, dIn, dProfit);

                            if (dFirstStrategyProfit == 0)
                                dFirstStrategyProfit = dProfit;
                        }
                    }

                    if (bIsTrigger)
                    {
                        // 當天同時有做多做空的策略, 當作直接平倉沒有虧損獲利
                        //
                        if (triggerList.Contains(StrategyDirection.Bull) && triggerList.Contains(StrategyDirection.Bear))
                        {
                            nBothBullBearCount++;
                            dFirstStrategyProfit = 0;
                        }

                        if (dFirstStrategyProfit > 0)
                            nTotalSuccessCount++;

                        nTotalTriggerCount++;
                        dTotalProfit += (dFirstStrategyProfit - 2);
                    }
                }
                double dTmp = (double)nTotalSuccessCount / (double)nTotalTriggerCount;
                double dSR = Math.Round(dTmp, 2);
                double dEV = Math.Round(dTotalProfit / nTotalTriggerCount, 2);
                orderLog.LogResult(nTotalTriggerCount, nTotalSuccessCount, nBothBullBearCount, dSR, dEV, dTotalProfit);
                orderLog.Save();
                Form1.g_UI_ShowInfo("Apply Strategy Finished");
            }
            else
                return;
        }

        private void LoadExcelFile(string sFilePath)
        {
            if (File.Exists(sFilePath))
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read))
                {
                    m_StrategyWorkBook = new XSSFWorkbook(fs);
                    m_StrategySheet = (ISheet)m_StrategyWorkBook.GetSheetAt(0);
                }                
            }            
        }
    }
}
