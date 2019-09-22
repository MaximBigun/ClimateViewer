using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ClimateViewer
{
    class DataReader
    {       
        public List<DataPoint> ReadAllData(string path)
        {
            List<DataPoint> data = new List<DataPoint>();
            List<string> dataFromFile;
            try
            {
                dataFromFile = File.ReadLines(@path).ToList();
            }
            catch
            {
                return data;
            }
            foreach (string s in dataFromFile)
            {
                try
                {
                    string[] splitData = s.Split(' ');
                    DateTime myDate = DateTime.ParseExact(splitData[0] + " " + splitData[1], "dd.MM.yyyy HH:mm:ss",
                                               System.Globalization.CultureInfo.InvariantCulture);
                    data.Add(new DataPoint(myDate, Convert.ToDouble(splitData[2]), Convert.ToDouble(splitData[3]), Convert.ToDouble(splitData[4]), Convert.ToDouble(splitData[5])));

                }
                catch
                {
                    using (StreamWriter sw = new StreamWriter("exeptionLog.txt", true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " The data in the file " + path + " is corrupted.");
                    }
                }
            }
            return data;
        }
    }
}
