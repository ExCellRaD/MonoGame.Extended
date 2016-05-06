using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    public class QuadraticBezier : Bezier
    {
        private Vector2 _controlPoint;

        public QuadraticBezier(Vector2 start, Vector2 control, Vector2 end) : base(start, end)
        {
            ControlPoint = control;
        }

        public Vector2 ControlPoint
        {
            get { return _controlPoint; }
            set
            {
                if (_controlPoint == value) return;
                _controlPoint = value;
                OnPointChange();
            }
        }

        private Vector2 _startControl;
        private Vector2 _controlEnd;

        protected override void OnPointChange()
        {
            base.OnPointChange();
            _startControl = _controlPoint - StartPoint;
            _controlEnd = EndPoint - _controlPoint;
        }

        public override int Order => 3;

        protected override int GetRecommendedResolution()
        {
            var length1 = _startControl.LengthSquared();
            var length2 = _controlEnd.LengthSquared();
            // if (length2 - length1 <= float.Epsilon) return 0;
            var anglediff = Angle.FromVector(_startControl) - Angle.FromVector(_controlEnd);
            anglediff.Wrap();
            anglediff.Radians = Math.Abs(anglediff.Radians) * 0.5f + 0.5f;
            var lengthdiff = Math.Sqrt(Math.Abs(length1 - length2));
            return (int)(anglediff * lengthdiff / 20d);
        }

        public override Vector2 GetPositionAt(float t)
        {
            t = Normalize(t);
            var i = 1 - t;
            return i * i * StartPoint
                   + 2 * i * t * ControlPoint
                   + t * t * EndPoint;
        }

        public override Angle GetTangentAt(float t)
        {
            t = Normalize(t);
            var result = 2 * ((1 - t) * _startControl + t * _controlEnd);
            return Angle.FromVector(result);
        }

        public override OrientedPoint GetOrientedPointAt(float t)
        {
            t = Normalize(t);
            var i = 1 - t;
            var pos = i * i * StartPoint
                   + 2 * i * t * ControlPoint
                   + t * t * EndPoint;
            var angle = 2 * (i * _startControl + t * _controlEnd);
            return new OrientedPoint(pos, Angle.FromVector(angle));
        }
    }
}