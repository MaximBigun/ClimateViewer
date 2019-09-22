using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateViewer
{
    public class DataPoint
    {
        public DateTime time;
        public double temperature;
        public double pressure;
        public double humidity;
        public double illuminance;

        public DataPoint()
        {
        }

        public DataPoint(DateTime time, double temperature, double pressure, double humidity, double illuminance)
        {
            this.time = time;
            this.temperature = temperature;
            this.pressure = pressure;
            this.humidity = humidity;
            this.illuminance = illuminance;
        }
    }
}
