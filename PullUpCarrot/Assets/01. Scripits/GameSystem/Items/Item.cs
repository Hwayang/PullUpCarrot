using System;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;           // ������ �̸�
    public uint cost;                 // ���� ����
    public string unlockCondition;    // �ر� ���� (ex: "ClearStage", "HaveGold" ��)
    public int conditionVal;          // �ر� ������ ���� �ʿ� ��

    public string itemAction;
}