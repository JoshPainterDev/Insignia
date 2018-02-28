using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestEventTrigger : EventTrigger
{

    public override void OnEndDrag(PointerEventData data)
    {
        GameObject settings =  GameObject.Find("SettingsManager");
        settings.GetComponent<SettingsManager>().PingNewVolumeSound();
    }
}
