using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace slasher
{
    public class AnimationComponent
    {
        private readonly Dictionary<string, Animation> _animations;
        private Animation _currentAnimation;
        public Animation CurrentAnimation => _currentAnimation;

        public AnimationComponent(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _currentAnimation = _animations["idle"];
        }

        public void Update(GameTime gameTime)
        {
            _currentAnimation?.Update(gameTime);
        }

        public void SetAnimation(string key, bool loop = true)
        {
            if (!_animations.ContainsKey(key))
                return;

            var anim = _animations[key];
            anim.IsLooping = loop;

            if (_currentAnimation != anim)
            {
                anim.Reset();
                _currentAnimation = anim;
            }
        }

        public Animation GetAnimation(string key)
        {
            return _animations.ContainsKey(key) ? _animations[key] : null;
        }

        public void UpdateAnimationState(PlayerState state, Vector2 velocity, bool isJumping, bool isDashing)
        {
            if (isJumping && state == PlayerState.AirAttack && !_currentAnimation.IsFinished)
                return;

            if (isJumping || isDashing ||
                state is PlayerState.Attack1 or PlayerState.Attack2 or PlayerState.Attack3 or
                    PlayerState.Defend or PlayerState.HurtBlock or PlayerState.SpecialAttack or PlayerState.AirAttack)
                return;

            if (velocity.X != 0)
                SetAnimation("run");
            else
                SetAnimation("idle");
        }
    }
}