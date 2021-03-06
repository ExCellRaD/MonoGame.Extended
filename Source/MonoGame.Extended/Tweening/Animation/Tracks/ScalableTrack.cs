using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Tweening.Animation.Tracks
{
    public class ScalableTrack<TTransformable> : Vector2Track<TTransformable>
        where TTransformable : class, IScalable
    {
        protected override void SetValue(Vector2 value) => 
            Transformable.Scale = value;
        protected override Vector2 GetValue() =>
            Transformable.Scale;
    }
}