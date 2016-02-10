using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Components.GameLogic.Actions
{
    /// <summary>
    /// the implementing classes should pass to thier constructors
    /// only references to condition checking related properties
    /// since checking for the condition happenes at every frame (update)
    /// </summary>
    public interface IAction
    {
        bool Holds { get; }
        void Apply();
    }
}
