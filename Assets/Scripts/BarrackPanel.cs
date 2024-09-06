using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ---> En cours / Sera utilis� dans prochaine maj (commit)

public class BarrackPanel : MonoBehaviour
{
    private Barrack currentBarrack;
    
    //DONN�ES DU PANEL

    // TEXTES
    [SerializeField] TextMeshProUGUI recruitedUnitNameTxt;
    [SerializeField] TextMeshProUGUI soldierCostLabel;
    [SerializeField] TextMeshProUGUI soldierCostVal;

    // BUTTONS
    [SerializeField] Button recruitSoldierButton; //A connecter lors de la selection(dans "DisplayPanel")

    // SLIDER
    public Slider recruitmentSlider;


    public void DisplayPanel(Barrack selectedBarrack)
    {
        currentBarrack = selectedBarrack;
        //R�cup�ration infos du b�timent
        UpdatePanel();
        //Afficher panel
        gameObject.SetActive(true);

    }

    public void HidePanel() //A appeler avant de d�selectionner (UnitSelectionManager)
    {
        StopRecruitmentUpdate();
        //Cacher panel
        gameObject.SetActive(false);
    }

    private void UpdatePanel()
    {
        //MAJ AUTRE INFOS (A FAIRE)

        //Update button
        recruitSoldierButton.onClick.RemoveAllListeners();
        recruitSoldierButton.onClick.AddListener(OnRecruitSoldierClick);
        UpdateRecruitmentButton();


        //V�rifie si caserne en cours de recrutement
        if (currentBarrack.isRecruiting)
        {
            LaunchRecruitmentUpdate();
        }
        else
        {
            ////Cacher slider
            recruitmentSlider.enabled = false;
        }
    }

    public void LaunchRecruitmentUpdate()
    {
        //Afficher nom de l'unit�
        recruitedUnitNameTxt.enabled = true;

        //Afficher slider
        recruitmentSlider.enabled = true;

        //start MAJ slider
        StartCoroutine(UpdateRecruitmentProgress());

        //hide cost
        soldierCostLabel.enabled = false;
        soldierCostVal.enabled = false;
    }

    public void StopRecruitmentUpdate() //� appeler quand recrutement termin�(depuis "Barrack")
    {
        //stop MAJ slider
        StopCoroutine("UpdateRecruitmentProgress");
        currentBarrack = null;
        //Cache slider
        recruitmentSlider.enabled = false;
        //Cacher nom de l'unit�
        recruitedUnitNameTxt.enabled = false;
        //display cost
        soldierCostLabel.enabled = true;
        soldierCostVal.enabled = true;
    }

    private IEnumerator UpdateRecruitmentProgress()
    {
        while (currentBarrack != null && currentBarrack.isRecruiting)
        {
            recruitmentSlider.value = currentBarrack.GetRecruitmentProgress();
            yield return new WaitForSeconds(1f);  // Mise � jour toutes les secondes
        }
    }

    public void UpdateRecruitmentButton()
    {
        ResourcesManager resourcesManager = ResourcesManager.instance;
        bool isRecruitmentPossible = (resourcesManager.totalPopulation < resourcesManager.populationMax) &&
        (resourcesManager.goldBarCount >= resourcesManager.soldierPrefab.GetComponent<Unit>().costToRecruit);

        recruitSoldierButton.interactable = isRecruitmentPossible;
    }

    //Appel� lors d'un clique sur bouton recrutement
    void OnRecruitSoldierClick() 
    {
        if (currentBarrack != null)
        {
            currentBarrack.RecruitSoldier();
        }
    }
}
