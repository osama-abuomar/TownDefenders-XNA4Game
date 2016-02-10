using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine.Iso_Tile_Engine
{
    public static class Camera
    {
        static private Vector2 _cameraLocation = Vector2.Zero;
        public static float Speed = 10f;

        public static int ViewWidth { get; set; }
        public static int ViewHeight { get; set; }
        public static int WorldWidth { get; set; }
        public static int WorldHeight { get; set; }

        public static Vector2 DisplayOffset { get; set; }

        public static Vector2 CameraLocation
        {
            get
            {
                return _cameraLocation;
            }
            set
            {
                _cameraLocation = new Vector2(
                    MathHelper.Clamp(value.X, 0f, WorldWidth - ViewWidth),
                    MathHelper.Clamp(value.Y, 0f, WorldHeight - ViewHeight));
            }
        }

        public static Rectangle BoundingBox
        {
            get { return new Rectangle(
                (int) CameraLocation.X, (int) CameraLocation.Y,
                ViewWidth, ViewHeight); }
        }

        public static Rectangle ExtendedBoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)CameraLocation.X - 5 * TileInfo.TileWidth,
                    (int)CameraLocation.Y - 2 * TileInfo.TileHeight,
                    ViewWidth +  10 * TileInfo.TileWidth,
                    ViewHeight + 4 * TileInfo.TileHeight);
            }
        }

        public static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return worldPosition - CameraLocation + DisplayOffset;
        }

        public static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return screenPosition + CameraLocation - DisplayOffset;
        }

        public static void Move(Vector2 offset)
        {
            CameraLocation += offset;
        }
    }
}
