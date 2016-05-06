using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes.Curves;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.ViewportAdapters;

namespace Demo.Curves
{
    public class Game1 : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private Camera2D _camera;
        private CurveBase _curve;
        private Texture2D _logoTexture;
        private int _count = 50;

        public Game1()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);
            _camera = new Camera2D(viewportAdapter);

            _logoTexture = Content.Load<Texture2D>("logo-square-128");

            //_curve = new CubicBezier(new Vector2(20, 20), new Vector2(), new Vector2(800, 0), new Vector2(780, 460));
            _curve = new LineSegment(new Vector2(20, 20),  new Vector2(780, 460));
            // _curve = new Arc(new Vector2(20, 20), new Vector2(), new Vector2(780, 460));
        }


        protected override void Update(GameTime gameTime)
        {

            ((LineSegment)_curve).EndPoint = Mouse.GetState().Position.ToVector2();
            _count = (int)(_curve.Length / 15);

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());

            for (var i = 0; i < _count; i++)
            {
                var t = i / (_count - 1f);
                var point = _curve.GetOrientedPointAt(t);
                _spriteBatch.Draw(_logoTexture, point.Position, rotation: -point.Angle, scale: new Vector2(0.1f, 0.03f), origin: new Vector2(0, 64));
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}