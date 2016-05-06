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
    }
}