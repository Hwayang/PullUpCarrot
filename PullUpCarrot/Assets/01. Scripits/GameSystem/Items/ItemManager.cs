using UnityEngine;
using System;
using System.Collections.Generic;

public class ItemEffectManager : MonoBehaviour
{
    public static ItemEffectManager Instance;

    // 효과 식별 키 -> 실행될 델리게이트(메서드) 맵핑
    private Dictionary<string, Action> effectTable;
    private Dictionary<string, Action> unlockTable;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Dictionary 초기화
        effectTable = new Dictionary<string, Action>();
        unlockTable = new Dictionary<string, Action>();

        // 아이템 이펙트 효과 등록
        {
            effectTable["Add"] = Effect_Add;
            effectTable["PullPowerUp"] = Effect_PullPowerUp;
            effectTable["CrackPowerDown"] = Effect_CrackPowerDown;
        }

        // 언락 이펙트 효과 등록
        {
            unlockTable["Buy"] = Unlock_Buy;
            unlockTable["Length"] = Unlock_Length;
        }
        
    }

#region 아이템 이펙트 메서드
    private void Effect_Add()
    {
        Debug.Log("[Heal] 체력을 회복했습니다!");
        // ...
    }

    private void Effect_PullPowerUp()
    {
        Debug.Log("[SwordAttack] 검 공격!");
        // ...
    }

    private void Effect_CrackPowerDown()
    {
        Debug.Log("[BowShot] 활 공격!");
        // ...
    }

    #endregion

#region 해금 조건 메서드

    private void Unlock_Buy()
    {
        
    }

    private void Unlock_Length()
    {

    }

#endregion

    // 외부에서 이 함수를 호출해 효과를 발동
    public void InvokeEffect(string effectKey)
    {
        if (effectTable.ContainsKey(effectKey))
        {
            effectTable[effectKey]?.Invoke();
        }
        else
        {
            Debug.LogWarning($"[ItemEffectManager] Effect key '{effectKey}' not found!");
        }
    }
}
