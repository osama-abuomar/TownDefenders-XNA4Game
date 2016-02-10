using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine.Components.GameLogic.Movement
{
    public interface IMovable
    {
        float Speed { get; set; }

        Vector2 MoveDirection { get; set; }

        Vector2 Distination { get; set; }

        Vector2 PreviousMoveDirection { get; set; }

        bool CanGetMoveOrder { get; }

        Vector2 CellLocation { get; }

        Vector2 Position { get; set; }

        int CurrentPathSegment { get; set; }

        List<Vector2> MovingPath { get; set; }

        Vector2 DrawOffset { get; }

        bool IsMoving { get; }

        void UpdateMovingAnimation();

        bool MoveDirectionChanged();

        void MoveBy(float X, float Y);

        void Stop();
    }
}

