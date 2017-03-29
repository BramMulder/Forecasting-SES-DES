using System;

namespace Forecasting
{
    public static class SesAlgorithm
    {
        public static Tuple<double[], double[]> ExecuteAlgorithm(double[] demand)
        {
            double bestError = -1;
            double bestAlpha = -1;
            double[] ses = new double[0];

            for (double i = 0.1; i <= 1; i = i + 0.1)
            {
                ses = ComputeSes(i , demand, AdditionalMethods.CalculateAlpha);
                var squaredError = CalculateSquaredError(ses, demand);

                //Update best error (and the alpha) if a better one is found
                if (bestError < 0 || squaredError < bestError)
                {
                    bestError = squaredError;
                    bestAlpha = Math.Round(i, 3);
                }
            }

            ses = ComputeSes(bestAlpha, demand, AdditionalMethods.CalculateAlpha);

            return new Tuple<double[], double[]>(demand, ses);
        }

        private static double[] ComputeSes(double alpha, double[] x, Func<double[], double> init)
        {
            double [] s = new double[x.Length];
            s[0] = init(x);

            for (int t = 1; t < x.Length; t++)
            {
                s[t] = alpha*x[t - 1] + (1 - alpha)*s[t - 1];
            }
            return s;
        }


        private static double CalculateSquaredError(double[] ses, double[] demand)
        {
            if (ses.Length != demand.Length)
                return -1;

            var squaredDistancesSum = 0.0;

            //Calculate the Sum of Squared Distances - Skip the first value in the arrays
            for (int j = 1; j < ses.Length; j++)
            {
                squaredDistancesSum += Math.Pow(demand[j] - ses[j], 2);
            }

            var squaredDistancesAverage = squaredDistancesSum/ses.Length;
            var squaredError = Math.Sqrt(squaredDistancesAverage);

            return squaredError;
        }
    }
}