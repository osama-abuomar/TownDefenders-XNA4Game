using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine.Components.GameLogic.Selection
{
    public interface ISelectionManager
    {
        void ManageSelection(ISelectable unit);
        void Update(GameTime gameTime);
        bool SelectionHappened { get; }
        List<ISelectable> CurrentlySelectesItems { get; set; }
        List<int> CurrentlySelectedGroups { get; set; }
    }
}
