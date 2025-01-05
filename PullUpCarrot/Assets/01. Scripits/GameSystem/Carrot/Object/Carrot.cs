using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Carrot : MonoBehaviour
{
    public bool isSatrt = false;

    private void OnMouseDown()
    {
        GameManager.Instance.currentState = GameManager.GameState.execution;
    }

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
}
