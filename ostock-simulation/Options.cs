using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;


namespace OStock_Simulation
{
    class Options
    {
        private Config m_Config = null;
        private IWorkbook m_StrategyWorkBook = null;
        private ISheet m_StrategySheet = null;
        private string m_sFileName = null;

        Dictionary<int, DateStrategyMap> m_DicDateStrategyMap;
        Dictionary<int, int> m_DicLetter;
        Dictionary<int, int> m_DicTemp;
        object Lock = new object();

        public Options(string sFileName, Dictionary<int, DateStrategyMap> dicDateStrategy)
        {
            m_DicDateStrategyMap = dicDateStrategy;
            m_DicLetter = new Dictionary<int, int>();
            m_DicTemp = new Dictionary<int, int>();
            for (int i = -500; i <= 500; i += 50)
            {
                m_DicLetter.Add(i, 0);
                m_DicTemp.Add(i, 0);
            }
            m_sFileName = sFileName;
            Init();
        }        

        public void Init()
        {
            if (!File.Exists(m_sFileName))
            {
                m_StrategyWorkBook = new XSSFWorkbook();
                m_StrategySheet = (ISheet)m_StrategyWorkBook.CreateSheet("Sheet1");

                IRow Row = m_StrategySheet.CreateRow(0);
                int nCount = 0;
                foreach(KeyValuePair<int, int> kp in m_DicLetter)
                {
                    Row.CreateCell(nCount).SetCellValue(kp.Key);
                    nCount++;
                }           
            }            
        }

        private void Reset()
        {
            m_DicLetter = new Dictionary<int, int>();
            m_DicTemp = new Dictionary<int, int>();
            for (int i = -500; i <= 500; i += 50)
            {
                m_DicLetter.Add(i, 0);
                m_DicTemp.Add(i, 0);
            }
        }

        public void Close()
        {
            using (FileStream fs = new FileStream(m_sFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                if (m_StrategyWorkBook != null)
                {
                    m_StrategyWorkBook.Write(fs);                    
                }
            }
        }

        public void Simulate()
        {
            try
            {
                SpotExcel strategyExcel = new SpotExcel("{0}.xlsx", "Options");                                

                for(int nDay = 1; nDay < 7; nDay++)
                {
                    int nCount = 0;
                    foreach (KeyValuePair<int, DateStrategyMap> kp in m_DicDateStrategyMap)
                    {
                        if (kp.Value.Date < Convert.ToDateTime("2006-11-21"))
                            continue;

                        DateStrategyMap baseDate = kp.Value;
                        DateStrategyMap preDate = null;
                        if (m_DicDateStrategyMap.ContainsKey(kp.Key - 1))
                            preDate = m_DicDateStrategyMap[kp.Key - 1];

                        // 法人留倉多, 隔天向下跳空
                        //
                        if (preDate != null && preDate.FFutures > 10000000)
                        {
                            if ((preDate.SpotClose - baseDate.SpotOpen) > 100)
                            {
                                double dHighest = baseDate.SpotHigh, dLowest = baseDate.SpotLow;
                                int i = 1;
                                for (i = 1; i < nDay; i++)
                                {                                                                      
                                    DateStrategyMap Date = m_DicDateStrategyMap[kp.Key + i];                                    
                                    dHighest = Math.Max(Date.SpotHigh, dHighest);
                                    dLowest = Math.Min(Date.SpotLow, dLowest);
                                    if (Date.Date.DayOfWeek == DayOfWeek.Wednesday)
                                        break;
                                }
                                if (nDay > 2 && i != nDay)
                                    continue;

                                double positive = Math.Round(dHighest - baseDate.SpotOpen, 2);
                                double negative = Math.Round(dLowest - baseDate.SpotOpen, 2);
                                AddStatic(negative);
                                AddStatic(positive);
                                nCount++;
                            }
                        }

                        // 法人留倉空, 隔天向上跳空
                        //
                        /*if (preDateStategy != null && preDateStategy.FFutures < -10000000)
                        {
                            if ((preDateStategy.SpotClose - dateStrategy.SpotOpen) < -100)
                            {
                                double positive = Math.Round(dateStrategy.SpotHigh - dateStrategy.SpotOpen, 2);
                                double negative = Math.Round(dateStrategy.SpotLow - dateStrategy.SpotOpen, 2);
                                AddStatic(negative);
                                AddStatic(positive);
                                nCount++;
                            }
                        }*/
                    }

                    // Write to Excel
                    //
                    int nSeq = 0;
                    IRow row = m_StrategySheet.CreateRow(m_StrategySheet.LastRowNum + 1);                    
                    foreach (KeyValuePair<int, int> kp in m_DicLetter)
                    {
                        double dValue = Math.Round((double)kp.Value / (double)nCount, 4);                        
                        dValue *= 100;
                        if (kp.Key == 0)
                            dValue = nCount;
                        string sValue = string.Format("{0}%", dValue);                        
                        row.CreateCell(nSeq).SetCellValue(dValue);
                        nSeq++;
                    }
                    Reset();
                }
                
                Close();                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void AddStatic(double n)
        {             
            if (m_DicLetter != null)
            {            
                foreach (KeyValuePair<int, int> kp in m_DicTemp)
                {
                    if(n >= 0)
                    {
                        if (n >= kp.Key && kp.Key > 0)
                        {
                            m_DicLetter[kp.Key]++;                            
                        }
                    }
                    else
                    {
                        if (n < kp.Key && kp.Key < 0)
                        {
                            m_DicLetter[kp.Key]++;
                        }
                    }
                }                
            }
        }
    }
}
