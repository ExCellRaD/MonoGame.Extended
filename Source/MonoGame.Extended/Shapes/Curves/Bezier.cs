using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    public abstract class Bezier : CurveBase
    {
        protected Bezier(Vector2 start, Vector2 end) : base(start, end) { }

        private float[] _lengths;

        public abstract int Order { get; }
        public int RecommendedResolution { get; private set; }

        public override float Length(int resolution)
        {
            if (_lengths != null && resolution == _lengths.Length) return _lengths[resolution - 1];
            return base.Length(resolution);
        }

        public void NormalizeLength(int resolution)
        {
            _lengths = null;
            if (resolution < 2)
            {
                return;
            }
            float resf = resolution - 1;
            var arr = new float[resolution];
            var previous = StartPoint;
            var total = 0f;
            for (int i = 1; i < resolution; i++)
            {
                var t = i / resf;
                var current = GetPositionAt(t);
                var lenth = (previous - current).Length();
                total += lenth;
                arr[i] = total;
                previous = current;
            }
            _lengths = arr;
        }

        protected float Normalize(float t)
        {
            if (_lengths == null) return t;
            var arr = _lengths;
            var count = arr.Length;
            t *= _lengths[count - 1];
            for (int i = 1; i < count; i++)
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
            var recommendedResolution = GetRecommendedResolution();
            if (recommendedResolution > 0 && recommendedResolution < 2) recommendedResolution = 2;
            RecommendedResolution = recommendedResolution;
            base.OnPointChange();
        }

        protected abstract int GetRecommendedResolution();

        ///// <summary>
        ///// Returns a point calculated by the bezier calculation for a given t.
        ///// The result is never normalized.
        ///// </summary>
        //public abstract Vector2 GetBezierPoint(float t);


        ///// <summary>
        ///// Returns the angle calculated by the bezier tangent calculation for a given t.
        ///// The result is never normalized.
        ///// </summary>
        //public abstract Angle GetBezierTangent(float t);

        //protected abstract OrientedPoint GetBezierOrientedPoint(float t);
    }
}