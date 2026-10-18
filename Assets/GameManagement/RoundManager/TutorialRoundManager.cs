using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TutorialRoundManager : NetworkBehaviour
{
    
    public TextMeshProUGUI valueTmp;
    public TextMeshProUGUI feedbackTmp;
    [SerializeField] private TextMeshPro[] numberDisplays;
    
    public int[] tutorialNumbers = new[] { 2, 24, 36, 483, 9206 };

    public NetworkVariable<int> currentIndex = new NetworkVariable<int>(0, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    private const string TutorialCompletionSaveKey = "is_tutorial_completed";

    public bool IsDone => currentIndex.Value >= tutorialNumbers.Length;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        currentIndex.OnValueChanged += HandleIndexChange;
        InitialSetup();
    }

    private void HandleIndexChange(int previousValue, int newValue)
    {
        if (newValue < tutorialNumbers.Length)
        {
            UpdateTexts(tutorialNumbers[newValue]);
        }
    }

    private void InitialSetup()
    {
        Debug.LogWarning($"Tutorial init: {currentIndex.Value}");
        UpdateTexts(tutorialNumbers[currentIndex.Value]);
    }

    private void UpdateTexts(int value)
    {
        foreach (TextMeshPro numberDisplay in numberDisplays)
        {
            numberDisplay.text = value.ToString();
        }
    }

    public void SelectNextNumber()
    {
        if (!IsServer)
        {
            return;
        }

        if (currentIndex.Value + 1 >= tutorialNumbers.Length)
        {
            PlayerPrefs.SetInt(TutorialCompletionSaveKey, 1);
            PlayerPrefs.Save();
            Debug.LogWarning("Tutorial Completed!");
            feedbackTmp.text = "Tutorial Done!";
            return;
        }

        feedbackTmp.text = "Good job!\nNext number!";
       
        StartCoroutine(HideAfter2s());
        currentIndex.Value++;
        UpdateTexts(tutorialNumbers[currentIndex.Value]);
    }

    private IEnumerator HideAfter2s()
    {
        yield return new WaitForSecondsRealtime(2);
        feedbackTmp.text = "";
    }
    public int CurrentNumber()
    {
        if (tutorialNumbers == null || currentIndex.Value >= tutorialNumbers.Length)
        {
            Debug.LogError("something wrong here");
            return -1;
        }
        return tutorialNumbers[currentIndex.Value];
    }
}
