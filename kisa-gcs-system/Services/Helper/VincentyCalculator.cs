namespace kisa_gcs_system.Services.Helper;

public class VincentyCalculator
{
    
    public double DistanceCalculater(double StartLat, double StartLon, double TargetLat, double TargetLon)
    {
        const double a = 6378137; // 지구의 장축 반지름 (단위: 미터)
        const double f = 1 / 298.257223563; // 편평률
        const double b = (1 - f) * a; // 지구의 단축 반지름 (단위: 미터)

        double U1 = Math.Atan((1 - f) * Math.Tan(StartLat * Math.PI / 180));
        double U2 = Math.Atan((1 - f) * Math.Tan(TargetLat * Math.PI / 180));
        double L = TargetLon * Math.PI / 180 - StartLon * Math.PI / 180;
        double sinU1 = Math.Sin(U1);
        double cosU1 = Math.Cos(U1);
        double sinU2 = Math.Sin(U2);
        double cosU2 = Math.Cos(U2);

        double lambda = L;
        double lambdaP, iterLimit = 100;

        double cosSqAlpha;
        double sinSigma;
        double cos2SigmaM;
        double cosSigma;
        double sigma;
        do
        {
            double sinLambda = Math.Sin(lambda);
            double cosLambda = Math.Cos(lambda);
            sinSigma = Math.Sqrt((cosU2 * sinLambda) * (cosU2 * sinLambda) +
                                 (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
            cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
            sigma = Math.Atan2(sinSigma, cosSigma);
            double sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
            cosSqAlpha = 1 - sinAlpha * sinAlpha;
            cos2SigmaM = cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;

            if (double.IsNaN(cos2SigmaM))
                cos2SigmaM = 0;

            double C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
            lambdaP = lambda;
            lambda = L + (1 - C) * f * sinAlpha *
                (sigma + C * sinSigma *
                (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));

        } while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0);

        if (iterLimit == 0)
        {
            throw new InvalidOperationException("Vincenty formula failed to converge.");
        }

        double uSq = cosSqAlpha * (a * a - (a * a - b * b) * cosSqAlpha) / (a * a);
        double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
        double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
        double deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 *
            (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) - B / 6 * cos2SigmaM *
            (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));

        double distance = b * A * (sigma - deltaSigma);

        return distance;    // 미터단위로 반환
    }

    private double ToRadians(double degree)
    {
        return degree * Math.PI / 180;
    }
}