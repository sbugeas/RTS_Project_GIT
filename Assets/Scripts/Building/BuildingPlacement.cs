using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingPlacement : MonoBehaviour
{
    public bool isBuildable = true;

    [SerializeField] private BuildingTmpCollider buildingTmpCol;

    private BuildingData _buildingData;

    private int _woodCost;
    private int _stoneCost;

    private Material[] materials;

    private Color[] originalColor;


    private void Start()
    {
        SaveOriginalColor();

        _buildingData = gameObject.GetComponent<BuildingData>();
        _woodCost = _buildingData.woodCost;
        _stoneCost = _buildingData.stoneCost;
    }

    private void FixedUpdate()
    {
        if ((buildingTmpCol.collisionHit < 1) && HasEnoughResources())
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
        if (isBuildable) 
        {
            SetYellowColor();
        }
        else 
        {
            SetRedColor();
        }
    }


    private void SetRedColor() 
    {
        //Pour chaque material
        for (int i = 0; i < materials.Length; i++) 
        {
            GetComponent<MeshRenderer>().materials[i].color = Color.red;
        }
    }

    private void SetYellowColor() ////
    {
        //Pour chaque material
        for (int i = 0; i < materials.Length; i++)
        {
            GetComponent<MeshRenderer>().materials[i].color = Color.green;
        }
    }

    public void SetOriginalColorAndRemoveScript() //// appelée lors du placement
    {
        //Pour chaque material
        for (int i = 0; i < materials.Length; i++)
        {
            //On applique leur couleur initiale
            GetComponent<MeshRenderer>().materials[i].color = originalColor[i];
        }

        //On retire ce script de l'objet 
        Destroy(this);
    }

    private void SaveOriginalColor() 
    {
        //Récupération des materials
        materials = GetComponent<MeshRenderer>().materials;
        originalColor = new Color[materials.Length];

        //Récupération couleur correspondante
        for (int i = 0; i < materials.Length; i++) 
        {
            originalColor[i] = materials[i].GetColor("_Color");
        }
    }

    private bool HasEnoughResources() 
    {
        return ((_woodCost <= ResourcesManager.instance.woodCound) && (_stoneCost <= ResourcesManager.instance.stoneCount));
    }
}
