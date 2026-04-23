using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Lesson08_MosquitoAttack;

public class Mosquito : Actor
{
    private const int NumFireBalls = 10, UpperRandomFiringRange = 160;
    private Random _rng;

    internal void Initialize(Vector2 position, float speed, Vector2 direction, Rectangle gameBoundingBox)
    {
        _position = position;
        _speed = speed;
        _direction = direction;
        _gameBoundingBox = gameBoundingBox;
        _state = State.Alive;

        _projectiles = new FireBall[NumFireBalls];
        for(int c = 0; c < NumFireBalls; c++)
        {
            _projectiles[c] = new FireBall();
            _projectiles[c].Initialize(50, _gameBoundingBox);
        }
        _rng = new Random();
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D texture = content.Load<Texture2D>("Mosquito");

        _animationAlive = 
            new SimpleAnimation(texture, texture.Width / 11, texture.Height, 11, 8f);
        _animationAlive.Paused = false;

        texture = content.Load<Texture2D>("Poof");
        _animationPoofing = 
            new SimpleAnimation(texture, texture.Width / 8, texture.Height, 8, 4);

        foreach(FireBall fb in _projectiles)
            fb.LoadContent(content);
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        switch(_state)
        {
            case State.Alive:
                _position += _direction * _speed * dt;
                if(BoundingBox.Left < _gameBoundingBox.Left || BoundingBox.Right > _gameBoundingBox.Right)
                {
                    _direction.X *= -1;
                }
                _animationAlive.Update(gameTime);
                if(_rng.Next(1, UpperRandomFiringRange) == 1)
                {
                    Shoot();
                }
                break;
            case State.Poofing:
                _animationPoofing.Update(gameTime);
                if(_animationPoofing.DonePlayingOnce)
                {
                    _state = State.Dead;
                }
                break;
            case State.Dead:
                break;
        }
        foreach(FireBall fb in _projectiles)
            fb.Update(gameTime);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        switch(_state)
        {
            case State.Alive:
                _animationAlive.Draw(spriteBatch, _position, SpriteEffects.None);
                break;
            case State.Poofing:
                _animationPoofing.Draw(spriteBatch, _position, SpriteEffects.None);
                break;
            case State.Dead:
                break;
        }
        foreach(FireBall fb in _projectiles)
            fb.Draw(spriteBatch);
    }

    internal void Die()
    {
        if(Alive)
        {
            _state = State.Poofing;
            _animationPoofing.Looping = false;
        }
    }

    internal void Shoot()
    {
        foreach(FireBall fb in _projectiles)
        {
            if(fb.Launchable)
            {
                float fireBallPositionY = BoundingBox.Bottom;
                float fireBallPositionX = BoundingBox.Center.X - fb.BoundingBox.Width / 2;
                Vector2 fireBallPosition = new Vector2(fireBallPositionX, fireBallPositionY);
                fb.Shoot(fireBallPosition, new Vector2(0, 1));
                return; //break;
            }
        }
    }
}