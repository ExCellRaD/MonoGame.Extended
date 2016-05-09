using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
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
        private CubicBezierCurve _curve;
        private CubicBezierCurve _curve1;
        private CubicBezierCurve _curve2;
        private Texture2D _logoTexture;

        private float _split = 0.5f;

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

            _curve = new CubicBezierCurve(new Vector2(20, 20), new Vector2(), new Vector2(800, 0), new Vector2(780, 460));
            //_curve = new LineSegment(new Vector2(20, 20), new Vector2(780, 460));
            // _curve = new Arc(new Vector2(20, 20), new Vector2(), new Vector2(780, 460));
            _prev = Mouse.GetState();
        }

        private bool _first = true;
        private MouseState _prev;

        protected override void Update(GameTime gameTime)
        {
            var state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed && _prev.LeftButton == ButtonState.Released)
            {
                _first = !_first;
            }
            _prev = state;

            if (_first)
                _curve.ControlPoint1 = state.Position.ToVector2();
            else
                _curve.ControlPoint2 = state.Position.ToVector2();

            _curve.Split(_split, out _curve1, out _curve2);
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();
            if (keyboardState.IsKeyDown(Keys.Up))
                _split *= 1.01f;
            else if (keyboardState.IsKeyDown(Keys.Down))
                _split *= 0.99f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());


            _spriteBatch.DrawLine(_curve.StartPoint, _curve.ControlPoint1, Color.Green);
            _spriteBatch.DrawLine(_curve.EndPoint, _curve.ControlPoint2, Color.Green);

            _spriteBatch.DrawCurve(_curve, Color.Red, 6, (int)(_curve.Length / 5));

            _spriteBatch.DrawLine(_curve1.StartPoint, _curve1.ControlPoint1, Color.Blue);
            _spriteBatch.DrawLine(_curve1.EndPoint, _curve1.ControlPoint2, Color.Blue);
            _spriteBatch.DrawLine(_curve2.StartPoint, _curve2.ControlPoint1, Color.Purple);
            _spriteBatch.DrawLine(_curve2.EndPoint, _curve2.ControlPoint2, Color.Purple);

            _spriteBatch.DrawCurve(_curve1, Color.White, 1, (int)(_curve1.Length / 5));
            _spriteBatch.DrawCurve(_curve2, Color.Yellow, 1, (int)(_curve2.Length / 5));

            _spriteBatch.Draw(_logoTexture, _curve.ControlPoint1, origin: new Vector2(64f), scale: new Vector2(0.2f));
            _spriteBatch.Draw(_logoTexture, _curve.ControlPoint2, origin: new Vector2(64f), scale: new Vector2(0.2f));

            _spriteBatch.Draw(_logoTexture, _curve1.ControlPoint1, origin: new Vector2(64f), scale: new Vector2(0.1f));
            _spriteBatch.Draw(_logoTexture, _curve1.ControlPoint2, origin: new Vector2(64f), scale: new Vector2(0.1f));
            _spriteBatch.Draw(_logoTexture, _curve2.ControlPoint1, origin: new Vector2(64f), scale: new Vector2(0.1f));
            _spriteBatch.Draw(_logoTexture, _curve2.ControlPoint2, origin: new Vector2(64f), scale: new Vector2(0.1f));

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}