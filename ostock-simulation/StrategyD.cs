using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OStock_Simulation
{
    // 基差, MA, 摩台
    //
    class StrategyD
    {
        private Config m_Config = null;
        private TechPointer m_Tech;

        public StrategyD(Config config, TechPointer Tech)
        {
            this.m_Config = config;
            this.m_Tech = Tech;
        }

        // 前一日基差開
        //
        public void CheckB(DateStrategyMap predsMap, DateStrategyMap dsMap)
        {
            double dBiasOpen = predsMap.SpotOpen - predsMap.FuturesOpen;
            double dBiasClose = predsMap.SpotClose - predsMap.FuturesClose;

            for (int i = 1; i < 31; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if(i < 10)
                    sStrategyID = string.Format("B0{0}", i);
                else
                    sStrategyID = string.Format("B{0}", i);

                switch (sStrategyID)
                {
                    case "B01":
                        if (dBiasOpen > 20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B02":
                        if (dBiasOpen > 40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B03":
                        if (dBiasOpen > 60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B04":
                        if (dBiasOpen > 80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B05":
                        if (dBiasOpen > 100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B06":
                        if (dBiasOpen < -20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B07":
                        if (dBiasOpen < -40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B08":
                        if (dBiasOpen < -60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B09":
                        if (dBiasOpen < -80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B10":
                        if (dBiasOpen < -100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B11":
                        if (dBiasClose > 20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B12":
                        if (dBiasClose > 40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B13":
                        if (dBiasClose > 60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B14":
                        if (dBiasClose > 80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B15":
                        if (dBiasClose > 100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B16":
                        if (dBiasClose < -20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B17":
                        if (dBiasClose < -40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B18":
                        if (dBiasClose < -60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B19":
                        if (dBiasClose < -80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B20":
                        if (dBiasClose < -100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B21":
                        if (dBiasOpen - dBiasClose > 20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B22":
                        if (dBiasOpen - dBiasClose > 40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B23":
                        if (dBiasOpen - dBiasClose > 60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B24":
                        if (dBiasOpen - dBiasClose > 80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B25":
                        if (dBiasOpen - dBiasClose > 100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B26":
                        if (dBiasOpen - dBiasClose < -20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B27":
                        if (dBiasOpen - dBiasClose < -40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B28":
                        if (dBiasOpen - dBiasClose < -60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B29":
                        if (dBiasOpen - dBiasClose < -80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "B30":
                        if (dBiasOpen - dBiasClose < -100)
                        {
                            bFlag = true;
                        }
                        break;
                }
                if(bFlag)
                    dsMap.AddStrategy("D" + sStrategyID);
            }
        }

        // 今日期貨開
        //
        public void CheckC(DateStrategyMap dsMap)
        {
            double dBiasOpen = dsMap.SpotOpen - dsMap.Get5KOpen(Convert.ToDateTime("09:00"));
            
            for (int i = 1; i < 21; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if (i < 10)
                    sStrategyID = string.Format("C0{0}", i);
                else
                    sStrategyID = string.Format("C{0}", i);

                switch (sStrategyID)
                {
                    case "C01":
                        if (dBiasOpen > 10)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C02":
                        if (dBiasOpen > 20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C03":
                        if (dBiasOpen > 30)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C04":
                        if (dBiasOpen > 40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C05":
                        if (dBiasOpen > 50)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C06":
                        if (dBiasOpen > 60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C07":
                        if (dBiasOpen > 70)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C08":
                        if (dBiasOpen > 80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C09":
                        if (dBiasOpen > 90)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C10":
                        if (dBiasOpen > 100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C11":
                        if (dBiasOpen < -10)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C12":
                        if (dBiasOpen < -20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C13":
                        if (dBiasOpen < -30)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C14":
                        if (dBiasOpen < -40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C15":
                        if (dBiasOpen < -50)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C16":
                        if (dBiasOpen < -60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C17":
                        if (dBiasOpen < -70)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C18":
                        if (dBiasOpen < -80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C19":
                        if (dBiasOpen < -90)
                        {
                            bFlag = true;
                        }
                        break;
                    case "C20":
                        if (dBiasOpen < -100)
                        {
                            bFlag = true;
                        }
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("D" + sStrategyID);
            }
        }

        // 前一日現貨收 - 今日期貨開
        //
        public void CheckD(DateStrategyMap predsMap, DateStrategyMap dsMap)
        {            
            double dSpotClose = predsMap.SpotClose;
            double dFuturesOpen = dsMap.FuturesOpen;

            for (int i = 1; i < 21; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if (i < 10)
                    sStrategyID = string.Format("D0{0}", i);
                else
                    sStrategyID = string.Format("D{0}", i);

                switch (sStrategyID)
                {
                    case "D01":
                        if (dSpotClose - dFuturesOpen > 10)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D02":
                        if (dSpotClose - dFuturesOpen > 20)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D03":
                        if (dSpotClose - dFuturesOpen > 30)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D04":
                        if (dSpotClose - dFuturesOpen > 40)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D05":
                        if (dSpotClose - dFuturesOpen > 50)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D06":
                        if (dSpotClose - dFuturesOpen > 60)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D07":
                        if (dSpotClose - dFuturesOpen > 70)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D08":
                        if (dSpotClose - dFuturesOpen > 80)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D09":
                        if (dSpotClose - dFuturesOpen > 90)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D10":
                        if (dSpotClose - dFuturesOpen > 100)
                        {
                            bFlag = true;
                        }

                        break;
                    case "D11":
                        if (dSpotClose - dFuturesOpen < -10)
                        {
                            bFlag = true;
                        }
                        break;
                    case "D12":
                        if (dSpotClose - dFuturesOpen < -20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "D13":
                        if (dSpotClose - dFuturesOpen < -30)
                        {
                            bFlag = true;
                        }
                        break;
                    case "D14":
                        if (dSpotClose - dFuturesOpen < -40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "D15":
                        if (dSpotClose - dFuturesOpen < -50)
                        {
                            bFlag = true;
                        }
                        break;
                    case "D16":
                        if (dSpotClose - dFuturesOpen < -60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "D17":
                        if (dSpotClose - dFuturesOpen < -70)
                        {
                            bFlag = true;
                        }
                        break;
                    case "D18":
                        if (dSpotClose - dFuturesOpen < -80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "D19":
                        if (dSpotClose - dFuturesOpen < -90)
                        {
                            bFlag = true;
                        }
                        break;
                    case "D20":
                        if (dSpotClose - dFuturesOpen < -100)
                        {
                            bFlag = true;
                        }
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("D" + sStrategyID);
            }                        
        }

        // 都為9點整的基差開
        //
        public void CheckE(DateStrategyMap dsMap)
        {            
            double dBiasOpen = dsMap.SpotOpen - dsMap.Get5KOpen(Convert.ToDateTime("9:00"));

            for (int i = 1; i < 21; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if (i < 10)
                    sStrategyID = string.Format("E0{0}", i);
                else
                    sStrategyID = string.Format("E{0}", i);

                switch (sStrategyID)
                {
                    case "E01":
                        if (dBiasOpen > 10)
                        {
                            bFlag = true;
                        }
                        break;
                    case "E02":
                        if (dBiasOpen > 20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "E03":
                        if (dBiasOpen > 30)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E04":
                        if (dBiasOpen > 40)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E05":
                        if (dBiasOpen > 50)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E06":
                        if (dBiasOpen > 60)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E07":
                        if (dBiasOpen > 70)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E08":
                        if (dBiasOpen > 80)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E09":
                        if (dBiasOpen > 90)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E10":
                        if (dBiasOpen > 100)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E11":
                        if (dBiasOpen < -10)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E12":
                        if (dBiasOpen < -20)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E13":
                        if (dBiasOpen < -30)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E14":
                        if (dBiasOpen < -40)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E15":
                        if (dBiasOpen < -50)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E16":
                        if (dBiasOpen < -60)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E17":
                        if (dBiasOpen < -70)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E18":
                        if (dBiasOpen < -80)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E19":
                        if (dBiasOpen < -90)
                        {
                            bFlag = true;
                        }

                        break;
                    case "E20":
                        if (dBiasOpen < -100)
                        {
                            bFlag = true;
                        }

                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("D" + sStrategyID);
            }                       
        }

        // 期貨在前15分鐘漲跌過大
        //
        public void CheckF(DateStrategyMap dsMap)
        {
            double dDiff = dsMap.Get5KOpen(Convert.ToDateTime("9:00")) - dsMap.Get5KOpen(Convert.ToDateTime("8:45"));

            for (int i = 1; i < 21; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if (i < 10)
                    sStrategyID = string.Format("F0{0}", i);
                else
                    sStrategyID = string.Format("F{0}", i);

                switch (sStrategyID)
                {
                    case "F01":
                        if (dDiff > 10)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F02":
                        if (dDiff > 20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F03":
                        if (dDiff > 30)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F04":
                        if (dDiff > 40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F05":
                        if (dDiff > 50)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F06":
                        if (dDiff > 60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F07":
                        if (dDiff > 70)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F08":
                        if (dDiff > 80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F09":
                        if (dDiff > 90)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F10":
                        if (dDiff > 100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F11":
                        if (dDiff < -10)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F12":
                        if (dDiff < -20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F13":
                        if (dDiff < -30)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F14":
                        if (dDiff < -40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F15":
                        if (dDiff < -50)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F16":
                        if (dDiff < -60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F17":
                        if (dDiff < -70)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F18":
                        if (dDiff < -80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F19":
                        if (dDiff < -90)
                        {
                            bFlag = true;
                        }
                        break;
                    case "F20":
                        if (dDiff < -100)
                        {
                            bFlag = true;
                        }
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("D" + sStrategyID);
            }
        }

        // 期貨跳空
        //
        public void CheckG(DateStrategyMap predsMap, DateStrategyMap dsMap)
        {
            double dDiff = dsMap.FuturesOpen - predsMap.FuturesClose;

            for (int i = 1; i < 21; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if (i < 10)
                    sStrategyID = string.Format("G0{0}", i);
                else
                    sStrategyID = string.Format("G{0}", i);

                switch (sStrategyID)
                {
                    case "G01":
                        if (dDiff > 20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G02":
                        if (dDiff > 40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G03":
                        if (dDiff > 60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G04":
                        if (dDiff > 80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G05":
                        if (dDiff > 100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G06":
                        if (dDiff > 120)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G07":
                        if (dDiff > 140)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G08":
                        if (dDiff > 160)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G09":
                        if (dDiff > 180)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G10":
                        if (dDiff > 200)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G11":
                        if (dDiff < -20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G12":
                        if (dDiff < -40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G13":
                        if (dDiff < -60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G14":
                        if (dDiff < -80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G15":
                        if (dDiff < -100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G16":
                        if (dDiff < -120)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G17":
                        if (dDiff < -140)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G18":
                        if (dDiff < -160)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G19":
                        if (dDiff < -180)
                        {
                            bFlag = true;
                        }
                        break;
                    case "G20":
                        if (dDiff < -200)
                        {
                            bFlag = true;
                        }
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("D" + sStrategyID);
            }
        }

        // 期貨昨漲跌 (期貨收-開)
        //
        public void CheckH(DateStrategyMap predsMap, DateStrategyMap dsMap)
        {
            double dDiff = predsMap.FuturesClose - predsMap.FuturesOpen;

            for (int i = 1; i < 21; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if (i < 10)
                    sStrategyID = string.Format("H0{0}", i);
                else
                    sStrategyID = string.Format("H{0}", i);

                switch (sStrategyID)
                {
                    case "H01":
                        if (dDiff > 20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H02":
                        if (dDiff > 40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H03":
                        if (dDiff > 60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H04":
                        if (dDiff > 80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H05":
                        if (dDiff > 100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H06":
                        if (dDiff > 120)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H07":
                        if (dDiff > 140)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H08":
                        if (dDiff > 160)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H09":
                        if (dDiff > 180)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H10":
                        if (dDiff > 200)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H11":
                        if (dDiff < -20)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H12":
                        if (dDiff < -40)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H13":
                        if (dDiff < -60)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H14":
                        if (dDiff < -80)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H15":
                        if (dDiff < -100)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H16":
                        if (dDiff < -120)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H17":
                        if (dDiff < -140)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H18":
                        if (dDiff < -160)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H19":
                        if (dDiff < -180)
                        {
                            bFlag = true;
                        }
                        break;
                    case "H20":
                        if (dDiff < -200)
                        {
                            bFlag = true;
                        }
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("D" + sStrategyID);
            }
        }

        // 摩台
        //
        public void CheckM(DateStrategyMap predsMap, DateStrategyMap predsMap2, DateStrategyMap dsMap)
        {
            double dFuturesOpen = dsMap.FuturesOpen;
            double dPreOpen = predsMap.FuturesOpen;
            double dPreClose = predsMap.FuturesClose; // 昨收
            double dPreClose2 = predsMap2.FuturesClose; // 前兩日收

            for (int i = 1; i < 28; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if (i < 10)
                    sStrategyID = string.Format("M0{0}", i);
                else
                    sStrategyID = string.Format("M{0}", i);

                if (m_Config.IsSTW(predsMap.Date))
                {                    
                    switch (sStrategyID)
                    {
                        case "M01":
                            // 前一日摩台指結算日上漲且紅K,並且今日開高
                            if (dPreClose > dPreClose2 && dPreClose - dPreOpen > 0 && dFuturesOpen > dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M02":
                            if (dPreClose > dPreClose2 && dPreClose - dPreOpen > 0 && dFuturesOpen < dPreClose)                            
                            {
                                bFlag = true;
                            }
                            break;
                        case "M03":
                            if (dPreClose > dPreClose2 && dPreClose - dPreOpen > 0)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M04":
                            if (dPreClose > dPreClose2 && dPreClose - dPreOpen < 0 && dFuturesOpen > dPreClose)                            
                            {
                                bFlag = true;
                            }
                            break;
                        case "M05":
                            if (dPreClose > dPreClose2 && dPreClose - dPreOpen < 0 && dFuturesOpen < dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M06":
                            if (dPreClose > dPreClose2 && dPreClose - dPreOpen < 0)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M07":
                            if (dPreClose > dPreClose2 && dFuturesOpen > dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M08":
                            if (dPreClose > dPreClose2 && dFuturesOpen < dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M09":
                            if (dPreClose > dPreClose2)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M10":
                            // 前一日摩台指結算日下跌且紅K,並且今日開高
                            if (dPreClose < dPreClose2 && dPreClose - dPreOpen > 0 && dFuturesOpen > dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M11":
                            if (dPreClose < dPreClose2 && dPreClose - dPreOpen > 0 && dFuturesOpen < dPreClose)                            
                            {
                                bFlag = true;
                            }
                            break;
                        case "M12":
                            if (dPreClose < dPreClose2 && dPreClose - dPreOpen > 0)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M13":
                            if (dPreClose < dPreClose2 && dPreClose - dPreOpen < 0 && dFuturesOpen > dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M14":
                            if (dPreClose < dPreClose2 && dPreClose - dPreOpen < 0 && dFuturesOpen < dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M15":
                            if (dPreClose < dPreClose2 && dPreClose - dPreOpen < 0)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M16":
                            if (dPreClose < dPreClose2 && dFuturesOpen > dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M17":
                            if (dPreClose < dPreClose2 && dFuturesOpen < dPreClose)                            
                            {
                                bFlag = true;
                            }
                            break;
                        case "M18":
                            if (dPreClose < dPreClose2)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M19":
                            if (dPreClose - dPreOpen > 0 && dFuturesOpen > dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M20":
                            if (dPreClose - dPreOpen > 0 && dFuturesOpen < dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M21":
                            if (dPreClose - dPreOpen > 0)                            
                            {
                                bFlag = true;
                            }
                            break;
                        case "M22":
                            if (dPreClose - dPreOpen < 0 && dFuturesOpen > dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M23":
                            if (dPreClose - dPreOpen < 0 && dFuturesOpen < dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M24":
                            if (dPreClose - dPreOpen < 0)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M25":
                            if (dFuturesOpen > dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M26":
                            if (dFuturesOpen < dPreClose)
                            {
                                bFlag = true;
                            }
                            break;
                        case "M27":
                            bFlag = true;
                            break;
                    }
                }

                if (bFlag)
                    dsMap.AddStrategy("D" + sStrategyID);
            }            
        }        

        // MA相關, V5 V6 V7 V8 V9 V19 V20 V21 V22 V23需當日開盤資料
        //
        public void CheckV(int nKey, DateStrategyMap dsMap, Dictionary<int, DateStrategyMap> DateStrategy)
        {            
            double dMA1, dMA2;
            double dFuturesOpen = dsMap.FuturesOpen;            
            double dPreFuturesClose = DateStrategy.ContainsKey(nKey - 1) ? DateStrategy[nKey - 1].FuturesClose : 0;

            for (int i = 1; i < 39; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if (i < 10)
                    sStrategyID = string.Format("V0{0}", i);
                else
                    sStrategyID = string.Format("V{0}", i);

                switch (sStrategyID)
                {
                    case "V05":
                        dMA1 = m_Tech.GetMA(5, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)
                            break;                        

                        if (dFuturesOpen - dMA1 > 0)
                            bFlag = true;
                        break;
                    case "V06":
                        dMA1 = m_Tech.GetMA(10, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)                  
                            break;
                        
                        if (dFuturesOpen - dMA1 > 0)                        
                            bFlag = true;                                                    
                        break;
                    case "V07":
                        dMA1 = m_Tech.GetMA(10, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)                  
                            break;
                        
                        if (dFuturesOpen - dMA1 > 0)
                            bFlag = true;
                        break;
                    case "V08":
                        dMA1 = m_Tech.GetMA(60, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)
                            break;
                        
                        if (dFuturesOpen - dMA1 > 0)                        
                            bFlag = true;                                                    
                        break;
                    case "V09":
                        dMA1 = m_Tech.GetMA(120, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)
                            break;
                        
                        if (dFuturesOpen - dMA1 > 0)                        
                            bFlag = true;                                                   
                        break;
                    case "V19":
                        dMA1 = m_Tech.GetMA(5, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)
                            break;
                        
                        if (dFuturesOpen - dMA1 < 0)                        
                            bFlag = true;                                                                           
                        break;
                    case "V20":
                        dMA1 = m_Tech.GetMA(10, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)
                            break;
                        
                        if (dFuturesOpen - dMA1 < 0)                        
                            bFlag = true;                                                                           
                        break;
                    case "V21":
                        dMA1 = m_Tech.GetMA(20, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)
                            break;
                        
                        if (dFuturesOpen - dMA1 < 0)                        
                            bFlag = true;                                                                           
                        break;
                    case "V22":
                        dMA1 = m_Tech.GetMA(60, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)
                            break;
                        
                        if (dFuturesOpen - dMA1 < 0)                        
                            bFlag = true;                                                                           
                        break;
                    case "V23":
                        dMA1 = m_Tech.GetMA(120, nKey, DateStrategy);
                        if (dFuturesOpen == 0 || dMA1 == 0)
                            break;
                        
                        if (dFuturesOpen - dMA1 < 0)                        
                            bFlag = true;                                                   
                        break;                        
                    case "V01":
                        dMA1 = m_Tech.GetMA(5, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(10, nKey, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 > 0)                        
                            bFlag = true;                                                   
                        break;      
                    case "V02":
                        dMA1 = m_Tech.GetMA(10, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(20, nKey, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 > 0)                        
                            bFlag = true;
                        break;
                    case "V03":
                        dMA1 = m_Tech.GetMA(20, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(60, nKey, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 > 0)                        
                            bFlag = true;
                        break;
                    case "V04":
                        dMA1 = m_Tech.GetMA(60, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(120, nKey, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 > 0)
                            bFlag = true;
                        break;
                    case "V10":
                        dMA1 = m_Tech.GetMA(5, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(5, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 > 0)
                            bFlag = true;
                        break;
                    case "V11":
                        dMA1 = m_Tech.GetMA(10, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(10, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 > 0)
                            bFlag = true;
                        break;
                    case "V12":
                        dMA1 = m_Tech.GetMA(20, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(20, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 > 0)
                            bFlag = true;
                        break;
                    case "V13":
                        dMA1 = m_Tech.GetMA(60, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(60, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 > 0)
                            bFlag = true;
                        break;
                    case "V14":
                        dMA1 = m_Tech.GetMA(120, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(120, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 > 0)
                            bFlag = true;
                        break;
                    case "V15":
                        dMA1 = m_Tech.GetMA(5, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(10, nKey, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 < 0)
                            bFlag = true;
                        break;
                    case "V16":
                        dMA1 = m_Tech.GetMA(10, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(20, nKey, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 < 0)
                            bFlag = true;
                        break;
                    case "V17":
                        dMA1 = m_Tech.GetMA(20, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(60, nKey, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 < 0)
                            bFlag = true;
                        break;
                    case "V18":
                        dMA1 = m_Tech.GetMA(60, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(120, nKey, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 < 0)
                            bFlag = true;
                        break;
                    case "V24":
                        dMA1 = m_Tech.GetMA(5, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(5, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 < 0)
                            bFlag = true;
                        break;
                    case "V25":
                        dMA1 = m_Tech.GetMA(10, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(10, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 < 0)
                            bFlag = true;
                        break;
                    case "V26":
                        dMA1 = m_Tech.GetMA(20, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(20, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 < 0)
                            bFlag = true;
                        break;
                    case "V27":
                        dMA1 = m_Tech.GetMA(60, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(60, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 < 0)
                            bFlag = true;
                        break;
                    case "V28":
                        dMA1 = m_Tech.GetMA(120, nKey, DateStrategy);
                        dMA2 = m_Tech.GetMA(120, nKey - 1, DateStrategy);
                        if (dMA2 == 0 || dMA1 == 0)
                            break;

                        if (dMA1 - dMA2 < 0)
                            bFlag = true;
                        break;
                    case "V29":
                        dMA1 = m_Tech.GetMA(5, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 > 0)                        
                            bFlag = true;                        
                        break;
                    case "V30":
                        dMA1 = m_Tech.GetMA(10, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 > 0)                        
                            bFlag = true;                        
                        break;
                    case "V31":
                        dMA1 = m_Tech.GetMA(20, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 > 0)                        
                            bFlag = true;                        
                        break;
                    case "V32":
                        dMA1 = m_Tech.GetMA(60, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 > 0)                        
                            bFlag = true;                        
                        break;
                    case "V33":
                        dMA1 = m_Tech.GetMA(120, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 > 0)                        
                            bFlag = true;                        
                        break;
                    case "V34":
                        dMA1 = m_Tech.GetMA(5, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 < 0)                        
                            bFlag = true;                        
                        break;
                    case "V35":
                        dMA1 = m_Tech.GetMA(10, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 < 0)                        
                            bFlag = true;                        
                        break;
                    case "V36":
                        dMA1 = m_Tech.GetMA(20, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 < 0)                        
                            bFlag = true;                        
                        break;
                    case "V37":
                        dMA1 = m_Tech.GetMA(60, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 < 0)                        
                            bFlag = true;                        
                        break;
                    case "V38":
                        dMA1 = m_Tech.GetMA(120, nKey, DateStrategy);
                        if (dPreFuturesClose == 0 || dMA1 == 0)
                            break;

                        if (dPreFuturesClose - dMA1 < 0)                        
                            bFlag = true;                        
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("D" + sStrategyID);
            }                            
        }
    }
}
