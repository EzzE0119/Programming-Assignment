using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public ObstacleData obstacleData;

    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                if (obstacleData.obstacles[index])
                {
                    Vector3 position = new Vector3(x, 1f, y);
                    Instantiate(obstaclePrefab, position, Quaternion.identity);
                }
            }
        }
    }
}
