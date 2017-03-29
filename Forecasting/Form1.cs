using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Forecasting
{
    public partial class Form1 : Form
    {
        private double[] _demand;
        private double[] _ses;
        private double[] _des;
        public Form1(double[] demand, double[] ses, double[] des)
        {
            _demand = demand;
            _ses = ses;
            _des = des;
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < _demand.Length; i++)
            {
                chart1.Series["Original Data"].Points.AddXY(i, _demand[i]);
                chart1.Series["Smoothed"].Points.AddXY(i, _ses[i]);
            }

            for (int j = 0; j < 10; j++)
            {
                chart1.Series["Smoothed"].Points.AddXY(_demand.Length+j, _ses[_demand.Length-1]);
            }

            chart1.Series["Original Data"].ChartType = SeriesChartType.FastLine;
            chart1.Series["Original Data"].Color = Color.Blue;

            chart1.Series["Smoothed"].ChartType = SeriesChartType.FastLine;
            chart1.Series["Smoothed"].Color = Color.Red;
        }

        private void chart2_Click(object sender, EventArgs e)
        {
            for (int k = 3; k < _demand.Length; k++)
            {
                chart2.Series["Original Data"].Points.AddXY(k, _demand[k]);
                chart2.Series["Forecasted"].Points.AddXY(k, _des[k]);
            }

            //TODO proper forecasting
            //for (int m = 0; m < 10; m++)
            //{
            //    chart2.Series["Forecasted"].Points.AddXY(_demand.Length + m, _des[_demand.Length - 1]);
            //}

            chart2.Series["Original Data"].ChartType = SeriesChartType.FastLine;
            chart2.Series["Original Data"].Color = Color.Red;

            chart2.Series["Forecasted"].ChartType = SeriesChartType.FastLine;
            chart2.Series["Forecasted"].Color = Color.Blue;
        }
    }
}
