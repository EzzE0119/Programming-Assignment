using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI: MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public Pathfinding pathfinding; // Reference to the pathfinding system
    public float moveSpeed = 5f; // Movement speed of the enemy
    private List<Vector3> currentPath; // Current path for the enemy to follow
    private int currentWaypointIndex; // Index of the current waypoint in the path
    private bool isMoving = false; // Flag to indicate if the enemy is currently moving
    public PlayerMovement PM;

    void Update()
    {
        if (playerTransform != null && !isMoving)
        {
            // Move the enemy towards the player unit
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the nearest neighboring tile to the player's position
        Vector3 playerPosition = playerTransform.position;
        Vector3[] adjacentTiles = new Vector3[]
        {
            playerPosition + Vector3.forward,
            playerPosition + Vector3.back,
            playerPosition + Vector3.right,
            playerPosition + Vector3.left
        };

        float closestDistance = Mathf.Infinity;
        Vector3 nearestTile = Vector3.zero;

        foreach (Vector3 tile in adjacentTiles)
        {
            float distance = Vector3.Distance(transform.position, tile);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestTile = tile;
            }
        }

        // Move towards the nearest neighboring tile instead of directly to the player
        if (pathfinding != null)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = nearestTile;
            currentPath = pathfinding.FindPath(startPosition, targetPosition);

            if (currentPath != null && currentPath.Count > 0)
            {
                currentWaypointIndex = 0;
                StartCoroutine(FollowPath());
            }
        }
    }

    IEnumerator FollowPath()
    {
        isMoving = true;
        while (currentWaypointIndex < currentPath.Count && PlayerMoved())
        {
            Vector3 targetPosition = currentPath[currentWaypointIndex];
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            currentWaypointIndex++;
        }

        // Enemy has reached the player's nearest neighboring tile, wait for player movement
        yield return new WaitUntil(() => PlayerMoved());
        // Player moved, resume chasing
        isMoving = false;
        MoveTowardsPlayer();
    }

    bool PlayerMoved()
    {
        // Check if the player has moved from their current position
        
        if(PM.isMoving)
            return false;
        return true;
    }
}
