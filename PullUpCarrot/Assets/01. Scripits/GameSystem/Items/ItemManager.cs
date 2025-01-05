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

    // ȿ�� �ĺ� Ű -> ����� ��������Ʈ(�޼���) ����
    private Dictionary<string, Action<float>> effectTable;
    private Dictionary<string, Action<float, Item>> unlockTable;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Dictionary �ʱ�ȭ
        effectTable = new Dictionary<string, Action<float>>
        {
            // ������ ����Ʈ ȿ�� ���
            { "Add", Effect_Add },
            { "PullPowerUp", Effect_PullPowerUp},
            { "CrackPowerDown", Effect_CrackPowerDown}
        };

        unlockTable = new Dictionary<string, Action<float, Item>>
        {
            // ��� ����Ʈ ȿ�� ���
            { "Buy", Unlock_Buy },
            { "Length", Unlock_Length}
        };
        
    }

#region ������ ����Ʈ �޼���
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

#region �ر� ���� �޼���

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

    // �ܺο��� �� �Լ��� ȣ���� ȿ���� �ߵ�
    public void InvokeItemEffect(Item itemData)
    {
        if (effectTable.TryGetValue(itemData.itemAction, out var action))
        {
            action.Invoke(itemData.actionVal);

            Debug.Log("������ ȿ�� ���");
        }
        else
        {
            Debug.LogWarning($"[ItemEffectManager] ȿ�� Ű '{itemData.itemAction}'�� ã�� �� �����ϴ�.");
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
            Debug.LogWarning($"[ItemEffectManager] ȿ�� Ű '{itemData.unlockCondition}'�� ã�� �� �����ϴ�.");
        }

    }
}
