using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

//Script activé suite à construction (lorsque achevée)
public class LoggerCamp : MonoBehaviour
{
    public int loggersCount = 0;
    public int maxLoggerCount = 3;
    public int starterLoggerCount = 0;

    public List<GameObject> loggers = new List<GameObject>();
    public List<GameObject> inactiveLoggers = new List<GameObject>();

    [SerializeField] private GameObject loggersPrefab;

    public Transform hallCheckpoint;
    public Transform warehouseCheckPoint;
    //public Transform campCheckpoint;

    [SerializeField] private LayerMask treeLayer;

    private float startRadius = 25f;
    private float maxRadius = 75f;
    private float radiusIncr = 25f;



    void Start()
    {
        AddLoggers(starterLoggerCount); //Modifier param si besoin

    }

    public void AddLoggers(int n) 
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(hallCheckpoint.position, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            //Pour ne pas dépasser le max
            if ((n + loggers.Count) > maxLoggerCount)
            {
                n = maxLoggerCount - loggers.Count;
            }
            //Add n loggers
            for (int i = 0; i < n; i++)
            {
                if(ResourcesManager.instance.inactivePopulationCount > 0) 
                {
                    GameObject newLogger = Instantiate(loggersPrefab, hit.position, Quaternion.identity);
                    newLogger.GetComponent<PeasantData>().workBuilding = this.gameObject.transform;
                    loggers.Add(newLogger);
                    loggersCount++;
                    ResourcesManager.instance.RemToInactivePop(1);
                }

            }

        }
    }

    public void RemoveLoggers(int n) 
    {
        //Pour ne pas dépasser loggers.Count
        if (n > loggers.Count) 
        {
            n = loggers.Count;
        }
        //Remove n loggers
        for (int i = 0; i < n; i++)
        {
            PeasantData curLoggerScript = loggers[i].GetComponent<PeasantData>();

            //Penser à changer état du bûcheron via Trigger (aller au hallCheckpoint)

            //Retirer stock, bâtiment de travail et arbre cible(+ maj isTargeted arbre cible)
            curLoggerScript.workBuilding = null;
            curLoggerScript.stock = 0;

            if ((curLoggerScript.targetTree != null) && curLoggerScript.targetTree.GetComponent<HealthTree>().isTargeted) 
            {
                curLoggerScript.targetTree.GetComponent<HealthTree>().isTargeted = false;
            }
            curLoggerScript.targetTree = null;

            //Suppression liste loggers et maj nombre loggers
            loggers.RemoveAt(i);
            loggersCount--;
        }
    }



    //Méthode pour donner cible au bûcheron si il n'en a pas (appellée via script d'état Idle du bûcheron)
    public Transform GiveNearestValidTree() 
    {
        float curRadius = startRadius;

        Transform target = null;
        float distanceOfTarget = Mathf.Infinity;

        while((target == null) && (curRadius <= maxRadius)) 
        {
            Collider[] trees = Physics.OverlapSphere(transform.position, curRadius, treeLayer);

            foreach(Collider tree in trees) 
            { 
                Transform treeCoords = tree.transform;
                float curDistance = Vector3.Distance(transform.position, treeCoords.position);

                if (curDistance < distanceOfTarget && !treeCoords.GetComponent<HealthTree>().isTargeted) 
                {
                    target = treeCoords;
                    distanceOfTarget = curDistance;
                }
            }

            curRadius += radiusIncr;
        }

        return target;

    }

    
    private void OnDrawGizmos() 
    {   //startRadius : 25f / maxRadius : 75f / radiusIncr = 25f
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 25f);
    }




}
