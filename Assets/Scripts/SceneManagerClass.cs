using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerClass : MonoBehaviour
{
   public void StratScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void StartSceneFunc()
    {
        StratScene("MainScene");
    }
    public void TitleStart()
    {
        //���� �ϰ� 

        Invoke("StartSceneFunc", 0.5f);

    }
}
