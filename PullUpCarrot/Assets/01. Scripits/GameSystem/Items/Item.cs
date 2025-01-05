using System;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;           // 아이템 이름
    public uint cost;                 // 구매 가격
    public string unlockCondition;    // 해금 조건 (ex: "ClearStage", "HaveGold" 등)
    public int conditionVal;          // 해금 조건을 위한 필요 값

    public string itemAction;
}