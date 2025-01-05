using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    CarrotManager carrotManager;

    [SerializeField]
    private float gameOverVal;

    [SerializeField]
    private Item item;

    [SerializeField]
    private ItemManager itemManager;

    private bool isGameOver = false;

    public float currentGold { get; set; }
    public float maxCarrotLength { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(carrotManager.crackForce > gameOverVal)
        {
            isGameOver = true;
        }

        Debug.Log("GameOveris : " + isGameOver);

        if(Input.GetKeyDown(KeyCode.V))
        {
            itemManager.InvokeItemEffect(item);

            Debug.Log("V ´­¸²");
            Debug.Log("CarrotCount" + carrotManager.maxCarrotCount);
        }
    }
}
