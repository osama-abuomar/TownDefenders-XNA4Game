using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture = Engine.Assets.Texture;


namespace Engine.Iso_Tile_Engine
{


    public class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }

    public class TileMap
    {

        public int MapWidth;
        public int MapHeight;
        private readonly float _maxdepth;
        private readonly float _depthOffset;
        public List<MapRow> Rows = new List<MapRow>();
        private readonly Texture2D _mouseMap;


        public MapCell MapCellAt(int X, int Y)
        {
            return this.Rows[Y].Columns[X];
        }

        public MapCell MapCellAt(Point position)
        {
            return this.Rows[position.Y].Columns[position.X];
        }

        public MapCell MapCellAt(Vector2 position)
        {
            return this.Rows[(int)position.Y].Columns[(int)position.X];
        }

        public TileMap(int width, int height)
        {

            MapWidth = width;
            MapHeight = height;

            _mouseMap =
                GameGraphics.GetTexture("mousemap").SourceTexture;

            Random random = new Random();

            // set cells depths here..
            _maxdepth = (MapWidth + 1) * ((MapHeight + 1) * TileInfo.TileWidth) / 10;

            for (int y = 0; y < MapHeight; y++)
            {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < MapWidth; x++)
                {
                    _depthOffset = 0.7f - ((x + (y * TileInfo.TileWidth)) / _maxdepth);

                    MapCell cell = new MapCell("grass_tile_" + random.Next(0, 4).ToString() + "_" + random.Next(0, 4).ToString()
                       , true);
                    cell.DrawDepth = _depthOffset;

                    cell.Location = new Point(x, y);

                    // determine if this cell resides on an odd row
                    if (y % 2 == 1) cell.OnOddRow = true;
                    else cell.OnOddRow = false;

                    thisRow.Columns.Add(cell);
                }
                Rows.Add(thisRow);
            }

           

        }

        public Point WorldToMapCell(Point worldPoint, out Point localPoint)
        {
            Point mapCell = new Point(
               worldPoint.X / _mouseMap.Width,
               worldPoint.Y / _mouseMap.Height * 2
               );

            int localPointX = worldPoint.X % _mouseMap.Width;
            int localPointY = worldPoint.Y % _mouseMap.Height;

            int dx = 0;
            int dy = 0;

            var myUint = new uint[1];

            if (new Rectangle(0, 0, _mouseMap.Width, _mouseMap.Height).Contains(localPointX, localPointY))
            {
                _mouseMap.GetData(0, new Rectangle(localPointX, localPointY, 1, 1), myUint, 0, 1);

                if (myUint[0] == 0xFF0000FF) // Red
                {
                    dx = -1;
                    dy = -1;
                    localPointX = localPointX + (_mouseMap.Width / 2);
                    localPointY = localPointY + (_mouseMap.Height / 2);
                }

                if (myUint[0] == 0xFF00FF00) // Green
                {
                    dx = -1;
                    localPointX = localPointX + (_mouseMap.Width / 2);
                    dy = 1;
                    localPointY = localPointY - (_mouseMap.Height / 2);
                }

                if (myUint[0] == 0xFF00FFFF) // Yellow
                {
                    dy = -1;
                    localPointX = localPointX - (_mouseMap.Width / 2);
                    localPointY = localPointY + (_mouseMap.Height / 2);
                }

                if (myUint[0] == 0xFFFF0000) // Blue
                {
                    dy = +1;
                    localPointX = localPointX - (_mouseMap.Width / 2);
                    localPointY = localPointY - (_mouseMap.Height / 2);
                }
            }

            mapCell.X += dx;
            mapCell.Y += dy - 2;

            localPoint = new Point(localPointX, localPointY);

            return mapCell;
        }

        public Point WorldToMapCell(Point worldPoint)
        {
            Point dummy;
            return WorldToMapCell(worldPoint, out dummy);
        }

        public Point WorldToMapCell(Vector2 worldPoint)
        {
            return WorldToMapCell(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }

        public MapCell GetCellAtWorldPoint(Point worldPoint)
        {
            Point mapPoint = WorldToMapCell(worldPoint);
            return Rows[mapPoint.Y].Columns[mapPoint.X];
        }

        public MapCell GetCellAtWorldPoint(Vector2 worldPoint)
        {
            return GetCellAtWorldPoint(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }

        public int GetSlopeMapHeight(Point localPixel, Texture slopeMap)
        {

            if (slopeMap == null) return 0;

            Color[] slopeColor = new Color[1];

            slopeMap.SourceTexture.GetData(
                0,
                new Rectangle(localPixel.X + slopeMap.SourceRectangle.X, localPixel.Y + slopeMap.SourceRectangle.Y, 1, 1),
                slopeColor,
                0,
                1);

            int offset = (int)(((float)(255 - slopeColor[0].R) / 255f) * TileInfo.HeightTileOffset);

            return offset;
        }

        public int GetSlopeHeightAtWorldPoint(Point worldPoint)
        {
            Point localPoint;
            Point mapPoint = WorldToMapCell(worldPoint, out localPoint);

            Texture slopeMap =
                Rows[mapPoint.Y].Columns[mapPoint.X].SlopeMap;

            return GetSlopeMapHeight(localPoint, slopeMap);
        }

        public int GetSlopeHeightAtWorldPoint(Vector2 worldPoint)
        {
            return GetSlopeHeightAtWorldPoint(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }

        public int GetOverallHeight(Point worldPoint)
        {
            Point mapCellPoint = WorldToMapCell(worldPoint);
            int height = Rows[mapCellPoint.Y].Columns[mapCellPoint.X].HeightTiles.Count * TileInfo.HeightTileOffset;
            height += GetSlopeHeightAtWorldPoint(worldPoint);

            return height;
        }

        public int GetOverallHeight(Vector2 worldPoint)
        {
            return GetOverallHeight(new Point((int)worldPoint.X, (int)worldPoint.Y));

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var firstSquare = new Vector2(Camera.CameraLocation.X / TileInfo.TileStepX, Camera.CameraLocation.Y / TileInfo.TileStepY);
            var firstX = (int)firstSquare.X;
            var firstY = (int)firstSquare.Y;

            for (var y = 0; y < TileEngineInfo.SquaresDown; y++)
            {
                for (var x = 0; x < TileEngineInfo.SquaresAcross; x++)
                {
                    // polling x, y of map cell (depends on camera position)
                    var mapx = (firstX + x);
                    var mapy = (firstY + y);
                    MapCellAt(mapx, mapy).Draw(spriteBatch);
                }
            }
        }
    }
}
