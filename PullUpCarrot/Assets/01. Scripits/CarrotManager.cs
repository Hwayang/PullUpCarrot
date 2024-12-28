using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.LightTransport;
using System.Collections;

public class CarrotManager : MonoBehaviour
{
#region public Value
    [Header("기본 객체 설정")]
    [SerializeField]
    CarrotSenser carrotSenser;

    [SerializeField]
    GameObject carrotTop;

    [SerializeField]
    GameObject carrotMid;

    [SerializeField]
    GameObject carrotBottom;


    [Header("당근에 적용될 수치")]

    [SerializeField]//당근의 최초 생성 지점
    Vector2 spawnPoint;

    [SerializeField] //최대 생성 가능한 당근의 개수 (길이)
    public uint maxCarrotCount;

    [SerializeField] //당근을 뽑는 힘의 보정값
    [UnityEngine.Range(0, 9)]
    private float forceCorrectValue;

    [SerializeField] //당근이 내려가는 속도
    [UnityEngine.Range(0, 0.05f)]
    private float downSpeed;
#endregion


#region region Value
    //생성된 당근을 담는 리스트
    List<GameObject> carrotList = new List<GameObject>();

    //생성된 당근의 개수
    uint carrotCount = 0;

    //당기는 힘의 양
    private float pullForce;

    //아직 화면에 닿아있는지 여부
    private bool isDrag = false;

    //최초 제스처 시작 좌표
    Vector2 startMousePos;

    //마지막 제스처 마무리 좌표
    Vector2 endMousePos;
#endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 새로 만든 오브젝트를 변수에 담는다
        GameObject newCarrot = Instantiate(carrotTop, new Vector2(0, 0), Quaternion.identity);

        // 그 인스턴스를 리스트에 추가
        carrotList.Add(newCarrot);
        carrotCount++;

        StartCoroutine(SummonCarrot());
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector2 screenPos = Input.mousePosition;

        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    // Update is called once per frame
    void Update()
    {
        //최초 화면을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = GetMouseWorldPosition();

            isDrag = true;
        }
        //화면을 누른 채 이동하고 있을 때
        else if (Input.GetMouseButton(0))
        {
            
        }
        //화면에서 손을 땠을 때
        else if (Input.GetMouseButtonUp(0))
        {
            endMousePos = GetMouseWorldPosition();

            pullForce = (endMousePos.y - startMousePos.y) * forceCorrectValue;
            
            isDrag = false;

            Debug.Log("startMousePos" + startMousePos);
            Debug.Log("endMousePos" + endMousePos);
            Debug.Log("pullForce" + pullForce);

            StartCoroutine(pullingCarrot(pullForce));
        }

        //화면을 홀드중이 아니면 당근이 지속적으로 내려간다.
        if(!isDrag)
        {
            foreach (var carrot in carrotList)
            {
                Vector2 curPos = carrot.gameObject.transform.position;

                curPos.y -= downSpeed;

                carrot.transform.position = curPos;
            }
        }

        /*
        if (Input.GetMouseButtonDown(0))
        {

            // Raycast로 클릭된 오브젝트 확인
            RaycastHit2D hit = Physics2D.Raycast(carrotSenser.transform.position, Vector2.down, 10, LayerMask.GetMask("Object"));

            if (hit.collider)
            {
                // 만약 클릭 대상이 당근 리스트에 있는 객체라면 드래그 시작
                if (carrotList.Contains(hit.collider.gameObject))
                {
                    draggedCarrot = hit.collider.gameObject;
                    isMouseDrag = true;
                    
                    // 드래그된 당근의 “드래그 시작 위치”
                    draggedCarrotStartPos = draggedCarrot.transform.position;

                    // 모든 당근의 현재 위치를 기록
                    initialPositions.Clear();
                    foreach (var carrot in carrotList)
                    {
                        initialPositions.Add(carrot.transform.position);
                    }

                    // 마우스와 당근 사이의 offset
                    Vector2 mousePos = GetMouseWorldPosition();
                    dragOffset = draggedCarrotStartPos - mousePos;
                }
            }
        }
        // 2) 드래그 중
        else if (Input.GetMouseButton(0) && isMouseDrag && draggedCarrot != null)
        {
            Vector2 mousePos = GetMouseWorldPosition();

            // draggedCarrot의 새 위치
            Vector2 newCarrotPos = mousePos + dragOffset;

            // 당근이 얼마나 이동했는지 (Delta)
            Vector2 delta = newCarrotPos - draggedCarrotStartPos;

            // 모든 당근 이동
            for (int i = 0; i < carrotList.Count; i++)
            {
                Vector2 targetPos = initialPositions[i] + delta;

                targetPos.x = 0;

                // initialPositions[i]에서 delta만큼 이동
                carrotList[i].transform.position = targetPos;
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
        }*/

    }

    IEnumerator pullingCarrot(float force)
    {
        float tempforce = force / 10;

        foreach (var carrot in carrotList)
        {
            Vector2 curCarrotPos = carrot.gameObject.transform.position;
            curCarrotPos.y += tempforce;

            carrot.gameObject.transform.position = curCarrotPos;
        }

        tempforce *= 0.5f;

        yield return null;

        if(tempforce > force)
        {
            yield break;
        }
    }

    IEnumerator SummonCarrot()
    {
        if(carrotCount >= maxCarrotCount)
        {
            yield break;
        }

        while(true)
        {
            // 만약 센서가 소환 신호를 줬고, carrotCount <= maxCarrotCount 등 조건이 맞을 때만
            if (carrotSenser.isCarrotSummon && carrotCount <= maxCarrotCount)
            {
                if (carrotCount == maxCarrotCount)
                {
                    GameObject newCarrot = Instantiate(carrotBottom);
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

            // 다음 프레임까지 대기 (또는 일정 초 대기)
            yield return null;
        }
    }
}
