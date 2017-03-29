using System;
using System.IO;
using System.Windows.Forms;

namespace Forecasting
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var data = ReadCsv();
            var sesResponse = SesAlgorithm.ExecuteAlgorithm(data);
            var desResponse = DesAlgorithm.ExecuteAlgorithm(data);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(sesResponse.Item1, sesResponse.Item2, desResponse.Item2));
        }

        private static double[] ReadCsv()
        {
            double[] demand;
            char[] delimiters =  { ';', ',' };
            const string fileLocation = @"SwordData.CSV";
            
            //Check if file exists
            bool fileExists = File.Exists(fileLocation);

            if (!fileExists)
                return null;

            StreamReader reader = new StreamReader(fileLocation);

            //Instantiate array with correct length
            int columnSize = 0;
            while (reader.ReadLine() != null) columnSize++;
            //Remove one because it also reads the last empty line
            demand = new double[columnSize - 1];
            reader.Close();

            using (reader)
            {
                reader = new StreamReader(fileLocation);
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    var fields = line.Split(delimiters[1]);
                    //If the fields read is not empty
                    if (!fields[0].Equals(""))
                        demand[Convert.ToInt32(fields[0]) - 1] = Convert.ToDouble(fields[1]);
                }
            }
            return demand;
        }


    }
}
