using System;

namespace Forecasting
{
    public class DesAlgorithm
    {
        public static Tuple<double[], double[]> ExecuteAlgorithm(double[] demand)
        {
            double bestError = -1;
            double bestAlpha = -1;
            double bestBeta = -1;
            double[] des = new double[0];

            for (double i = 0.1; i <= 1; i = i + 0.1)
            {
                for (double k = 0.1; k <= 1; k = k + 0.1)
                {
                    des = ComputeDes(i, k, demand, AdditionalMethods.CalculateAlpha);
                    var squaredError = CalculateSquaredError(des, demand);

                    //Update best error (and the alpha) if a better one is found
                    if (bestError < 0 || squaredError < bestError)
                    {
                        bestError = squaredError;
                        bestAlpha = Math.Round(i, 3);
                        bestBeta = Math.Round(k, 3);
                    }
                }
            }

            des = ComputeDes(bestAlpha, bestBeta, demand, AdditionalMethods.CalculateAlpha);

            return new Tuple<double[], double[]>(demand, des);
        }

        private static double[] ComputeDes(double alpha, double beta, double[] x, Func<double[], double> init)
        {
            double[] s = new double[x.Length];
            double[] b = new double[x.Length];
            double[] forecasts = new double[x.Length+1];
            s[0] = init(x);
            s[1] = alpha * x[1] + (1 - alpha) * (s[1 - 1] + (x[0] - x[0]));

            for (int t = 2; t < x.Length; t++)
            {
                var prevTrend = x[t - 1] - x[t - 2];
                var smoothed = alpha * x[t] + (1 - alpha) * (s[t - 1] + (int)prevTrend);
                s[t] = smoothed;
                var estimate = beta*(s[t] - s[t - 1]) + (1 - beta)*prevTrend;
                b[t] = estimate;

                forecasts[t+1] = s[t] + b[t];
            }
            return forecasts;
        }


        private static double CalculateSquaredError(double[] des, double[] demand)
        {
            if (des.Length-1 != demand.Length)
                return -1;

            var squaredDistancesSum = 0.0;

            //Calculate the Sum of Squared Distances - Skip the first value in the arrays
            for (int j = 3; j < demand.Length; j++)
            {
                squaredDistancesSum += Math.Pow(demand[j] - des[j], 2);
            }

            var squaredDistancesAverage = squaredDistancesSum / des.Length;
            var squaredError = Math.Sqrt(squaredDistancesAverage);

            return squaredError;
        }
    }
}