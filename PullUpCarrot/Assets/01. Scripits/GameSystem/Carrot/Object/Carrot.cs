using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Carrot : MonoBehaviour
{
    public bool isSatrt = false;

    private void OnMouseDown()
    {
        // 만약 클릭 위치가 UI 오브젝트 위라면, 당근 클릭 로직을 무시
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("무시");
            return;
        }

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
