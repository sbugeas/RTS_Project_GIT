using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ---> En cours / Sera utilisé dans prochaine maj (commit)

public class BarrackPanel : MonoBehaviour
{
    private Barrack currentBarrack;
    
    //DONNÉES DU PANEL

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
        //Récupération infos du bâtiment
        UpdatePanel();
        //Afficher panel
        gameObject.SetActive(true);

    }

    public void HidePanel() //A appeler avant de déselectionner (UnitSelectionManager)
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


        //Vérifie si caserne en cours de recrutement
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
        //Afficher nom de l'unité
        recruitedUnitNameTxt.enabled = true;

        //Afficher slider
        recruitmentSlider.enabled = true;

        //start MAJ slider
        StartCoroutine(UpdateRecruitmentProgress());

        //hide cost
        soldierCostLabel.enabled = false;
        soldierCostVal.enabled = false;
    }

    public void StopRecruitmentUpdate() //à appeler quand recrutement terminé(depuis "Barrack")
    {
        //stop MAJ slider
        StopCoroutine("UpdateRecruitmentProgress");
        currentBarrack = null;
        //Cache slider
        recruitmentSlider.enabled = false;
        //Cacher nom de l'unité
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
            yield return new WaitForSeconds(1f);  // Mise à jour toutes les secondes
        }
    }

    public void UpdateRecruitmentButton()
    {
        ResourcesManager resourcesManager = ResourcesManager.instance;
        bool isRecruitmentPossible = (resourcesManager.totalPopulation < resourcesManager.populationMax) &&
        (resourcesManager.goldBarCount >= resourcesManager.soldierPrefab.GetComponent<Unit>().costToRecruit);

        recruitSoldierButton.interactable = isRecruitmentPossible;
    }

    //Appelé lors d'un clique sur bouton recrutement
    void OnRecruitSoldierClick() 
    {
        if (currentBarrack != null)
        {
            currentBarrack.RecruitSoldier();
        }
    }
}
