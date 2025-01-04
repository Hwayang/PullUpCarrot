using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Carrot : MonoBehaviour
{
    public void ApplySquashStretch(float stretchRatio, GameObject targetCarrot)
    {
        //Debug.Log("stratch");
        float stretchFactor;

        if (targetCarrot.CompareTag("CarrotTop") || targetCarrot.CompareTag("CarrotBottom"))
        {
            stretchFactor = Mathf.Lerp(0.363f, 0.443f, stretchRatio);
            targetCarrot.transform.DOScale(new Vector2(stretchFactor, 0.44f), 0.2f);
        }
        else
        {
            stretchFactor = Mathf.Lerp(0.36f, 0.44f, stretchRatio);
            targetCarrot.transform.DOScale(new Vector2(stretchFactor, 0.36f), 0.2f);
        }
    }

    public void ResetSquashStretch(GameObject targetCarrot)
    {

        if (targetCarrot.CompareTag("CarrotTop") || targetCarrot.CompareTag("CarrotBottom"))
        {
            targetCarrot.transform.DOScale(new Vector2(0.36f, 0.36f), 1).SetEase(Ease.OutElastic);
        }
        else
        {
            targetCarrot.transform.DOScale(new Vector2(0.36f, 0.36f), 1).SetEase(Ease.OutElastic);
        }


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {

    }
}
