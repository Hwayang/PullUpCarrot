using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void Update()
    {
        // 매 프레임, UserDataManager의 currentGold를 가져와서 표시
        goldText.text = UserData.Instance.currentGold.ToString();
    }
}