using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Pathfinding pathfinding;
    public float speed = 5f;
    public bool isMoving = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Vector3 targetPos = GetMouseWorldPosition();
            StartCoroutine(MoveAlongPath(targetPos));
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private IEnumerator MoveAlongPath(Vector3 targetPos)
    {
        List<Vector3> path = pathfinding.FindPath(transform.position, targetPos);
        if (path != null)
        {
            isMoving = true;
            foreach (Vector3 waypoint in path)
            {
                while (transform.position != waypoint)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
                    yield return null;
                }
            }
            isMoving = false;
        }
    }
}
