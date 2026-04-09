using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08_MosquitoAttack;

public class Actor
{
    protected SimpleAnimation _animationAlive, _animationPoofing;
    protected Vector2 _position, _direction;
    protected Rectangle _gameBoundingBox;
    protected float _speed;
    protected Projectile[] _projectiles;
    protected enum State { Alive, Poofing, Dead }
    protected State _state;
    internal bool Alive { get => _state == State.Alive; }

    internal Rectangle BoundingBox
    {
        get
        {
            return new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_animationAlive.FrameDimensions.X,
                (int)_animationAlive.FrameDimensions.Y
            );
        }
    }
}