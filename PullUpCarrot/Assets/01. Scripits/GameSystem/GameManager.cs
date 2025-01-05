using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    CarrotManager carrotManager;

    [SerializeField]
    private float gameOverVal;

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
    }
}
