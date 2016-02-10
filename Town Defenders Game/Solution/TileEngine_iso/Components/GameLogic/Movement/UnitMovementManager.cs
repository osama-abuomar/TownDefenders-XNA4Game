using System;
using Engine.Components.GameLogic.PathTakingLogic;
using Engine.Iso_Tile_Engine;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Components.GameLogic.Movement
{
    public class UnitMovementManager
    {

        private readonly GameWorld _gameWorldRef;

        private bool _mouseRightButtonClicked =
            InputManager.MouseRightButtonClicked();



        public UnitMovementManager(GameWorld gameWorldRef)
        {
            _gameWorldRef = gameWorldRef;
        }

        public void Update(GameTime gameTime)
        {
            _mouseRightButtonClicked =
                InputManager.MouseRightButtonClicked();
        }

        public void ManageMovement(IMovable movable)
        {

            movable.PreviousMoveDirection = movable.MoveDirection;

            #region new move-to command
            if (movable.CanGetMoveOrder && _mouseRightButtonClicked)
            {
                var rightClickLocation = GetMouseCellLocation();

                var path = PathFinder.FindPath( movable.CellLocation, rightClickLocation, _gameWorldRef.Map );
                movable.MovingPath = path;

                if (path != null && path.Count > 1)
                {
                    movable.CurrentPathSegment = 1;

                    // initially move to first segment..
                    SetMovableToMoveToCell(movable, movable.MovingPath[movable.CurrentPathSegment]); // or [1]
                }

            }
            #endregion

            #region update already moving objects in this frame
            if (movable.IsMoving)
            {

                if (MovableReachedCurrentDistination(movable))
                {
                    if (++movable.CurrentPathSegment == movable.MovingPath.Count)
                    {
                        // here movable reached its final distination
                        movable.Stop();
                        Console.WriteLine("distination location reached");

                    }
                    else
                    {
                        SetMovableToMoveToCell(movable, movable.MovingPath[movable.CurrentPathSegment]);
                    }
                }

         

                if (movable.MoveDirectionChanged())
                {
                    movable.UpdateMovingAnimation();
                }

                movable.MoveBy(movable.MoveDirection.X * movable.Speed, movable.MoveDirection.Y * movable.Speed);
            

                //commented
                #region Clamping
                //float visualX = MathHelper.Clamp(
                //    movable.Position.X, 100, Camera.WorldWidth - 100); 
                //float visualY = MathHelper.Clamp(
                //    movable.Position.Y, 100 , Camera.WorldHeight - 100);

                //movable.Position = new Vector2(visualX, visualY);
                #endregion

                //commented
                #region Camera Follows the Character
                //// Camera follows the character if he comes closer to any of the edges of the screen by 100 pixels.
                //Vector2 testPosition = Camera.WorldToScreen(movable.Position);

                //if (testPosition.X < 100)
                //{
                //    Camera.Move(new Vector2(testPosition.X - 100, 0));
                //}

                //if (testPosition.X > (Camera.ViewWidth - 100))
                //{
                //    Camera.Move(new Vector2(testPosition.X - (Camera.ViewWidth - 100), 0));
                //}

                //if (testPosition.Y < 100)
                //{
                //    Camera.Move(new Vector2(0, testPosition.Y - 100));
                //}

                //if (testPosition.Y > (Camera.ViewHeight - 100))
                //{
                //    Camera.Move(new Vector2(0, testPosition.Y - (Camera.ViewHeight - 100)));
                //}
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// static method to find a path for a movable object from cell to cell using a tile map
        /// </summary>
        /// <param name="movable"></param>
        /// <param name="fromCell"></param>
        /// <param name="toCell"></param>
        /// <param name="mapRef"></param>
        public static void SetMovableToMovePath(IMovable movable, Vector2 fromCell, Vector2 toCell, TileMap mapRef)
        {
            var path = PathFinder.FindPath(fromCell, toCell, mapRef);
            movable.MovingPath = path;

            if (path == null || path.Count <= 1) return;

            movable.CurrentPathSegment = 1;

            // initially move to first segment..
            Vector2 cellCenter = mapRef.MapCellAt(movable.MovingPath[movable.CurrentPathSegment]).CenterPosition;
            movable.Distination = cellCenter;

            Vector2 difference = movable.Distination - movable.Position;

            if (difference != Vector2.Zero)
            {
                movable.MoveDirection = Vector2.Normalize(difference);
            }
            
        }

        private static bool MovableReachedCurrentDistination(IMovable movable)
        {
            return Math.Abs((int)movable.Position.X - (int)movable.Distination.X) < (movable.Speed + 1)
                               && Math.Abs((int)movable.Position.Y - (int)movable.Distination.Y) < (movable.Speed + 1);

        }

        private static void SetMovableToMoveToLocation(IMovable movable, Vector2 loc)
        {
            movable.Distination = loc;

            Vector2 difference = movable.Distination - movable.Position;

            if (difference != Vector2.Zero)
            {
                movable.MoveDirection = Vector2.Normalize(difference);
            }
        }

        public void SetMovableToMoveToCell(IMovable movable, Vector2 cellLocation)
        {
            Vector2 cellCenter = _gameWorldRef.Map.MapCellAt((int)cellLocation.X, (int)cellLocation.Y).CenterPosition;
            SetMovableToMoveToLocation(movable, cellCenter);
        }

        private Vector2 GetMouseCellLocation()
        {
            var clickPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            var clickWorldPosition = Camera.ScreenToWorld(clickPosition);
            var cell = _gameWorldRef.Map.GetCellAtWorldPoint(clickWorldPosition).Location;
            var v2Cell = new Vector2(cell.X, cell.Y);
            return v2Cell;
        }

    }
}
