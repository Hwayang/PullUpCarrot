using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void Update()
    {
        // �� ������, UserDataManager�� currentGold�� �����ͼ� ǥ��
        goldText.text = UserData.Instance.currentGold.ToString();
    }
}