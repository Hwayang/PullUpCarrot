using UnityEngine;
using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    CarrotManager carrotManager;

    public static ItemManager Instance;

    // 효과 식별 키 -> 실행될 델리게이트(메서드) 맵핑
    private Dictionary<string, Action<float>> effectTable;
    private Dictionary<string, Action<float, Item>> unlockTable;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Dictionary 초기화
        effectTable = new Dictionary<string, Action<float>>
        {
            // 아이템 이펙트 효과 등록
            { "Add", Effect_Add },
            { "PullPowerUp", Effect_PullPowerUp},
            { "CrackPowerDown", Effect_CrackPowerDown}
        };

        unlockTable = new Dictionary<string, Action<float, Item>>
        {
            // 언락 이펙트 효과 등록
            { "Buy", Unlock_Buy },
            { "Length", Unlock_Length}
        };
        
    }

#region 아이템 이펙트 메서드
    private void Effect_Add(float addCount)
    {
        carrotManager.maxCarrotCount += (uint)addCount;
    }

    private void Effect_PullPowerUp(float powerVal)
    {
        carrotManager.minTotalForce += powerVal;
        carrotManager.maxTotalForce += powerVal;
    }

    private void Effect_CrackPowerDown(float powerVal)
    {
        carrotManager.crackForce -= powerVal;
    }

    #endregion

#region 해금 조건 메서드

    private void Unlock_Buy(float goldCount, Item targetItem)
    {
        if(gameManager.currentGold >= goldCount)
        {
            targetItem.canUse = true;
        }
    }

    private void Unlock_Length(float length, Item targetItem)
    {
        if(gameManager.maxCarrotLength >= length)
        {
            targetItem.canUse = true; 
        }
    }

#endregion

    // 외부에서 이 함수를 호출해 효과를 발동
    public void InvokeItemEffect(Item itemData)
    {
        if (effectTable.TryGetValue(itemData.itemAction, out var action))
        {
            action.Invoke(itemData.actionVal);

            Debug.Log("아이템 효과 사용");
        }
        else
        {
            Debug.LogWarning($"[ItemEffectManager] 효과 키 '{itemData.itemAction}'를 찾을 수 없습니다.");
        }
    }

    public void InvokeUnlockEffect(Item itemData)
    {
        if(unlockTable.TryGetValue(itemData.unlockCondition, out var action))
        {
            action.Invoke(itemData.conditionVal, itemData);
        }
        else
        {
            Debug.LogWarning($"[ItemEffectManager] 효과 키 '{itemData.unlockCondition}'를 찾을 수 없습니다.");
        }

    }
}
