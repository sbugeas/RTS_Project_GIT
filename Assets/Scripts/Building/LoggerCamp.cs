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

    [SerializeField] private LayerMask treeLayer;

    private float startRadius = 25f;
    private float maxRadius = 75f;
    private float radiusIncr = 25f;


    void Start()
    {
        AddLoggers(starterLoggerCount);

    }

    public void AddLoggers(int n) 
    {
        if((loggersCount + n) <= maxLoggerCount) 
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(hallCheckpoint.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                //Add n loggers
                for (int i = 0; i < n; i++)
                {
                    if (ResourcesManager.instance.inactivePopulationCount > 0)
                    {
                        GameObject newLogger = Instantiate(loggersPrefab, hit.position, Quaternion.identity);
                        newLogger.GetComponent<LoggerData>().workBuilding = this.gameObject.transform;
                        loggers.Add(newLogger);
                        loggersCount++;
                        ResourcesManager.instance.RemToInactivePop(1);
                    }

                }

            }

        }
    
    }

    public void RemoveLoggers(int n) 
    {
        if(loggers.Count >= n)
        {
            //Remove n loggers
            for (int i = 0; i < n; i++)
            {
                LoggerData curLoggerScript = loggers[i].GetComponent<LoggerData>();

                //Change l'état du bûcheron (aller au hallCheckpoint)
                loggers[i].GetComponent<Animator>().SetBool("isFired", true);

                //Retirer stock et arbre cible(+ maj isTargeted arbre cible) --> bâtiment de travail retiré dans script State ReturnHall
                curLoggerScript.stock = 0;

                if ((curLoggerScript.targetTree != null) && curLoggerScript.targetTree.GetComponent<HealthTree>().isTargeted)
                {
                    curLoggerScript.targetTree.GetComponent<HealthTree>().isTargeted = false;
                }
                curLoggerScript.targetTree = null;

                //Suppression de l'unité dans liste loggers + MAJ nombre loggers
                loggers.RemoveAt(i);
                loggersCount--;
            }

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
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 25f);
    }




}
