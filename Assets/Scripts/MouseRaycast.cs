using UnityEngine;
using UnityEngine.UI;

public class MouseRaycast : MonoBehaviour
{
    public Text positionText;
    public Material hoverMaterial; // The material to apply when hovering
    private TileInfo previousTile; // The previously hovered tile
    private Material originalMaterial; // The original material of the previously hovered tile

    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                positionText.text = $"Tile: ({tileInfo.x}, {tileInfo.y})";

                // Change the material of the currently hovered tile
                MeshRenderer renderer = hit.collider.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    // Revert the material of the previously hovered tile
                    if (previousTile != null && previousTile != tileInfo)
                    {
                        previousTile.GetComponent<MeshRenderer>().material = originalMaterial;
                    }

                    // Store the original material of the current tile
                    if (previousTile != tileInfo)
                    {
                        originalMaterial = renderer.material;
                        renderer.material = hoverMaterial;
                        previousTile = tileInfo;
                    }
                }
            }
        }
        else
        {
            // If the raycast does not hit any tile, revert the material of the previously hovered tile
            if (previousTile != null)
            {
                previousTile.GetComponent<MeshRenderer>().material = originalMaterial;
                previousTile = null;
            }
        }
    }
}
