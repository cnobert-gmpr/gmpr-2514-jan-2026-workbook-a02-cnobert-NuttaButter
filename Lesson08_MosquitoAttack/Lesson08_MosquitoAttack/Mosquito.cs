using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08_MosquitoAttack;

public class Mosquito
{
    private SimpleAnimation _animation, _animationPoofing;

    private Vector2 _position;
    private Vector2 _direction;
    private float _speed;

    private enum State{ Alive, Poofing, Dead };
    private State _state;
    internal bool Alive { get => _state == State.Alive; }
    private Rectangle _gameBoundingBox;

    internal Rectangle BoundingBox
    {
        get
        {
            return new Rectangle((int)_position.X,(int)_position.Y,(int)_animation.FrameDimensions.X,(int)_animation.FrameDimensions.Y);
        }
    }

    internal void Initialize(Vector2 position, float speed, Vector2 direction, Rectangle gameBoundingBox)
    {
        _position = position;
        _speed = speed;
        _direction = direction;
        _gameBoundingBox = gameBoundingBox;
        _state = State.Alive;
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D texture = content.Load<Texture2D>("Mosquito");

        _animation = new SimpleAnimation(texture, texture.Width / 11, texture.Height, 11, 8f);
        _animation.Paused = false;

        texture = content.Load<Texture2D>("Poof");
        _animationPoofing = new SimpleAnimation(texture, texture.Width/8, texture.Height, 8, 4);
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        switch (_state)
        {
            case State.Alive:
                _position += _direction * _speed * dt;
                if(BoundingBox.Left < _gameBoundingBox.Left || BoundingBox.Right > _gameBoundingBox.Right)
                {
                    _direction.X *= -1;
                }
                _animation.Update(gameTime);
                break;
            case State.Poofing:
            _animationPoofing.Update(gameTime);
                if (_animationPoofing.DonePlayingOnce)
                {
                    _state = State.Dead;
                }
                break;
            case State.Dead:
                break;
        }
        
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        switch(_state)
        {
            case State.Alive:
                _animation.Draw(spriteBatch, _position, SpriteEffects.None);
                break;
            case State.Poofing:
                _animationPoofing.Draw(spriteBatch, _position, SpriteEffects.None);
                break;
            case State.Dead:
                break;
        }
        
    }

    internal void Die()
    {
        if(Alive){
            _state = State.Poofing;
            _animationPoofing.Looping = false;
        }
    }
}