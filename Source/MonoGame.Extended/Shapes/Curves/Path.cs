using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    public interface IPath
    {
        Vector2 EndPoint { get; set; }
        Vector2 StartPoint { get; set; }
        float Length { get; }
        Vector2 GetPositionAt(float t);
        Angle GetTangentAt(float t);
        OrientedPoint GetOrientedPointAt(float t);
    }

    public class Path : IPath
    {
        public Path()
        {
            _segments = new List<CurveBase>();
        }

        private readonly List<CurveBase> _segments;
        public int SegmentCount { get; private set; }
        public float Length { get; private set; }
        public Vector2 EndPoint

        {
            get { return _segments[SegmentCount - 1].EndPoint; }
            set { _segments[SegmentCount - 1].EndPoint = value; }
        }
        public Vector2 StartPoint

        {
            get { return _segments[0].EndPoint; }
            set { _segments[0].EndPoint = value; }
        }

        private void AddSeg(CurveBase segment)
        {
            _segments.Add(segment);
            SegmentCount++;
            Length += segment.Length;
        }

        public LineSegment AddLine(Vector2 end)
        {
            var segment = new LineSegment(EndPoint, end);
            AddSeg(segment);
            return segment;
        }

        public Arc AddArc(Vector2 middle, Vector2 end)
        {
            var segment = new Arc(EndPoint, middle, end);
            AddSeg(segment);
            return segment;
        }

        public Bezier AddBezier(Vector2 end, params Vector2[] controlPoints)
        {
            var segment = Bezier.Create(EndPoint, end, controlPoints);
            AddSeg(segment);
            return segment;
        }

        private CurveBase GetSegmentAt(ref float t)
        {
            t *= Length;
            for (int i = 0; i < SegmentCount; i++)
            {
                var seg = _segments[i];
                var length = seg.Length;
                if (t > length)
                {
                    t -= length;
                    continue;
                }
                t /= length;
                return seg;
            }
            return _segments[SegmentCount - 1];
        }

        public Vector2 GetPositionAt(float t)
        {
            return GetSegmentAt(ref t).GetPositionAt(t);
        }

        public Angle GetTangentAt(float t)
        {
            return GetSegmentAt(ref t).GetTangentAt(t);
        }

        public OrientedPoint GetOrientedPointAt(float t)
        {
            return GetSegmentAt(ref t).GetOrientedPointAt(t);
        }


    }
}