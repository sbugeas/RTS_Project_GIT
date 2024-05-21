using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingPlacement : MonoBehaviour
{
    public bool isBuildable = true;

    [SerializeField] private GameObject _child_one_house;
    [SerializeField] private GameObject _child_two_house;
    [SerializeField] private GameObject _child_three_house;

    //A Améliorer
    [SerializeField] private Material mat_child_one_house;
    [SerializeField] private Material mat_child_two_house;
    [SerializeField] private Material mat_child_three_house;

    [SerializeField]private int _collisionHit = 0;

    [SerializeField]private List<GameObject> checkersList = new List<GameObject>();
    [SerializeField] private List<GameObject> validCheckersList = new List<GameObject>();

    //A ajuster
    [SerializeField] private float _CheckerRadius = 0.3f;

    //A ajuster
    [SerializeField] private LayerMask _ground;
    [SerializeField] private bool checkersOk = false;
    private Camera _cam;


    private void Start()
    {
        _cam = UnitSelectionManager.instance.cam;
    }

    private void FixedUpdate()
    {
        if ((_collisionHit < 1) && checkersOk)
        {
            isBuildable = true;
        }
        else
        {
            isBuildable = false;
        }
    }

    private void Update()
    {
        checkersOk = CheckPlacement();

        //Améliorer : vérifier si l'objet n'a pas déjà le bon mat/bonne couleur
        if (isBuildable) 
        {
            _child_one_house.GetComponent<MeshRenderer>().material.SetColor("_Color", mat_child_one_house.color);
            _child_two_house.GetComponent<MeshRenderer>().material.SetColor("_Color", mat_child_two_house.color);
            _child_three_house.GetComponent<MeshRenderer>().material.SetColor("_Color", mat_child_three_house.color);
        }
        else 
        {
            _child_one_house.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            _child_two_house.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            _child_three_house.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        //Peut être pas utile de vérifier les enfants
        if ((go != this.gameObject) && (go != _child_one_house) && (go != _child_two_house) && (go != _child_three_house)) 
        {
            _collisionHit++;
        } 
        
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject go = other.gameObject;

        if ((go != this.gameObject) && (go != _child_one_house) && (go != _child_two_house) && (go != _child_three_house))
        {
            _collisionHit--;
        }
    }

    private bool CheckPlacement() 
    {
        bool res = false;
        int checkerOkNb = 0;

        for(int i = 0; i < checkersList.Count; i++) 
        {
            Vector3 _center = checkersList[i].transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(_center, _CheckerRadius, _ground);
            bool isUnderGround = VerifUnderGround(checkersList[i]);

            if (hitColliders.Length > 0 || isUnderGround)
            {
                checkerOkNb++;
            }
        }

        //Les 4 doivent être OK
        if (checkerOkNb == 4) 
        {
            res = true;
        }

        return res;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkersList[0].transform.position, _CheckerRadius);
        Gizmos.DrawWireSphere(checkersList[1].transform.position, _CheckerRadius);
        Gizmos.DrawWireSphere(checkersList[2].transform.position, _CheckerRadius);
        Gizmos.DrawWireSphere(checkersList[3].transform.position, _CheckerRadius);
    }

    private bool VerifUnderGround(GameObject _go)
    {
        RaycastHit hit;

        //maxDistance à aujuster / Draw le raycast pour tests
        if (Physics.Raycast(_go.transform.position, Vector3.up, out hit, 1.5f, _ground))
        {
            return true;
        }
        else 
        {
            return false;
        }

    }

}
