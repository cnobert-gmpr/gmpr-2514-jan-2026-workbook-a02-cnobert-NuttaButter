using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson07_Pong;

public class Ball
{
    private const float _CollisionTimerInterval = 0.4f;
    private Vector2 _position, _direction, _dimensions;
    private float _speed, _collisionTimer;
    private Texture2D _texture;
    private Rectangle _playAreaBoundingBox;

    internal Rectangle BoundingBox
    {
        get
        {
            return new Rectangle(_position.ToPoint(), _dimensions.ToPoint());
        }
    }

    internal void Initialize(Vector2 position, Vector2 dimensions, Vector2 direction, float speed, Rectangle playAreaBoundingBox)
    {
        _position = position;
        _speed = speed;
        _dimensions = dimensions;
        _direction = direction;
        _playAreaBoundingBox = playAreaBoundingBox;

    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("Ball");
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        _collisionTimer += dt;

        _position += _direction * _speed * dt;

        // bounce the ball off left and right sides
        if(_position.X <= _playAreaBoundingBox.Left || _position.X >= _playAreaBoundingBox.Right)
        {
            // _direction.X *= -1;
            _position.X = 325;
            _position.Y = 225;
        }
        if(_position.Y <= _playAreaBoundingBox.Top || _position.Y >= _playAreaBoundingBox.Bottom)
        {
            _direction.Y *= -1;
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        Rectangle ballRectangle = new Rectangle((int) _position.X, (int) _position.Y, (int) _dimensions.X, (int) _dimensions.Y);
        spriteBatch.Draw(_texture, ballRectangle, Color.White);
    }

    // have this return a boolean so we can tell if it collided with the paddle or not
    internal void ProcessCollision(Rectangle otherBoundingBox)
    {
        if (_collisionTimer >= _CollisionTimerInterval && BoundingBox.Intersects(otherBoundingBox))
        {
            // inside is the collision aftermath
            _collisionTimer = 0;
            Rectangle intersection = Rectangle.Intersect(BoundingBox, otherBoundingBox);
            // the intersection is the portion of the colliding box (the ball's)

            // if its a horizontal rectangle, it would be a top/bottom collision
            if(intersection.Width > intersection.Height)
            {
                _direction.Y *= -1;
            }
            // else its a vertical rectangle, it would be a side to side collision
            else
            {
                _direction.X *= -1;
            }
        }
    }
}