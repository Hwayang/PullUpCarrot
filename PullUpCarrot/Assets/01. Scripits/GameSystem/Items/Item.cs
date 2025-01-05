using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item", order = 0)]
public class Item : ScriptableObject
{
    public bool canUse { get; set; } = false;

    public string itemName;           // ������ �̸�
    public uint cost;                 // ���� ����
    public string unlockCondition;    // �ر� ����
    public int conditionVal;          // �ر� ������ ���� �ʿ� ��
    public string itemAction;         // ������ �ɷ�
    public int actionVal;             // �ɷ��� ��
}