using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OStock_Simulation
{
    class StrategyC
    {
        public StrategyC()
        {
            
        }

        public void CheckB(int nKey, DateStrategyMap dsMap, Dictionary<int, DateStrategyMap> DateStrategy)
        {
            for (int i = 1; i < 26; i++)
            {
                bool bFlag = false;
                string sStrategyID;

                if (i < 10)
                    sStrategyID = string.Format("B0{0}", i);
                else
                    sStrategyID = string.Format("B{0}", i);

                switch (sStrategyID)
                {
                    case "B01":
                        bFlag = CalcB(1, nKey, DateStrategy, 3e6);
                        break;
                    case "B02":
                        bFlag = CalcB(2, nKey, DateStrategy, 3e6);
                        break;
                    case "B03":
                        bFlag = CalcB(3, nKey, DateStrategy, 3e6);
                        break;
                    case "B04":
                        bFlag = CalcB(4, nKey, DateStrategy, 3e6);
                        break;
                    case "B05":
                        bFlag = CalcB(1, nKey, DateStrategy, 6e6);
                        break;
                    case "B06":
                        bFlag = CalcB(2, nKey, DateStrategy, 6e6);
                        break;
                    case "B07":
                        bFlag = CalcB(3, nKey, DateStrategy, 6e6);
                        break;
                    case "B08":
                        bFlag = CalcB(4, nKey, DateStrategy, 6e6);
                        break;
                    case "B09":
                        bFlag = CalcB(1, nKey, DateStrategy, 9e6);
                        break;
                    case "B10":
                        bFlag = CalcB(2, nKey, DateStrategy, 9e6);
                        break;
                    case "B11":
                        bFlag = CalcB(3, nKey, DateStrategy, 9e6);
                        break;
                    case "B12":
                        bFlag = CalcB(4, nKey, DateStrategy, 9e6);
                        break;
                    case "B14":
                        bFlag = CalcB(1, nKey, DateStrategy, -3e6);
                        break;
                    case "B15":
                        bFlag = CalcB(2, nKey, DateStrategy, -3e6);
                        break;
                    case "B16":
                        bFlag = CalcB(3, nKey, DateStrategy, -3e6);
                        break;
                    case "B17":
                        bFlag = CalcB(4, nKey, DateStrategy, -3e6);
                        break;
                    case "B18":
                        bFlag = CalcB(1, nKey, DateStrategy, -6e6);
                        break;
                    case "B19":
                        bFlag = CalcB(2, nKey, DateStrategy, -6e6);
                        break;
                    case "B20":
                        bFlag = CalcB(3, nKey, DateStrategy, -6e6);
                        break;
                    case "B21":
                        bFlag = CalcB(4, nKey, DateStrategy, -6e6);
                        break;
                    case "B22":
                        bFlag = CalcB(1, nKey, DateStrategy, -9e6);
                        break;
                    case "B23":
                        bFlag = CalcB(2, nKey, DateStrategy, -9e6);
                        break;
                    case "B24":
                        bFlag = CalcB(3, nKey, DateStrategy, -9e6);
                        break;
                    case "B25":
                        bFlag = CalcB(4, nKey, DateStrategy, -9e6);
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("C" + sStrategyID);
            }                        
        }

        private bool CalcB(int nDay, int nKey, Dictionary<int, DateStrategyMap> DateStrategy, double dMoney)
        {
            if (nKey <= nDay + 1)
                return false;

            for (int i = 0; i < nDay; i++)
            {
                DateStrategyMap firstdsMap = DateStrategy[nKey - nDay + i - 1];
                DateStrategyMap seconddsMap = DateStrategy[nKey - nDay + i];
                if (dMoney > 0)
                {
                    if (!(seconddsMap.FFutures - firstdsMap.FFutures >= dMoney && seconddsMap.FFutures > 0))
                        return false;
                }
                else
                {
                    if (!(seconddsMap.FFutures - firstdsMap.FFutures <= dMoney && seconddsMap.FFutures < 0))
                        return false;
                }
            }
            
            return true;
        }

        public void CheckC(int nKey, DateStrategyMap dsMap, Dictionary<int, DateStrategyMap> DateStrategy)
        {
            for (int i = 1; i < 26; i++)
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
                        bFlag = CalcC(1, nKey, DateStrategy, 3e6);
                        break;
                    case "C02":
                        bFlag = CalcC(2, nKey, DateStrategy, 3e6);
                        break;
                    case "C03":
                        bFlag = CalcC(3, nKey, DateStrategy, 3e6);
                        break;
                    case "C04":
                        bFlag = CalcC(4, nKey, DateStrategy, 3e6);
                        break;
                    case "C05":
                        bFlag = CalcC(1, nKey, DateStrategy, 6e6);
                        break;
                    case "C06":
                        bFlag = CalcC(2, nKey, DateStrategy, 6e6);
                        break;
                    case "C07":
                        bFlag = CalcC(3, nKey, DateStrategy, 6e6);
                        break;
                    case "C08":
                        bFlag = CalcC(4, nKey, DateStrategy, 6e6);
                        break;
                    case "C09":
                        bFlag = CalcC(1, nKey, DateStrategy, 9e6);
                        break;
                    case "C10":
                        bFlag = CalcC(2, nKey, DateStrategy, 9e6);
                        break;
                    case "C11":
                        bFlag = CalcC(3, nKey, DateStrategy, 9e6);
                        break;
                    case "C12":
                        bFlag = CalcC(4, nKey, DateStrategy, 9e6);
                        break;
                    case "C14":
                        bFlag = CalcC(1, nKey, DateStrategy, -3e6);
                        break;
                    case "C15":
                        bFlag = CalcC(2, nKey, DateStrategy, -3e6);
                        break;
                    case "C16":
                        bFlag = CalcC(3, nKey, DateStrategy, -3e6);
                        break;
                    case "C17":
                        bFlag = CalcC(4, nKey, DateStrategy, -3e6);
                        break;
                    case "C18":
                        bFlag = CalcC(1, nKey, DateStrategy, -6e6);
                        break;
                    case "C19":
                        bFlag = CalcC(2, nKey, DateStrategy, -6e6);
                        break;
                    case "C20":
                        bFlag = CalcC(3, nKey, DateStrategy, -6e6);
                        break;
                    case "C21":
                        bFlag = CalcC(4, nKey, DateStrategy, -6e6);
                        break;
                    case "C22":
                        bFlag = CalcC(1, nKey, DateStrategy, -9e6);
                        break;
                    case "C23":
                        bFlag = CalcC(2, nKey, DateStrategy, -9e6);
                        break;
                    case "C24":
                        bFlag = CalcC(3, nKey, DateStrategy, -9e6);
                        break;
                    case "C25":
                        bFlag = CalcC(4, nKey, DateStrategy, -9e6);
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("C" + sStrategyID);
            }                        
        }

        private bool CalcC(int nDay, int nKey, Dictionary<int, DateStrategyMap> DateStrategy, double dMoney)
        {
            if (nKey <= nDay + 1)
                return false;
            
            DateStrategyMap lastdsMap = DateStrategy[nKey - 1];
            DateStrategyMap firstdsMap = DateStrategy[nKey - nDay - 1];
            if (dMoney > 0)
            {
                if (lastdsMap.FFutures - firstdsMap.FFutures >= dMoney && lastdsMap.FFutures > 0 && lastdsMap.FOptions > 0)
                    return true;
            }
            else
            {
                if (lastdsMap.FFutures - firstdsMap.FFutures <= dMoney && lastdsMap.FFutures < 0 && lastdsMap.FOptions < 0)
                    return true;
            }
            
            return false;
        }

        public void CheckD(int nKey, DateStrategyMap dsMap, Dictionary<int, DateStrategyMap> DateStrategy)
        {
            for (int i = 1; i < 26; i++)
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
                        bFlag = CalcD(1, nKey, DateStrategy, 3e6);
                        break;
                    case "D02":
                        bFlag = CalcD(2, nKey, DateStrategy, 3e6);
                        break;
                    case "D03":
                        bFlag = CalcD(3, nKey, DateStrategy, 3e6);
                        break;
                    case "D04":
                        bFlag = CalcD(4, nKey, DateStrategy, 3e6);
                        break;
                    case "D05":
                        bFlag = CalcD(1, nKey, DateStrategy, 6e6);
                        break;
                    case "D06":
                        bFlag = CalcD(2, nKey, DateStrategy, 6e6);
                        break;
                    case "D07":
                        bFlag = CalcD(3, nKey, DateStrategy, 6e6);
                        break;
                    case "D08":
                        bFlag = CalcD(4, nKey, DateStrategy, 6e6);
                        break;
                    case "D09":
                        bFlag = CalcD(1, nKey, DateStrategy, 9e6);
                        break;
                    case "D10":
                        bFlag = CalcD(2, nKey, DateStrategy, 9e6);
                        break;
                    case "D11":
                        bFlag = CalcD(3, nKey, DateStrategy, 9e6);
                        break;
                    case "D12":
                        bFlag = CalcD(4, nKey, DateStrategy, 9e6);
                        break;
                    case "D14":
                        bFlag = CalcD(1, nKey, DateStrategy, -3e6);
                        break;
                    case "D15":
                        bFlag = CalcD(2, nKey, DateStrategy, -3e6);
                        break;
                    case "D16":
                        bFlag = CalcD(3, nKey, DateStrategy, -3e6);
                        break;
                    case "D17":
                        bFlag = CalcD(4, nKey, DateStrategy, -3e6);
                        break;
                    case "D18":
                        bFlag = CalcD(1, nKey, DateStrategy, -6e6);
                        break;
                    case "D19":
                        bFlag = CalcD(2, nKey, DateStrategy, -6e6);
                        break;
                    case "D20":
                        bFlag = CalcD(3, nKey, DateStrategy, -6e6);
                        break;
                    case "D21":
                        bFlag = CalcD(4, nKey, DateStrategy, -6e6);
                        break;
                    case "D22":
                        bFlag = CalcD(1, nKey, DateStrategy, -9e6);
                        break;
                    case "D23":
                        bFlag = CalcD(2, nKey, DateStrategy, -9e6);
                        break;
                    case "D24":
                        bFlag = CalcD(3, nKey, DateStrategy, -9e6);
                        break;
                    case "D25":
                        bFlag = CalcD(4, nKey, DateStrategy, -9e6);
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("C" + sStrategyID);
            }
        }

        private bool CalcD(int nDay, int nKey, Dictionary<int, DateStrategyMap> DateStrategy, double dMoney)
        {
            if (nKey <= nDay + 1)
                return false;

            DateStrategyMap lastdsMap = DateStrategy[nKey - 1];
            DateStrategyMap firstdsMap = DateStrategy[nKey - nDay - 1];
            if (dMoney > 0)
            {
                if (lastdsMap.FFutures - firstdsMap.FFutures >= dMoney && lastdsMap.FFutures > 0)
                    return true;
            }
            else
            {
                if (lastdsMap.FFutures - firstdsMap.FFutures <= dMoney && lastdsMap.FFutures < 0)
                    return true;
            }

            return false;
        }

        public void CheckE(int nKey, DateStrategyMap dsMap, Dictionary<int, DateStrategyMap> DateStrategy)
        {
            for (int i = 1; i < 26; i++)
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
                        bFlag = CalcE(1, nKey, DateStrategy, 3e6);
                        break;
                    case "E02":
                        bFlag = CalcE(2, nKey, DateStrategy, 3e6);
                        break;
                    case "E03":
                        bFlag = CalcE(3, nKey, DateStrategy, 3e6);
                        break;
                    case "E04":
                        bFlag = CalcE(4, nKey, DateStrategy, 3e6);
                        break;
                    case "E05":
                        bFlag = CalcE(1, nKey, DateStrategy, 6e6);
                        break;
                    case "E06":
                        bFlag = CalcE(2, nKey, DateStrategy, 6e6);
                        break;
                    case "E07":
                        bFlag = CalcE(3, nKey, DateStrategy, 6e6);
                        break;
                    case "E08":
                        bFlag = CalcE(4, nKey, DateStrategy, 6e6);
                        break;
                    case "E09":
                        bFlag = CalcE(1, nKey, DateStrategy, 9e6);
                        break;
                    case "E10":
                        bFlag = CalcE(2, nKey, DateStrategy, 9e6);
                        break;
                    case "E11":
                        bFlag = CalcE(3, nKey, DateStrategy, 9e6);
                        break;
                    case "E12":
                        bFlag = CalcE(4, nKey, DateStrategy, 9e6);
                        break;
                    case "E14":
                        bFlag = CalcE(1, nKey, DateStrategy, -3e6);
                        break;
                    case "E15":
                        bFlag = CalcE(2, nKey, DateStrategy, -3e6);
                        break;
                    case "E16":
                        bFlag = CalcE(3, nKey, DateStrategy, -3e6);
                        break;
                    case "E17":
                        bFlag = CalcE(4, nKey, DateStrategy, -3e6);
                        break;
                    case "E18":
                        bFlag = CalcE(1, nKey, DateStrategy, -6e6);
                        break;
                    case "E19":
                        bFlag = CalcE(2, nKey, DateStrategy, -6e6);
                        break;
                    case "E20":
                        bFlag = CalcE(3, nKey, DateStrategy, -6e6);
                        break;
                    case "E21":
                        bFlag = CalcE(4, nKey, DateStrategy, -6e6);
                        break;
                    case "E22":
                        bFlag = CalcE(1, nKey, DateStrategy, -9e6);
                        break;
                    case "E23":
                        bFlag = CalcE(2, nKey, DateStrategy, -9e6);
                        break;
                    case "E24":
                        bFlag = CalcE(3, nKey, DateStrategy, -9e6);
                        break;
                    case "E25":
                        bFlag = CalcE(4, nKey, DateStrategy, -9e6);
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("C" + sStrategyID);
            }
        }

        private bool CalcE(int nDay, int nKey, Dictionary<int, DateStrategyMap> DateStrategy, double dMoney)
        {
            if (nKey <= nDay + 1)
                return false;

            DateStrategyMap lastdsMap = DateStrategy[nKey - 1];
            DateStrategyMap firstdsMap = DateStrategy[nKey - nDay - 1];
            if (dMoney > 0)
            {
                if (lastdsMap.FFutures - firstdsMap.FFutures >= dMoney && lastdsMap.FOptions > 0)
                    return true;
            }
            else
            {
                if (lastdsMap.FFutures - firstdsMap.FFutures <= dMoney && lastdsMap.FOptions < 0)
                    return true;
            }

            return false;
        }

        public void CheckF(int nKey, DateStrategyMap dsMap, Dictionary<int, DateStrategyMap> DateStrategy)
        {
            for (int i = 1; i < 26; i++)
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
                        bFlag = CalcF(1, nKey, DateStrategy, 3e6);
                        break;
                    case "F02":
                        bFlag = CalcF(2, nKey, DateStrategy, 3e6);
                        break;
                    case "F03":
                        bFlag = CalcF(3, nKey, DateStrategy, 3e6);
                        break;
                    case "F04":
                        bFlag = CalcF(4, nKey, DateStrategy, 3e6);
                        break;
                    case "F05":
                        bFlag = CalcF(1, nKey, DateStrategy, 6e6);
                        break;
                    case "F06":
                        bFlag = CalcF(2, nKey, DateStrategy, 6e6);
                        break;
                    case "F07":
                        bFlag = CalcF(3, nKey, DateStrategy, 6e6);
                        break;
                    case "F08":
                        bFlag = CalcF(4, nKey, DateStrategy, 6e6);
                        break;
                    case "F09":
                        bFlag = CalcF(1, nKey, DateStrategy, 9e6);
                        break;
                    case "F10":
                        bFlag = CalcF(2, nKey, DateStrategy, 9e6);
                        break;
                    case "F11":
                        bFlag = CalcF(3, nKey, DateStrategy, 9e6);
                        break;
                    case "F12":
                        bFlag = CalcF(4, nKey, DateStrategy, 9e6);
                        break;
                    case "F14":
                        bFlag = CalcF(1, nKey, DateStrategy, -3e6);
                        break;
                    case "F15":
                        bFlag = CalcF(2, nKey, DateStrategy, -3e6);
                        break;
                    case "F16":
                        bFlag = CalcF(3, nKey, DateStrategy, -3e6);
                        break;
                    case "F17":
                        bFlag = CalcF(4, nKey, DateStrategy, -3e6);
                        break;
                    case "F18":
                        bFlag = CalcF(1, nKey, DateStrategy, -6e6);
                        break;
                    case "F19":
                        bFlag = CalcF(2, nKey, DateStrategy, -6e6);
                        break;
                    case "F20":
                        bFlag = CalcF(3, nKey, DateStrategy, -6e6);
                        break;
                    case "F21":
                        bFlag = CalcF(4, nKey, DateStrategy, -6e6);
                        break;
                    case "F22":
                        bFlag = CalcF(1, nKey, DateStrategy, -9e6);
                        break;
                    case "F23":
                        bFlag = CalcF(2, nKey, DateStrategy, -9e6);
                        break;
                    case "F24":
                        bFlag = CalcF(3, nKey, DateStrategy, -9e6);
                        break;
                    case "F25":
                        bFlag = CalcF(4, nKey, DateStrategy, -9e6);
                        break;
                }

                if (bFlag)
                    dsMap.AddStrategy("C" + sStrategyID);
            }
        }

        private bool CalcF(int nDay, int nKey, Dictionary<int, DateStrategyMap> DateStrategy, double dMoney)
        {
            if (nKey <= nDay + 1)
                return false;

            DateStrategyMap lastdsMap = DateStrategy[nKey - 1];
            DateStrategyMap firstdsMap = DateStrategy[nKey - nDay - 1];
            if (dMoney > 0)
            {
                if (lastdsMap.FFutures - firstdsMap.FFutures >= dMoney)
                    return true;
            }
            else
            {
                if (lastdsMap.FFutures - firstdsMap.FFutures <= dMoney)
                    return true;
            }

            return false;
        }
    }
}
