using Unity.Netcode;

public abstract class StickManager : NetworkBehaviour
{
   public abstract void MoveOneUp();
   public abstract void MoveOneDown();
}
