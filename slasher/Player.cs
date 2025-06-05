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
    Idle, Run, Jump, Dash, Defend, HurtBlock,
    Attack1, Attack2, Attack3, AirAttack, SpecialAttack
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

    public Vector2 Position { get => _position; set => _position = value; }
    public Vector2 Velocity { get => _velocity; set => _velocity = value; }
    public PlayerState State => _state;
    public PlayerStateData StateData => _stateData;
    public bool IsJumping => _state == PlayerState.Jump;
    public bool IsDashing => _state == PlayerState.Dash;
    public bool IsFacingRight => _stateData.IsFacingRight;
    public Animation CurrentAnimation => _animation.CurrentAnimation;
    public GameTime GameTime => _gameTime;

    public Player(ContentManager content, GraphicsDevice graphicsDevice, TiledMapTileLayer collisionLayer, int tileSize, int groundY)
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
        _stateData.TotalTime = (float)gameTime.TotalGameTime.TotalSeconds;

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

        Rectangle groundSensor = new Rectangle(current.X, current.Bottom, current.Width, 2);
        bool isGroundBeneath = IsColliding(groundSensor);

        Rectangle futureBoxX = new Rectangle(
            (int)(current.X + _stateData.Velocity.X * dt),
            current.Y,
            current.Width,
            current.Height
        );

        if (IsColliding(futureBoxX))
        {
            if (_stateData.Velocity.X > 0)
            {
                int tileX = (int)Math.Floor((float)futureBoxX.Right / _stateData.TileSize) * _stateData.TileSize;
                float offset = current.Right - _position.X;
                _position.X = tileX - offset - 1;
            }
            else if (_stateData.Velocity.X < 0)
            {
                int tileX = (int)Math.Ceiling((float)futureBoxX.Left / _stateData.TileSize) * _stateData.TileSize;
                float offset = current.Left - _position.X;
                _position.X = tileX - offset + 1;
            }
            _stateData.Velocity.X = 0;
        }
        else
        {
            _position.X = proposedPosition.X;
        }

        Rectangle futureBoxY = new Rectangle(
            current.X,
            (int)(current.Y + _stateData.Velocity.Y * dt),
            current.Width,
            current.Height
        );

        if (IsColliding(futureBoxY))
        {
            if (_stateData.Velocity.Y > 0)
            {
                int tileY = (int)Math.Floor((float)futureBoxY.Bottom / _stateData.TileSize) * _stateData.TileSize;
                float offset = current.Top - _position.Y;
                _position.Y = tileY - current.Height - offset;
                _stateData.Velocity.Y = 0;
                _stateData.IsGrounded = true;
            }
            else if (_stateData.Velocity.Y < 0)
            {
                int tileY = (int)Math.Ceiling((float)futureBoxY.Top / _stateData.TileSize) * _stateData.TileSize;
                float offset = current.Top - _position.Y;
                _position.Y = tileY - offset;
                _stateData.Velocity.Y = 0;
            }
        }
        else
        {
            _position.Y = proposedPosition.Y;
            _stateData.IsGrounded = isGroundBeneath;
        }
    }

    private bool IsColliding(Rectangle entity)
    {
        int tileWidth = _stateData.TileSize;
        int tileHeight = _stateData.TileSize;

        int originX = entity.X / tileWidth;
        int originY = entity.Y / tileHeight;
        int endX = (entity.Right - 1) / tileWidth;
        int endY = (entity.Bottom - 1) / tileHeight;

        for (int x = originX; x <= endX; x++)
        {
            for (int y = originY; y <= endY; y++)
            {
                if (x < 0 || y < 0 || x >= _stateData.CollisionLayer.Width || y >= _stateData.CollisionLayer.Height)
                    continue;

                var tile = _stateData.CollisionLayer.GetTile((ushort)x, (ushort)y);
                if (tile.GlobalIdentifier == 66)
                    return true;
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

    public void SetState(PlayerState state) => _state = state;
    public void SetAnimation(string key, bool loop = true) => _animation.SetAnimation(key, loop);

    public void SetStateAndAnimation(PlayerState newState, string animationName, bool loop = true)
    {
        _animation.SetAnimation(animationName, loop);
        _state = newState;
    }

    public Animation GetAnimation(string key) => _animation.GetAnimation(key);

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
