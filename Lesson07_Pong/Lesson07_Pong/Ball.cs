using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson07_Pong;

public class Ball
{
    private Vector2 _position, _direction, _dimensions;
    private float _speed;
    private Texture2D _texture;
    private Rectangle _playAreaBoundingBox;

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

        _position += _direction * _speed * dt;

        // bounce the ball off left and right sides
        if(_position.X <= _playAreaBoundingBox.Left || _position.X >= _playAreaBoundingBox.Right)
        {
            _direction.X *= -1;
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
}