using UnityEngine;

public class Carrot : MonoBehaviour
{
    [SerializeField]
    [Range(0, 0.05f)]
    private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPos = transform.position;

        if(Input.GetMouseButton(0))
        {
            currentPos.y += speed;
        }

        this.transform.position = currentPos;
    }
}
