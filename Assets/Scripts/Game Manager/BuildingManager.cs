using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;



public class BuildingManager : MonoBehaviour
{
    [SerializeField] GameObject homePrefab; //Rajouter les autre prefabs
    [SerializeField] GameObject loggerCampPrefab;

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
        //Remplacer par AddListener sur UI contruction(créer méthode ou l'intégrer dans une)
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!buildingInstantiated) 
            {
                //Sera changé pour être valable peut importe le bâtiment
                PrepareToCreateBuilding();
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

            //Positionnement sur terrain(ground)
            currBuilding.transform.position = targetBuilding;

            if(Input.GetMouseButtonDown(0) && (currBuilding.GetComponent<BuildingPlacement>().isBuildable)) 
            {
                //Sera changer pour s'adapter à tous les bâtiment
                PlaceBuilding();
            }
            else if (Input.GetMouseButtonDown(1)) 
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


    //Améliorer pour être utilisé avec tous les bâtiments(prefab en paramêtre)
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

    //Améliorer pour être utilisé avec tous les bâtiments(BuildingData doit rester) --> PlaceBuilding()
    //Pour les spécificités, faire vérif dans la fonction selon tag
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
            else if (currBuilding.CompareTag("loggerCamp"))
            {
                BuildLoggerCamp(dataScript);
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

}
