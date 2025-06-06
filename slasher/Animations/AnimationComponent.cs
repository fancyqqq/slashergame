using Microsoft.Xna.Framework;
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
            if (!_animations.ContainsKey(key)) return;

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
            return _animations.TryGetValue(key, out var anim) ? anim : null;
        }
    }
}