using Microsoft.Xna.Framework;
using MonoGame.Extended.Particles;

namespace MonoGame.Extended.Shapes.Curves
{
    public abstract class CurveF : IPathF
    {
        private Vector2 _startPoint;
        private Vector2 _endPoint;

        protected CurveF(Vector2 start, Vector2 end) {
            _startPoint = start;
            _endPoint = end;
        }

        /// <summary>
        /// For creating a polygon edge, endpoint is the startpoint of next edge
        /// Don't calculate anything here
        /// </summary>
        protected CurveF(Vector2 start) {
            _startPoint = start;
        }

        public Vector2 StartPoint
        {
            get { return _startPoint; }
            set
            {
                if (_startPoint == value) return;
                _startPoint = value;
                OnPointChange();
            }
        }

        public Vector2 EndPoint
        {
            get { return _endPoint; }
            set
            {
                if (_endPoint == value) return;
                _endPoint = value;
                OnPointChange();
            }
        }

        public abstract float Left { get; protected set; }
        public abstract float Top { get; protected set; }
        public abstract float Right { get; protected set; }
        public abstract float Bottom { get; protected set; }
        public abstract float Length { get; protected set; }

        public RectangleF GetBoundingRectangle() {
            var left = Left;
            var top = Top;
            return new RectangleF(left, top, Right - left, Bottom - top);
        }

        public Vector2 PointOnOutline(float t) {
            if (t < float.Epsilon) return StartPoint;
            if (1-t < float.Epsilon) return EndPoint;
            return GetPointOnCurve(t);
        }

        /// <summary>
        /// Returns the point at t on the curve, 1 being startpoint and 0 being endpoint
        /// </summary>
        /// <param name="t">a value between 0 and 1</param>
        /// <returns></returns>
        protected abstract Vector2 GetPointOnCurve(float t);

        protected abstract void OnPointChange();
    }
}