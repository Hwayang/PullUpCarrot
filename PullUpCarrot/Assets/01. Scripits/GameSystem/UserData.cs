using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData Instance;
    public uint currentGold { get; set; }
    public uint maxCarrotLength { get; set; }

    [SerializeField]
    SerializableDictionary<Item, bool> currentUnlockItemList;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }
}
