using UnityEngine;

public class MultiplayerSessionThings : MonoBehaviour
{
    private static MultiplayerSessionThings instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
