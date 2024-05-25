using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public int gridSize = 10;
    public Pathfinding pathfinding; // Reference to the Pathfinding component

    void Start()
    {
        GenerateGrid();
        if (pathfinding != null)
        {
            pathfinding.InitializeGrid();
        }
        else
        {
            Debug.LogError("Pathfinding component not assigned in GridManager.");
        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                TileInfo tileInfo = cube.AddComponent<TileInfo>();
                tileInfo.SetPosition(x, y);
            }
        }
    }
}
