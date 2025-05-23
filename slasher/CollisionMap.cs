using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;

namespace slasher
{
    public class CollisionMap
    {
        private readonly TiledMapTileLayer _layer;
        private readonly int _tileSize;

        public CollisionMap(TiledMapTileLayer layer, int tileSize)
        {
            _layer = layer;
            _tileSize = tileSize;
        }

        public bool IsColliding(Rectangle rect)
        {
            int left = rect.Left / _tileSize;
            int right = (rect.Right - 1) / _tileSize;
            int top = rect.Top / _tileSize;
            int bottom = (rect.Bottom - 1) / _tileSize;

            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    if (x < 0 || y < 0 || x >= _layer.Width || y >= _layer.Height)
                        continue;

                    var tile = _layer.GetTile((ushort)x, (ushort)y);
                    

                    
                    if (tile.GlobalIdentifier != 0)
                    {
                        return true;
                    }

                }
            }

            return false;
        }
    }
}