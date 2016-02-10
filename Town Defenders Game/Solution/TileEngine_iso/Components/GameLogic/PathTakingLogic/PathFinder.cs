
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Engine.Iso_Tile_Engine;

namespace Engine.Components.GameLogic.PathTakingLogic
{
    public static class PathFinder
    {
        private enum NodeStatus { Open, Closed };
        private static  Dictionary<Vector2, NodeStatus> nodeStatus = new Dictionary<Vector2, NodeStatus>();
        private const int DirectCostEw = 64;
        private const int DirectCostNs = 32;
        private const int DirectCostDiagonaly = 35;
        private static  List<PathNode> OpenList = new List<PathNode>();
        private static  Dictionary<Vector2, float> NodeCosts = new Dictionary<Vector2, float>();

        

        static private void AddNodeToOpenList(PathNode node)
        {
            int index = 0;
            float cost = node.TotalCost;
            while ((OpenList.Count() > index) &&
            (cost < OpenList[index].TotalCost))
            {
                index++;
            }
            OpenList.Insert(index, node);
            NodeCosts[node.GridLocation] = node.TotalCost;
            nodeStatus[node.GridLocation] = NodeStatus.Open;
        }
        
        static private IEnumerable<PathNode> FindAdjacentNodes( PathNode currentNode, PathNode endNode, TileMap mapRef)
        {
            var adjacentNodes = new List<PathNode>();
            var current = currentNode.GridLocation;

            var v2Pointer = current.WalkTo(Direction.N);
            if(mapRef.MapCellAt(v2Pointer).Walkable)
                adjacentNodes.Add(new PathNode(currentNode, endNode, v2Pointer, mapRef.MapCellAt(v2Pointer).CenterPosition, currentNode.DirectCost + PathFinder.DirectCostNs));

            v2Pointer = current.WalkTo(Direction.NE);
            if (mapRef.MapCellAt(v2Pointer).Walkable)
                adjacentNodes.Add(new PathNode(currentNode, endNode, v2Pointer, mapRef.MapCellAt(v2Pointer).CenterPosition, currentNode.DirectCost + PathFinder.DirectCostDiagonaly));

            v2Pointer = current.WalkTo(Direction.E);
            if (mapRef.MapCellAt(v2Pointer).Walkable)
                adjacentNodes.Add(new PathNode(currentNode, endNode, v2Pointer, mapRef.MapCellAt(v2Pointer).CenterPosition, currentNode.DirectCost + PathFinder.DirectCostEw));

            v2Pointer = current.WalkTo(Direction.SE);
            if (mapRef.MapCellAt(v2Pointer).Walkable)
                adjacentNodes.Add(new PathNode(currentNode, endNode, v2Pointer, mapRef.MapCellAt(v2Pointer).CenterPosition, currentNode.DirectCost + PathFinder.DirectCostDiagonaly));

            v2Pointer = current.WalkTo(Direction.S);
            if (mapRef.MapCellAt(v2Pointer).Walkable)
                adjacentNodes.Add(new PathNode(currentNode, endNode, v2Pointer, mapRef.MapCellAt(v2Pointer).CenterPosition, currentNode.DirectCost + PathFinder.DirectCostNs));

            v2Pointer = current.WalkTo(Direction.SW);
            if (mapRef.MapCellAt(v2Pointer).Walkable)
                adjacentNodes.Add(new PathNode(currentNode, endNode, v2Pointer, mapRef.MapCellAt(v2Pointer).CenterPosition, currentNode.DirectCost + PathFinder.DirectCostDiagonaly));

            v2Pointer = current.WalkTo(Direction.W);
            if (mapRef.MapCellAt(v2Pointer).Walkable)
                adjacentNodes.Add(new PathNode(currentNode, endNode, v2Pointer, mapRef.MapCellAt(v2Pointer).CenterPosition, currentNode.DirectCost + PathFinder.DirectCostEw));

            v2Pointer = current.WalkTo(Direction.NW);
            if (mapRef.MapCellAt(v2Pointer).Walkable)
                adjacentNodes.Add(new PathNode(currentNode, endNode, v2Pointer, mapRef.MapCellAt(v2Pointer).CenterPosition, currentNode.DirectCost + PathFinder.DirectCostDiagonaly));

            return adjacentNodes;
        }

        static public List<Vector2> FindPath(Vector2 startTile, Vector2 endTile, TileMap mapRef)
        {
            if (!mapRef.MapCellAt(startTile).Walkable || !mapRef.MapCellAt(endTile).Walkable)
            {
                return null;
            }
            OpenList.Clear();
            NodeCosts.Clear();
            nodeStatus.Clear();
            var endNode = new PathNode(null, null, endTile, mapRef.MapCellAt(endTile).CenterPosition, 0);
            var startNode = new PathNode(null, endNode, startTile, mapRef.MapCellAt(startTile).CenterPosition, 0);
            AddNodeToOpenList(startNode);
            while (OpenList.Count > 0)
            {
                var currentNode = OpenList[OpenList.Count - 1];
                if (currentNode.IsEqualToNode(endNode))
                {
                    var bestPath = new List<Vector2>();
                    while (currentNode != null)
                    {
                        bestPath.Insert(0, currentNode.GridLocation);
                        currentNode = currentNode.ParentNode;
                    }
                    return bestPath;
                }
                OpenList.Remove(currentNode);
                NodeCosts.Remove(currentNode.GridLocation);
                foreach (
                var possibleNode in
                FindAdjacentNodes(currentNode, endNode, mapRef))
                {
                    if (nodeStatus.ContainsKey(possibleNode.GridLocation))
                    {
                        if (nodeStatus[possibleNode.GridLocation] ==
                        NodeStatus.Closed)
                        {
                            continue;
                        }
                        if (
                        nodeStatus[possibleNode.GridLocation] ==
                        NodeStatus.Open)
                        {
                            if (possibleNode.TotalCost >=
                            NodeCosts[possibleNode.GridLocation])
                            {
                                continue;
                            }
                        }
                    }
                    AddNodeToOpenList(possibleNode);
                }
                nodeStatus[currentNode.GridLocation] = NodeStatus.Closed;
            }
            return null;
        }
    }
}
