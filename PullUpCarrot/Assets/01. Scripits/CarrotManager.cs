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
    Vector2 spawnPoint;

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
        //ȭ���� ���� ä �̵��ϰ� ���� ��
        else if (Input.GetMouseButton(0))
        {
            
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

            StartCoroutine(pullingCarrot(pullForce));
        }

        //ȭ���� Ȧ������ �ƴϸ� ����� ���������� ��������.
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

            // Raycast�� Ŭ���� ������Ʈ Ȯ��
            RaycastHit2D hit = Physics2D.Raycast(carrotSenser.transform.position, Vector2.down, 10, LayerMask.GetMask("Object"));

            if (hit.collider)
            {
                // ���� Ŭ�� ����� ��� ����Ʈ�� �ִ� ��ü��� �巡�� ����
                if (carrotList.Contains(hit.collider.gameObject))
                {
                    draggedCarrot = hit.collider.gameObject;
                    isMouseDrag = true;
                    
                    // �巡�׵� ����� ���巡�� ���� ��ġ��
                    draggedCarrotStartPos = draggedCarrot.transform.position;

                    // ��� ����� ���� ��ġ�� ���
                    initialPositions.Clear();
                    foreach (var carrot in carrotList)
                    {
                        initialPositions.Add(carrot.transform.position);
                    }

                    // ���콺�� ��� ������ offset
                    Vector2 mousePos = GetMouseWorldPosition();
                    dragOffset = draggedCarrotStartPos - mousePos;
                }
            }
        }
        // 2) �巡�� ��
        else if (Input.GetMouseButton(0) && isMouseDrag && draggedCarrot != null)
        {
            Vector2 mousePos = GetMouseWorldPosition();

            // draggedCarrot�� �� ��ġ
            Vector2 newCarrotPos = mousePos + dragOffset;

            // ����� �󸶳� �̵��ߴ��� (Delta)
            Vector2 delta = newCarrotPos - draggedCarrotStartPos;

            // ��� ��� �̵�
            for (int i = 0; i < carrotList.Count; i++)
            {
                Vector2 targetPos = initialPositions[i] + delta;

                targetPos.x = 0;

                // initialPositions[i]���� delta��ŭ �̵�
                carrotList[i].transform.position = targetPos;
            }
        }
        // 3) ���콺�� ���� ��
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
            // ���� ������ ��ȯ ��ȣ�� ���, carrotCount <= maxCarrotCount �� ������ ���� ����
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

            // ���� �����ӱ��� ��� (�Ǵ� ���� �� ���)
            yield return null;
        }
    }
}
