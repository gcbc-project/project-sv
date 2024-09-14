using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPopUpActiveTab : MonoBehaviour
{
    public GameObject ActiveTab;
    public void SetActiveTab()
    {
        ActiveTab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+1, gameObject.transform.position.z);
    
    }
}
