using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson02;

public class IntroGame : Game
{
    // this object represents the screen
    private GraphicsDeviceManager _graphics;
    // an object that batches up draw commands
    // so they can be sent all at once
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private float _speed = 150;
    private int _width = 80, _height = 50, _xPosition = 100, _yPosition = 150;
    
    public IntroGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // new texture that is 1x1 pixel
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new [] {Color.White});
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if(_xPosition > 700 || _xPosition < 0){
            _speed = _speed * -1;
        } else {
            _xPosition += (int)(_speed * gameTime.ElapsedGameTime.TotalSeconds);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Wheat);
        // all draw commands should always begin with the spritebatch begin and end
        _spriteBatch.Begin();
        Rectangle rect = new Rectangle(_xPosition, _yPosition, _width, _height);
        _spriteBatch.Draw(_pixel, rect, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
