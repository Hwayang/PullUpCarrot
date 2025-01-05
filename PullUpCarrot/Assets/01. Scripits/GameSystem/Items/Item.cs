using System;
using UnityEngine;

[System.Serializable]
public class Item
{
    public bool canUse { get; set; } = false;

    public string itemName;           // ������ �̸�
    public uint cost;                 // ���� ����
    public string unlockCondition;    // �ر� ����
    public int conditionVal;          // �ر� ������ ���� �ʿ� ��
    public string itemAction;         // ������ �ɷ�
    public int actionVal;             // �ɷ��� ��
}