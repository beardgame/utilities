namespace Bearded.Utilities;

public interface IInterpolationMethod2d
{
    public double Interpolate(double value00, double value10, double value01, double value11, double u, double v);
}
