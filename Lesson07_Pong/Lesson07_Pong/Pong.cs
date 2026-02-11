using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07_Pong;

public class Pong : Game
{
    private const int _windowWidth = 750, _windowHeight = 450;
    private const int _PlayAreaEdgeLineWidth = 12;
    private const int _paddleWidth = 8, _paddleHeight = 124;
    private const float _PaddleSpeed = 240, _BallSpeed = 60;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture, _paddleTexture;
    private Ball _ball;
    private Vector2 _paddlePosition, _paddleDirection, _paddleDimensions, _paddlePosition2, _paddleDirection2;
    private float _paddleSpeed;

    // C# properties are the getters and setters
    internal Rectangle PlayAreaBoundingBox
    {
        get
        {
            return new Rectangle(0, _PlayAreaEdgeLineWidth, _windowWidth, _windowHeight - (2 * _PlayAreaEdgeLineWidth));
        }
    }
    public Pong()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _windowWidth;
        _graphics.PreferredBackBufferHeight = _windowHeight;
        _graphics.ApplyChanges();

        _ball = new Ball();
        _ball.Initialize(new Vector2(150, 195), new Vector2(21, 21), new Vector2(-1, -1), _BallSpeed, PlayAreaBoundingBox);

        _paddlePosition = new Vector2(690, 180);
        _paddlePosition2 = new Vector2(54, 180);
        _paddleSpeed = _PaddleSpeed;
        _paddleDimensions = new Vector2(_paddleWidth, _paddleHeight);
        _paddleDirection = Vector2.Zero;
        _paddleDirection2 = Vector2.Zero;


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _backgroundTexture = Content.Load<Texture2D>("Court");
        _paddleTexture = Content.Load<Texture2D>("Paddle");

        _ball.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        _ball.Update(gameTime);

        KeyboardState kbState = Keyboard.GetState();
        if (kbState.IsKeyDown(Keys.Up))
        {
            _paddleDirection = new Vector2(0, -1);
        } else if (kbState.IsKeyDown(Keys.Down))
        {
            _paddleDirection = new Vector2(0, 1);
        } else
        {
            _paddleDirection = Vector2.Zero;
        }

        _paddlePosition += _paddleDirection * _paddleSpeed * dt;

        if(_paddlePosition.Y <= PlayAreaBoundingBox.Top)
        {
            _paddlePosition.Y = PlayAreaBoundingBox.Top;
        } else if ((_paddlePosition.Y + _paddleDimensions.Y) >= PlayAreaBoundingBox.Bottom)
        {
            _paddlePosition.Y = PlayAreaBoundingBox.Bottom - _paddleDimensions.Y;
        }

        if (kbState.IsKeyDown(Keys.W))
        {
            _paddleDirection2 = new Vector2(0, -1);
        } else if (kbState.IsKeyDown(Keys.S))
        {
            _paddleDirection2 = new Vector2(0, 1);
        } else
        {
            _paddleDirection2 = Vector2.Zero;
        }

        _paddlePosition2 += _paddleDirection2 * _paddleSpeed * dt;

        if(_paddlePosition2.Y <= PlayAreaBoundingBox.Top)
        {
            _paddlePosition2.Y = PlayAreaBoundingBox.Top;
        } else if ((_paddlePosition2.Y + _paddleDimensions.Y) >= PlayAreaBoundingBox.Bottom)
        {
            _paddlePosition2.Y = PlayAreaBoundingBox.Bottom - _paddleDimensions.Y;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _windowWidth, _windowHeight), Color.White);

        _ball.Draw(_spriteBatch);

        Rectangle paddleRectangle = new Rectangle((int) _paddlePosition.X, (int) _paddlePosition.Y, (int) _paddleDimensions.X, (int) _paddleDimensions.Y);
        _spriteBatch.Draw(_paddleTexture, paddleRectangle, Color.White);
        Rectangle paddleRectangle2 = new Rectangle((int) _paddlePosition2.X, (int) _paddlePosition2.Y, (int) _paddleDimensions.X, (int) _paddleDimensions.Y);
        _spriteBatch.Draw(_paddleTexture, paddleRectangle2, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
