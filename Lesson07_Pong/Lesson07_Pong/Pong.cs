using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07_Pong;

public class Pong : Game
{
    private const int _windowWidth = 750, _windowHeight = 450, _BallWidthAndHeight = 20;
    private const int _PlayAreaEdgeLineWidth = 12;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture, _ballTexture;
    private Vector2 _ballPosition, _ballDirection;
    private float _ballSpeed;

    // C# properties are the getters and setters
    internal Rectangle PlayAreaBoundingBox
    {
        get
        {
            return new Rectangle(0, 0, _windowWidth, _windowHeight);
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

        _ballPosition = new Vector2(150, 195);
        _ballSpeed = 60;
        _ballDirection.X = -1;
        _ballDirection.Y = -1;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _backgroundTexture = Content.Load<Texture2D>("Court");
        _ballTexture = Content.Load<Texture2D>("Ball");
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
        _ballPosition += _ballDirection * _ballSpeed * dt;

        // bounce the ball off left and right sides
        if(_ballPosition.X <= PlayAreaBoundingBox.Left || _ballPosition.X >= PlayAreaBoundingBox.Right)
        {
            _ballDirection.X *= -1;
        }
        if(_ballPosition.Y <= PlayAreaBoundingBox.Top || _ballPosition.Y >= PlayAreaBoundingBox.Bottom)
        {
            _ballDirection.Y *= -1;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _windowWidth, _windowHeight), Color.White);

        Rectangle ballRectangle = new Rectangle((int) _ballPosition.X, (int) _ballPosition.Y, _BallWidthAndHeight, _BallWidthAndHeight); 
        _spriteBatch.Draw(_ballTexture, ballRectangle, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
