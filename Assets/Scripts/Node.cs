using UnityEngine;

public class Node
{
    public Vector3 position;
    public bool isWalkable;
    public Node parent;
    public int gCost;
    public int hCost;

    public int fCost { get { return gCost + hCost; } }

    public Node(Vector3 _position, bool _isWalkable)
    {
        position = _position;
        isWalkable = _isWalkable;
    }
}
