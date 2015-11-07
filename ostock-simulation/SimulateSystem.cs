using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OStock_Simulation
{
    class SimulateSystem
    {
        private Config m_Config = null;
        private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        Dictionary<int, DateStrategyMap> m_DateStrategy = new Dictionary<int, DateStrategyMap>();
        
        private IWorkbook m_BiasWorkBook = null;
        private ISheet m_BiasSheet = null;
        private IWorkbook m_FPWorkBook = null;
        private ISheet m_FPSheet = null;
        private IWorkbook m_MIN5KWorkBook = null;
        private ISheet m_MIN5KSheet = null;

        private TechPointer m_Tech;
        private StrategyD m_StrategyD;
        private StrategyC m_StrategyC;

        public SimulateSystem(Config config)
        {
            m_Config = config;
        }

        public async Task Init()
        {
            m_Tech = new TechPointer();
            m_StrategyD = new StrategyD(m_Config, m_Tech);
            m_StrategyC = new StrategyC();

            // .NET 4.5 寫法
            await Task.Run(() => {
                InitializeData();
            });

            // .NET 2.0 寫法
            //Thread initializeDataThread = new Thread(() => InitializeData());
            //initializeDataThread.Start();
        }

        private void InitializeData()
        {
            Form1.g_UI_ShowInfo("Initializing Data...");

            LoadData();

            rwLock.EnterWriteLock();
            try
            {
                m_DateStrategy.Clear();

                // 起始日期: 2007/07/02, 法人資料的起點, 結束日期為
                //
                DateTime dtStartDate = Convert.ToDateTime("2007/07/02");
                DateTime dtEndDate = DateTime.Now.AddDays(-1);

                int nKey = 1;
                int nBiasStartIndex = 0, nBiasEndIndex = m_BiasSheet.LastRowNum, nFPStartIndex = 0, n5KStartIndex = 0;

                for (int i = nBiasStartIndex; i < m_BiasSheet.LastRowNum + 1; i++)
                {
                    IRow biasRow = m_BiasSheet.GetRow(i);
                    if (biasRow == null || biasRow.GetCell(0).CellType != CellType.Numeric)
                        continue;

                    DateTime biasDate = ConvertToDateTime(biasRow.GetCell(0).NumericCellValue);
                    if (dtStartDate.CompareTo(biasDate) == 0)
                        nBiasStartIndex = i;
                }

                for (int i = nBiasStartIndex; i < nBiasEndIndex; i++)
                {
                    bool bFPFlag = false, b5KFlag = false;

                    IRow biasRow = m_BiasSheet.GetRow(i);
                    if (biasRow == null || biasRow.GetCell(0).CellType != CellType.Numeric)
                        continue;

                    DateTime biasDate = ConvertToDateTime(biasRow.GetCell(0).NumericCellValue);

                    IRow fpRow = null;
                    for (int j = nFPStartIndex; j < m_FPSheet.LastRowNum; j++)
                    {
                        fpRow = m_FPSheet.GetRow(j);
                        if (fpRow == null || fpRow.GetCell(0).CellType != CellType.Numeric)
                            continue;

                        DateTime fpDate = ConvertToDateTime(fpRow.GetCell(0).NumericCellValue);
                        if (biasDate.CompareTo(fpDate) == 0)
                        {
                            nFPStartIndex = j;
                            bFPFlag = true;
                            break;
                        }
                    }

                    IRow FiveKRow = null;
                    for (int j = n5KStartIndex; j < m_MIN5KSheet.LastRowNum; j++)
                    {
                        FiveKRow = m_MIN5KSheet.GetRow(j);
                        if (FiveKRow == null || FiveKRow.GetCell(0).CellType != CellType.Numeric)
                            continue;

                        DateTime FiveKDate = ConvertToDateTime(FiveKRow.GetCell(0).NumericCellValue);
                        if (biasDate.CompareTo(FiveKDate) == 0)
                        {
                            n5KStartIndex = j;
                            b5KFlag = true;
                            break;
                        }
                    }

                    if (bFPFlag && b5KFlag && !m_DateStrategy.ContainsKey(nKey))
                    {
                        // 建立Map, Key為日期, Value為DateStrategyMap
                        // DateStrategyMap存日期, 期貨現貨開高低收, 5分K開高低收量
                        //
                        CreateStrategyMap(nKey, biasDate, biasRow, fpRow, n5KStartIndex);
                        nKey++;
                    }
                    else if (!bFPFlag && !b5KFlag)
                    {
                        Form1.g_UI_ShowInfo("法人缺少{0}資料", biasDate.Date);
                        Form1.g_UI_ShowInfo("5分K缺少{0}資料", biasDate.Date);
                    }
                    else if (!bFPFlag)
                    {
                        Form1.g_UI_ShowInfo("法人缺少{0}資料", biasDate.Date);
                    }
                    else if (!b5KFlag)
                    {
                        Form1.g_UI_ShowInfo("5分K缺少{0}資料", biasDate.Date);
                    }
                }

                // 所有Map建好之後開始回測策略放入DateStrategyMap
                //
                if (m_DateStrategy != null)
                {
                    foreach (KeyValuePair<int, DateStrategyMap> kp in m_DateStrategy)
                    {
                        m_StrategyD.CheckC(kp.Value);
                        m_StrategyD.CheckE(kp.Value);
                        m_StrategyD.CheckF(kp.Value);

                        m_StrategyD.CheckV(kp.Key, kp.Value, m_DateStrategy);
                        m_StrategyC.CheckB(kp.Key, kp.Value, m_DateStrategy);
                        m_StrategyC.CheckC(kp.Key, kp.Value, m_DateStrategy);
                        m_StrategyC.CheckD(kp.Key, kp.Value, m_DateStrategy);
                        m_StrategyC.CheckE(kp.Key, kp.Value, m_DateStrategy);
                        m_StrategyC.CheckF(kp.Key, kp.Value, m_DateStrategy);

                        // DB, DD, DG, DH 需上個交易日的資料
                        //
                        if (m_DateStrategy.ContainsKey(kp.Key - 1))
                        {
                            DateStrategyMap predsMap = m_DateStrategy[kp.Key - 1];
                            m_StrategyD.CheckB(predsMap, kp.Value);
                            m_StrategyD.CheckD(predsMap, kp.Value);
                            m_StrategyD.CheckG(predsMap, kp.Value);
                            m_StrategyD.CheckH(predsMap, kp.Value);

                            // DM 需前兩個交易日的資料
                            //
                            if (m_DateStrategy.ContainsKey(kp.Key - 2))
                            {
                                DateStrategyMap predsMap2 = m_DateStrategy[kp.Key - 2];
                                m_StrategyD.CheckM(predsMap, predsMap2, kp.Value);
                            }
                        }
                    }
                    Form1.g_UI_ShowInfo("Initialize Data Finished");
                    Form1.g_UI_ShowInfo("Ready to Rock");
                }
            }
            catch (Exception ex)
            {
                Form1.g_UI_ShowInfo(ex.ToString());
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        private void LoadData()
        {
            try
            {
                LoadBiasExcel(m_Config.BiasFile);

                LoadFPExcel(m_Config.FPYZFile);

                Load5MINKExcelFile(m_Config.MIN5KFile);
            }
            catch (Exception ex)
            {
                Form1.g_UI_ShowInfo(ex.ToString());
            }
        }

        private void LoadBiasExcel(string sFilePath)
        {
            if (File.Exists(sFilePath))
            {
                Form1.g_UI_ShowInfo("Loading Bias xml");
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read))
                {
                    m_BiasWorkBook = new HSSFWorkbook(fs);
                    m_BiasSheet = (ISheet)m_BiasWorkBook.GetSheetAt(0);
                }
                Form1.g_UI_ShowInfo("Load Bias xml Finished");
            }
            else
            {
                Form1.g_UI_ShowInfo("Cannot find Bias xml");
            }
        }

        private void LoadFPExcel(string sFilePath)
        {
            if (File.Exists(sFilePath))
            {
                Form1.g_UI_ShowInfo("Loading FP xml");
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read))
                {
                    m_FPWorkBook = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
                    m_FPSheet = (ISheet)m_FPWorkBook.GetSheetAt(0);
                }
                Form1.g_UI_ShowInfo("Load FP xml Finished");
            }
            else
            {
                Form1.g_UI_ShowInfo("Cannot find FP xml");
            }
        }

        private void Load5MINKExcelFile(string sFilePath)
        {
            if (File.Exists(sFilePath))
            {
                Form1.g_UI_ShowInfo("Loading 5K xml");
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read))
                {
                    m_MIN5KWorkBook = new XSSFWorkbook(fs);
                    m_MIN5KSheet = (XSSFSheet)m_MIN5KWorkBook.GetSheetAt(0);
                }
                Form1.g_UI_ShowInfo("Load 5K xml Finished");
            }
            else
            {
                Form1.g_UI_ShowInfo("Cannot find 5K xml");
            }
        }

        private void CreateStrategyMap(int nKey, DateTime date, IRow biasRow, IRow fpRow, int n5KStartIndex)
        {
            DateStrategyMap dsMap = new DateStrategyMap(date);
            double dFuturesOpen = biasRow.GetCell(1).NumericCellValue;
            double dFuturesHigh = biasRow.GetCell(2).NumericCellValue;
            double dFuturesLow = biasRow.GetCell(3).NumericCellValue;
            double dFuturesClose = biasRow.GetCell(4).NumericCellValue;
            double dSpotOpen = biasRow.GetCell(5).NumericCellValue;
            double dSpotHigh = biasRow.GetCell(6).NumericCellValue;
            double dSpotLow = biasRow.GetCell(7).NumericCellValue;
            double dSpotClose = biasRow.GetCell(8).NumericCellValue;

            dsMap.FuturesOpen = dFuturesOpen;
            dsMap.FuturesHigh = dFuturesHigh;
            dsMap.FuturesLow = dFuturesLow;
            dsMap.FuturesClose = dFuturesClose;
            dsMap.SpotOpen = dSpotOpen;
            dsMap.SpotHigh = dSpotHigh;
            dsMap.SpotLow = dSpotLow;
            dsMap.SpotClose = dSpotClose;

            dsMap.FFutures = fpRow.GetCell(12).NumericCellValue;
            dsMap.FOptions = fpRow.GetCell(13).NumericCellValue;

            for (int j = n5KStartIndex; j < m_MIN5KSheet.LastRowNum; j++)
            {
                IRow FiveKRow = m_MIN5KSheet.GetRow(j);
                if (FiveKRow == null || FiveKRow.GetCell(0).CellType != CellType.Numeric)
                    continue;

                DateTime FiveKDateTime = ConvertToDateTime(FiveKRow.GetCell(0).NumericCellValue);

                if (date.CompareTo(FiveKDateTime) == 0)
                {
                    FiveKDateTime = FiveKDateTime.Add(Convert.ToDateTime(FiveKRow.GetCell(1).StringCellValue).TimeOfDay);
                    double[] dOHLCV = new double[5];
                    dOHLCV[0] = FiveKRow.GetCell(2).NumericCellValue;
                    dOHLCV[1] = FiveKRow.GetCell(3).NumericCellValue;
                    dOHLCV[2] = FiveKRow.GetCell(4).NumericCellValue;
                    dOHLCV[3] = FiveKRow.GetCell(5).NumericCellValue;
                    dOHLCV[4] = FiveKRow.GetCell(6).NumericCellValue;

                    dsMap.Add5K(FiveKDateTime, dOHLCV);
                }
                else
                    break;
            }

            m_DateStrategy.Add(nKey, dsMap);
        }

        private DateTime ConvertToDateTime(double dDate)
        {
            string sDate = Convert.ToString(dDate);
            sDate = sDate.Substring(0, 4) + "/" + sDate.Substring(4, 2) + "/" + sDate.Substring(6, 2);

            return Convert.ToDateTime(sDate);
        }

        public async Task StartSimulateAsync(bool bIsCross, DateTime dtStartDate, DateTime dtEndDate)
        {
            rwLock.EnterReadLock();
            try
            {                
                StrategyExcel strategyExcel = new StrategyExcel(m_Config, "Raw_{0:yyyyMMdd}-{1:yyyyMMdd}_R{2}_C{3}.xlsx", dtStartDate, dtEndDate, m_Config.SG, m_Config.SL);
                if (!strategyExcel.Init())
                {
                    Form1.g_UI_ShowInfo("Excel Already Exist");
                    return;
                }

                Form1.g_UI_ShowInfo("Simulating...");

                string[][] AllStrategy = m_Config.AllStrategy;

                // 交集
                if (bIsCross)
                {
                    for (int i = 0; i < AllStrategy.Length; i++)
                    {
                        for (int j = 0; j < AllStrategy[i].Length; j++)
                        {
                            if (i + 1 < AllStrategy.Length)
                            {
                                for (int k = i + 1; k < AllStrategy.Length; k++)
                                {
                                    for (int l = 0; l < AllStrategy[k].Length; l++)
                                    {
                                        await Task.Run (() => {
                                            SimulateStrategyBull5K(dtStartDate, dtEndDate, strategyExcel, AllStrategy[i][j], AllStrategy[k][l]);
                                            SimulateStrategyBear5K(dtStartDate, dtEndDate, strategyExcel, AllStrategy[i][j], AllStrategy[k][l]);
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < AllStrategy.Length; i++)
                    {
                        for (int j = 0; j < AllStrategy[i].Length; j++)
                        {
                            await Task.Run(() => {
                                SimulateStrategyBull5K(dtStartDate, dtEndDate, strategyExcel, AllStrategy[i][j], "");
                                SimulateStrategyBear5K(dtStartDate, dtEndDate, strategyExcel, AllStrategy[i][j], "");
                            });
                        }
                    }
                }
                
                strategyExcel.Close();
                Form1.g_UI_ShowInfo("Simulate Finished");
            }
            catch (Exception ex)
            {
                Form1.g_UI_ShowInfo(ex.ToString());
            }
            finally
            {
                rwLock.ExitReadLock();
            }

        }

        private void SimulateStrategyBull5K(DateTime dtStartDate, DateTime dtEndDate, StrategyExcel strategyExcel, string sStrategyName1, string sStrategyName2)
        {
            //string json = await webClient.DownloadStringTaskAsync("http://api.openweathermap.org/data/2.5/weather?q=Taipei,tw");


            int nAT = 0, n2AT = 0, nDirection = 1;
            double dSR = 0, dEV = 0, d2SR = 0, d2EV = 0;
            int nSG = m_Config.SG, nSL = m_Config.SL;
            if (m_Config.SG == 0)
                nSG = 10000;
            if (m_Config.SL == 0)
                nSL = 10000;

            try
            { 
                foreach (KeyValuePair<int, DateStrategyMap> kp in m_DateStrategy)
                {
                    DateStrategyMap dsMap = kp.Value;
                    if (dsMap.Date.CompareTo(dtStartDate) < 0 || dsMap.Date.CompareTo(dtEndDate) > 0)
                        continue;

                    if (dsMap.IsContainStrategy(sStrategyName1) && dsMap.IsContainStrategy(sStrategyName2))
                    {
                        nAT++;
                        double dIn;
                        bool bHit = false;
                        DateTime Start = Convert.ToDateTime("08:45");
                        DateTime End = Convert.ToDateTime("13:25");

                        if (m_Config.IsStrategy0900(sStrategyName1) || m_Config.IsStrategy0900(sStrategyName2))
                        {
                            Start = Convert.ToDateTime("09:00");
                            dIn = dsMap.Get5KOpen(Start);
                        }
                        else
                            dIn = dsMap.FuturesOpen;

                        double dOut = dsMap.Get5KClose(End);

                        if (m_DateStrategy.Count - kp.Key <= m_Config.NearPeriod)
                            n2AT++;

                        while (Start.CompareTo(End) < 1)
                        {
                            // 打到停損
                            //
                            if (dIn - dsMap.Get5KLow(Start) >= nSL)
                            {
                                bHit = true;
                                dEV -= m_Config.SL;

                                if (m_DateStrategy.Count - kp.Key <= m_Config.NearPeriod)
                                    d2EV++;
                                break;
                            }
                            // 打到停利, 如果是不停利是進不來的
                            //
                            else if (dsMap.Get5KHigh(Start) - dIn >= nSG)
                            {
                                bHit = true;
                                dEV += m_Config.SG;
                                dSR++;

                                if (m_DateStrategy.Count - kp.Key <= m_Config.NearPeriod)
                                {
                                    d2EV += m_Config.SG;
                                    d2SR++;
                                }
                                break;
                            }
                            Start = Start.AddMinutes(5);
                        }
                        if (!bHit)
                        {
                            double dResult = dOut - dIn;

                            if (dResult > 0)
                                dSR++;
                            dEV += dResult;

                            if (m_DateStrategy.Count - kp.Key <= m_Config.NearPeriod)
                            {
                                d2EV += dResult;
                                if (dResult >= 0)
                                    d2SR++;
                            }
                        }
                    }
                }
                dSR = dSR / nAT;
                dEV = dEV / nAT - 2;
                d2SR = d2SR / n2AT;
                d2EV = d2EV / n2AT - 2;
                strategyExcel.Write(sStrategyName1 + sStrategyName2, m_Config.SG, m_Config.SL, nAT, dSR, dEV, n2AT, d2SR, d2EV, nDirection);
            }
            catch (Exception ex)
            {
                Form1.g_UI_ShowInfo(ex.ToString());
            }
        }

        private void SimulateStrategyBear5K(DateTime dtStartDate, DateTime dtEndDate, StrategyExcel strategyExcel, string sStrategyName1, string sStrategyName2)
        {
            int nAT = 0, n2AT = 0, nDirection = 2;
            double dSR = 0, dEV = 0, d2SR = 0, d2EV = 0;
            int nSG = m_Config.SG, nSL = m_Config.SL;
            if (m_Config.SG == 0)
                nSG = 10000;
            if (m_Config.SL == 0)
                nSL = 10000;

            try
            { 
                foreach (KeyValuePair<int, DateStrategyMap> kp in m_DateStrategy)
                {
                    DateStrategyMap dsMap = kp.Value;
                    if (dsMap.Date.CompareTo(dtStartDate) < 0 || dsMap.Date.CompareTo(dtEndDate) > 0)
                        continue;

                    if (dsMap.IsContainStrategy(sStrategyName1) && dsMap.IsContainStrategy(sStrategyName2))
                    {
                        nAT++;
                        double dIn;
                        bool bHit = false;
                        DateTime Start = Convert.ToDateTime("08:45");
                        DateTime End = Convert.ToDateTime("13:25");

                        if (m_Config.IsStrategy0900(sStrategyName1) || m_Config.IsStrategy0900(sStrategyName2))
                        {
                            Start = Convert.ToDateTime("09:00");
                            dIn = dsMap.Get5KOpen(Start);
                        }
                        else
                            dIn = dsMap.FuturesOpen;

                        double dOut = dsMap.Get5KClose(End);

                        if (m_DateStrategy.Count - kp.Key <= m_Config.NearPeriod)
                            n2AT++;

                        while (Start.CompareTo(End) < 1)
                        {
                            // 打到停損
                            //
                            if (dsMap.Get5KHigh(Start) - dIn >= nSL)
                            {
                                bHit = true;
                                dEV -= m_Config.SL;

                                if (m_DateStrategy.Count - kp.Key <= m_Config.NearPeriod)
                                    d2EV++;
                                break;
                            }
                            // 打到停利
                            //
                            else if (dIn - dsMap.Get5KLow(Start) >= nSG)
                            {
                                bHit = true;
                                dEV += m_Config.SG;
                                dSR++;

                                if (m_DateStrategy.Count - kp.Key <= m_Config.NearPeriod)
                                {
                                    d2EV += m_Config.SG;
                                    d2SR++;
                                }
                                break;
                            }
                            Start = Start.AddMinutes(5);
                        }

                        if (!bHit)
                        {
                            double dResult = dIn - dOut;

                            if (dResult > 0)
                                dSR++;
                            dEV += dResult;

                            if (m_DateStrategy.Count - kp.Key <= m_Config.NearPeriod)
                            {
                                d2EV += dResult;
                                if (dResult >= 0)
                                    d2SR++;
                            }
                        }
                    }
                }
                dSR = dSR / nAT;
                dEV = dEV / nAT - 2;
                d2SR = d2SR / n2AT;
                d2EV = d2EV / n2AT - 2;
                strategyExcel.Write(sStrategyName1 + sStrategyName2, m_Config.SG, m_Config.SL, nAT, dSR, dEV, n2AT, d2SR, d2EV, nDirection);
            }
            catch (Exception ex)
            {
                Form1.g_UI_ShowInfo(ex.ToString());
            }
        }

        public async Task FilterStrategy(string[] sFileNames)
        {
            FilterStrategy filterStrategy = new FilterStrategy(m_Config);
            await Task.Run(() => {
                filterStrategy.FilterAll(sFileNames);
            });
        }

        public async Task ApplyStrategy(string sFilePath, DateTime dtStartDate, DateTime dtEndDate, bool bBull, bool bBear)
        {
            ApplyStrategy applyStrategy = new ApplyStrategy(m_Config, m_DateStrategy, sFilePath, dtStartDate, dtEndDate, bBull, bBear);
            await Task.Run(() => {
                applyStrategy.ExecApply();
            });
        }

        public async Task SimulateOptions()
        {
            if (m_DateStrategy != null)
            {
                await Task.Run(() => {
                    Options op = new Options("C:\\Users\\Johnny\\Desktop\\Workspace\\Options.xlsx", m_DateStrategy);
                    op.Simulate();
                });
            }
        }
    }
}
