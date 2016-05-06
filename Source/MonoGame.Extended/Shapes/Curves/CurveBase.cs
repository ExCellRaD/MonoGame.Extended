using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Particles;

namespace MonoGame.Extended.Shapes.Curves
{
    public abstract class CurveBase : IPath
    {
        private Vector2 _startPoint;
        private Vector2 _endPoint;

        protected CurveBase(Vector2 start, Vector2 end)
        {
            _startPoint = start;
            _endPoint = end;
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

        public abstract float Length { get; }

        /// <summary>
        /// Invokes after a point has changed
        /// </summary>
        public EventHandler OnPointChanged { get; set; }

        protected virtual void OnPointChange()
        {
            OnPointChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract Vector2 GetPositionAt(float t);

        public abstract Angle GetTangentAt(float t);

        public abstract OrientedPoint GetOrientedPointAt(float t);
    }
}