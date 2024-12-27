using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CarrotManager : MonoBehaviour
{
    [SerializeField]
    Vector2 spawnPoint;

    [SerializeField]
    GameObject carrotTop;

    [SerializeField]
    GameObject carrotMid;

    [SerializeField]
    GameObject carrotBottom;

    List<Carrot> carrotList = new List<Carrot>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
