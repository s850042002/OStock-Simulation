using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OStock_Simulation
{
    class DateStrategyMap
    {
        private DateTime m_Date;
        private ArrayList m_Strategy = new ArrayList();
        private double[] m_arrFuturesOHLC = new double[4];
        private double[] m_arrSpotOHLC = new double[4];
        private Dictionary<DateTime, double[]> m_5K = new Dictionary<DateTime, double[]>();
        private double m_dFFutures = 0;
        private double m_dFOptions = 0;

        public DateStrategyMap(DateTime dt)
        {
            this.m_Date = dt;
        }

        public DateTime Date
        {
            get
            {
                return m_Date;
            }            
        }

        public double FuturesOpen 
        {
            get 
            {
                return m_arrFuturesOHLC[0];
            }
            set
            {
                m_arrFuturesOHLC[0] = value;
            }
        }

        public double FuturesHigh
        {
            get
            {
                return m_arrFuturesOHLC[1];
            }
            set
            {
                m_arrFuturesOHLC[1] = value;
            }
        }

        public double FuturesLow
        {
            get
            {
                return m_arrFuturesOHLC[2];
            }
            set
            {
                m_arrFuturesOHLC[2] = value;
            }
        }

        public double FuturesClose
        {
            get
            {
                return m_arrFuturesOHLC[3];
            }
            set
            {
                m_arrFuturesOHLC[3] = value;
            }
        }

        public double SpotOpen
        {
            get
            {
                return m_arrSpotOHLC[0];
            }
            set
            {
                m_arrSpotOHLC[0] = value;
            }
        }

        public double SpotHigh
        {
            get
            {
                return m_arrSpotOHLC[1];
            }
            set
            {
                m_arrSpotOHLC[1] = value;
            }
        }

        public double SpotLow
        {
            get
            {
                return m_arrSpotOHLC[2];
            }
            set
            {
                m_arrSpotOHLC[2] = value;
            }
        }

        public double SpotClose
        {
            get
            {
                return m_arrSpotOHLC[3];
            }
            set
            {
                m_arrSpotOHLC[3] = value;
            }
        }

        public double FFutures
        {
            get
            {
                return m_dFFutures;
            }
            set
            {
                m_dFFutures = value;
            }
        }

        public double FOptions
        {
            get
            {
                return m_dFOptions;
            }
            set
            {
                m_dFOptions = value;
            }
        }

        public void Add5K(DateTime Time, double[] OHLCV)
        {
            if (!m_5K.ContainsKey(Time))
            {
                m_5K.Add(Time, OHLCV);
            }
        }

        public Dictionary<DateTime, double[]> Get5KAll()
        {
            return m_5K;
        }

        public double Get5KOpen(DateTime Time)
        {
            Time = m_Date.Add(Time.TimeOfDay);
            if (m_5K.ContainsKey(Time))
            {
                return m_5K[Time][0];
            }
            return 0;
        }

        public double Get5KHigh(DateTime Time)
        {
            Time = m_Date.Add(Time.TimeOfDay);
            if (m_5K.ContainsKey(Time))
            {
                return m_5K[Time][1];
            }
            return 0;
        }

        public double Get5KLow(DateTime Time)
        {
            Time = m_Date.Add(Time.TimeOfDay);
            if (m_5K.ContainsKey(Time))
            {
                return m_5K[Time][2];
            }
            return 0;
        }

        public double Get5KClose(DateTime Time)
        {
            Time = m_Date.Add(Time.TimeOfDay);
            if (m_5K.ContainsKey(Time))
            {
                return m_5K[Time][3];
            }
            return 0;
        }

        public void AddStrategy(string sStrategy)
        {
            if (!m_Strategy.Contains(sStrategy))
            {
                m_Strategy.Add(sStrategy);
            }
        }

        public void RemoveStrategy(string sStrategy)
        {
            m_Strategy.Remove(sStrategy);
        }

        public bool IsContainStrategy(string sStrategy)
        {
            if (m_Strategy.Contains(sStrategy))
                return true;
            else if(sStrategy == "") // 給單跑策略
                return true;
            else
                return false;
        }
    }
}
