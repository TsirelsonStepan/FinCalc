namespace FinCalc.Calculate;

public static partial class Basic
{
    public static double LinearRegressionSlope(IReadOnlyList<double> x, IReadOnlyList<double> y)
    {
        if (x.Count != y.Count)
            throw new ArgumentException("x and y must have same length");

        if (x.Count < 2)
            throw new ArgumentException("At least two observations required");

        double meanX = x.Average();
        double meanY = y.Average();

        double sumXY = 0;
        double sumXX = 0;

        for (int i = 0; i < x.Count; i++)
        {
            double dx = x[i] - meanX;
            double dy = y[i] - meanY;

            sumXY += dx * dy;
            sumXX += dx * dx;
        }

        if (sumXX == 0)
            throw new InvalidOperationException("Variance of X is zero");

        return sumXY / sumXX;
    }
}