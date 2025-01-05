using UnityEngine;
using System;
using System.Collections.Generic;

public class ItemEffectManager : MonoBehaviour
{
    public static ItemEffectManager Instance;

    // ȿ�� �ĺ� Ű -> ����� ��������Ʈ(�޼���) ����
    private Dictionary<string, Action> effectTable;
    private Dictionary<string, Action> unlockTable;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Dictionary �ʱ�ȭ
        effectTable = new Dictionary<string, Action>();
        unlockTable = new Dictionary<string, Action>();

        // ������ ����Ʈ ȿ�� ���
        {
            effectTable["Add"] = Effect_Add;
            effectTable["PullPowerUp"] = Effect_PullPowerUp;
            effectTable["CrackPowerDown"] = Effect_CrackPowerDown;
        }

        // ��� ����Ʈ ȿ�� ���
        {
            unlockTable["Buy"] = Unlock_Buy;
            unlockTable["Length"] = Unlock_Length;
        }
        
    }

#region ������ ����Ʈ �޼���
    private void Effect_Add()
    {
        Debug.Log("[Heal] ü���� ȸ���߽��ϴ�!");
        // ...
    }

    private void Effect_PullPowerUp()
    {
        Debug.Log("[SwordAttack] �� ����!");
        // ...
    }

    private void Effect_CrackPowerDown()
    {
        Debug.Log("[BowShot] Ȱ ����!");
        // ...
    }

    #endregion

#region �ر� ���� �޼���

    private void Unlock_Buy()
    {
        
    }

    private void Unlock_Length()
    {

    }

#endregion

    // �ܺο��� �� �Լ��� ȣ���� ȿ���� �ߵ�
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
