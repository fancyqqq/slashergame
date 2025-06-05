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

            animations["idle"] = new Animation(content.Load<Texture2D>("IDLE"), 8, 0.1f, 96, graphicsDevice,
                new Rectangle(115, 146, 55, 96));
            animations["run"] = new Animation(content.Load<Texture2D>("RUN"), 16, 0.05f, 96, graphicsDevice,
                new Rectangle(115, 146, 55, 96));
            animations["jump_start"] = new Animation(content.Load<Texture2D>("JUMP-START"), 3, 0.1f, 96, graphicsDevice,
                new Rectangle(115, 146, 55, 96));
            animations["jump_trans"] = new Animation(content.Load<Texture2D>("JUMP-TRANSITION"), 3, 0.1f, 96, graphicsDevice,
                new Rectangle(115, 146, 55, 96));
            animations["jump_trans"].IsLooping = false;
            animations["jump_fall"] = new Animation(content.Load<Texture2D>("JUMP-FALL"), 3, 0.1f, 96, graphicsDevice,
                new Rectangle(115, 146, 55, 96));
            animations["attack1"] = new Animation(content.Load<Texture2D>("ATTACK 1"), 7, 0.06f, 96, graphicsDevice,
                new Rectangle(145, 146, 55, 96));
            animations["attack2"] = new Animation(content.Load<Texture2D>("ATTACK 2"), 7, 0.06f, 96, graphicsDevice,
                new Rectangle(145, 146, 55, 96));
            animations["attack3"] = new Animation(content.Load<Texture2D>("ATTACK 3"), 6, 0.06f, 96, graphicsDevice,
                new Rectangle(145, 146, 55, 96));
            animations["special_attack"] = new Animation(content.Load<Texture2D>("SPECIAL ATTACK"), 14, 0.07f, 96, graphicsDevice,
                new Rectangle(135, 50, 55, 192));
            animations["air_attack"] = new Animation(content.Load<Texture2D>("AIR ATTACK"), 6, 0.08f, 96, graphicsDevice,
                new Rectangle(115, 146, 55, 96));
            animations["dash"] = new Animation(content.Load<Texture2D>("DASH NO WIND IMPACT"), 8, 0.05f, 96, graphicsDevice,
                new Rectangle(115, 146, 55, 96));
            animations["wind"] = new Animation(content.Load<Texture2D>("WIND"), 6, 0.066f, 96, graphicsDevice, new Rectangle());
            animations["defend"] = new Animation(content.Load<Texture2D>("DEFEND"), 1, 0.1f, 96, graphicsDevice,
                new Rectangle(130, 146, 55, 96));
            animations["defend"].IsLooping = true;
            animations["defend_hurt"] = new Animation(content.Load<Texture2D>("DEFEND-hurt"), 5, 0.1f, 96, graphicsDevice,
                new Rectangle(115, 146, 55, 96));
            animations["defend_hurt"].IsLooping = false;

            return animations;
        }
    }
}
