using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    public interface IPath
    {
        /// <summary>
        /// The startpoint or first position of this path.
        /// </summary>
        Vector2 StartPoint { get; set; }
        /// <summary>
        /// The endpoint or last position of this path.
        /// </summary>
        Vector2 EndPoint { get; set; }
        /// <summary>
        /// Returns the calculated curvelength.
        /// </summary>
        float Length { get; }
        /// <summary>
        /// Returns the point calculated by the curve at t.
        /// </summary>
        /// <param name="t">a value between 0 and 1</param>
        Vector2 GetPositionAt(float t);
        /// <summary>
        /// Returns the angle calculated by the curve tangent calculations at t.
        /// </summary>
        /// <param name="t">a value between 0 and 1</param>
        Angle GetTangentAt(float t);
        /// <summary>
        /// Returns the position and angle calculated by the curve calculations at t.
        /// </summary>
        /// <param name="t">a value between 0 and 1</param>
        OrientedPoint GetOrientedPointAt(float t);
    }
}