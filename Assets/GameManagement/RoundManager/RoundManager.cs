using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    [SerializeField] private TextMeshPro[] numberDisplays;
    
    public int[] possibleNumbers = new[] { 2, 27, 34, 423, 9206 };

    public NetworkVariable<int> selectedIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        selectedIndex.OnValueChanged += HandleIndexChange;
        InitialSetup();
    }

    private void InitialSetup()
    {
        UpdateTexts(possibleNumbers[selectedIndex.Value]);
    }

    private void HandleIndexChange(int previousvalue, int newvalue)
    {
        if (newvalue >= possibleNumbers.Length)
        {
            return;
        }
        UpdateTexts(possibleNumbers[newvalue]);
        
    }

    private void UpdateTexts(int value)
    {
        
        foreach (TextMeshPro numberDisplay in numberDisplays)
        {
            numberDisplay.text = value.ToString();
        }
    }

    public void SelectRandNumber()
    {
        if (!IsServer)
        {
            return;
        }
        List<int> removedPrev = new List<int>();
        for (int i = 0; i < possibleNumbers.Length; i++)
        {
            removedPrev.Add(i);
        }

        removedPrev.Remove(selectedIndex.Value);

        if (removedPrev.Count == 0)
        {
            selectedIndex.Value = 0;
            return;
        }
        int randIndex = Random.Range(0, removedPrev.Count);
        
        selectedIndex.Value = removedPrev[randIndex];
    }

}
