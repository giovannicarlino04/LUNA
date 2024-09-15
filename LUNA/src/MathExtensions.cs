using MathNet.Numerics;
using System;

namespace LUNA
{
    public static class MathExtensions
    {
        public static double Clamp(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}