using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    /// 

    List<Node> openNodes;
    List<Node> closedNodes;
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        Node startNode = new Node(startPos, null, 0, CalculateH(startPos, endPos));

        openNodes = new List<Node>();
        closedNodes = new List<Node>();

        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            Node currentNode = null;
            foreach (Node node in openNodes)
            {
                if (currentNode == null || node.FScore < currentNode.FScore)
                {
                    currentNode = node;
                    if (currentNode.position == endPos)
                    {
                        List<Vector2Int> pathNodes = new List<Vector2Int>();
                        List<Vector2Int> path = new List<Vector2Int>();
                        pathNodes.Add(currentNode.position);
                        while (currentNode.parent != null)
                        {
                            currentNode = currentNode.parent;
                            pathNodes.Add(currentNode.position);
                        }

                        for (int i = pathNodes.Count - 1; i >= 0; i--)
                        {
                            path.Add(pathNodes[i]);
                        }

                        return path;
                    }
                }
            }

            openNodes.Remove(currentNode);
            List<Vector2Int> neighbours = CheckNeighbours(currentNode, grid);

            foreach (Vector2Int neighbour in neighbours)
            {
                Node neighbourNode = new Node(neighbour, currentNode, CalculateG(currentNode, startPos, neighbour), CalculateH(neighbour, endPos));

                if (!openNodes.Contains(neighbourNode))
                {
                    openNodes.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }



    public List<Vector2Int> CheckNeighbours(Node thisNode, Cell[,] grid)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (thisNode.position.x - 1 >= 0)
        {
            if (!grid[thisNode.position.x, thisNode.position.y].HasWall(Wall.LEFT))
                neighbours.Add(grid[thisNode.position.x - 1, thisNode.position.y].gridPosition);
        }

        if (thisNode.position.x + 1 <= grid.GetLength(0))
        {
            if (!grid[thisNode.position.x, thisNode.position.y].HasWall(Wall.RIGHT))
                neighbours.Add(grid[thisNode.position.x + 1, thisNode.position.y].gridPosition);
        }

        if (thisNode.position.y - 1 >= 0)
        {
            if (!grid[thisNode.position.x, thisNode.position.y].HasWall(Wall.DOWN))
                neighbours.Add(grid[thisNode.position.x, thisNode.position.y - 1].gridPosition);
        }

        if (thisNode.position.y + 1 <= grid.GetLength(1))
        {
            if (!grid[thisNode.position.x, thisNode.position.y].HasWall(Wall.UP))
                neighbours.Add(grid[thisNode.position.x, thisNode.position.y + 1].gridPosition);
        }

        return neighbours;
    }
    
    public int CalculateH(Vector2Int thisPos, Vector2Int endPos)
    {
        return Mathf.RoundToInt(Vector2Int.Distance(thisPos, endPos));
    }

    public int CalculateG(Node previousNode, Vector2Int startPos, Vector2Int thisNode)
    {
        /*
        if (previousNode.GScore + Mathf.RoundToInt(Vector2Int.Distance(previousNode.position, thisNode)) < Mathf.RoundToInt(Vector2Int.Distance(startPos, thisNode)))
        {
            return (int)(previousNode.GScore + Mathf.RoundToInt(Vector2Int.Distance(previousNode.position, thisNode)));
        }
        else return Mathf.RoundToInt(Vector2Int.Distance(startPos, thisNode));
        */

        return (int)(previousNode.GScore + Mathf.RoundToInt(Vector2Int.Distance(previousNode.position, thisNode)));
    }
}
