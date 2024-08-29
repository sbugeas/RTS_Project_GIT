using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;



public class BuildingManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Tilemap tilemap;

    [SerializeField] GridDrawing gridDrawing;

    private GameObject instantiatedBuilding;
    private GameObject currentPrefab;

    public GameObject homePrefab;
    public GameObject loggerCampPrefab;
    public GameObject stoneMinerHutPrefab;
    public GameObject barrackPrefab;

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
        instantiatedBuilding = null;
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
                // Convertir la position du monde en position sur la grille
                Vector3Int gridPosition = tilemap.WorldToCell(hit.point);

                // Convertir la position de la grille en position du monde
                targetBuilding = tilemap.CellToWorld(gridPosition);
            }
            else
            {
                //Si pas de hit
                targetBuilding = instantiatedBuilding.transform.position;
            }

            //Prépositionnement du bâtiment
            instantiatedBuilding.transform.position = targetBuilding;

            if(Input.GetMouseButtonDown(0)) 
            {
                //Si 'buildable' et ne cible pas un élément de l'UI
                if ((EventSystem.current.IsPointerOverGameObject() == false) && (instantiatedBuilding.GetComponent<BuildingPlacement>().isBuildable)) 
                {
                    PlaceBuilding();
                }
            }
            else if (Input.GetMouseButtonDown(1)) //On annule la construction
            {
                CancelConstruction();
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

    public void SelectBarrackPrefab()
    {
        currentPrefab = barrackPrefab;
    }

    public void PrepareToCreateBuilding()
    {
        if(currentPrefab != null) 
        {
            if (buildingInstantiated) 
            {
                CancelConstruction();
            }

            //Instanciation
            instantiatedBuilding = Instantiate(currentPrefab, this.gameObject.transform);

            //Parentage
            Transform _buildings = GameObject.Find("Buildings").transform;
            instantiatedBuilding.transform.SetParent(_buildings);

            //Fait apparaître la grille
            gridDrawing.DisplayGrid(true);

            buildingInstantiated = true;
        }
    }


    private void PlaceBuilding()
    {
        if(instantiatedBuilding != null)
        {
            BuildingData dataScript = instantiatedBuilding.GetComponent<BuildingData>();

            //Retirer collider de construction
            GameObject BuildingTmpCollider = instantiatedBuilding.transform.Find("buildingTmpCollider").gameObject;
            if (BuildingTmpCollider != null) 
            {
                Destroy(BuildingTmpCollider);
            }

            //Retirer script de placement
            instantiatedBuilding.GetComponent<BuildingPlacement>().SetOriginalColorAndRemoveScript();
            //Destroy(instantiatedBuilding.GetComponent<BuildingPlacement>());

            instantiatedBuilding.GetComponent<NavMeshObstacle>().enabled = true;

            if (instantiatedBuilding.GetComponent<BoxCollider>() != null) 
            {
                instantiatedBuilding.GetComponent<BoxCollider>().isTrigger = false;
            }
            

            buildingInstantiated = false;

            SpendResources(dataScript);

            //Si c'est une maison
            if (instantiatedBuilding.CompareTag("home"))
            {
                BuildHome(dataScript);
            }
            else if (instantiatedBuilding.CompareTag("loggerCamp")) //Si c'est un camp de bûcheron
            {
                BuildLoggerCamp(dataScript);
            }
            else if (instantiatedBuilding.CompareTag("stoneMinerHut")) //Si c'est une cabane de mineur
            {
                BuildStoneMinerHut(dataScript);
            }
            else if (instantiatedBuilding.CompareTag("barrack"))
            {
                BuildBarrack(dataScript);
            }

            instantiatedBuilding = null;
            currentPrefab = null;

            CanvasManager.instance.CloseAllOpenedPanel();

            //Fait disparaître la grille
            gridDrawing.DisplayGrid(false);
        }

    }

    private void CancelConstruction() 
    {
        if(instantiatedBuilding != null) 
        {
            Destroy(instantiatedBuilding);
        }
        instantiatedBuilding = null;
        buildingInstantiated = false;

        //Fait disparaître la grille
        gridDrawing.DisplayGrid(false);
    }

    private void SpendResources(BuildingData dataScript)
    {
        ResourcesManager.instance.RemoveWood(dataScript.woodCost);
        ResourcesManager.instance.RemoveStone(dataScript.stoneCost);
        //Ajouter les autres au fur et à mesure
    }

    private void BuildHome(BuildingData dataScript) 
    {
        ResourcesManager.instance.AddToMaxPop(dataScript.popGiven);
    }

    private void BuildLoggerCamp(BuildingData dataScript) 
    {
        if (dataScript.transform.GetComponent<LoggerCamp>() != null) 
        {
            dataScript.transform.GetComponent<LoggerCamp>().enabled = true;
        }
    }

    private void BuildStoneMinerHut(BuildingData dataScript)
    {
        if (dataScript.transform.GetComponent<StoneMinerHut>() != null)
        {
            dataScript.transform.GetComponent<StoneMinerHut>().enabled = true;
        }
    }

    private void BuildBarrack(BuildingData dataScript)
    {
        //Faire convex sur le collider
        MeshCollider collider = dataScript.transform.GetComponent<MeshCollider>();
        collider.isTrigger = false;
        collider.convex = false;

        //Activer autres zones obstacle
        dataScript.transform.Find("otherObstacleZones").gameObject.SetActive(true);

        //Activer script caserne(à faire)
        dataScript.transform.GetComponent<Barrack>().enabled = true;
    }

    
}
