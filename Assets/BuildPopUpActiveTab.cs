using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPopUpActiveTab : MonoBehaviour
{
    public GameObject ActiveTab;
    public GameObject ActiveTabContent;
    public List<GameObject> NotActiveTabContent;
    public void SetActiveTab()
    {
        ActiveTab.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+1, gameObject.transform.position.z);
        ActiveTabContent.SetActive(true);
        NotActiveTabContent[0].SetActive(false);
        NotActiveTabContent[1].SetActive(false);
        NotActiveTabContent[2].SetActive(false);


    }
}
