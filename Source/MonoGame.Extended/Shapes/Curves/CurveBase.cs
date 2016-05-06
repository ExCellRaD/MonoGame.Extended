using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Particles;

namespace MonoGame.Extended.Shapes.Curves
{
    public abstract class CurveBase
    {
        private Vector2 _startPoint;
        private Vector2 _endPoint;

        protected CurveBase(Vector2 start, Vector2 end)
        {
            _startPoint = start;
            _endPoint = end;
        }

        /// <summary>
        /// The startpoint or first position of this <see cref="CurveBase"/>
        /// </summary>
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

        /// <summary>
        /// The endpoint or last position of this <see cref="CurveBase"/>
        /// </summary>
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


        /// <summary>
        /// Returns an approximation of the length by getting some points and checking the distance between.
        /// </summary>
        /// <param name="resolution">The amount of points calculated.</param>
        public virtual float Length(int resolution)
        {
            var previous = GetPositionAt(0);
            float result = 0;
            for (var i = 1f; i < resolution; i++)
            {
                var current = GetPositionAt(i / resolution);
                result += Vector2.Distance(previous, current);
                previous = current;
            }
            return result;
        }

        /// <summary>
        /// Invokes after a point has changed
        /// </summary>
        public EventHandler OnPointChanged { get; set; }

        protected virtual void OnPointChange()
        {
            OnPointChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Returns the point calculated by the curve at t.
        /// </summary>
        /// <param name="t">a value between 0 and 1</param>
        public abstract Vector2 GetPositionAt(float t);

        /// <summary>
        /// Returns the angle calculated by the curve tangent calculations at t.
        /// </summary>
        /// <param name="t">a value between 0 and 1</param>
        public abstract Angle GetTangentAt(float t);

        /// <summary>
        /// Returns the position and angle calculated by the curve calculations at t.
        /// </summary>
        /// <param name="t">a value between 0 and 1</param>
        public abstract OrientedPoint GetOrientedPointAt(float t);
    }
}