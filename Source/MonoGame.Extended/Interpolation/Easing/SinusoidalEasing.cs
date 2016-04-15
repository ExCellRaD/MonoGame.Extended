using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Interpolation.Easing
{
    public class SinusoidalEasing : EasingFunction
    {
        protected override double Function(double t) {
            return Math.Sin(t * MathHelper.PiOver2);
        }
    }
}