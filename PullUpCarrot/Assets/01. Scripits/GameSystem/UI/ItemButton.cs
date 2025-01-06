using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private Item itemData;     // Inspector에서 직접 할당하거나, 코드로 설정
    [SerializeField] private TMP_Text itemNameText; // 버튼 위에 표시할 이름 TextMeshPro (없다면 UnityEngine.UI.Text 사용)
    [SerializeField] private TMP_Text costText;     // 버튼 위에 표시할 가격
    [SerializeField] private Button buyOrUseButton; // 실제 버튼 컴포넌트

    private void Start()
    {
        // 만약 Inspector에서 ItemData를 미리 할당했다면, 여기서 UI 초기화
        //if (itemData != null)
        //{
        //    UpdateUI(itemData);
        //}

        // 버튼 클릭 시 OnClickItem 함수를 호출하도록 등록
        buyOrUseButton.onClick.AddListener(OnClickItem);
    }

    /// <summary>
    /// 외부(ShopManager 등)에서 이 함수를 호출해 아이템 정보를 세팅할 수도 있음
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
        // 필요하면 아이콘, 설명, 효과값 등도 표시 가능
    }

    /// <summary>
    /// 버튼 클릭 시 실행될 로직
    /// </summary>
    private void OnClickItem()
    {
        if (itemData == null) return;

        // 예시 1) 아이템 구매 로직 (Gold 차감 등)
        if (UserData.Instance.currentGold >= itemData.cost)
        {
            UserData.Instance.currentGold -= itemData.cost;
            Debug.Log($"구매 성공! 남은 골드: {UserData.Instance.currentGold}");

            // 예시 2) 아이템 효과 발동
            // ItemEffectManager 등에서 effectKey를 통해 효과 호출
            ItemManager.Instance.InvokeItemEffect(itemData);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
}
