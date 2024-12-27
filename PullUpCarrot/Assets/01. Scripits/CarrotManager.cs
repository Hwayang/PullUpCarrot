using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Mathematics;

public class CarrotManager : MonoBehaviour
{
    [SerializeField]
    Vector2 spawnPoint;

    [SerializeField]
    CarrotSenser carrotSenser;

    [SerializeField]
    GameObject carrotTop;

    [SerializeField]
    GameObject carrotMid;

    [SerializeField]
    GameObject carrotBottom;

    List<GameObject> carrotList = new List<GameObject>();

    [SerializeField]
    public uint maxCarrotCount;

    [SerializeField]
    [UnityEngine.Range(0, 0.05f)]
    private float upSpeed;

    [SerializeField]
    [UnityEngine.Range(0, 0.05f)]
    private float downSpeed;

    uint carrotCount = 0;
    bool alreadySummoned = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // ���� ���� ������Ʈ�� ������ ��´�
        GameObject newCarrot = Instantiate(carrotTop, new Vector2(0, 0), Quaternion.identity);

        // �� �ν��Ͻ��� ����Ʈ�� �߰�
        carrotList.Add(newCarrot);

        carrotCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            foreach (GameObject carrot in carrotList)
            {
                Vector2 currentPos = carrot.transform.position;
                currentPos.y += upSpeed;
                carrot.transform.position = currentPos;
            }
        }
        else
        {
            foreach (GameObject carrot in carrotList)
            {
                Vector2 currentPos = carrot.transform.position;
                currentPos.y -= downSpeed;
                carrot.transform.position = currentPos;
            }
        }

        if (carrotSenser.isCarrotSummon && carrotCount <= maxCarrotCount)
        {
            if (carrotCount == maxCarrotCount)
            {
                // ������Ʈ ����
                GameObject newCarrot = Instantiate(carrotBottom);
                // ����Ʈ�� �ν��Ͻ� �߰�
                carrotList.Add(newCarrot);
                carrotCount++;
            }
            else
            {
                GameObject newCarrot = Instantiate(carrotMid);
                carrotList.Add(newCarrot);
                carrotCount++;
            }

            carrotSenser.isCarrotSummon = false;
        }
    }
}
