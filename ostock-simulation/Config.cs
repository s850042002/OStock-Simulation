using System;
using System.Collections.Generic;
using System.Xml;
using System.Threading;

namespace OStock_Simulation
{
    public class ConfigParam
    {
        // 基差(期貨, 現貨), 三大法人, 5分K的檔案
        //
        public static string BIAS_FILEPATH = "BiasFile";
        public static string FP_YZ_FILEPATH = "FPYZFile";
        public static string FP_ZIN_FILEPATH = "FPZINFile";
        public static string FP_TS_FILEPATH = "FPTSFile";
        public static string MIN5K_FILEPATH = "MIN5KFile";        

        // 下單的參數
        //
        public static string ORDER_PARAM_REALORDER = "RealOrder";
        public static string ORDER_PARAM_TARGET = "Target";
        public static string ORDER_PARAM_QT = "QT";
        public static string ORDER_PARAM_MAXQT = "MaxQT";

        // 要篩選策略的資料夾, 篩選後的策略檔案
        //
        public static string OUTPUTDIRECTORY = "OutputDirectory";
        public static string FILTER_PARAM = "FilterParam";
        public static string FILTER_PARAM_AT = "AT";
        public static string FILTER_PARAM_SR = "SR";
        public static string FILTER_PARAM_EV = "EV";
        public static string FILTER_PARAM_AT2Year = "AT2Year";
        public static string FILTER_PARAM_SR2Year = "SR2Year";
        public static string FILTER_PARAM_EV2Year = "EV2Year";
    }

    public class Config
    {
        XmlDocument m_xmlDoc;
        ReaderWriterLockSlim m_rw_xmlDoc = new ReaderWriterLockSlim();
        OrderParam m_OrderParam;
        Dictionary<string, string> m_AllStrategy = new Dictionary<string, string>();
        FilterParam m_FilterParam;

        public void Init(string sConfigFile)
        {
            try
            {
                m_rw_xmlDoc.EnterWriteLock();

                m_xmlDoc = new XmlDocument();

                m_xmlDoc.Load(sConfigFile);

                m_OrderParam = new OrderParam(m_xmlDoc);

                m_FilterParam = new FilterParam(m_xmlDoc);

                Form1.g_UI_ShowInfo("Load Config.xml");
                //Log.Write("Load Config.xml");
            }
            finally
            {
                m_rw_xmlDoc.ExitWriteLock();
            }
        }

