using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;



public class BuildingManager : MonoBehaviour
{
    [SerializeField] GameObject homePrefab;
    [SerializeField] GameObject loggerCampPrefab;
    [SerializeField] GameObject stoneMinerHutPrefab;

    [SerializeField] Camera cam;
    [SerializeField] GameObject currBuilding; //champ sérialisé pour tests

    private GameObject currentPrefab;

    public LayerMask ground;

    public bool buildingInstantiated = false;

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
        currentPrefab = null;
    }

    void Update()
    {

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

            //Positionnement sur terrain(ground)
            currBuilding.transform.position = targetBuilding;

            if(Input.GetMouseButtonDown(0)) 
            {
                //Si 'buildable' et ne cible pas un élément de l'UI
                if ((EventSystem.current.IsPointerOverGameObject() == false) && (currBuilding.GetComponent<BuildingPlacement>().isBuildable)) 
                {
                    PlaceBuilding();
                }
            }
            else if (Input.GetMouseButtonDown(1)) //On annule la construction
            {
                Destroy(currBuilding);
                currBuilding = null;
                buildingInstantiated = false;
            }
            
        }


        
    }

    public void SelectHomePrefab() 
    {
        currentPrefab = homePrefab;
    }

    public void SelectLoggerCampPrefab()
    {
        currentPrefab = loggerCampPrefab;
    }

    public void SelectStoneMinerHutPrefab()
    {
        currentPrefab = stoneMinerHutPrefab;
    }

    public void PrepareToCreateBuilding()
    {
        if(!buildingInstantiated && currentPrefab != null)
        {
            //Instanciation
            currBuilding = Instantiate(currentPrefab, this.gameObject.transform);

            //Parentage
            Transform _buildings = GameObject.Find("Buildings").transform;
            currBuilding.transform.SetParent(_buildings);

            buildingInstantiated = true;
        }

    }

    private void PlaceBuilding()
    {
        if(currBuilding != null)
        {
            BuildingData dataScript = currBuilding.GetComponent<BuildingData>();

            Destroy(currBuilding.GetComponent<BuildingPlacement>());

            currBuilding.GetComponent<NavMeshObstacle>().enabled = true;
            currBuilding.GetComponent<BoxCollider>().isTrigger = false;

            buildingInstantiated = false;

            //Si c'est une maison
            if (currBuilding.CompareTag("home"))
            {
                BuildHome(dataScript);
            }
            else if (currBuilding.CompareTag("loggerCamp")) //Si c'est un camp de bûcheron
            {
                BuildLoggerCamp(dataScript);
            }
            else if (currBuilding.CompareTag("stoneMinerHut")) //Si c'est une cabane de mineur
            {
                BuildStoneMinerHut(dataScript);
            }

            currBuilding = null;
            currentPrefab = null;

            CanvasManager.instance.CloseAllOpenedPanel();
        }

    }

    private void BuildHome(BuildingData dataScript) 
    {
        ResourcesManager.instance.RemoveWood(dataScript.woodCost);
        ResourcesManager.instance.AddToMaxPop(dataScript.popGiven);
    }

    private void BuildLoggerCamp(BuildingData dataScript) 
    {
        ResourcesManager.instance.RemoveWood(dataScript.woodCost);

        if(dataScript.transform.GetComponent<LoggerCamp>() != null) 
        {
            dataScript.transform.GetComponent<LoggerCamp>().enabled = true;
        }
    }

    private void BuildStoneMinerHut(BuildingData dataScript)
    {
        ResourcesManager.instance.RemoveWood(dataScript.woodCost);

        if (dataScript.transform.GetComponent<StoneMinerHut>() != null)
        {
            dataScript.transform.GetComponent<StoneMinerHut>().enabled = true;
        }
    }

}
