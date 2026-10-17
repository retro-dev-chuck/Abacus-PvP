using Unity.Netcode;
using UnityEngine;

public class BottomStickManager : StickManager
{
    private const int SingleStoneValue = 1;
    
    //for late joiners, we do this so we can adjust on join yes
    public NetworkVariable<int> topStoneCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
        topStoneCount.OnValueChanged += HandleStoneValueChanged;
        MoveInstant(topStoneCount.Value); 
    }

    public override void OnNetworkDespawn()
    {
        topStoneCount.OnValueChanged -= HandleStoneValueChanged;
    }
    //this is for a late join, so if other player has moved stones around we move these instantly so they get the synced version
    private void MoveInstant(int count)
    {
        //count is top stone count, 0th is the topmost stone
        for (int i = 0; i < count; i++)
        {
            if (stonesBottom.TryPop(out var stone))
            {
                stonesTop.Push(stone);
                //I guess push each stone by 1 stone amount
                stone.localPosition = top.localPosition + i * SingleStoneHeight * Vector3.right;
            }
        }
        //no need to move bottom stones since they are already in "resting" state
    }

    private void HandleStoneValueChanged(int previousValue, int newValue)
    {
        if (IsOwner)
        {
            return;
        }
        
        if (previousValue == newValue)
        {
            return;
        }
        
        while (stonesTop.Count < newValue)
        {
            if (stonesBottom.TryPop(out var stone))
            {
                stonesTop.Push(stone);
                stone.localPosition = top.localPosition + (stonesTop.Count - 1) * SingleStoneHeight * Vector3.right;
            }
            else break;
        }
        while (stonesTop.Count > newValue)
        {
            if (stonesTop.TryPop(out var stone))
            {
                stonesBottom.Push(stone);
                stone.localPosition = bottom.localPosition + (stonesBottom.Count - 1) * SingleStoneHeight * Vector3.left;
            }
            else break;
        }
    }

    private void Initialize()
    {
        stonesBottom.Clear();
        stonesTop.Clear();
        foreach (Transform stone in stones)
        {
            stonesBottom.Push(stone);
        }
    }

    public override void MoveOneUp()
    {
        if (!IsOwner)
        {
            return;
        }
        
        if (stonesBottom.TryPop(out var stone))
        {
            stonesTop.Push(stone);
            //because I rotated abacus by 90 degrees it's not up/down
            stone.localPosition = top.localPosition + (stonesTop.Count - 1) * SingleStoneHeight * Vector3.right;
            topStoneCount.Value = stonesTop.Count;
        }
        
    }

    public override void MoveOneDown()
    {
        if (!IsOwner)
        {
            return;
        }
        if (stonesTop.TryPop(out var stone))
        {
            stonesBottom.Push(stone);
            //it's after the push so -1
            stone.localPosition = bottom.localPosition + (stonesBottom.Count - 1) * SingleStoneHeight * Vector3.left;
            topStoneCount.Value = stonesTop.Count;
        }
    }

    public override int GetCurrentValue()
    {
        return stonesTop?.Count * SingleStoneValue ?? 0;
    }
}
