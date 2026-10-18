using TMPro;
using Unity.Services.Multiplayer.Components;
using UnityEngine;

public class RoomCodeUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomCodeText;

    private void Start()
    {
        InvokeRepeating(nameof(TryFetchCode), 0.2f, 0.5f);
    }
    private void TryFetchCode()
    {
        var sessionConnector = FindObjectOfType<SessionConnectorBehaviour>();
        
        if (sessionConnector != null && sessionConnector.SessionConnector != null && sessionConnector.SessionConnector.MultiplayerSession != null)
        {
            var sessionId = sessionConnector.SessionConnector.CreateOrJoinSessionId;
            
            if (sessionId != null && !string.IsNullOrEmpty(sessionId))
            {
                if (roomCodeText != null)
                {
                    roomCodeText.text = $"Room Code: {sessionId}";
                }
                CancelInvoke(nameof(TryFetchCode));
            }
        }
    }

    public void CopyCodeToClipboard()
    {
        var sessionConnector = FindObjectOfType<SessionConnectorBehaviour>();
        
        if (sessionConnector != null && sessionConnector.SessionConnector != null && sessionConnector.SessionConnector.MultiplayerSession != null){
            var session = sessionConnector.SessionConnector.MultiplayerSession.Session;

            if (session != null && !string.IsNullOrEmpty(session.Code))
            {
                GUIUtility.systemCopyBuffer = session.Code;
                Debug.Log($"Copied room code to clipboard: {session.Code}");
            }
        
        }
    }
}
