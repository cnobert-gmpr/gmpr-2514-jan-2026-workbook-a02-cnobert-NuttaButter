using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08_MosquitoAttack;

public class MosquitoAttackGame : Game
{
    private const int _windowWidth = 550, _windowHeight = 400, _NumMosquitoes = 10;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _background;
    private SpriteFont _font;
    private string _message = "";
    private KeyboardState _kbPreviousState, _kbCurrentState;

    // possible states are: playing = 0, paused = 1, over = 2
    private enum GameState {Playing, Paused, Over}
    private GameState _gameState;
    private Cannon _cannon;
    private Mosquito[] _mosquitoes;

    private Rectangle BoundingBox
    {
        get { return new Rectangle(0, 0, _windowWidth, _windowHeight); }
    }
    public MosquitoAttackGame()
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

        _cannon = new Cannon();
        _cannon.Initialize(new Vector2(50, 325), 70);

        _mosquitoes = new Mosquito[_NumMosquitoes];

        for(int c =0 ; c < _NumMosquitoes; c++)
        {
            _mosquitoes[c] = new Mosquito();
        }
        Random random = new Random();
        foreach(Mosquito mosquito in _mosquitoes)
        {
            int direction = random.Next(1, 3) == 2? -1: 1;
            int xPosition = random.Next(1, _windowWidth - 50);
            int yPosition = random.Next(1, 151);
            int speed = random.Next(150, 251);
            mosquito.Initialize(new Vector2(xPosition, yPosition), speed, new Vector2(direction, 0), BoundingBox);
        }

        _gameState = GameState.Playing;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _background = Content.Load<Texture2D>("Background");
        _font = Content.Load<SpriteFont>("SystemArialFont");
        _cannon.LoadContent(Content);
        foreach(Mosquito mosquito in _mosquitoes)
        {
            mosquito.LoadContent(Content);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        _kbCurrentState = Keyboard.GetState();
        switch (_gameState)
        {
            case GameState.Playing:
                #region keyboard input
                if (_kbCurrentState.IsKeyDown(Keys.A))
                    _cannon.Direction = new Vector2(-1, 0);
                else if (_kbCurrentState.IsKeyDown(Keys.D))
                    _cannon.Direction = new Vector2(1, 0);
                else
                    _cannon.Direction = Vector2.Zero;

                if(Pressed(Keys.P)){
                    _gameState = GameState.Paused;
                    _message = "Game Paused, press P to start playing again.";
                }
                #endregion

                _cannon.Update(gameTime);
                foreach(Mosquito mosquito in _mosquitoes)
                {
                    mosquito.Update(gameTime);
                }

                break;
            case GameState.Paused:
                if(Pressed(Keys.P)){
                    _gameState = GameState.Playing;
                    _message = "";
                }
                break;
            case GameState.Over:
                break;
        }
        _kbPreviousState = _kbCurrentState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        switch (_gameState)
        {
            case GameState.Playing:
                _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
                _cannon.Draw(_spriteBatch);
                foreach(Mosquito mosquito in _mosquitoes)
                {
                    mosquito.Draw(_spriteBatch);
                }
                break;
            case GameState.Paused:
                _spriteBatch.Draw(_background, Vector2.Zero, Color.Silver);
                _cannon.Draw(_spriteBatch);
                foreach(Mosquito mosquito in _mosquitoes)
                {
                    mosquito.Draw(_spriteBatch);
                }
                _spriteBatch.DrawString(_font, _message, new Vector2(80, 160), Color.OrangeRed);
                break;
            case GameState.Over:
                break;
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private bool Pressed(Keys key)
    {
        return _kbCurrentState.IsKeyDown(key) && _kbPreviousState.IsKeyUp(key);
    }
}
