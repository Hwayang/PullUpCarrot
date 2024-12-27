using UnityEngine;

public class CarrotSenser : MonoBehaviour
{
    public bool isCarrotSummon { get; set; } = false;
    public bool isCarrotDelete { get; set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCarrotSummon = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isCarrotDelete = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
