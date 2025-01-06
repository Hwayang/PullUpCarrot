using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private Item itemData;     // Inspector���� ���� �Ҵ��ϰų�, �ڵ�� ����
    [SerializeField] private TMP_Text itemNameText; // ��ư ���� ǥ���� �̸� TextMeshPro (���ٸ� UnityEngine.UI.Text ���)
    [SerializeField] private TMP_Text costText;     // ��ư ���� ǥ���� ����
    [SerializeField] private Button buyOrUseButton; // ���� ��ư ������Ʈ

    private void Start()
    {
        // ���� Inspector���� ItemData�� �̸� �Ҵ��ߴٸ�, ���⼭ UI �ʱ�ȭ
        //if (itemData != null)
        //{
        //    UpdateUI(itemData);
        //}

        // ��ư Ŭ�� �� OnClickItem �Լ��� ȣ���ϵ��� ���
        buyOrUseButton.onClick.AddListener(OnClickItem);
    }

    /// <summary>
    /// �ܺ�(ShopManager ��)���� �� �Լ��� ȣ���� ������ ������ ������ ���� ����
    /// </summary>
    public void SetItemData(Item data)
    {
        itemData = data;
        //UpdateUI(itemData);
    }

    private void UpdateUI(Item data)
    {
        itemNameText.text = data.itemName;
        costText.text = data.cost.ToString();
        // �ʿ��ϸ� ������, ����, ȿ���� � ǥ�� ����
    }

    /// <summary>
    /// ��ư Ŭ�� �� ����� ����
    /// </summary>
    private void OnClickItem()
    {
        if (itemData == null) return;

        // ���� 1) ������ ���� ���� (Gold ���� ��)
        if (UserData.Instance.currentGold >= itemData.cost)
        {
            UserData.Instance.currentGold -= itemData.cost;
            Debug.Log($"���� ����! ���� ���: {UserData.Instance.currentGold}");

            // ���� 2) ������ ȿ�� �ߵ�
            // ItemEffectManager ��� effectKey�� ���� ȿ�� ȣ��
            ItemManager.Instance.InvokeItemEffect(itemData);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }
}
