using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.LightTransport;

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

    // 드래그 시작 시, 드래그된 당근(GameObject)
    private GameObject draggedCarrot = null;
    
    // 드래그 시작 시점에 “각 당근의 위치”를 기록해둘 리스트
    private List<Vector3> initialPositions = new List<Vector3>();

    private Vector3 draggedCarrotStartPos;

    uint carrotCount = 0;

    //마우스 드래그 여부
    private bool isMouseDrag = false;

    // 드래그된 당근의 마우스 클릭 offset
    private Vector3 dragOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 새로 만든 오브젝트를 변수에 담는다
        GameObject newCarrot = Instantiate(carrotTop, new Vector2(0, 0), Quaternion.identity);

        // 그 인스턴스를 리스트에 추가
        carrotList.Add(newCarrot);

        carrotCount++;
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector2 screenPos = Input.mousePosition;

        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            float maxDistance = 10;

            Debug.DrawRay(carrotSenser.transform.position, Vector2.down * maxDistance, Color.red);

            // Raycast로 클릭된 오브젝트 확인
            RaycastHit2D hit = Physics2D.Raycast(carrotSenser.transform.position, Vector2.down, 10, LayerMask.GetMask("Object"));

            if (hit.collider)
            {
                // 만약 클릭 대상이 당근 리스트에 있는 객체라면 드래그 시작
                if (carrotList.Contains(hit.collider.gameObject))
                {
                    draggedCarrot = hit.collider.gameObject;
                    isMouseDrag = true;

                    // 모든 당근의 현재 위치를 기록
                    initialPositions.Clear();
                    foreach (var carrot in carrotList)
                    {
                        initialPositions.Add(carrot.transform.position);
                    }

                    // 드래그된 당근의 “드래그 시작 위치”
                    draggedCarrotStartPos = draggedCarrot.transform.position;

                    // 마우스와 당근 사이의 offset
                    Vector3 mousePos = GetMouseWorldPosition();
                    dragOffset = draggedCarrotStartPos - mousePos;
                }
            }
        }
        // 2) 드래그 중
        else if (Input.GetMouseButton(0) && isMouseDrag && draggedCarrot != null)
        {
            Vector3 mousePos = GetMouseWorldPosition();

            // draggedCarrot의 새 위치
            Vector3 newCarrotPos = mousePos + dragOffset;

            // 당근이 얼마나 이동했는지 (Delta)
            Vector3 delta = newCarrotPos - draggedCarrotStartPos;

            // 모든 당근 이동
            for (int i = 0; i < carrotList.Count; i++)
            {
                // initialPositions[i]에서 delta만큼 이동
                carrotList[i].transform.position = initialPositions[i] + delta;
            }
        }
        // 3) 마우스를 뗐을 때
        else if (Input.GetMouseButtonUp(0) && isMouseDrag)
        {
            isMouseDrag = false;
            draggedCarrot = null;
        }
        else
        {
            foreach(var carrot in carrotList)
            {
                Vector2 curPos = carrot.gameObject.transform.position;

                curPos.y -= downSpeed;

                carrot.transform.position = curPos;
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
