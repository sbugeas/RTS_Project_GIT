using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager instance;

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> selectedUnitsList = new List<GameObject>();

    public bool enemyDetected;
    public bool attackCursorVisible;
    public Vector2 hotSpot = Vector2.zero;

    public Texture2D originalCursor;
    public Texture2D attackCursor;

    public Camera cam;

    public LayerMask unitsLayer;
    public LayerMask enemyUnitsLayer;
    public LayerMask ground;
    public LayerMask buildingLayerP1;

    public string unitsTag;

    public int max_size_group = 10;
    public float gapUnit = 1.5f;

    public Transform groundMarker;
    public Transform selectedBuilding;

    [SerializeField] GameObject UnitSelectionBoxGO;
    private UnitSelectionBox unitSelectionBoxScript;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Il y a plus d'une instance de UnitSelectionManager dans la scène !");
            return;
        }

        instance = this;

        SetWantedCursor(originalCursor);
        attackCursorVisible = false;
    }


    private void Start()
    {
        selectedBuilding = null;
        cam = Camera.main;
        unitSelectionBoxScript = UnitSelectionBoxGO.GetComponent<UnitSelectionBox>();
    }



    private void Update()
    {
        //Clique gauche
        if (Input.GetMouseButtonDown(0) && (BuildingManager.instance.buildingInstantiated == false))
        {
            //Si ne cible pas un élément de l'UI
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //On détécte une unité
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitsLayer))
                {
                    SelectByClicking(hit.collider.gameObject);
                }
                else
                {
                    DeselectAll();
                    CanvasManager.instance.CloseAllOpenedPanel();

                    //On détécte un bâtiment
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingLayerP1))
                    {
                        selectedBuilding = hit.transform;

                        //On le sélectionne
                        CanvasManager.instance.SelectBuilding(selectedBuilding);
                    }

                    
                }
            }


        }



        if (selectedUnitsList.Count > 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Si on détecte ennemi
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyUnitsLayer))
            {
                //Pour l'affichage du curseur combat
                enemyDetected = true;

                //Détécter clique droit(= engager combat)
                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;

                    //Envoyer target aux unités sélectionnées
                    foreach (GameObject unit in selectedUnitsList) 
                    {
                        unit.GetComponent<Unit>().isCommandedToMove = false;
                        unit.GetComponent<AttackController>().targetToAttack = target;
                        unit.GetComponent<Unit>().isCommandedToAttack = true;
                    }

                }

            }
            else 
            {
                enemyDetected = false;

                //Commande de déplacement
                if (Input.GetMouseButtonDown(1) && Physics.Raycast(ray, out hit) && !unitSelectionBoxScript.isDragging)
                {
                    int totalSelectedUnits = selectedUnitsList.Count;
                    Vector3 target = hit.point;

                    Vector3 groupPos = GiveGrpMiddlePos(selectedUnitsList);

                    Vector3 movementDir = (target - groupPos).normalized;
                    Quaternion rotation = Quaternion.LookRotation(movementDir);

                    groundMarker.transform.position = target;
                    groundMarker.transform.rotation = rotation;

                    //Pour chaque unité sélectionnée
                    foreach (GameObject unit in selectedUnitsList)
                    {
                        unit.GetComponent<Unit>().isCommandedToAttack = false;
                        unit.GetComponent<Unit>().isCommandedToMove = true;

                        //On retire la cible
                        if (unit.GetComponent<AttackController>().targetToAttack != null) 
                        {
                            unit.GetComponent<AttackController>().targetToAttack = null;
                        }

                    }

                    //On envoie à la destination
                    if (totalSelectedUnits == 1)
                    {
                        selectedUnitsList[0].gameObject.GetComponent<NavMeshAgent>().SetDestination(target);
                    }
                    else
                    {
                        Vector3 lineDirection = (groundMarker.transform.right).normalized;
                        MoveUnitGroup(target, movementDir, lineDirection);
                    }


                }

            }

            
        }


        if (!attackCursorVisible && enemyDetected) 
        {
            SetWantedCursor(attackCursor);
            attackCursorVisible = true;
        }
        else if(attackCursorVisible && !enemyDetected)
        {
            SetWantedCursor(originalCursor);
            attackCursorVisible = false;
        }



    }

    //-------------------------------------------------

    private void MoveUnitGroup(Vector3 _target, Vector3 movementDir, Vector3 lineDir)
    {
        //On récupère le nombre d'unités pour chaque ligne
        int nbUnit_fstLine;
        int nbUnit_scndLine;
        int totalSelectedUnits = selectedUnitsList.Count;

        if (totalSelectedUnits < 5)
        {
            nbUnit_fstLine = totalSelectedUnits;
        }
        else
        {
            nbUnit_fstLine = 5;
        }

        nbUnit_scndLine = totalSelectedUnits - nbUnit_fstLine;
        
        //Déclaration + remplissage listes par ligne
        List<GameObject> unitFstLine = new List<GameObject>();
        List<GameObject> unitScndLine = new List<GameObject>();

        for (int i = 0; i < nbUnit_fstLine; i++) 
        {
            unitFstLine.Add(selectedUnitsList[i]);
        }
        for (int i = nbUnit_fstLine; i < totalSelectedUnits; i++)
        {
            unitScndLine.Add(selectedUnitsList[i]);
        }

        //LOGIQUE DE PLACEMENT

        //Calcul points de référence
        Vector3 refPointFstLine = _target + (movementDir * (gapUnit / 2));
        Vector3 refPointScndLine = _target - (movementDir * (gapUnit / 2));

        //Calcul et placement des unité de la première ligne
        GivePosOfLineAndMove(unitFstLine, refPointFstLine, lineDir);

        //Si il y a une deuxième ligne
        if (nbUnit_scndLine > 0) 
        {
            //Calcul et placement des unité de la deuxième ligne
            GivePosOfLineAndMove(unitScndLine, refPointScndLine, lineDir);
        }

    }

    //Calcul la destination des unités d'une ligne et les déplace
    private void GivePosOfLineAndMove(List<GameObject> _unitList, Vector3 refPoint, Vector3 lineDir) 
    {
        int nbUnit = _unitList.Count;
        float curGap;

        //Nb unités pair(et au moins 2 pour exécuter cette fonction)
        if (nbUnit % 2 == 0) 
        {
            curGap = gapUnit / 2;

            for (int i = 0; i < nbUnit; i += 2) 
            {
                Vector3 targetRightUnit = refPoint + (lineDir * curGap);
                Vector3 targetLeftUnit = refPoint - (lineDir * curGap);

                _unitList[i].gameObject.GetComponent<NavMeshAgent>().SetDestination(targetRightUnit);
                _unitList[i+1].gameObject.GetComponent<NavMeshAgent>().SetDestination(targetLeftUnit);

                curGap += gapUnit;
            }
        }
        else //Nb unités impair
        {
            curGap = gapUnit;

            _unitList[0].gameObject.GetComponent<NavMeshAgent>().SetDestination(refPoint);

            for (int i = 1; i < nbUnit; i += 2) 
            {   
                Vector3 targetRightUnit = refPoint + (lineDir * curGap);
                Vector3 targetLeftUnit = refPoint - (lineDir * curGap);

                _unitList[i].gameObject.GetComponent<NavMeshAgent>().SetDestination(targetRightUnit);
                _unitList[i + 1].gameObject.GetComponent<NavMeshAgent>().SetDestination(targetLeftUnit);

                curGap += gapUnit;
            }

        }


    }


    private void MoveUnitToPosition(GameObject unit, Vector3 unitPos)
    {
        unit.GetComponent<NavMeshAgent>().SetDestination(unitPos);
    }

    private Vector3 GiveGrpMiddlePos(List<GameObject> unitGroup) 
    { 
        Vector3 totalPos = Vector3.zero;

        foreach (GameObject unit in unitGroup)
        {
            totalPos += unit.transform.position;
        }

        return (totalPos / unitGroup.Count);
    }


    //Retiré momentanément car les textures sont mal configurée
    private void SetWantedCursor(Texture2D _texture) 
    {
        //Cursor.SetCursor(_texture, hotSpot, CursorMode.Auto);
    }


    private void TriggerSelectionIndicator(GameObject _unit, bool isVisible)
    {
        _unit.transform.Find("indicator").gameObject.SetActive(isVisible);
    }

    public void SelectUnit(GameObject _unit, bool isSelected)
    {
        if (isSelected) 
        {
            selectedUnitsList.Add(_unit);
        }
        
        TriggerSelectionIndicator(_unit, isSelected);
    }

    public void DragSelect(GameObject _unit)
    {
        if ((selectedUnitsList.Contains(_unit) == false) && selectedUnitsList.Count < max_size_group)
        {
            SelectUnit(_unit, true);
        }
    }

    public void DeselectAll()
    {
        foreach (GameObject go in selectedUnitsList)
        {
            TriggerSelectionIndicator(go, false);
        }

        selectedUnitsList.Clear();

    }

    private void SelectByClicking(GameObject _unit)
    {
        DeselectAll();
        SelectUnit(_unit, true);
    }

        
}