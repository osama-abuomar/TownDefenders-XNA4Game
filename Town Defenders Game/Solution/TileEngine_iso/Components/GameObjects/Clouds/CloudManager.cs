using System;
using System.Collections.Generic;
using Engine.Assets;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture = Engine.Assets.Texture;

namespace Engine.Components.GameObjects.Clouds
{
    public enum CloudDirection { East, West }

    class CloudVisual
    {
        public Texture CloudTexture;
        public Texture CloudShadowTexture;
    }

    public class CloudManager
    {

        private readonly TileMap _mapRef;
        private readonly int _cloudCount;
        private readonly CloudDirection _cloudDirection;
        private Point _shadowOffset;
        
        private readonly List<CloudVisual> _cloudVisuals;

        private readonly float[] _xpositions;
        private readonly float[] _ypositions;
        private readonly float[] _speeds;
        private const float StartDrawDepth = 0.2f;
        private const float DrawDepthIncrement = 0.0001f;
        private readonly float[] _drawDepths;
        private readonly List<int> _cloudVisualIndex;

        readonly int _startingXWest;
        readonly int _endingXWest;
        readonly int _startingXEast;
        readonly int _endingXEast;

        readonly Random _random = new Random();

        private const int MaxCloudWidth = 881; // calculated from max width cloud texture 
        private const int MaxCloudHeight = 475; // calculated from max width cloud texture 

        public CloudManager(TileMap mapRef, int cloudCount, CloudDirection cloudDirection, float speed)
        {
            _mapRef = mapRef;
            _cloudCount = cloudCount;
            _cloudDirection = cloudDirection;

            const double epsilon = 0.2f;
            if (Math.Abs(speed - 0.0f) < epsilon) throw new Exception("cloud speed too slow");

            _startingXWest = mapRef.MapWidth * TileInfo.TileStepX;
            _endingXWest = -1 * MaxCloudWidth;

            _startingXEast = _endingXWest;
            _endingXEast = _startingXWest;

            _xpositions = new float[cloudCount];
            _ypositions = new float[cloudCount];
            _speeds = new float[cloudCount];
            _drawDepths = new float[cloudCount];

            int x, y;
            for (int i = 0; i < cloudCount; i++)
            {
                x = _random.Next(0, mapRef.MapWidth * TileInfo.TileStepX - MaxCloudWidth);
                y = _random.Next(-1 * MaxCloudHeight, mapRef.MapHeight * TileInfo.TileStepY - MaxCloudHeight);
                _xpositions[i] = x;
                _ypositions[i] = y;
                _speeds[i] = _random.Next(1, ((int)(speed * 10.0f))) / 10.0f;
                _drawDepths[i] = StartDrawDepth + i*DrawDepthIncrement;
            }

            _shadowOffset = new Point(CloudInfo.CloudShadowOffsetX, CloudInfo.CloudShadowOffsetY);



            _cloudVisuals = new List<CloudVisual>();
            CloudVisual cv1, cv2, cv3;

            cv1 = new CloudVisual();
            cv1.CloudTexture = GameGraphics.GetTexture("cloud1");
            cv1.CloudShadowTexture = GameGraphics.GetTexture("cloud1_shadow");

            cv2 = new CloudVisual();
            cv2.CloudTexture = GameGraphics.GetTexture("cloud2");
            cv2.CloudShadowTexture = GameGraphics.GetTexture("cloud2_shadow");

            cv3 = new CloudVisual();
            cv3.CloudTexture = GameGraphics.GetTexture("cloud3");
            cv3.CloudShadowTexture = GameGraphics.GetTexture("cloud3_shadow");

            _cloudVisuals.Add(cv1);
            _cloudVisuals.Add(cv2);
            _cloudVisuals.Add(cv3);

            _cloudVisualIndex = new List<int>();
            for (var i = 0; i < cloudCount; i++)
            {
                _cloudVisualIndex.Add(_random.Next(0, _cloudVisuals.Count));
            }
        }



        public void Update(GameTime gameTime)
        {

            for (var i = 0; i < _cloudCount; i++)
            {
                // move

                // repositioning
                switch (_cloudDirection)
                {
                    case CloudDirection.East:
                        _xpositions[i] += _speeds[i];
                        if (_xpositions[i] > _endingXEast)
                        {
                            _xpositions[i] = _startingXEast;
                            _xpositions[i] -= _random.Next(0, _mapRef.MapWidth * TileInfo.TileStepX / 2);  // so that clouds don't start over with same x differences all the time - more natural bahaviour
                            _ypositions[i] = _random.Next(-1 * MaxCloudHeight, _mapRef.MapHeight * TileInfo.TileStepY - MaxCloudHeight);
                        }
                        break;
                    case CloudDirection.West:
                        _xpositions[i] -= _speeds[i];
                        if (_xpositions[i] < _endingXWest)
                        {
                            _xpositions[i] = _startingXWest;
                            _xpositions[i] += _random.Next(0, _mapRef.MapWidth * TileInfo.TileStepX / 2);  // so that clouds don't start over with same x differences all the time - more natural bahaviour
                            _ypositions[i] = _random.Next(-1 * MaxCloudHeight, _mapRef.MapHeight * TileInfo.TileStepY - MaxCloudHeight);
                        }
                        break;
                }
            }
        }

        Rectangle ScreenRectangle = new Rectangle(0,0,1920,1080); // support larger res is better

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _cloudCount; i++)
            {

                if (!ScreenRectangle.Intersects(_cloudVisuals[_cloudVisualIndex[i]].CloudTexture.SourceRectangle))
                {
                    continue;
                }

                // draw the floating cloud
                spriteBatch.Draw(
                    _cloudVisuals[_cloudVisualIndex[i]].CloudTexture.SourceTexture,
                    Camera.WorldToScreen(
                        new Vector2(_xpositions[i], _ypositions[i])),
                    _cloudVisuals[_cloudVisualIndex[i]].CloudTexture.SourceRectangle,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    _drawDepths[i]); //TODO to be changes - very close to screen ! 


                // draw it's shadow
                spriteBatch.Draw(
                    _cloudVisuals[_cloudVisualIndex[i]].CloudShadowTexture.SourceTexture,
                    Camera.WorldToScreen(
                        new Vector2(_xpositions[i] + _shadowOffset.X, _ypositions[i] + _shadowOffset.Y)),
                    _cloudVisuals[_cloudVisualIndex[i]].CloudShadowTexture.SourceRectangle,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    CloudInfo.CloudsShadowDrawDepth); //TODO to be changes - very close to screen ! 

            }
        }
    }
}

