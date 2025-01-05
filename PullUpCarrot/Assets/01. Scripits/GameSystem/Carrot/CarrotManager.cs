using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.LightTransport;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;

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
    [UnityEngine.Range(0, 1.5f)]
    public float distCorrectValue;

    [SerializeField] //당근을 뽑기 위해 홀드하는 시간의 보정값
    [UnityEngine.Range(0, 1)]
    public float holdCorrectValue;

    [SerializeField] //당근이 내려가는 속도
    [UnityEngine.Range(0, 0.05f)]
    public float downSpeed;

    [SerializeField] //당근에 가해지는 힘의 그래프의 기울기
    [UnityEngine.Range(2, 3)]
    private float exponentVal;

    [SerializeField]
    public float maxTotalForce;

    [SerializeField]
    public float minTotalForce;

    [SerializeField]
    private SerializableDictionary<float, float> crackRange;


    #endregion


    #region region Value
    //생성된 당근을 담는 리스트
    List<GameObject> carrotList = new List<GameObject>();

    //생성된 당근의 개수
    uint carrotCount = 0;

    //당기는 힘의 양
    private float pullForce;

    //당근에 적용될 크랙의 양
    public float crackForce { get; set; }

    //아직 화면에 닿아있는지 여부
    private bool isDrag = false;

    //당근이 올라가고 있는지 여부
    private bool isUp = false;

    //최초 제스처 시작 좌표
    Vector2 startMousePos;

    //마지막 제스처 마무리 좌표
    Vector2 endMousePos;

    private float holdTime = 0f;    // 홀드한 시간

    private float maxHoldTime = 2f; // 최대 홀드 시간(스트레치 제한
    #endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 새로 만든 오브젝트를 변수에 담는다
        GameObject newCarrot = Instantiate(carrotTop, new Vector2(0, -spawnCorVal), Quaternion.identity);

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
        else if(Input.GetMouseButton(0) && isDrag)
        {
            holdTime += Time.deltaTime; // 프레임마다 경과 시간 추가
            holdTime = Mathf.Clamp(holdTime, 0, maxHoldTime); // 최대 홀드 시간 제한

            foreach (GameObject carrotObj in carrotList)
            {
                Carrot carrot = carrotObj.GetComponent<Carrot>();
                carrot.ApplySquashStretch(holdTime / maxHoldTime , carrotObj); // 실시간으로 스쿼시 & 스트레치
            }
        }
        //화면에서 손을 땠을 때
        else if (Input.GetMouseButtonUp(0))
        {
            endMousePos = GetMouseWorldPosition();

            holdTime = 0;

            //우상향을 그리는 지수함수꼴
            //모바일 환경 특징상 위아래 이동에 한계가 있으므로 dist의 보정값을 hold보다 좀 더 높게 설정
            pullForce = Mathf.Clamp((holdTime / holdCorrectValue) + Mathf.Pow(((endMousePos.y - startMousePos.y) * distCorrectValue), exponentVal), minTotalForce, maxTotalForce);
            
            //crackForce = (holdTime / holdCorrectVal) + (mouseMoveDist) * forceCorrectVal
            crackForce += JudgeCrack(pullForce);

            Debug.Log(crackForce);

            isDrag = false;
            isUp = true;

            //Debug.Log("startMousePos" + startMousePos);
            //Debug.Log("endMousePos" + endMousePos);

            float mousePos = endMousePos.y - startMousePos.y;

            Debug.Log("MousePos" + mousePos);
            Debug.Log("pullForce" + pullForce);

            foreach (GameObject carrotObj in carrotList)
            {
                Carrot carrot = carrotObj.GetComponent<Carrot>();
                carrot.ResetSquashStretch(carrotObj); // 당근에 스쿼시 & 스트레치 적용
            }

            StartCoroutine(pullingCarrot(pullForce));
        }

        //화면을 홀드중이 아니면 당근이 지속적으로 내려간다.
        if(!isDrag || !isUp)
        {
            foreach (var carrot in carrotList)
            {
                Vector2 curPos = carrot.gameObject.transform.position;

                curPos.y -= downSpeed;

                carrot.transform.position = curPos;
            }
        }

    }

    private float JudgeCrack(float pullforce)
    {
        float crackVal = 0;

        foreach (var crack in crackRange.ToDictionary())
        {
            crackVal = (crack.Key < pullforce) ? pullforce : crack.Value;

            if(pullforce != crackVal){ break; }
        }

        return crackVal;
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

        tempforce *= 4f;

        yield return new WaitForSeconds(0.4f);

        if(tempforce > force)
        {
            isUp = false;
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
                    Vector2 targetLoc = new Vector2(0, carrotList.Last().transform.position.y - spawnCorVal / 2);
                 
                    GameObject newCarrot = Instantiate(carrotBottom);
                    carrotList.Add(newCarrot);

                    carrotList.Last().transform.position = targetLoc;

                    carrotCount++;
                }
                else
                {
                    Vector2 targetLoc = new Vector2(0, carrotList.Last().transform.position.y - spawnCorVal / 2);

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
