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
    [Header("�⺻ ��ü ����")]
    [SerializeField]
    CarrotSenser carrotSenser;

    [SerializeField]
    GameObject carrotTop;

    [SerializeField]
    GameObject carrotMid;

    [SerializeField]
    GameObject carrotBottom;


    [Header("��ٿ� ����� ��ġ")]

    [SerializeField]//����� ���� ���� ����
    [UnityEngine.Range(0, 10)]
    float spawnCorVal;

    [SerializeField] //�ִ� ���� ������ ����� ���� (����)
    public uint maxCarrotCount;

    [SerializeField] //����� �̴� ���� ������
    [UnityEngine.Range(0, 9)]
    private float forceCorrectValue;

    [SerializeField] //����� �������� �ӵ�
    [UnityEngine.Range(0, 0.05f)]
    private float downSpeed;
#endregion


#region region Value
    //������ ����� ��� ����Ʈ
    List<GameObject> carrotList = new List<GameObject>();

    //������ ����� ����
    uint carrotCount = 0;

    //���� ���� ��
    private float pullForce;

    //���� ȭ�鿡 ����ִ��� ����
    private bool isDrag = false;

    //����� �ö󰡰� �ִ��� ����
    private bool isUp = false;

    //���� ����ó ���� ��ǥ
    Vector2 startMousePos;

    //������ ����ó ������ ��ǥ
    Vector2 endMousePos;
#endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ���� ���� ������Ʈ�� ������ ��´�
        GameObject newCarrot = Instantiate(carrotTop, new Vector2(0, 0), Quaternion.identity);

        // �� �ν��Ͻ��� ����Ʈ�� �߰�
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
        //���� ȭ���� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = GetMouseWorldPosition();

            isDrag = true;
        }
        //ȭ�鿡�� ���� ���� ��
        else if (Input.GetMouseButtonUp(0))
        {
            endMousePos = GetMouseWorldPosition();

            pullForce = (endMousePos.y - startMousePos.y) * forceCorrectValue;
            
            isDrag = false;

            Debug.Log("startMousePos" + startMousePos);
            Debug.Log("endMousePos" + endMousePos);
            Debug.Log("pullForce" + pullForce);

            isUp = true;
            StartCoroutine(pullingCarrot(pullForce));
        }

        //ȭ���� Ȧ������ �ƴϸ� ����� ���������� ��������.
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
            // ���� ������ ��ȯ ��ȣ�� ���, carrotCount <= maxCarrotCount �� ������ ���� ����
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

            // ���� �����ӱ��� ��� (�Ǵ� ���� �� ���)
            yield return null;
        }
    }
}
