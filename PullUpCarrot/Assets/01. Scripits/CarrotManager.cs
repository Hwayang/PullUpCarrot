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
    private float speed;

    uint carrotCount = 0;
    bool alreadySummoned = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // 새로 만든 오브젝트를 변수에 담는다
        GameObject newCarrot = Instantiate(carrotTop, new Vector2(0, 0), Quaternion.identity);

        // 그 인스턴스를 리스트에 추가
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
                currentPos.y += speed;
                carrot.transform.position = currentPos;
            }
        }

        if (carrotSenser.isCarrotSummon && carrotCount <= maxCarrotCount)
        {
            if (carrotCount == maxCarrotCount)
            {
                // 오브젝트 생성
                GameObject newCarrot = Instantiate(carrotBottom);
                // 리스트에 인스턴스 추가
                carrotList.Add(newCarrot);
            }
            else
            {
                GameObject newCarrot = Instantiate(carrotMid);
                carrotList.Add(newCarrot);
                carrotCount++;
            }

            carrotSenser.isCarrotSummon = false;
        }

        if (carrotSenser.isCarrotDelete)
        {
            // 예: 가장 먼저 들어온 (인덱스 0) 당근부터 제거한다고 가정
            GameObject target = carrotList[0];

            // 씬에서도 제거
            Destroy(target);

            // 리스트에서도 제거
            carrotList.RemoveAt(0);

            carrotSenser.isCarrotDelete = false;
        }
    }
}