        public bool IsTradeDay(DateTime dtDate)
        {
            try
            {
                m_rw_xmlDoc.EnterReadLock();
                //DateTime dtNow = DateTime.Now;

                if (dtDate.DayOfWeek == DayOfWeek.Saturday || dtDate.DayOfWeek == DayOfWeek.Sunday)
                    return false;

                XmlNode node = m_xmlDoc.SelectSingleNode("Configuration/Holiday");

                if (node == null)
                    return false;

                string sYear = dtDate.Year.ToString();
                XmlNode DateNode = node.SelectSingleNode("Year" + sYear);

                if (DateNode == null)
                    return false;

                string[] arrHoliday = DateNode.InnerText.Split(';');
                for (int i = 0; i < arrHoliday.Length; i++)
                {
                    DateTime dtHoliday = Convert.ToDateTime(arrHoliday[i]);
                    if (dtHoliday.Date.CompareTo(dtDate) == 0)
                        return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                m_rw_xmlDoc.ExitReadLock();
            }
        }

        public bool IsSettlementDay(DateTime dtDate)
        {
            try
            {
                m_rw_xmlDoc.EnterReadLock();                

                XmlNode node = m_xmlDoc.SelectSingleNode("Configuration/SettlementDate");

                if (node == null)
                    return false;

                string sYear = dtDate.Year.ToString();
                XmlNode DateNode = node.SelectSingleNode("Year" + sYear);

                if (DateNode == null)
                    return false;

                string[] arrHoliday = DateNode.InnerText.Split(';');
                for (int i = 0; i < arrHoliday.Length; i++)
                {
                    DateTime dtSettlementDay = Convert.ToDateTime(sYear + "/" + arrHoliday[i]);
                    if (dtSettlementDay.Date.CompareTo(dtDate) == 0)
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                m_rw_xmlDoc.ExitReadLock();
            }
        }

        public bool IsSTW(DateTime dtDate)
        {
            try
            {
                m_rw_xmlDoc.EnterReadLock();

                if (dtDate.DayOfWeek == DayOfWeek.Saturday || dtDate.DayOfWeek == DayOfWeek.Sunday)
                    return false;

                XmlNode node = m_xmlDoc.SelectSingleNode("Configuration/STW");

                if (node == null)
                    return false;

                string sYear = dtDate.Year.ToString();
                XmlNode DateNode = node.SelectSingleNode("Year" + sYear);

                if (DateNode == null)
                    return false;

                string[] arrHoliday = DateNode.InnerText.Split(';');
                for (int i = 0; i < arrHoliday.Length; i++)
                {
                    DateTime dtSTW = Convert.ToDateTime(sYear + "/" + arrHoliday[i]);
                    if (dtSTW.Date.CompareTo(dtDate) == 0)
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                m_rw_xmlDoc.ExitReadLock();
            }
        }

        public int SG 
        {
            get 
            {
                XmlNode node = m_xmlDoc.SelectSingleNode("Configuration/SimulateParam/SG");

                if (node == null)
                    return 0;

                return Convert.ToInt32(node.InnerText);
            }
        }

        public int SL
        {
            get
            {
                XmlNode node = m_xmlDoc.SelectSingleNode("Configuration/SimulateParam/SL");

                if (node == null)
                    return 0;

                return Convert.ToInt32(node.InnerText);
            }
        }

        public int NearPeriod
        {
            get
            {
                XmlNode node = m_xmlDoc.SelectSingleNode("Configuration/SimulateParam/NearPeriod");

                if (node == null)
                    return 0;

                return Convert.ToInt32(node.InnerText);
            }
        }

        public bool IsStrategy0900(string sStrategy)
        {            
            try
            {
                m_rw_xmlDoc.EnterReadLock();

                XmlNode node = m_xmlDoc.SelectSingleNode("Configuration/SimulateParam/Strategy0900");

                if (node == null)
                    return false;

                string[] arr = node.InnerText.Trim().Split(';');
                foreach (string item in arr)
                {
                    if (sStrategy.Contains(item))
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                m_rw_xmlDoc.ExitReadLock();
            }
        }

        public string BiasFile
        {
            get
            {
                try
                {
                    return GetParam(ConfigParam.BIAS_FILEPATH);
                }
                catch
                {
                    return null;
                }
            }
        }

        public string FPYZFile
        {
            get
            {
                try
                {
                    return GetParam(ConfigParam.FP_YZ_FILEPATH);
                }
                catch
                {
                    return null;
                }
            }
        }

        public string FPZINFile
        {
            get
            {
                try
                {
                    return GetParam(ConfigParam.FP_ZIN_FILEPATH);
                }
                catch
                {
                    return null;
                }
            }
        }

        public string FPTSFile
        {
            get
            {
                try
                {
                    return GetParam(ConfigParam.FP_TS_FILEPATH);
                }
                catch
                {
                    return null;
                }
            }
        }

        public string MIN5KFile
        {
            get
            {
                try
                {
                    return GetParam(ConfigParam.MIN5K_FILEPATH);
                }
                catch
                {
                    return null;
                }
            }
        }

        public string OutputDirectory
        {
            get
            {
                try
                {
                    return GetParam(ConfigParam.OUTPUTDIRECTORY);
                }
                catch
                {
                    return null;
                }
            }
        }

        public OrderParam OrderParam
        {
            get
            {
                try
                {
                    if (m_OrderParam != null)
                        return m_OrderParam;
                    else
                        return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public FilterParam FilterParam
        {
            get
            {
                try
                {
                    if (m_FilterParam != null)
                        return m_FilterParam;
                    else
                        return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string[][] AllStrategy
        {
            get
            {
                try
                {
                    return GetAllStrategy();
                }
                catch
                {
                    return null;
                }
            }
        }

        private string[][] GetAllStrategy()
        {
            try
            {
                m_rw_xmlDoc.EnterReadLock();

                string sPath = string.Format("Configuration/Strategy");

                XmlNode node = m_xmlDoc.SelectSingleNode(sPath);

                if (node == null || !node.HasChildNodes)
                    return null;

                string[][] arrStrategyName = new string[node.ChildNodes.Count][];
                int i = 0, j = 0;
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    string[] tmpStrategyName = subNode.InnerText.Split(';');
                    arrStrategyName[i] = new string[tmpStrategyName.Length];

                    foreach (string sStrategyName in tmpStrategyName)
                    {                        
                        arrStrategyName[i][j] = sStrategyName.Trim();
                        j++;
                    }
                    i++;
                    j = 0;
                    //m_AllStrategy.Add(subNode.Name, subNode.InnerText);
                }

                return arrStrategyName;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                m_rw_xmlDoc.ExitReadLock();
            }
        }

        private string GetParam(string sKey)
        {
            try
            {
                m_rw_xmlDoc.EnterReadLock();

                string sPath = string.Format("Configuration/{0}", sKey);

                XmlNode node = m_xmlDoc.SelectSingleNode(sPath);

                if (node == null || !node.HasChildNodes)
                    return null;

                return node.ChildNodes[0].Value.Trim();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                m_rw_xmlDoc.ExitReadLock();
            }
        }
    }

    // 下單的參數設定
    //
    public class OrderParam
    {
        ReaderWriterLockSlim m_rw_xmlDoc = new ReaderWriterLockSlim();
        XmlDocument m_xmlDoc;

        public OrderParam(XmlDocument xmlDoc)
        {
            this.m_xmlDoc = xmlDoc;
        }

        public bool RealOrder
        {
            get
            {
                try
                {
                    if (GetOrderParam(ConfigParam.ORDER_PARAM_REALORDER) == "1")
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public string Target
        {
            get
            {
                try
                {
                    return GetOrderParam(ConfigParam.ORDER_PARAM_TARGET);
                }
                catch
                {
                    return null;
                }
            }
        }

        public int QT
        {
            get
            {
                try
                {
                    return Convert.ToInt32(GetOrderParam(ConfigParam.ORDER_PARAM_QT));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int MaxQT
        {
            get
            {
                try
                {
                    return Convert.ToInt32(GetOrderParam(ConfigParam.ORDER_PARAM_MAXQT));
                }
                catch
                {
                    return 0;
                }
            }
        }        

        private string GetOrderParam(string sKey)
        {
            try
            {
                m_rw_xmlDoc.EnterReadLock();

                string sPath = string.Format("Configuration/OrderParam/{0}", sKey);

                XmlNode node = m_xmlDoc.SelectSingleNode(sPath);

                if (node == null || !node.HasChildNodes)
                    return null;

                return node.ChildNodes[0].Value.Trim();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                m_rw_xmlDoc.ExitReadLock();
            }
        }        
    }

    // 篩選策略的參數設定
    //
    public class FilterParam
    {
        ReaderWriterLockSlim m_rw_xmlDoc = new ReaderWriterLockSlim();
        XmlDocument m_xmlDoc;

        public FilterParam(XmlDocument xmlDoc)
        {
            this.m_xmlDoc = xmlDoc;
        }

        public double AT
        {
            get
            {
                try
                {
                    return GetFilterParam(ConfigParam.FILTER_PARAM_AT);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public double SR
        {
            get
            {
                try
                {
                    return GetFilterParam(ConfigParam.FILTER_PARAM_SR);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public double EV
        {
            get
            {
                try
                {
                    return GetFilterParam(ConfigParam.FILTER_PARAM_EV);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public double AT2Year
        {
            get
            {
                try
                {
                    return GetFilterParam(ConfigParam.FILTER_PARAM_AT2Year);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public double SR2Year
        {
            get
            {
                try
                {
                    return GetFilterParam(ConfigParam.FILTER_PARAM_SR2Year);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public double EV2Year
        {
            get
            {
                try
                {
                    return GetFilterParam(ConfigParam.FILTER_PARAM_EV2Year);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private double GetFilterParam(string sKey)
        {
            try
            {
                m_rw_xmlDoc.EnterReadLock();

                string sPath = string.Format("Configuration/FilterParam/{0}", sKey);

                XmlNode node = m_xmlDoc.SelectSingleNode(sPath);

                if (node == null || !node.HasChildNodes)
                    return 0;

                return Convert.ToDouble(node.ChildNodes[0].Value.Trim());
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                m_rw_xmlDoc.ExitReadLock();
            }
        }
    }
}
