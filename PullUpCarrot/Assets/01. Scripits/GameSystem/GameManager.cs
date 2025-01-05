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

    public GameState currentState;

    public float currentGold { get; set; }
    public float maxCarrotLength { get; set; }

    void Awake()
    {
        EnterReadyState();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.ready:
                // �غ� �ܰ� ���� �ݺ� (�ʿ��ϴٸ�)
                break;

            case GameState.execution:
                // ���� �ܰ� ���� (Input üũ ��)
                break;

            case GameState.end:
                // ����(���) �ܰ� ����
                break;
        }

        if (carrotManager.crackForce > gameOverVal)
        {
            EndGame();
        }

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

        currentGold = carrotManager.carrotCount / goldCorVal;
        // ���� ��� ����/������ Ȯ�� �� ��� ���
        Debug.Log("End State: ���� ��� �� ��� ǥ��");
    }
}
