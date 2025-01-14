using UnityEngine;

public class CarrotSenser : MonoBehaviour
{
    public bool isCarrotSummon { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            isCarrotSummon = true;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isCarrotSummon = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
