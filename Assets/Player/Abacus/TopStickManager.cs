using Unity.Netcode;
using UnityEngine;

public class TopStickManager : StickManager
{
    //reverse because we count the ones at the bottom as "score" 
    public NetworkVariable<int> bottomStoneCount = new (0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    private const int SingleStoneValue = 5;
    
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
        bottomStoneCount.OnValueChanged += HandleStoneValueChanged;
        MoveInstant(bottomStoneCount.Value); 
    }

    public override void OnNetworkDespawn()
    {
        bottomStoneCount.OnValueChanged -= HandleStoneValueChanged;
    }
    
    private void MoveInstant(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (stonesTop.TryPop(out var stone))
            {
                stonesBottom.Push(stone);
                stone.localPosition = bottom.localPosition + i * SingleStoneHeight * Vector3.right;
            }
        }
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
        
        while (stonesBottom.Count < newValue)
        {
            if (stonesTop.TryPop(out var stone))
            {
                stonesBottom.Push(stone);
                stone.localPosition = top.localPosition + (stonesBottom.Count - 1) * SingleStoneHeight * Vector3.right;
            }
            else break;
        }
        
        while (stonesBottom.Count > newValue)
        {
            if (stonesBottom.TryPop(out var stone))
            {
                stonesTop.Push(stone);
                stone.localPosition = bottom.localPosition + (stonesTop.Count - 1) * SingleStoneHeight * Vector3.left;
            }
            else break;
        }
    }

    //init to top, since default pos is reversed, yes code duplication 
    private void Initialize()
    {
        stonesBottom.Clear();
        stonesTop.Clear();
        foreach (Transform stone in stones)
        {
            stonesTop.Push(stone);
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
            bottomStoneCount.Value = stonesBottom.Count;
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
            bottomStoneCount.Value = stonesBottom.Count;
        }
    }

    public override int GetCurrentValue()
    {
        return stonesBottom?.Count * SingleStoneValue ?? 0;
    }
}
