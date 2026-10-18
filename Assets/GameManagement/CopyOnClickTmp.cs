using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CopyOnClickTmp : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI roomCodeText;
    [SerializeField] private string copiedFeedbackText = "Copied room code!";
    private string originalText;

    private void Awake()
    {
        roomCodeText = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string codeToCopy = GetCodeFromDisplay();

        if (!string.IsNullOrEmpty(codeToCopy))
        {
            GUIUtility.systemCopyBuffer = codeToCopy;
            StopAllCoroutines();
            StartCoroutine(ShowCopiedFeedback());
        }
    }

    private string GetCodeFromDisplay()
    {
        return roomCodeText.text.Replace("Room Code:", "").Trim();
    }

    private System.Collections.IEnumerator ShowCopiedFeedback()
    {
        originalText = roomCodeText.text;
        roomCodeText.text = copiedFeedbackText;
        
        yield return new WaitForSecondsRealtime(2.0f);
        
        roomCodeText.text = originalText;
    }
}