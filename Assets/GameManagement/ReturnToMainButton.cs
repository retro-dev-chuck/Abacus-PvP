using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMainButton : MonoBehaviour
{
    [SerializeField] private Button btn;
    private const string MainMenuSceneName = "MainMenu";

    private void OnEnable()
    {
        btn.onClick.AddListener(ReturnToMainMenu);
    }
    private void OnDisable()
    {
        btn.onClick.RemoveListener(ReturnToMainMenu);
    }

    private void ReturnToMainMenu()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
        }

        SceneManager.LoadScene(MainMenuSceneName);
    }
}
