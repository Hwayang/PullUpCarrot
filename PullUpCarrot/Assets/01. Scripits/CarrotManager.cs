using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.LightTransport;
using System.Collections;
using System.Linq;

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
    [UnityEngine.Range(0, 10)]
    float spawnCorVal;

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
                    Vector2 targetLoc = new Vector2(0, carrotList.Last().transform.position.y - spawnCorVal);
                 
                    GameObject newCarrot = Instantiate(carrotBottom);
                    carrotList.Add(newCarrot);

                    carrotList.Last().transform.position = targetLoc;

                    carrotCount++;
                }
                else
                {
                    Vector2 targetLoc = new Vector2(0, carrotList.Last().transform.position.y - spawnCorVal);

                    GameObject newCarrot = Instantiate(carrotMid);
                    carrotList.Add(newCarrot);

                    carrotList.Last().transform.position = targetLoc;

                    carrotCount++;
                }

                carrotSenser.isCarrotSummon = false;
            }

            // 다음 프레임까지 대기 (또는 일정 초 대기)
            yield return null;
        }
    }
}
