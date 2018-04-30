using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AbilitySelectManager : MonoBehaviour
{
    public GameObject AbilitySelectPref;
    public GameObject AbilityGrid;
    public GameObject ASprefab;
    public GameObject descriptionPanel;
    public GameObject powerText;
    public GameObject typeText;
    public GameObject cooldownText;
    public GameObject sampleImage;
    public GameObject ability1;
    public GameObject ability2;
    public GameObject ability3;
    public GameObject ability4;
    public Color highlightColor;
    public Color normalColor;
    public Color physicalColor;
    public Color magicalColor;
    public Color utilityColor;
    public GameObject AbilityTab;
    public GameObject abilityTabText;

    private int ASslot = 0;
    private int currentASnum = 0;
    private int totalAbiliyUnlocks = 0;
    private int maxAbilities = 0;
    private int prevHighlightIndex = -1;
    private Ability currentAbility;

    // Use this for initialization
    void Start ()
    {
        totalAbiliyUnlocks = GameController.controller.totalAbilities;
        //totalAbiliyUnlocks = 15;
        maxAbilities = GameController.controller.TOTAL_ABILITIES;

        //REMOVE THIS SHIT
        //for(int f = 0; f < 8; ++f)
        //    GameController.controller.unlockedAbilities[f] = true;
        //REMOVE THIS LATER

        //GameController.controller.playerAbility1 = AbilityToolsScript.tools.LookUpAbility("none");
        //GameController.controller.playerAbility2 = AbilityToolsScript.tools.LookUpAbility("none");
        //GameController.controller.playerAbility3 = AbilityToolsScript.tools.LookUpAbility("none");
        //GameController.controller.playerAbility4 = AbilityToolsScript.tools.LookUpAbility("none");

        LoadInitialIcons();
    }

    public void OpenAbilityTab()
    {
        AbilityTab.SetActive(true);
    }

    public void CloseAbilityTab()
    {
        AbilityTab.SetActive(false);
    }

    public void LoadAbilitiesPanel(int ASnum)
    {
        print("HERE!");
        if (ASslot != ASnum)
        {
            if(ASslot != 0)
            {
                for (int k = 0; k < AbilityGrid.transform.childCount; ++k)
                {
                    Destroy(AbilityGrid.transform.GetChild(k).gameObject);
                }
            }

            ASslot = ASnum;
            int popUps = 0;
            int lastIndex = 1;

            // set all of the needed ability panels
            for (int i = 0; i < totalAbiliyUnlocks; ++i)
            {
                for (int j = lastIndex; j < maxAbilities; ++j)
                {
                    if (GameController.controller.unlockedAbilities[j] == true)
                    {
                        Ability curAbility = AbilityToolsScript.tools.IndexToAbilityLookUp(j);
                        GameObject selectClone = (GameObject)Instantiate(ASprefab, Vector3.zero, transform.rotation);
                        selectClone.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { AbilityOptionSelected(selectClone); });
                        selectClone.transform.SetParent(AbilityGrid.transform);
                        selectClone.transform.localScale = new Vector3(1, 1, 1);
                        selectClone.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = curAbility.Name;
                        selectClone.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Resources.Load(curAbility.Icon, typeof(Sprite)) as Sprite;
                        ++popUps;
                        lastIndex = j;
                    }
                }
            }
            popUps = 0;
        }

        switch(ASnum)
        {
            case 1:
                abilityTabText.GetComponent<Text>().text = "Select Ability 1";
                break;
            case 2:
                abilityTabText.GetComponent<Text>().text = "Select Ability 2";
                break;
            case 3:
                abilityTabText.GetComponent<Text>().text = "Select Ability 3";
                break;
            case 4:
                abilityTabText.GetComponent<Text>().text = "Select Ability 4";
                break;
        }
        
        AbilitySelectPref.SetActive(true);
    }

    public void ClosePopUp()
    {
        prevHighlightIndex = -1;
        AbilitySelectPref.SetActive(false);
    }

    public void AbilityOptionSelected(GameObject button)
    {
        int index = button.transform.GetSiblingIndex();
        currentASnum = index;


        if (prevHighlightIndex != -1)
            AbilityGrid.transform.GetChild(prevHighlightIndex).GetChild(0).GetComponent<Image>().color = normalColor;

        currentAbility = AbilityToolsScript.tools.LookUpAbility(AbilityGrid.transform.GetChild(index).GetChild(0).GetChild(0).GetComponent<Text>().text);
        AbilityGrid.transform.GetChild(index).GetChild(0).GetComponent<Image>().color = highlightColor;
        prevHighlightIndex = index;
        sampleImage.GetComponent<Image>().sprite = button.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite;
        powerText.GetComponent<Text>().text = currentAbility.BaseDamage.ToString();
        descriptionPanel.GetComponent<Text>().text = currentAbility.Description;
        cooldownText.GetComponent<Text>().text = currentAbility.Cooldown.ToString();
        setType(currentAbility.Type);
    }

    public void ConfirmAbilitySelect()
    {
        switch(ASslot)
        {
            case 1:
                if (currentAbility.Name == GameController.controller.playerAbility2.Name) // 1==2
                {
                    GameController.controller.playerAbility2 = GameController.controller.playerAbility1;
                    setIcon(2);
                }
                else if (currentAbility.Name == GameController.controller.playerAbility3.Name) // 1==3
                {
                    GameController.controller.playerAbility3 = GameController.controller.playerAbility1;
                    setIcon(3);
                }
                else if (currentAbility.Name == GameController.controller.playerAbility4.Name) // 1==4
                {
                    GameController.controller.playerAbility4 = GameController.controller.playerAbility1;
                    setIcon(4);
                }

                GameController.controller.playerAbility1 = currentAbility;
                break;
            case 2:
                if (currentAbility.Name == GameController.controller.playerAbility1.Name) // 2==1
                {
                    GameController.controller.playerAbility1 = GameController.controller.playerAbility2;
                    setIcon(1);
                }
                else if (currentAbility.Name == GameController.controller.playerAbility3.Name) // 2==3
                {
                    GameController.controller.playerAbility3 = GameController.controller.playerAbility2;
                    setIcon(3);
                }
                else if (currentAbility.Name == GameController.controller.playerAbility4.Name) // 2==4
                {
                    GameController.controller.playerAbility4 = GameController.controller.playerAbility2;
                    setIcon(4);
                }

                GameController.controller.playerAbility2 = currentAbility;
                break;
            case 3:
                if (currentAbility.Name == GameController.controller.playerAbility1.Name) // 3==1
                {
                    GameController.controller.playerAbility1 = GameController.controller.playerAbility3;
                    setIcon(1);
                }
                else if (currentAbility.Name == GameController.controller.playerAbility2.Name) // 3==2
                {
                    GameController.controller.playerAbility2 = GameController.controller.playerAbility3;
                    setIcon(2);
                }
                else if (currentAbility.Name == GameController.controller.playerAbility4.Name) // 3==4
                {
                    GameController.controller.playerAbility4 = GameController.controller.playerAbility3;
                    setIcon(4);
                }

                GameController.controller.playerAbility3 = currentAbility;
                break;
            case 4:
                if (currentAbility.Name == GameController.controller.playerAbility1.Name) // 4==1
                {
                    GameController.controller.playerAbility1 = GameController.controller.playerAbility4;
                    setIcon(1);
                }
                else if (currentAbility.Name == GameController.controller.playerAbility2.Name) // 4==2
                {
                    GameController.controller.playerAbility2 = GameController.controller.playerAbility4;
                    setIcon(2);
                }
                else if (currentAbility.Name == GameController.controller.playerAbility3.Name) // 4==3
                {
                    GameController.controller.playerAbility3 = GameController.controller.playerAbility4;
                    setIcon(3);
                }

                GameController.controller.playerAbility4 = currentAbility;
                break;
        }

        setIcon(ASslot);
        AbilitySelectPref.SetActive(false);
    }

    public void setIcon(int abilityNum)
    {
        switch(abilityNum)
        {
            case 1:
                ability1.GetComponent<Image>().sprite = Resources.Load(GameController.controller.playerAbility1.Icon, typeof(Sprite)) as Sprite;
                break;
            case 2:
                ability2.GetComponent<Image>().sprite = Resources.Load(GameController.controller.playerAbility2.Icon, typeof(Sprite)) as Sprite;
                break;
            case 3:
                ability3.GetComponent<Image>().sprite = Resources.Load(GameController.controller.playerAbility3.Icon, typeof(Sprite)) as Sprite;
                break;
            case 4:
                ability4.GetComponent<Image>().sprite = Resources.Load(GameController.controller.playerAbility4.Icon, typeof(Sprite)) as Sprite;
                break;
        }
    }

    public void setType(AbilityType type)
    {
        switch(type)
        {
            case AbilityType.Physical:
                typeText.GetComponent<Text>().color = physicalColor;
                typeText.GetComponent<Text>().text = "Physical";
                break;
            case AbilityType.Magical:
                typeText.GetComponent<Text>().color = magicalColor;
                typeText.GetComponent<Text>().text = "Magical";
                break;
            case AbilityType.Utility:
                typeText.GetComponent<Text>().color = utilityColor;
                typeText.GetComponent<Text>().text = "Utility";
                break;
        }
    }

    public void LoadInitialIcons()
    {
        for (int i = 1; i < 5; ++i)
            setIcon(i);
    }
}
