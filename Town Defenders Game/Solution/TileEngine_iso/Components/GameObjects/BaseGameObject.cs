
using Engine.Components.GameLogic;

namespace Engine.Components.GameObjects
{
    public class BaseGameObject
    {
        public GameWorld GameWorldRef { get; set; }


        protected static int NextId = 0;
        public int Id { get; private set; }

        public BaseGameObject()
        {
            Id = NextId++;
        }

    }
}
