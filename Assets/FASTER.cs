using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FASTER : MonoBehaviour
{
    int imgIndex = 0;
   public List<Sprite> imgList = new List<Sprite>(); 
    public void ChangeTimeX()
    {
        imgIndex++;
        if (imgIndex > 2) imgIndex -= 3;
        gameObject.GetComponent<Image>().sprite = imgList[imgIndex];
        //�ð���� ���� �Լ�
        Time.timeScale = imgIndex + 1;
    }
}
