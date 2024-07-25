using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class UnitSelectionBox : MonoBehaviour
{
    Camera myCam;

    [SerializeField]
    RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    public bool isDragging;

    private void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
        isDragging = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Vérifie si un élément de l'UI(ex : bouton) n'est pas ciblé pour éviter concurrence
            if (EventSystem.current.IsPointerOverGameObject()) 
            {
                isDragging = false;
                return;
            }

            isDragging = true;
            startPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        //Dragging
        if (Input.GetMouseButton(0) && isDragging)
        {
            //Selection dynamique
            if (boxVisual.rect.width > 0 || boxVisual.rect.height > 0) 
            {
                SelectUnits();
            }

            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        //End of dragging
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            if (boxVisual.rect.width > 0 || boxVisual.rect.height > 0)
            {
                SelectUnits();
            }

            //Réinitialisation
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
            isDragging = false;
        }
    }

    void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        // Calcul du centre de la box (visuel)
        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        // Ajuste position par rapport au centre de la box (visuel) 
        boxVisual.position = boxCenter;

        // Détermine la taille(hauteur et largeur) de la box (visuel) 
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        // Applique la taille (visuel) 
        boxVisual.sizeDelta = boxSize;
    }


    //Fait la MAJ des positions de début et de fin de la box selon orientation par rapport à la position de départ
    void DrawSelection()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }


        if (Input.mousePosition.y < startPosition.y)
        {
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    //Sélectionne les unités présentent dans la box
    void SelectUnits()
    {
        foreach (GameObject unit in UnitSelectionManager.instance.allUnitsList)
        {
            //Si dans la box, on selectionne
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {

                if (unit.CompareTag(UnitSelectionManager.instance.unitsTag)) 
                {
                    UnitSelectionManager.instance.DragSelect(unit);
                }
              
            }
            else // Sinon on déselectionne
            {
                if(UnitSelectionManager.instance.selectedUnitsList.Contains(unit) == true) 
                {
                    UnitSelectionManager.instance.selectedUnitsList.Remove(unit);
                    UnitSelectionManager.instance.SelectUnit(unit, false);
                }    
            }
        }
    }
}
