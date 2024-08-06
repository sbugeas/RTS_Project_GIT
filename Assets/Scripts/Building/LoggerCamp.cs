using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

//Script activé suite à construction (lorsque achevée)
public class LoggerCamp : MonoBehaviour
{
    public int loggersCount = 0;
    public int maxLoggerCount = 4;
    public int starterLoggerCount = 0;

    public List<GameObject> loggers = new List<GameObject>();
    public List<GameObject> inactiveLoggers = new List<GameObject>();

    [SerializeField] private GameObject loggersPrefab;

    public Transform hallCheckpoint;
    public Transform warehouseCheckPoint;

    [SerializeField] private LayerMask treeLayer;

    private float startRadius = 100f;
    private float maxRadius = 600f;
    private float radiusIncr = 100f;
    private float currentRadius;


    void Start()
    {
        hallCheckpoint = GameObject.Find("hallCheckpoint").transform;
        warehouseCheckPoint = GameObject.Find("wareHouseCheckpoint").transform;

        currentRadius = startRadius;

        AddLoggers(starterLoggerCount);

    }

    //Ajout loggers (en fin)
    public void AddLoggers(int n) //Améliorer(idem pour StoneQuarry) pour par ex ajouter 2 bûcheron, si on souhaite en ajouter 5 et qu'on ne peut en ajouter que 2
    {
        if(((loggersCount + n) <= maxLoggerCount) && (hallCheckpoint != null)) 
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(hallCheckpoint.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                //Add n loggers
                for (int i = 0; i < n; i++)
                {
                    ResourcesManager resourcesManagerScript = ResourcesManager.instance;

                    if (resourcesManagerScript.totalPopulation < resourcesManagerScript.populationMax) // Inutile, retirer après travail en cours (idem pour StoneQuarry)
                    {
                        GameObject newLogger = Instantiate(loggersPrefab, hit.position, Quaternion.identity);
                        newLogger.GetComponent<LoggerData>().workBuilding = this.gameObject.transform;
                        loggers.Add(newLogger);
                        loggersCount++;
                        ResourcesManager.instance.AddToTotalPop(1);
                    }

                }

            }

        }
    
    }

    //Suppression loggers (en fin)
    public void RemoveLoggers(int n) 
    {
        if(loggers.Count >= n)
        {
            //Remove n loggers
            for (int i = 0; i < n; i++)
            {
                LoggerData curLoggerScript = loggers[loggers.Count - 1].GetComponent<LoggerData>();

                //Change l'état du bûcheron (aller au hallCheckpoint)
                curLoggerScript.transform.GetComponent<Animator>().SetBool("isFired", true);

                //Retirer stock et arbre cible(+ maj isTargeted arbre cible) --> bâtiment de travail retiré dans script State ReturnHall
                curLoggerScript.stock = 0;

                if ((curLoggerScript.targetTree != null) && curLoggerScript.targetTree.GetComponent<HealthTree>().isTargeted)
                {
                    curLoggerScript.targetTree.GetComponent<HealthTree>().isTargeted = false;
                }
                curLoggerScript.targetTree = null;

                //Suppression de l'unité dans liste des inactifs (si elle s'y trouve)
                if (inactiveLoggers.Contains(curLoggerScript.gameObject))
                {
                    inactiveLoggers.Remove(curLoggerScript.gameObject);
                }

                //Suppression de l'unité dans liste loggers + MAJ nombre loggers
                loggers.RemoveAt(loggers.Count - 1);
                loggersCount--;

            }

        }

    }



    //Méthode pour donner cible au bûcheron si il n'en a pas (appellée via script d'état Idle du bûcheron)
    public Transform GiveNearestValidTree() 
    {
        Transform target = null;
        float distanceOfTarget = Mathf.Infinity;

        while((target == null) && (currentRadius <= maxRadius)) 
        {
            Collider[] trees = Physics.OverlapSphere(transform.position, currentRadius, treeLayer);
            Transform curTarget = null;

            foreach(Collider tree in trees) 
            { 
                Transform treeCoords = tree.transform;
                float curDistance = Vector3.Distance(transform.position, treeCoords.position);

                if (curDistance < distanceOfTarget && !treeCoords.GetComponent<HealthTree>().isTargeted) 
                {
                    curTarget = treeCoords;
                    distanceOfTarget = curDistance;
                }
            }
            target = curTarget;
            currentRadius += radiusIncr;
        }

        currentRadius = startRadius;
        return target;

    }

    
    private void OnDrawGizmos() 
    {
        //Portée de départ
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, startRadius);

        //Portée max
        Gizmos.color = UnityEngine.Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }




}
