using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace slasher
{
    public static class AnimationLoader
    {
        public static Dictionary<string, Animation> LoadAnimations(ContentManager content, GraphicsDevice graphicsDevice)
        {
            var animations = new Dictionary<string, Animation>();

            Animation Create(string key, string texture, int frames, float duration, Rectangle hitbox, bool loop = true)
            {
                var anim = new Animation(key, content.Load<Texture2D>(texture), frames, duration, 96, graphicsDevice, hitbox);
                anim.IsLooping = loop;
                return anim;
            }

            Rectangle defaultBox = new Rectangle(115, 146, 55, 96);
            Rectangle bigBox = new Rectangle(135, 50, 55, 192);
            Rectangle hurtBox = new Rectangle(145, 146, 55, 96);

            animations["idle"] = Create("idle", "IDLE", 8, 0.1f, defaultBox);
            animations["run"] = Create("run", "RUN", 16, 0.05f, defaultBox);
            animations["jump_start"] = Create("jump_start", "JUMP-START", 3, 0.1f, defaultBox);
            animations["jump_trans"] = Create("jump_trans", "JUMP-TRANSITION", 3, 0.1f, defaultBox, false);
            animations["jump_fall"] = Create("jump_fall", "JUMP-FALL", 3, 0.1f, defaultBox);
            animations["attack1"] = Create("attack1", "ATTACK 1", 7, 0.06f, hurtBox);
            animations["attack2"] = Create("attack2", "ATTACK 2", 7, 0.06f, hurtBox);
            animations["attack3"] = Create("attack3", "ATTACK 3", 6, 0.06f, hurtBox);
            animations["special_attack"] = Create("special_attack", "SPECIAL ATTACK", 14, 0.07f, bigBox);
            animations["air_attack"] = Create("air_attack", "AIR ATTACK", 6, 0.08f, defaultBox);
            animations["dash"] = Create("dash", "DASH NO WIND IMPACT", 8, 0.05f, defaultBox);
            animations["wind"] = Create("wind", "WIND", 6, 0.066f, new Rectangle());
            animations["defend"] = Create("defend", "DEFEND", 1, 0.1f, new Rectangle(130, 146, 55, 96));
            animations["defend_hurt"] = Create("defend_hurt", "DEFEND-hurt", 5, 0.1f, defaultBox, false);
            animations["wall_contact"] = Create("wall_contact", "WALL CONTACT INV", 3, 0.1f, defaultBox);
            animations["wall_slide"] = Create("wall_slide", "WALL SLIDE INV", 3, 0.1f, defaultBox);

            return animations;
        }
    }
}
