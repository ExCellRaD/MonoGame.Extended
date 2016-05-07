using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    public class CubicBezier : Bezier
    {
        public enum CubicBezierType //TODO implement this
        {
            Serpentine,
            Cusp,
            Loop,
            /// <summary>
            /// Controlpoints are equal
            /// </summary>
            Quadratic,
            /// <summary>
            /// Last three points are equal
            /// </summary>
            Line,
            /// <summary>
            /// All points are equal
            /// </summary>
            Point,
        }

        private Vector2 _controlPoint1;
        private Vector2 _controlPoint2;

        public CubicBezier(Vector2 start, Vector2 controlpoint1, Vector2 controlpoint2, Vector2 end) : base(start, end)
        {
            _controlPoint1 = controlpoint1;
            _controlPoint2 = controlpoint2;
        }

        public Vector2 ControlPoint1
        {
            get { return _controlPoint1; }
            set
            {
                if (_controlPoint1 == value) return;
                _controlPoint1 = value;
                OnPointChange();
            }
        }

        public Vector2 ControlPoint2
        {
            get { return _controlPoint2; }
            set
            {
                if (_controlPoint2 == value) return;
                _controlPoint2 = value;
                OnPointChange();
            }
        }

        public override int Order => 4;

        protected override int GetRecommendedResolution()
        {
            var length1 = (_controlPoint1 - StartPoint).LengthSquared();
            var length2 = (_controlPoint2 - _controlPoint1).LengthSquared();
            var length3 = (EndPoint - _controlPoint2).LengthSquared();
            // if (length2 - length1 <= float.Epsilon) return 0;
            var avg = (length1 + length2 + length3) / 3;
            return (int)(Math.Sqrt(Math.Max(Math.Max(length1, length2), length3) - avg) / 10d);
        }

        public override Vector2 GetPositionAt(float t)
        {
            t = Normalize(t);
            var i = 1f - t;
            var ii = i * i;
            var tt = t * t;
            return i * ii * StartPoint +
                   3f * ii * t * _controlPoint1 +
                   3f * i * tt * _controlPoint2 +
                   tt * t * EndPoint;
        }

        public override Angle GetTangentAt(float t)
        {
            t = Normalize(t);
            var i = 1f - t;
            var ii = i * i;
            var tt = t * t;
            var result = -ii * StartPoint +
                          (3f * ii - 2f * i) * _controlPoint1 +
                          (-3f * tt + 2f * t) * _controlPoint2 +
                          tt * EndPoint;
            return Angle.FromVector(result);
        }

        public override OrientedPoint GetOrientedPointAt(float t)
        {
            t = Normalize(t);
            var i = 1f - t;
            var ii = i * i;
            var tt = t * t;

            var start = ii * StartPoint;
            var end = tt * EndPoint;

            var pos = i * start +
                   3f * ii * t * _controlPoint1 +
                   3f * i * tt * _controlPoint2 +
                  t * end;
            var angle = -start +
                          (3f * ii - 2f * i) * _controlPoint1 +
                          (-3f * tt + 2f * t) * _controlPoint2 +
                          end;
            return new OrientedPoint(pos, Angle.FromVector(angle));
        }

        public void Split(float t, out CubicBezier first, out CubicBezier second)
        {
            t = Normalize(t);
            var i = 1f - t;
            var ii = i * i;
            var tt = t * t;
            var middle = tt * t * EndPoint + 3 * tt * i * _controlPoint2 + 3 * t * ii * _controlPoint1 + ii * i * StartPoint;
            first = new CubicBezier(StartPoint,
                t * _controlPoint1 + i * StartPoint,
                tt * _controlPoint2 + 2 * t * i * _controlPoint1 + ii * StartPoint,
                middle);
            second = new CubicBezier(middle,
                tt * EndPoint + 2 * t * i * _controlPoint2 + ii * _controlPoint1,
                t * EndPoint + i * _controlPoint2,
                EndPoint);
        }
    }
}