using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

//Script activé suite à construction (lorsque achevée)
public class StoneMinerHut : MonoBehaviour
{
    public int stoneMinerCount = 0;
    public int maxStoneMiner = 4;
    public int starterMinerCount = 2;

    public List<GameObject> stoneMiners = new List<GameObject>();
    public List<GameObject> inactiveStoneMiners = new List<GameObject>();

    [SerializeField] private GameObject minerPrefab;

    public Transform hallCheckpoint;
    public Transform warehouseCheckPoint;

    [SerializeField] private LayerMask rockLayer;

    private float startRadius = 25f;
    private float maxRadius = 75f;
    private float radiusIncr = 25f;


    void Start()
    {
        hallCheckpoint = GameObject.Find("hallCheckpoint").transform;
        warehouseCheckPoint = GameObject.Find("wareHouseCheckpoint").transform;

        AddStoneMiners(starterMinerCount);

    }

    //Ajout mineur (en fin) 
    public void AddStoneMiners(int n)
    {
        if (((stoneMinerCount + n) <= maxStoneMiner) && (hallCheckpoint != null))
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(hallCheckpoint.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                //Add n miners
                for (int i = 0; i < n; i++)
                {
                    ResourcesManager resourcesManagerScript = ResourcesManager.instance;

                    if (resourcesManagerScript.totalPopulation < resourcesManagerScript.populationMax) 
                    {
                        GameObject newMiner = Instantiate(minerPrefab, hit.position, Quaternion.identity);
                        newMiner.GetComponent<StoneMinerData>().workBuilding = this.gameObject.transform;
                        stoneMiners.Add(newMiner);
                        stoneMinerCount++;
                        ResourcesManager.instance.AddToTotalPop(1);
                    }

                }

            }

        }

    }

    ////Remove n miners
    public void RemoveStoneMiners(int n)
    {
        if (stoneMiners.Count >= n)
        {
            for (int i = 0; i < n; i++)
            {
                StoneMinerData curStoneMinerScript = stoneMiners[stoneMiners.Count - 1].GetComponent<StoneMinerData>();

                //Change l'état du bûcheron (aller au hallCheckpoint)
                curStoneMinerScript.transform.GetComponent<Animator>().SetBool("isFired", true);

                //Retirer stock et rocher cible --> bâtiment de travail retiré dans script State ReturnHall
                curStoneMinerScript.stock = 0;
                curStoneMinerScript.targetRock = null;
                
                //Suppression de l'unité dans liste des inactifs (si elle s'y trouve)
                if (inactiveStoneMiners.Contains(curStoneMinerScript.gameObject))
                {
                    inactiveStoneMiners.Remove(curStoneMinerScript.gameObject);
                }

                //Suppression de l'unité dans liste loggers + MAJ nombre loggers
                stoneMiners.RemoveAt(stoneMiners.Count - 1);
                stoneMinerCount--;

            }
        }
    }


    //Méthode pour donner cible au mineur si il n'en a pas (appellée via son script d'état Idle)
    public Transform GiveNearestValidRock()
    {
        float curRadius = startRadius;

        Transform target = null;
        float distanceOfTarget = Mathf.Infinity;

        while ((target == null) && (curRadius <= maxRadius))
        {
            Collider[] rocks = Physics.OverlapSphere(transform.position, curRadius, rockLayer);

            foreach (Collider rock in rocks)
            {
                Transform rockCoords = rock.transform;
                float curDistance = Vector3.Distance(transform.position, rockCoords.position);

                if (curDistance < distanceOfTarget)
                {
                    target = rockCoords;
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
