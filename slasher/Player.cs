using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace slasher
{
    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Attack1,
        Attack2,
        Attack3,
        Dash,
        Defend,
        HurtBlock,
        SpecialAttack,
        AirAttack
    }

    public class Player
    {
        private Vector2 _position = new(300, 200);
        private Vector2 _velocity = Vector2.Zero;
        private PlayerState _state = PlayerState.Idle;
        private const int SpriteCanvasSize = 96;
        private const int SpriteScale = 3;
        private const int SpriteScreenCenterX = (SpriteCanvasSize / 2) * SpriteScale;

        private readonly MovementComponent _movement;
        private readonly JumpComponent _jump;
        private readonly AttackComponent _attack;
        private readonly DashComponent _dash;
        private readonly BlockComponent _block;
        private readonly AnimationComponent _animation;
        private readonly PhysicsComponent _physics;

        public Rectangle BoundingBox
        {
            get
            {
                var hitbox = _animation.CurrentAnimation.Hitbox;

                int hitboxX = (int)_position.X + hitbox.X;

                if (!_movement.IsFacingRight)
                {
                    int centerOffset = (hitbox.X + hitbox.Width / 2) - SpriteScreenCenterX;
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

        public bool IsJumping
        {
            get => _jump.IsJumping;
            set => _jump.IsJumping = value;
        }

        public bool IsDashing => _dash.IsDashing;
        public bool IsFacingRight => _movement.IsFacingRight;
        
        public Animation CurrentAnimation => _animation.CurrentAnimation;

        public void DebugDraw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            spriteBatch.Draw(pixel, BoundingBox, Color.Red * 0.5f);
        }

        public Player(ContentManager content, GraphicsDevice graphicsDevice, CollisionMap collisionMap, int groundY)
        {
            var animations = AnimationLoader.LoadAnimations(content, graphicsDevice);
            _animation = new AnimationComponent(animations);
            _movement = new MovementComponent(this);
            _jump = new JumpComponent(this);
            _attack = new AttackComponent(this);
            _dash = new DashComponent(this);
            _block = new BlockComponent(this);
            _physics = new PhysicsComponent(this, collisionMap, groundY);
        }

        public void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
            _dash.Update(ks, gameTime);
            _block.Update(ms);
            _attack.Update(ms);
            _movement.Update(ks);
            _jump.Update(ks, gameTime);
            _attack.UpdatePhases();
            _block.UpdatePhases();
            _physics.Update(gameTime);

            _animation.UpdateAnimationState(_state, _velocity, _jump.IsJumping, _dash.IsDashing);
            _animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _dash.Draw(spriteBatch);

            spriteBatch.Draw(
                _animation.CurrentAnimation.CurrentFrame,
                _position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                3f,
                _movement.IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
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
            _block.TriggerBlockHurt();
        }
        
        public bool IsBlockedAbove()
        {
            var box = BoundingBox;
            var ceilingSensor = new Rectangle(box.X, box.Y - 2, box.Width, 2);
            return _physics.CheckCollisionBox(ceilingSensor);
        }

    }
}