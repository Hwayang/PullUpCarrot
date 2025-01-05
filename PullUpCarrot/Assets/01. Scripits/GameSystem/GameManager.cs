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
        // 당근에 아이템을 사용하는 단계 UI 표시, 
        // 필요한 초기화 등
        Debug.Log("Preparation State: 아이템 사용 UI On");
    }

    // (2) 실행 단계로 전환
    public void StartExecution()
    {
        currentState = GameState.execution;
        // 당근 뽑기 로직 초기화
        Debug.Log("Execution State: 당근 뽑기 시작");
    }

    // (3) 종료 단계로 전환
    public void EndGame()
    {
        currentState = GameState.end;

        currentGold = carrotManager.carrotCount; /// goldCorVal;
        // 뽑은 당근 길이/내구도 확인 → 골드 계산
        Debug.Log("End State: 보상 계산 및 결과 표시");
    }
}
