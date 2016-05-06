using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    public class LineSegment : CurveBase
    {
        public LineSegment(Vector2 start, Vector2 end) : base(start, end) { }
        private Angle _tangent;
        private float _length;
        protected override void OnPointChange()
        {
            var total = EndPoint - StartPoint;
            _tangent = Angle.FromVector(total);
            _length = total.Length();
            base.OnPointChange();
        }

        public override float Length => _length;

        public override Vector2 GetPositionAt(float t) => Vector2.Lerp(StartPoint, EndPoint, t);

        public override Angle GetTangentAt(float t) => _tangent;

        public override OrientedPoint GetOrientedPointAt(float t)
        {
            return new OrientedPoint(Vector2.Lerp(StartPoint, EndPoint, t), _tangent);
        }
    }
}