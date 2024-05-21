using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class BuildingManager : MonoBehaviour
{
    [SerializeField] GameObject homePrefab;
    [SerializeField] Camera cam;
    [SerializeField] GameObject currBuilding;

    public LayerMask ground;

    private bool buildingInstantiated = false;

    public static BuildingManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Il y a plus d'une instance de BuildingManager dans la scène !");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        currBuilding = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!buildingInstantiated) 
            {
                CreateHome();
            }
            
        }

        

        if (buildingInstantiated) 
        {
            Vector3 targetBuilding;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                targetBuilding = hit.point;
            }
            else
            {
                //Sécurité(si pas de hit)
                targetBuilding = currBuilding.transform.position;
            }

            currBuilding.transform.position = targetBuilding;

            if(Input.GetMouseButtonDown(0) && (currBuilding.GetComponent<BuildingPlacement>().isBuildable)) 
            {
                //Attention peut être bugs à cause de la désactivation
                //currBuilding.GetComponent<BuildingPlacement>().enabled = false;

                Destroy(currBuilding.GetComponent<BuildingPlacement>());
                currBuilding.GetComponent<BoxCollider>().isTrigger = false;

                ///////////////////////////////////////
                //currBuilding.GetComponent<>().;
                //////////////////////////////////////

                currBuilding = null;
                buildingInstantiated = false;
            }
            
        }


        
    }

    private void CreateHome()
    {
        currBuilding = Instantiate(homePrefab, this.gameObject.transform);
        buildingInstantiated = true;
    }



    
}
