using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson09Platformer;

public class Platformer : Game
{
    internal const float _Gravity = 100;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private const int _WindowWidth = 550, _WindowHeight = 400;
    private Rectangle _gameBoundingBox = new Rectangle(0, 0, _WindowWidth, _WindowHeight);
    private Player _player;
    private Collider _ground;

    private List<Platform> _platforms;

    public Platformer()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _WindowWidth;
        _graphics.PreferredBackBufferHeight = _WindowHeight;
        _graphics.ApplyChanges();

        _player = new Player(new Vector2(50,50), _gameBoundingBox);
        _player.Initialize();

        _ground = new Collider(new Vector2(0, 300), new Vector2(_WindowWidth, 1), ColliderType.Top);
        _platforms = new List<Platform>();
        _platforms.Add(new Platform(new Vector2(100, 200), new Vector2(70, 10)));
        _platforms.Add(new Platform(new Vector2(150, 250), new Vector2(70, 10)));
        _platforms.Add(new Platform(new Vector2(425, 250), new Vector2(120, 10)));
        _platforms.Add(new Platform(new Vector2(50, 150), new Vector2(30, 10)));
        _platforms.Add(new Platform(new Vector2(150, 100), new Vector2(100, 10)));
        _platforms.Add(new Platform(new Vector2(350, 50), new Vector2(150, 10)));
        _platforms.Add(new Platform(new Vector2(325, 200), new Vector2(50, 10)));

        // base.Initialzize calls LoadContent so make sure you initialize these above first
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _player.LoadContent(Content);
        _ground.LoadContent(GraphicsDevice);
        foreach(Platform p in _platforms)
            p.LoadContent(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState kbState = Keyboard.GetState();

        if(kbState.IsKeyDown(Keys.Left))
            _player.MoveHorizontally(-1);
        else if(kbState.IsKeyDown(Keys.Right))
            _player.MoveHorizontally(1);
        else
            _player.Stop();

        if(kbState.IsKeyDown(Keys.Space))
            _player.Jump();

        _ground.ProcessCollision(_player, gameTime);
        foreach(Platform p in _platforms)
            p.ProcessCollisions(_player, gameTime);

        _player.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _player.Draw(_spriteBatch);
        _ground.Draw(_spriteBatch);
        foreach(Platform p in _platforms)
            p.Draw(_spriteBatch);
            
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
