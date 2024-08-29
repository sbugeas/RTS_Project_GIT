using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class GridDrawing : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Material lineMaterial; // Mat de la grille
    [SerializeField] float lineWidth = 0.05f;

    [SerializeField] int cellNbX = 60;
    [SerializeField] int cellNbZ = 60;


    void Start()
    {
        DrawGrid();
        DisplayGrid(false);
    }

    public void DisplayGrid(bool _toDisplay)
    {
        this.gameObject.SetActive(_toDisplay);
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        // Crée un nouvel objet pour la ligne + parente à cet objet + lui affecte un LineRenderer
        GameObject lineObject = new GameObject("GridLine");
        lineObject.transform.parent = this.transform;
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        //Affectation positions
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        //Affectation material et largeur
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial;
    }

    private void DrawGrid()
    {
        if(tilemap == null) 
        {
            Debug.Log("Tilemap is null");
            return;
        }

        BoundsInt bounds = tilemap.cellBounds; //limite de la tilemap
        float cellSize = tilemap.cellSize.x; //taille d'une cellule(ici, la grille est carré donc x = z)

        //Lignes verticales
        for (int x = bounds.x; x <= (bounds.x + cellNbX); x++)
        {
            Vector3 cur_start = new Vector3 (x * cellSize, 0, bounds.z);
            Vector3 cur_end = new Vector3 (x * cellSize, 0, (bounds.z + cellNbZ) * cellSize);

            DrawLine(cur_start, cur_end);
        }

        //Lignes horizontales
        for (int z = bounds.z; z <= (bounds.z + cellNbZ); z++)
        {
            Vector3 cur_start = new Vector3(bounds.x, 0, z * cellSize);
            Vector3 cur_end = new Vector3((bounds.x + cellNbX) * cellSize, 0, z * cellSize);

            DrawLine(cur_start, cur_end);
        }
    }


    
}
