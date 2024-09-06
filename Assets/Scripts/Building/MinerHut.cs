using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

//Script activé suite à construction (lorsque achevée)
public class MinerHut : MonoBehaviour
{
    public int minerCount = 0;
    public int maxMiners = 4;
    public int startMinerCount = 2;

    public List<GameObject> miners = new List<GameObject>();
    public List<GameObject> inactiveMiners = new List<GameObject>();

    [SerializeField] private GameObject minerPrefab;

    public Transform hallCheckpoint;
    public Transform warehouseCheckPoint;

    [SerializeField] private LayerMask resourceLayer;

    private float startRadius = 100f;
    private float maxRadius = 600f;
    private float radiusIncr = 100f;


    void Start()
    {
        hallCheckpoint = GameObject.Find("hallCheckpoint").transform;
        warehouseCheckPoint = GameObject.Find("wareHouseCheckpoint").transform;

        AddMiner(startMinerCount);

    }

    //Add n miners (from end) 
    public void AddMiner(int n)
    {
        if (((minerCount + n) <= maxMiners) && (hallCheckpoint != null))
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
                        newMiner.GetComponent<MinerData>().workBuilding = this.gameObject.transform;
                        miners.Add(newMiner);
                        minerCount++;
                        ResourcesManager.instance.AddToTotalPop(1);
                    }

                }

            }

        }

    }

    ////Remove n miners (from end)
    public void RemoveMiner(int n)
    {
        if (miners.Count >= n)
        {
            for (int i = 0; i < n; i++)
            {
                MinerData curMinerScript = miners[miners.Count - 1].GetComponent<MinerData>();

                //Change l'état du mineur (aller au hallCheckpoint)
                curMinerScript.transform.GetComponent<Animator>().SetBool("isFired", true);

                //Retirer stock et ressource cible --> bâtiment de travail retiré dans script State ReturnHall
                curMinerScript.stock = 0;
                curMinerScript.targetResource = null;
                
                //Suppression du mineur dans liste des inactifs (si elle s'y trouve)
                if (inactiveMiners.Contains(curMinerScript.gameObject))
                {
                    inactiveMiners.Remove(curMinerScript.gameObject);
                }

                //Suppression de l'unité dans liste miners + MAJ nombre loggers
                miners.RemoveAt(miners.Count - 1);
                minerCount--;

            }
        }
    }


    //Méthode pour donner cible au mineur si il n'en a pas (appellée via son script d'état Idle)
    public Transform GiveNearestValidResource()
    {
        float curRadius = startRadius;

        Transform target = null;
        float distanceOfTarget = Mathf.Infinity;

        while ((target == null) && (curRadius <= maxRadius))
        {
            Collider[] resources = Physics.OverlapSphere(transform.position, curRadius, resourceLayer);

            foreach (Collider _resource in resources)
            {
                Transform resourceCoords = _resource.transform;
                float curDistance = Vector3.Distance(transform.position, resourceCoords.position);

                if (curDistance < distanceOfTarget)
                {
                    target = resourceCoords;
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
        Gizmos.DrawWireSphere(transform.position, 100f);
    }




}
