using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ClimateViewer
{
    public partial class mainForm : Form
    {
        public Timer timer = new Timer();
        List<DataPoint> climateData = new List<DataPoint>();
        private int timerInterval = 1000 * 60 * 5; //10 minutes
        private int temperatureMin = 19;
        private int temperatureMax = 23;
        private int pressureMin = 740;
        private int pressureMax = 745;
        private int humidityMin = 40;
        private int humidityMax = 60;
        private int illuminanceMin = 300;
        private int illuminanceMax = 500;

        public mainForm()
        {
            InitializeComponent();
            timer.Interval = timerInterval;
            timer.Tick += Timer_Tick;
            timer.Start();
            Analyze();
        }

        public void BuildGraph(List<DataPoint> climateData)
        {
            chartTemperature.Series[0].Points.Clear();
            chartPressure.Series[0].Points.Clear();
            chartHumidity.Series[0].Points.Clear();
            chartIlluminance.Series[0].Points.Clear();
            foreach (DataPoint p in climateData)
            {
                if (p.time != new DateTime())
                {
                    chartTemperature.Series[0].Points.AddXY(p.time, p.temperature);
                    chartPressure.Series[0].Points.AddXY(p.time, p.pressure);
                    chartHumidity.Series[0].Points.AddXY(p.time, p.humidity);
                    chartIlluminance.Series[0].Points.AddXY(p.time, p.illuminance);
                }
            }
        }
        
        public void Analyze()
        {
            string path = datePicker.Value.Date.ToString("yyyy-MM-dd") + ".txt";
            DataReader reader = new DataReader();
            climateData = reader.ReadAllData(path);
            BuildGraph(climateData);
            if (climateData.Any<DataPoint>() && (DateTime.Now - climateData.Last<DataPoint>().time).TotalMinutes < timerInterval)
            {
                DataPoint currentValue = climateData.Last<DataPoint>();
                string log = "";
                if (currentValue.temperature < temperatureMin || currentValue.temperature > temperatureMax)
                    log += " temperature  = " + currentValue.temperature;
                if (currentValue.pressure < pressureMin || currentValue.pressure > pressureMax)
                    log += " pressure  = " + currentValue.pressure;
                if (currentValue.humidity < humidityMin || currentValue.humidity > humidityMax)
                    log += " humidity  = " + currentValue.humidity;
                if (currentValue.illuminance < illuminanceMin || currentValue.illuminance > illuminanceMax)
                    log += " illuminance  = " + currentValue.illuminance;
                if (log != "")
                    using (StreamWriter sw = new StreamWriter("climateLog.txt", true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(currentValue.time.ToString() + log);
                    }
            }
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            if (datePicker.Value.Date != DateTime.Today)
                timer.Stop();
            else
                timer.Start();
            Analyze();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Analyze();
        }

        private void b_log_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("climateLog.txt");
            }
            catch
            {
                MessageBox.Show("There are no log entries.");
            }
        }
    }
}
