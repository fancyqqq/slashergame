using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;

namespace slasher;

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    Dash,
    Defend,
    HurtBlock,
    Attack1,
    Attack2,
    Attack3,
    AirAttack,
    SpecialAttack
}

public class Player
{
    private Vector2 _position = new(300, 200);
    private Vector2 _velocity = Vector2.Zero;
    private PlayerState _state = PlayerState.Idle;
    private readonly AnimationComponent _animation;
    private readonly PlayerStateData _stateData;
    private GameTime _gameTime;

    public Rectangle BoundingBox
    {
        get
        {
            var hitbox = _animation.CurrentAnimation.Hitbox;
            int hitboxX = (int)_position.X + hitbox.X;

            if (!_stateData.IsFacingRight)
            {
                int centerOffset = (hitbox.X + hitbox.Width / 2) - ((96 / 2) * 3);
                hitboxX -= 2 * centerOffset;
            }

            return new Rectangle(
                hitboxX,
                (int)_position.Y + hitbox.Y,
                hitbox.Width,
                hitbox.Height
            );
        }
    }

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    public PlayerState State => _state;
    public PlayerStateData StateData => _stateData;
    public bool IsJumping => _state == PlayerState.Jump;
    public bool IsDashing => _state == PlayerState.Dash;
    public bool IsFacingRight => _stateData.IsFacingRight;
    public Animation CurrentAnimation => _animation.CurrentAnimation;
    public GameTime GameTime => _gameTime;

    public Player(ContentManager content, GraphicsDevice graphicsDevice, TiledMapTileLayer collisionLayer, int tileSize,
        int groundY)
    {
        _stateData = new PlayerStateData
        {
            GroundY = groundY,
            CollisionLayer = collisionLayer,
            TileSize = tileSize
        };
        var animations = AnimationLoader.LoadAnimations(content, graphicsDevice);
        _animation = new AnimationComponent(animations);
    }

    public void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
    {
        _gameTime = gameTime;
        _stateData.Velocity = _velocity;

        if (!_stateData.CanDash)
        {
            _stateData.DashCooldownTimer -= 1f / 60f;
            if (_stateData.DashCooldownTimer <= 0f)
                _stateData.CanDash = true;
        }

        ApplyPhysics(gameTime);

        _velocity = _stateData.Velocity;
        _animation.UpdateAnimationState(_state, _velocity, IsJumping, IsDashing);
        _animation.Update(gameTime);
    }

