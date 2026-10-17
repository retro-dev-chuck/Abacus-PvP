using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class StickManager : NetworkBehaviour
{
   [SerializeField] private bool isTop;
      
   [SerializeField] protected Transform top;
   [SerializeField] protected Transform bottom;
   [SerializeField] protected Transform[] stones;
   
   protected Stack<Transform> stonesTop = new Stack<Transform>();
   protected Stack<Transform> stonesBottom = new Stack<Transform>();
   protected const float SingleStoneHeight = 0.08f;
   public bool IsTop => isTop;
   public abstract void MoveOneUp();
   public abstract void MoveOneDown();
   public abstract int GetCurrentValue();
}
