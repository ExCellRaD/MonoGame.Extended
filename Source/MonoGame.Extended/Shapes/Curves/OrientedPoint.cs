using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Shapes.Curves
{
    public struct OrientedPoint
    {
        public Vector2 Position { get; set; }
        public Angle Angle { get; set; }

        public OrientedPoint(Vector2 position, Angle angle)
        {
            Position = position;
            Angle = angle;
        }

        public void GetOffsettedPoints(float distance, out Vector2 right, out Vector2 left)
        {
            var angle = Angle + new Angle(MathHelper.PiOver2);
            var vector = angle.ToVector(distance);
            right = Position + vector;
            left = Position - vector;
        }
    }
}