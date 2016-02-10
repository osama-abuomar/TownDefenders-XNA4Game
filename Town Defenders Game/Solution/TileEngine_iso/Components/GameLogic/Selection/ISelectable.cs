
using Microsoft.Xna.Framework;

namespace Engine.Components.GameLogic.Selection
{
    public interface ISelectable
    {
        int SelectionGroup { get; set; }

        void Select();

        void DeSelect();

        bool IsSelected { get; set; }

        Rectangle SelectBounds { get; }

        int Id { get; } // implemented by the GameObject (Grandpa)

    }
}
