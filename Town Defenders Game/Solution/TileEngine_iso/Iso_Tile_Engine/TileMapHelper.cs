
using Microsoft.Xna.Framework;

namespace Engine.Iso_Tile_Engine
{

    public enum Direction { NW, NE, SW, SE, E, W, S, N }

    public static class TileMapHelper
    {
        // Extention methods
        public static Point WalkTo(this Point location ,Direction direction)
        {
            int x = location.X;
            int y = location.Y;

            switch (direction)
            {
                case Direction.NE:
                    y--;
                    if (y % 2 == 0) x++;
                    break;

                case Direction.NW:
                     y--;
                    if (y % 2 == 1) x--;
                    break;

                case Direction.SE:
                     y++;
                    if (y % 2 == 0) x++;
                    break;

                case Direction.SW:
                     y++;
                    if (y % 2 == 1) x--;
                    break;

                case Direction.E:
                    x++;
                    break;
                case Direction.W:
                    x--;
                    break;
                case Direction.N:
                    y -= 2;
                    break;
                case Direction.S:
                    y += 2;
                    break;
            }

            return new Point(x, y);
        }

        public static Vector2 WalkTo(this Vector2 location, Direction direction)
        {
            var loc = new Point((int)location.X, (int)location.Y);
            var result = loc.WalkTo(direction);
            return new Vector2(result.X, result.Y);
        }
    }
}
