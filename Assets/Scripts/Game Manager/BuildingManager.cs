using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
                PrepareToCreateHome();
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
                //Si pas de hit
                targetBuilding = currBuilding.transform.position;
            }

            //Prépositionnement sur terrain(ground)
            currBuilding.transform.position = targetBuilding;

            if(Input.GetMouseButtonDown(0) && (currBuilding.GetComponent<BuildingPlacement>().isBuildable)) 
            {
                PlaceHome();
            }
            else if (Input.GetMouseButtonDown(1)) 
            {
                Destroy(currBuilding);
                currBuilding = null;
                buildingInstantiated = false;
            }
            
        }


        
    }

    private void PrepareToCreateHome()
    {
        currBuilding = Instantiate(homePrefab, this.gameObject.transform);

        Transform _buildings = GameObject.Find("Buildings").transform; 
        currBuilding.transform.SetParent(_buildings);

        buildingInstantiated = true;
    }

    private void PlaceHome()
    {
        if(currBuilding != null)
        {
            BuildingData dataScript = currBuilding.GetComponent<BuildingData>();

            Destroy(currBuilding.GetComponent<BuildingPlacement>());

            currBuilding.GetComponent<NavMeshObstacle>().enabled = true;
            currBuilding.GetComponent<BoxCollider>().isTrigger = false;
            currBuilding = null;
            buildingInstantiated = false;

            ResourcesManager.instance.AddToTotalPop(dataScript.popGiven);
            ResourcesManager.instance.RemoveWood(dataScript.woodCost);

        }

    }



}
