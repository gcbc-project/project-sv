using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBuildPopUp : MonoBehaviour
{
    public GameObject PopUpPrefab;
    public void SetActiveFalse()
    {

    PopUpPrefab.SetActive(false);
    }

}
