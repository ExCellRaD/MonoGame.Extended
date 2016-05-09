using System;
using System.Collections.Generic;
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

        public static void CreateMesh(CurveBase curve, int resolution, float width, out Vector2[] positions, out Vector2[] uvs, out short[] indices)
        {
            var resf = resolution - 1f;
            indices = new short[resolution * 6];
            positions = new Vector2[resolution * 2];
            uvs = new Vector2[resolution * 2];
            var j = 0;
            var k = 0;
            for (var i = 0; i < resolution; i++)
            {
                var t = i / resf;
                var point = curve.GetOrientedPointAt(t);
                point.GetOffsettedPoints(width, out positions[k], out positions[k+1]);
                uvs[k++] = new Vector2(0, t);
                uvs[k++] = new Vector2(1, t);

                if (i == 0) continue;

                var start = i * 6;
                indices[j++] = (short)start;
                indices[j++] = (short)(start + 1);
                indices[j++] = (short)(start - 1);
                indices[j++] = (short)start;
                indices[j++] = (short)(start - 1);
                indices[j++] = (short)(start - 2);
            }
        }
    }
}