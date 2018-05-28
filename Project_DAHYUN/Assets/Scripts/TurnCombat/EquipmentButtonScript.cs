using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EquipmentButtonScript : MonoBehaviour {
    private GameObject menuManager;
    private Button myButton;

    private void Awake()
    {
        menuManager = GameObject.Find("CharacterMenuManager");
    }

    public void OnClicked(Button button)
    {
        int indexI = 0;
        int indexJ = 0;

        string a = button.name;
        string b = string.Empty;
        int val;

        for (int i = 0; i < a.Length; i++)
        {
            if (char.IsDigit(a[i]))
                b += a[i];
        }

        int.TryParse(b, out val);
        indexJ = val % 10;
        indexI = val / 10;

        //
        menuManager.GetComponent<Character_Menu_Manager>().HighlightEpqButton(this.gameObject, indexI, indexJ);
    }

    public void OnConfirmation(Button button)
    {
        myButton = button;
        menuManager.GetComponent<Character_Menu_Manager>().ConfirmSelection();
        Invoke("DestroyPanel", 0.2f);
    }

    private void DestroyPanel()
    {
        Destroy(myButton.transform.parent.parent.parent.parent.gameObject);
    }
}
