﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    /// <summary>
    /// Describes a curve that goes through three points, laying on the circumference of a circle
    /// </summary>
    public class Arc : CurveBase
    {
        private Angle _startAngle;
        private Angle _arcAngle;
        private Vector2 _middlePoint;


        public Arc(Vector2 start, Vector2 middle, Vector2 end) : base(start, end)
        {
            _middlePoint = middle;
            Calculate(start, middle, end);
        }

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

        public override float Length => Radius * _arcAngle;

        protected override void OnPointChange()
        {
            Calculate(StartPoint, MiddlePoint, EndPoint);
            base.OnPointChange();
        }

        private static float Gradient(Vector2 a, Vector2 b)
        {
            var ax = a.X;
            var ay = a.Y;
            if (ax == b.X) ax += GradientIncrement;
            if (ay == b.Y) ay += GradientIncrement;
            return (b.Y - ay) / (b.X - ax);
        }

        private void Calculate(Vector2 a, Vector2 b, Vector2 c)
        {
            var ma = Gradient(a, b);
            var mb = Gradient(b, c);
            var x0 = (ma * mb * (a.Y - c.Y) + mb * (a.X + b.X) - ma * (b.X + c.X)) / (2 * (mb - ma));
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
                _arcAngle = new Angle(_arcAngle.Radians - MathHelper.TwoPi);
        }
    }
}