using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    /// <summary>
    /// N-point bezier
    /// </summary>
    public class NBezier : Bezier
    {
        private Vector2[] _controlPoints;
        public NBezier(Vector2 start, Vector2 end, params Vector2[] controlpoints) : base(start, end)
        {
            _controlPoints = controlpoints;
        }

        public override int Order => _controlPoints.Length + 2;

        public void SetOrder(int order)
        {
            if (Order == order) return;
            if (order < 2) throw new InvalidOperationException("Any curve needs at least 2 points");
            var result = new Vector2[order - 2];
            Array.Copy(_controlPoints, result, Math.Min(order, _controlPoints.Length));
            _controlPoints = result;
            OnPointChange();

        }

        public Vector2 GetControlPoint(int i)
        {
            return _controlPoints[i];
        }
        public void SetControlPoint(int i, Vector2 value)
        {
            _controlPoints[i] = value;
        }

        public Vector2 this[int i]
        {
            get { return _controlPoints[i]; }
            set
            {
                if (_controlPoints[i] == value) return;
                _controlPoints[i] = value;
                OnPointChange();
            }
        }


        protected override int GetRecommendedResolution()
        {
            var lengths = _controlPoints.Union(new[] { StartPoint, EndPoint }).ToArray();
            var avg = 0d;
            var highest = 0d;
            var smallest = double.PositiveInfinity;
            var n = lengths.Length;
            for (int i = 0; i < n; i++)
            {
                var length = lengths[i].LengthSquared();
                avg += length;
                highest = Math.Max(highest, length);
                smallest = Math.Min(smallest, length);
            }
            avg /= n;
            return (int)(Math.Sqrt(highest - avg + avg - smallest) / 10d);
        }

        public override Vector2 GetPositionAt(float t)
        {
            t = Normalize(t);
            var length = _controlPoints.Length - 1;
            var result = StartPoint * (float)Bernstein(0, length + 2, t);
            for (var i = 0; i <= length;)
            {
                result += _controlPoints[i] * (float)Bernstein(i++, length + 2, t);
            }
            result += EndPoint * (float)Bernstein(length + 1, length + 2, t);
            return result;
        }

        public override OrientedPoint GetOrientedPointAt(float t)
        {
            t = Normalize(t);
            //TODO
            return new OrientedPoint();
        }

        public override Angle GetTangentAt(float t)
        {
            t = Normalize(t);
            if (t > 0.0001f)
                return Angle.FromVector(GetPositionAt(t - 0.0001f) - GetPositionAt(t));
            return Angle.FromVector(GetPositionAt(t) - GetPositionAt(t + 0.0001f));
        }

        private static double Bernstein(int i, int n, double t)
        {
            var powers = Math.Pow(t, i) * Math.Pow(1 - t, n - i);
            long r = 1;
            long d;
            if (i > n) return 0;
            for (d = 1; d <= i; d++)
            {
                r *= n--;
                r /= d;
            }
            return r * powers;
        }
    }
}