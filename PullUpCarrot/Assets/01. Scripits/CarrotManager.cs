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

    // �巡�� ���� ��, �巡�׵� ���(GameObject)
    private GameObject draggedCarrot = null;
    
    // �巡�� ���� ������ ���� ����� ��ġ���� ����ص� ����Ʈ
    private List<Vector3> initialPositions = new List<Vector3>();

    private Vector3 draggedCarrotStartPos;

    uint carrotCount = 0;

    //���콺 �巡�� ����
    private bool isMouseDrag = false;

    // �巡�׵� ����� ���콺 Ŭ�� offset
    private Vector3 dragOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ���� ���� ������Ʈ�� ������ ��´�
        GameObject newCarrot = Instantiate(carrotTop, new Vector2(0, 0), Quaternion.identity);

        // �� �ν��Ͻ��� ����Ʈ�� �߰�
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

            // Raycast�� Ŭ���� ������Ʈ Ȯ��
            RaycastHit2D hit = Physics2D.Raycast(carrotSenser.transform.position, Vector2.down, 10, LayerMask.GetMask("Object"));

            if (hit.collider)
            {
                // ���� Ŭ�� ����� ��� ����Ʈ�� �ִ� ��ü��� �巡�� ����
                if (carrotList.Contains(hit.collider.gameObject))
                {
                    draggedCarrot = hit.collider.gameObject;
                    isMouseDrag = true;

                    // ��� ����� ���� ��ġ�� ���
                    initialPositions.Clear();
                    foreach (var carrot in carrotList)
                    {
                        initialPositions.Add(carrot.transform.position);
                    }

                    // �巡�׵� ����� ���巡�� ���� ��ġ��
                    draggedCarrotStartPos = draggedCarrot.transform.position;

                    // ���콺�� ��� ������ offset
                    Vector3 mousePos = GetMouseWorldPosition();
                    dragOffset = draggedCarrotStartPos - mousePos;
                }
            }
        }
        // 2) �巡�� ��
        else if (Input.GetMouseButton(0) && isMouseDrag && draggedCarrot != null)
        {
            Vector3 mousePos = GetMouseWorldPosition();

            // draggedCarrot�� �� ��ġ
            Vector3 newCarrotPos = mousePos + dragOffset;

            // ����� �󸶳� �̵��ߴ��� (Delta)
            Vector3 delta = newCarrotPos - draggedCarrotStartPos;

            // ��� ��� �̵�
            for (int i = 0; i < carrotList.Count; i++)
            {
                // initialPositions[i]���� delta��ŭ �̵�
                carrotList[i].transform.position = initialPositions[i] + delta;
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
