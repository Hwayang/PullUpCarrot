using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    CarrotManager carrotManager;

    [SerializeField]
    private float gameOverVal;

    [SerializeField]
    private uint goldCorVal;

    private bool isGameOver = false;

    public enum GameState
    {
        ready,
        execution,
        end
    }

    public GameState currentState { get; set; }

    public static GameManager Instance;

    public float currentGold { get; set; }
    public float maxCarrotLength { get; set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        EnterReadyState();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.ready:
                carrotManager.enabled = false;
                break;

            case GameState.execution:
                carrotManager.enabled = true;
                break;

            case GameState.end:
                carrotManager.enabled = false;
                break;
        }

        if(Input.GetMouseButtonDown(0))
        {
            StartExecution();
        }    

        //if (carrotManager.crackForce > gameOverVal)
        //{
        //    EndGame();
        //}

        Debug.Log("GameOveris : " + isGameOver);
    }

    private void EnterReadyState()
    {
        currentState = GameState.ready;
        // ��ٿ� �������� ����ϴ� �ܰ� UI ǥ��, 
        // �ʿ��� �ʱ�ȭ ��
        Debug.Log("Preparation State: ������ ��� UI On");
    }

    // (2) ���� �ܰ�� ��ȯ
    public void StartExecution()
    {
        currentState = GameState.execution;
        // ��� �̱� ���� �ʱ�ȭ
        Debug.Log("Execution State: ��� �̱� ����");
    }

    // (3) ���� �ܰ�� ��ȯ
    public void EndGame()
    {
        currentState = GameState.end;

        currentGold = carrotManager.carrotCount; /// goldCorVal;
        // ���� ��� ����/������ Ȯ�� �� ��� ���
        Debug.Log("End State: ���� ��� �� ��� ǥ��");
    }
}
