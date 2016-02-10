
using Microsoft.Xna.Framework;

namespace Engine.Components.GameLogic.PathTakingLogic
{
    public class PathNode
    {

        public PathNode ParentNode;
        public PathNode EndNode;
        private Vector2 _gridLocation;
        private readonly Vector2 _cellCenterPosition;
        public float TotalCost;
        public float DirectCost;

        public Vector2 GridLocation
        {
            get { return _gridLocation; }
            set
            {
                _gridLocation = value;
            }
        }
        public int GridX
        {
            get { return (int)_gridLocation.X; }
        }
        public int GridY
        {
            get { return (int)_gridLocation.Y; }
        }

        public PathNode(
            PathNode parentNode,
            PathNode endNode,
            Vector2 gridLocation,
            Vector2 cellCenterPosition,
            float cost)
        {
            _cellCenterPosition = cellCenterPosition;
            ParentNode = parentNode;
            GridLocation = gridLocation;
            EndNode = endNode; DirectCost = cost;
            if (endNode != null)
            {
                TotalCost = DirectCost + LinearCost();
            }
        }

        public float LinearCost()
        {
            return (Vector2.Distance( EndNode._cellCenterPosition, _cellCenterPosition ));
        }
        public bool IsEqualToNode(PathNode node)
        {
            return (GridLocation == node.GridLocation);
        }
    }
}
