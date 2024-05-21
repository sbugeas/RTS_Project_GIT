using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager instance;

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> selectedUnitsList = new List<GameObject>();

    public bool attackCursorVisible;

    public Camera cam;

    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask enemy;

    public int max_size_group = 10;



    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Il y a plus d'une instance de UnitSelectionManager dans la scène !");
            return;
        }

        instance = this;
    }


    private void Start()
    {
        cam = Camera.main;
    }



    private void Update()
    {

        //Clique gauche
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                SelectByClicking(hit.collider.gameObject);
            }
            else 
            {
                DeselectAll();
            }

        }



        if (selectedUnitsList.Count > 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Si on détecte ennemi
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemy))
            {
                //Afficher curseur combat(+ tard)
                attackCursorVisible = true;

                //Détécter clique droit(= engager combat)
                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;

                    //Envoyer target aux unités sélectionnées
                    foreach (GameObject unit in selectedUnitsList) 
                    {
                        //Si ce sont des unités de combat (peut être utile + tard)
                        if (unit.GetComponent<AttackController>()) 
                        {   
                            unit.GetComponent<Unit>().isCommandedToMove = false;
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }

                }

            }
            else 
            {
                attackCursorVisible = false;

                if (Input.GetMouseButtonDown(1) && Physics.Raycast(ray, out hit))
                {
                    //On envoie destination aux unités
                    foreach (GameObject unit in selectedUnitsList)
                    {
                        unit.GetComponent<Unit>().isCommandedToMove = true;

                        if (unit.GetComponent<AttackController>().targetToAttack != null) 
                        {
                            unit.GetComponent<AttackController>().targetToAttack = null;

                            /* A SUPPRIMER APRES + DE TESTS
                            
                            //Pour éviter le prob de suppression des unitées détectées sur OnTriggerEnter alors qu'ils ne sont pas sortis / A améliorer
                            float distanceFromTarget = Vector3.Distance(unit.transform.position, unit.GetComponent<AttackController>().targetToAttack.position);

                            if (distanceFromTarget >= 12f) 
                            {
                                unit.GetComponent<AttackController>().targetToAttack = null;
                            }

                            */

                        }
                        unit.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                    }

                }

            }

            
        }

            
    }



    private void TriggerSelectionIndicator(GameObject _unit, bool isVisible)
    {
        _unit.transform.GetChild(0).gameObject.SetActive(isVisible);
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
   
