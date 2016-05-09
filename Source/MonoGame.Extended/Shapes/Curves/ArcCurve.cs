using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    /// <summary>
    /// Describes a curve that goes through three points, laying on the circumference of a circle
    /// </summary>
    public class ArcCurve : CurveBase
    {
        private Angle _startAngle;
        private Angle _arcAngle;
        private Vector2 _middlePoint;


        public ArcCurve(Vector2 start, Vector2 middle, Vector2 end) : base(start, end)
        {
            _middlePoint = middle;
            Calculate(start, middle, end);
        }

        private ArcCurve(Vector2 start, Vector2 end) : base(start, end) { }

        public float Radius { get; private set; }
        public Vector2 Center { get; private set; }
        public static float GradientIncrement { get; set; } = 0.00001f;

        public Vector2 MiddlePoint
        {
            get { return _middlePoint; }
            set
            {
                if (_middlePoint == value) return;
                _middlePoint = value;
                OnPointChange();
            }
        }

        public override Vector2 GetPositionAt(float t)
        {
            var r = Center + new Angle(_startAngle + t * _arcAngle).ToVector(Radius);
            return r;
        }

        public override Angle GetTangentAt(float t)
        {

            var angle = _startAngle + (_arcAngle * t);
            return new Angle(angle + MathHelper.PiOver2);
        }

        public override OrientedPoint GetOrientedPointAt(float t)
        {
            var angle = new Angle(_startAngle + _arcAngle * t);
            return new OrientedPoint(
               Center + angle.ToVector(Radius),
                new Angle(angle + Math.Sign(_arcAngle) * MathHelper.PiOver2));
        }

        public override float Length => Math.Abs(Radius * _arcAngle);

        protected override void OnPointChange()
        {
            Calculate(StartPoint, MiddlePoint, EndPoint);
            base.OnPointChange();
        }

        private static float Gradient(Vector2 a, Vector2 b)
        {
            var ax = a.X;
            var ay = a.Y;
            if (Math.Abs(ax - b.X) < float.Epsilon) ax += GradientIncrement;
            if (Math.Abs(ay - b.Y) < float.Epsilon) ay += GradientIncrement;
            return (b.Y - ay) / (b.X - ax);
        }

        private void Calculate(Vector2 a, Vector2 b, Vector2 c)
        {
            var ma = Gradient(a, b);
            var mb = Gradient(b, c);
            if (Math.Abs(ma - mb) < float.Epsilon) mb += GradientIncrement;
            var x0 = (ma * mb * (a.Y - c.Y) + mb * (a.X + b.X) - ma * (b.X + c.X)) / (2 * (mb - ma));
            if (float.IsNaN(x0)) x0 = 0f;
            var y0 = (-2 * x0 + a.X + b.X) / (2 * ma) + (a.Y + b.Y) * 0.5f;
            Center = new Vector2(x0, y0);

            var start = a - Center;
            Radius = start.Length();
            _startAngle = Angle.FromVector(start);
            var a1 = Angle.FromVector(b - Center) - _startAngle;
            a1.WrapPositive();
            var a2 = Angle.FromVector(c - Center) - _startAngle;
            a2.WrapPositive();
            _arcAngle = a2;
            _startAngle.Wrap();

            if (a1 > a2)
                _arcAngle.Radians -= MathHelper.TwoPi;
        }

        public void Split(float t, out ArcCurve first, out ArcCurve second)
        {
            var middle = GetPositionAt(t);
            first = FromCenter(Center, StartPoint, middle, _arcAngle > 0);
            second = FromCenter(Center, middle, EndPoint, _arcAngle > 0);
        }
        //TODO check
        public static ArcCurve FromCenter(Vector2 center, Vector2 start, Vector2 end, bool positive)
        {
            var s = start - center;
            var sa = Angle.FromVector(s);
            var ea = Angle.FromVector(end - center) - sa;
            ea.WrapPositive();
            if (!positive)
            {
                ea.Radians -= MathHelper.TwoPi;
            }
            var m = new Angle(sa.Radians + 0.5f*ea.Radians);

            var length = center.Length();
            var arc = new ArcCurve(start, end)
            {
                Center = center,
                Radius = length,
                _startAngle = sa,
                _arcAngle = ea,
                _middlePoint = center + m.ToVector(length)
            };
            arc._startAngle.WrapPositive();
            return arc;
        }
    }
}