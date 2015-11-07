using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;

namespace OStock_Simulation
{
    class FilterStrategy
    {
        private Config m_Config = null;
        private IWorkbook m_StrategyWorkBook = null;
        private ISheet m_StrategySheet = null;

        private Dictionary<string, ArrayList> m_StrategyDic = new Dictionary<string, ArrayList>();

        public FilterStrategy(Config config)
        {
            m_Config = config;            
        }      

        public void FilterAll(string[] arrFiles)
        {
            Form1.g_UI_ShowInfo("Start to Filter the Strategy...");

            try
            {
                // 輸出過濾後的策略檔案與要過濾的檔案在同一目錄下
                //
                string sFilteredFileName = Path.GetFileName(arrFiles[0]).Replace("Raw", "Filtered");
                string sFilteredFilePath = string.Format("{0}\\{1}", m_Config.OutputDirectory, sFilteredFileName);
                LoadExcelFile(sFilteredFilePath);

                m_StrategyDic.Clear();

                if (m_StrategySheet != null)
                {
                    foreach (string sResultFile in arrFiles)
                    {
                        ExecFilter(sResultFile);
                    }

                    foreach (KeyValuePair<string, ArrayList> kp in m_StrategyDic)
                    {
                        double PreSR = 0, PreEV = 0, PreSR2year = 0, PreEV2year = 0;
                        IRow PreRow = null;

                        foreach (IRow Row in kp.Value)
                        {
                            double SR = Row.GetCell(4) != null ? Row.GetCell(4).NumericCellValue : 0;
                            double EV = Row.GetCell(5) != null ? Row.GetCell(5).NumericCellValue : 0;
                            double SR2year = Row.GetCell(7) != null ? Row.GetCell(7).NumericCellValue : 0;
                            double EV2year = Row.GetCell(8) != null ? Row.GetCell(8).NumericCellValue : 0;

                            if (SR > PreSR || EV > PreEV || SR2year > PreSR2year || EV2year > PreEV2year)
                            {
                                // 全部數據都更佳
                                //
                                if (SR >= PreSR && EV >= PreEV && SR2year >= PreSR2year && EV2year >= PreEV2year)
                                    PreRow = Row;
                                // 成功率多7%以上, 而且期望值不少於3以上
                                //
                                else if (SR - PreSR > 0.07 && EV - PreEV > -3)
                                    PreRow = Row;
                                // 期望值多5以上, 而且成功率不少於2%以上
                                //
                                else if (SR - PreSR > -0.02 && EV - PreEV > 5)
                                    PreRow = Row;
                                // 兩年成功率高出8%以上, 而且期望值不少於5以上
                                //
                                else if (SR2year - PreSR2year > 0.08 && EV2year - PreEV2year > -5)
                                    PreRow = Row;
                                // 兩年期望值多10以上, 而且成功率不少於5%以上
                                //
                                else if (SR2year - PreSR2year > -0.05 && EV2year - PreEV2year > 10)
                                    PreRow = Row;
                            }

                            PreSR = PreRow.GetCell(4) != null ? PreRow.GetCell(4).NumericCellValue : 0;
                            PreEV = PreRow.GetCell(5) != null ? PreRow.GetCell(5).NumericCellValue : 0;
                            PreSR2year = PreRow.GetCell(7) != null ? PreRow.GetCell(7).NumericCellValue : 0;
                            PreEV2year = PreRow.GetCell(8) != null ? PreRow.GetCell(8).NumericCellValue : 0;
                        }

                        IRow NewRow = m_StrategySheet.CreateRow(m_StrategySheet.LastRowNum + 1);

                        for (int j = 0; j < PreRow.LastCellNum; j++)
                        {
                            if (PreRow.GetCell(j) == null)
                            {
                                NewRow.CreateCell(j).SetCellValue("");
                            }
                            else if (PreRow.GetCell(j).CellType == CellType.Numeric)
                            {
                                double dValue = PreRow.GetCell(j).NumericCellValue;
                                NewRow.CreateCell(j).SetCellValue(dValue);
                            }
                            else if (PreRow.GetCell(j).CellType == CellType.String)
                            {
                                string sValue = PreRow.GetCell(j).StringCellValue;
                                NewRow.CreateCell(j).SetCellValue(sValue);
                            }
                        }
                    }

                    using (FileStream fs = new FileStream(sFilteredFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        m_StrategyWorkBook.Write(fs);
                    }
                    Form1.g_UI_ShowInfo("Filter Finished");
                }
            }
            catch(Exception ex)
            {
                Form1.g_UI_ShowInfo(ex.ToString());
            }
        }

        private void ExecFilter(string sFilePath)
        {
            using (FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook StrategyWorkBook = new XSSFWorkbook(fs);
                ISheet StrategySheet = (ISheet)StrategyWorkBook.GetSheetAt(0);

                if (StrategySheet != null)
                {
                    for (int i = 1; i < StrategySheet.LastRowNum; i++)
                    {
                        double AT = 0, SR = 0, EV = 0, AT2year = 0, SR2year = 0, EV2year = 0;
                        IRow Row = StrategySheet.GetRow(i);

                        AT = Row.GetCell(3).CellType != CellType.Error ? Row.GetCell(3).NumericCellValue : 0;
                        SR = Row.GetCell(4).CellType != CellType.Error ? Row.GetCell(4).NumericCellValue : 0;
                        EV = Row.GetCell(5).CellType != CellType.Error ? Row.GetCell(5).NumericCellValue : 0;
                        AT2year = Row.GetCell(6).CellType != CellType.Error ? Row.GetCell(6).NumericCellValue : 0;
                        SR2year = Row.GetCell(7).CellType != CellType.Error ? Row.GetCell(7).NumericCellValue : 0;
                        EV2year = Row.GetCell(8).CellType != CellType.Error ? Row.GetCell(8).NumericCellValue : 0;

                        if (AT >= m_Config.FilterParam.AT && AT2year >= m_Config.FilterParam.AT2Year &&
                            SR >= m_Config.FilterParam.SR && SR2year >= m_Config.FilterParam.SR2Year &&
                            EV >= m_Config.FilterParam.EV && EV2year >= m_Config.FilterParam.EV2Year)
                        {
                            string sStrateName = Row.GetCell(0).StringCellValue;

                            if (m_StrategyDic.ContainsKey(sStrateName))
                            {
                                ArrayList StrategyList = m_StrategyDic[sStrateName];
                                StrategyList.Add(Row);
                            }
                            else
                            {
                                ArrayList StrategyList = new ArrayList();
                                StrategyList.Add(Row);
                                m_StrategyDic.Add(sStrateName, StrategyList);
                            }
                        }
                    }
                }
            }
        }

        private bool LoadExcelFile(string sFilePath)
        {            
            if (File.Exists(sFilePath))
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read))
                {
                    m_StrategyWorkBook = new XSSFWorkbook(fs);
                    m_StrategySheet = (ISheet)m_StrategyWorkBook.GetSheetAt(0);
                }
                return true;
            }
            else
            {
                m_StrategyWorkBook = new XSSFWorkbook();
                m_StrategySheet = (ISheet)m_StrategyWorkBook.CreateSheet("Sheet1");
                return true;
            }
        }
    }
}
