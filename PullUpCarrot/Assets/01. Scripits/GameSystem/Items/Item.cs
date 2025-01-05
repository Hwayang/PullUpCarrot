using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item", order = 0)]
public class Item : ScriptableObject
{
    public bool canUse { get; set; } = false;

    public string itemName;           // 아이템 이름
    public uint cost;                 // 구매 가격
    public string unlockCondition;    // 해금 조건
    public int conditionVal;          // 해금 조건을 위한 필요 값
    public string itemAction;         // 아이템 능력
    public int actionVal;             // 능력의 값
}