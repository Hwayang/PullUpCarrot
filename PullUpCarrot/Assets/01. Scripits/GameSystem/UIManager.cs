using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    [SerializeField]GameManager gameManager;

    [SerializeField] private GameObject readyUI;
    [SerializeField] private GameObject executionUI;
    [SerializeField] private GameObject endUI;


    // Update is called once per frame
    void Update()
    {
        switch(gameManager.currentState)
        {
            case GameManager.GameState.ready:
                {
                    readyUI.SetActive(true);
                    executionUI.SetActive(false);
                    endUI.SetActive(false);
                    break;
                }
            case GameManager.GameState.execution:
                {
                    readyUI.SetActive(false);
                    executionUI.SetActive(true);
                    endUI.SetActive(false);
                    break;
                }
            case GameManager.GameState.end:
                {
                    readyUI.SetActive(false);
                    executionUI.SetActive(false);
                    endUI.SetActive(true);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
