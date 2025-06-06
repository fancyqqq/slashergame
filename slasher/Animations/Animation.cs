using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace slasher
{
    public class Animation
    {
        public string Name { get; }

        private List<Texture2D> _frames;
        private float _frameTime;
        private float _timer;
        private int _currentFrame;

        public bool IsLooping { get; set; } = true;
        public bool IsFinished => !IsLooping && _currentFrame >= _frames.Count - 1;
        public Texture2D CurrentFrame => _frames[_currentFrame];
        public Rectangle Hitbox { get; private set; }

        public Animation(string name, Texture2D spriteSheet, int frameCount, float frameDuration, int spriteSize, GraphicsDevice graphicsDevice, Rectangle hitbox)
        {
            Name = name;
            _frames = new List<Texture2D>();
            _frameTime = frameDuration;
            Hitbox = hitbox;

            int cellSize = 96;
            int offset = (cellSize - spriteSize) / 2;

            for (int i = 0; i < frameCount; i++)
            {
                int x = i * cellSize + offset;
                int y = offset;

                Rectangle sourceRect = new Rectangle(x, y, spriteSize, spriteSize);
                Texture2D frame = new Texture2D(graphicsDevice, spriteSize, spriteSize);
                Color[] data = new Color[spriteSize * spriteSize];
                spriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                frame.SetData(data);

                _frames.Add(frame);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsFinished) return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= _frameTime)
            {
                _timer = 0f;
                _currentFrame++;

                if (_currentFrame >= _frames.Count)
                    _currentFrame = IsLooping ? 0 : _frames.Count - 1;
            }
        }

        public void Reset()
        {
            _currentFrame = 0;
            _timer = 0f;
        }
    }
}