    private void ApplyPhysics(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (!_stateData.IsGrounded)
        {
            float gravity = _stateData.Velocity.Y > 0
                ? _stateData.Gravity * _stateData.FallGravityMultiplier
                : _stateData.Gravity;
            _stateData.Velocity.Y += gravity * dt;
        }

        Vector2 proposedPosition = _position + _stateData.Velocity * dt;
        Rectangle current = BoundingBox;

        List<Rectangle> predictiveBoxes = new List<Rectangle>
        {
            current,
            new Rectangle(current.Left - _stateData.TileSize, current.Y, _stateData.TileSize, current.Height),
            new Rectangle(current.Right, current.Y, _stateData.TileSize, current.Height),
            new Rectangle(current.X, current.Top - _stateData.TileSize, current.Width, _stateData.TileSize),
            new Rectangle(current.X, current.Bottom, current.Width, _stateData.TileSize),
            new Rectangle(current.Left - _stateData.TileSize, current.Top - _stateData.TileSize, _stateData.TileSize,
                _stateData.TileSize),
            new Rectangle(current.Right, current.Top - _stateData.TileSize, _stateData.TileSize, _stateData.TileSize)
        };

        Rectangle groundSensor = new Rectangle(current.X, current.Bottom, current.Width, 2);
        bool isGroundBeneath = IsColliding(groundSensor);

        bool xCollision = false;
        foreach (var box in predictiveBoxes)
        {
            Rectangle futureBoxX = new Rectangle(
                (int)(box.X + _stateData.Velocity.X * dt),
                box.Y,
                box.Width,
                box.Height
            );
            if (IsColliding(futureBoxX))
            {
                xCollision = true;
                if (_stateData.Velocity.X > 0)
                {
                    float tileX = (float)Math.Floor(futureBoxX.Right / (float)_stateData.TileSize) *
                                  _stateData.TileSize;
                    float offsetX = current.Right - _position.X;
                    _position.X = tileX - offsetX - 1;
                    _stateData.Velocity.X = 0;
                }
                else if (_stateData.Velocity.X < 0)
                {
                    float tileX = (float)Math.Ceiling(futureBoxX.Left / (float)_stateData.TileSize) *
                                  _stateData.TileSize;
                    float offsetX = current.Left - _position.X;
                    _position.X = tileX - offsetX + 1;
                    _stateData.Velocity.X = 0;
                }

                break;
            }
        }

        if (!xCollision)
        {
            _position.X = proposedPosition.X;
        }

        bool yCollision = false;
        foreach (var box in predictiveBoxes)
        {
            Rectangle futureBoxY = new Rectangle(
                box.X,
                (int)(box.Y + _stateData.Velocity.Y * dt),
                box.Width,
                box.Height
            );
            if (IsColliding(futureBoxY))
            {
                yCollision = true;
                if (_stateData.Velocity.Y > 0)
                {
                    float tileY = (float)Math.Floor(futureBoxY.Bottom / (float)_stateData.TileSize) *
                                  _stateData.TileSize;
                    float offsetY = current.Top - _position.Y;
                    _position.Y = tileY - current.Height - offsetY;
                    _stateData.Velocity.Y = 0;
                    _stateData.IsGrounded = true;
                }
                else if (_stateData.Velocity.Y < 0)
                {
                    float tileY = (float)Math.Ceiling(futureBoxY.Top / (float)_stateData.TileSize) *
                                  _stateData.TileSize;
                    float offsetY = current.Top - _position.Y;
                    _position.Y = tileY - offsetY;
                    _stateData.Velocity.Y = 0;
                }

                break;
            }
        }

        if (!yCollision)
        {
            _position.Y = proposedPosition.Y;
            _stateData.IsGrounded = isGroundBeneath;
        }

        System.Diagnostics.Debug.WriteLine(
            $"IsGrounded: {_stateData.IsGrounded}, Position.Y: {_position.Y}, Velocity.Y: {_stateData.Velocity.Y}");
    }

    private bool IsColliding(Rectangle rect)
    {
        int left = rect.Left / _stateData.TileSize;
        int right = (rect.Right - 1) / _stateData.TileSize;
        int top = rect.Top / _stateData.TileSize;
        int bottom = (rect.Bottom - 1) / _stateData.TileSize;

        bool collisionDetected = false;
        for (int y = top; y <= bottom; y++)
        {
            for (int x = left; x <= right; x++)
            {
                if (x < 0 || y < 0 || x >= _stateData.CollisionLayer.Width || y >= _stateData.CollisionLayer.Height)
                    continue;

                var tile = _stateData.CollisionLayer.GetTile((ushort)x, (ushort)y);
                if (tile.GlobalIdentifier == 66)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        if (_stateData.WindPlaying)
        {
            spriteBatch.Draw(
                _stateData.WindEffect.CurrentFrame,
                _stateData.WindPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                3f,
                _stateData.WindFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f);
        }

        spriteBatch.Draw(
            _animation.CurrentAnimation.CurrentFrame,
            _position,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            3f,
            _stateData.IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
            0f);
    }

    public void SetState(PlayerState state)
    {
        _state = state;
    }

    public void SetAnimation(string key, bool loop = true)
    {
        _animation.SetAnimation(key, loop);
    }

    public void SetStateAndAnimation(PlayerState newState, string animationName, bool loop = true)
    {
        _animation.SetAnimation(animationName, loop);
        _state = newState;
    }

    public Animation GetAnimation(string key)
    {
        return _animation.GetAnimation(key);
    }

    public void TriggerBlockHurt()
    {
        _stateData.IsDefendPressed = false;
        _state = PlayerState.HurtBlock;
    }

    public bool IsBlockedAbove()
    {
        var box = BoundingBox;
        var ceilingSensor = new Rectangle(box.X, box.Y - 2, box.Width, 2);
        return IsColliding(ceilingSensor);
    }
}