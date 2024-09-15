using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEntity : MonoBehaviour
{
    [SerializeField]
    Animator bodyAnimator;
    [SerializeField]
    Animator clothesAnimator;
    [SerializeField]
    Animator hairAnimator;

    public void SetOutfit(HumanOutfit humanOutfit)
    {
        bodyAnimator.SetInteger("Id", Mathf.Clamp(humanOutfit.Body, 0, GameData.Human_BodyCount - 1));
        clothesAnimator.SetInteger("Id", Mathf.Clamp(humanOutfit.Clothes, 0, GameData.Human_ClothesCount - 1));
        hairAnimator.SetInteger("Id", Mathf.Clamp(humanOutfit.Hair, 0, GameData.Human_HairCount - 1));
    }
}
