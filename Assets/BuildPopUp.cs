using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPopUp : MonoBehaviour
{
    public GameObject PopUpPrefab;
    public void SetActive()
    {
        PopUpPrefab.SetActive(true);

    }
}
