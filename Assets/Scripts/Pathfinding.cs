using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Node[,] grid;
    private int gridSize = 10;
    private List<Node> openList;
    private HashSet<Node> closedList;
    public LayerMask obstacle;
    public void InitializeGrid()
    {
        grid = new Node[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x, 1.5f, y);
                bool isWalkable = !Physics.CheckSphere(position, 0.1f, obstacle);
                grid[x, y] = new Node(position, isWalkable);
            }
        }
    }

    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = GetNodeFromWorldPoint(startPos);
        Node targetNode = GetNodeFromWorldPoint(targetPos);

        openList = new List<Node>();
        closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || (openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs((int)nodeA.position.x - (int)nodeB.position.x);
        int dstY = Mathf.Abs((int)nodeA.position.z - (int)nodeB.position.z);

        return dstX + dstY;
    }

    private Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.z);
        return grid[x, y];
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = Mathf.RoundToInt(node.position.x) + x;
                int checkY = Mathf.RoundToInt(node.position.z) + y;

                if (checkX >= 0 && checkX < gridSize && checkY >= 0 && checkY < gridSize)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }
}
