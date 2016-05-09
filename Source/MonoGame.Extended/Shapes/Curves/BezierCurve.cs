using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    //handles the normalizing for each type of bezier
    public abstract class BezierCurve : CurveBase
    {
        protected BezierCurve(Vector2 start, Vector2 end) : base(start, end)
        {
            _lengths = null;
        }

        private float[] _lengths;

        public abstract int Order { get; }
        public int Resolution { get; private set; }

        public override float Length
        {
            get
            {
                if (_lengths == null)
                {
                    NormalizeLength();
                }
                return _lengths[Resolution - 1];
            }
        }

        private void NormalizeLength()
        {
            _normalizing = true;
            Resolution = Math.Max(5, GetRecommendedResolution());
            float resf = Resolution - 1;
            var arr = new float[Resolution];
            var previous = StartPoint;
            var total = 0f;
            for (int i = 1; i < Resolution; i++)
            {
                var t = i / resf;
                var current = GetPositionAt(t);
                var lenth = (previous - current).Length();
                total += lenth;
                arr[i] = total;
                previous = current;
            }
            _lengths = arr;
            _normalizing = false;
        }

        private bool _normalizing;

        protected float Normalize(float t)
        {
            if (_lengths == null)
            {
                if (_normalizing) return t;
                NormalizeLength();
            }

            var arr = _lengths;
            var count = arr.Length;

            var start = Math.Min(count - 1, (int)(count * t));

            t *= _lengths[count - 1];

            if (_lengths[start] > t)
            {
                for (int i = start - 1; i >= 0; i--)
                {
                    var prev = _lengths[i];
                    if (prev > t) continue;
                    var next = _lengths[i + 1];
                    return (i + (t - prev) / (next - prev)) / (count - 1); //todo check
                }
                return 0;
            }

            for (int i = start + 1; i < count; i++)
            {
                var next = _lengths[i];
                if (next < t) continue;
                var prev = _lengths[--i];
                return (i + (t - prev) / (next - prev)) / (count - 1);
            }
            return 1;
        }

        protected override void OnPointChange()
        {
            _lengths = null;
            base.OnPointChange();
        }

        protected abstract int GetRecommendedResolution();

        public static BezierCurve Create(Vector2 start, Vector2 end, Vector2[] controlPoints)
        {
            BezierCurve result;
            switch (controlPoints.Length)
            {
                case 1:
                    result = new QuadraticBezierCurve(start, controlPoints[0], end);
                    break;
                case 2:
                    result = new CubicBezierCurve(start, controlPoints[0], controlPoints[1], end);
                    break;
                default:
                    result = new NBezierCurve(start, end, controlPoints);
                    break;
            }
            return result;
        }
    }
}