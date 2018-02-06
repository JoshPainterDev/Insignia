using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUnlock_C : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Image>().enabled = false;
        this.transform.GetChild(0).GetComponent<Text>().enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        this.GetComponent<LerpScript>().LerpToScale(Vector3.zero, new Vector3(1, 1, 1), 2.0f);
        this.GetComponent<LerpScript>().LerpToColor(new Color(1, 1, 1, 0), Color.white, 2.0f);
        this.GetComponent<Image>().enabled = true;

        this.transform.GetChild(0).GetComponent<LerpScript>().LerpToScale(Vector3.zero, new Vector3(0.5f, 0.5f, 1), 2.0f);
        this.transform.GetChild(0).GetComponent<Text>().enabled = true;

        StartCoroutine(killMe());
    }

    IEnumerator killMe()
    {
        Color origColor = this.transform.GetChild(0).GetComponent<Text>().color;
        yield return new WaitForSeconds(3.5f);
        this.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), 2.0f);
        this.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(origColor, new Color(1, 1, 1, 0), 2.0f);
        yield return new WaitForSeconds(1.2f);
        Destroy(this);
    }
}
