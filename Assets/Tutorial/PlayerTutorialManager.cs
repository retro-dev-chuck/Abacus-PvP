using Unity.Netcode;
using UnityEngine;

public class PlayerTutorialManager : NetworkBehaviour
{
   private TutorialRoundManager tutorialRoundManager;
   private bool isInTutorial;

   [SerializeField] private TopStickManager[] topStickManagers;
   [SerializeField] private BottomStickManager[] bottomStickManagers;

   private int resultingValue = 0;
   
   
   public override void OnNetworkSpawn()
   {
      base.OnNetworkSpawn();

      if (!IsOwner) return; // Only run local player logic

      // Look for the tutorial manager in this scene

      foreach (var stickManager in topStickManagers)
      {
         stickManager.bottomStoneCount.OnValueChanged += HandleValueChange;
      }
      foreach (var stickManager in bottomStickManagers)
      {
         stickManager.topStoneCount.OnValueChanged += HandleValueChange;
      }
   }

   private void HandleValueChange(int previousvalue, int newvalue)
   {
      tutorialRoundManager = FindObjectOfType<TutorialRoundManager>();

      // If the tutorial manager exists, we are in the tutorial!
      isInTutorial = tutorialRoundManager != null;

      if (isInTutorial)
      {
         Debug.Log("Player spawned in Tutorial Mode.");
      }
      else
      {
         Debug.Log("Player spawned in Multiplayer Game Mode.");
         return;
      }

      Debug.LogWarning($"Hello value is {CalculateAbacusValue()}" );
      if (tutorialRoundManager.CurrentNumber() == CalculateAbacusValue())
      {
         tutorialRoundManager.SelectNextNumber();
         if (tutorialRoundManager.IsDone)
         {
            //todo: update the tutorial text here
         }
      }
   }

   public int CalculateAbacusValue()
   {
      if (topStickManagers.Length != bottomStickManagers.Length)
      {
         Debug.LogError("Non matching top bottom");
         return 0;
      }

      float sum = 0;
      for (int i = 0; i < topStickManagers.Length; i++)
      {
         sum += (topStickManagers[topStickManagers.Length - 1 -i].GetCurrentValue() + bottomStickManagers[topStickManagers.Length - 1 -i].GetCurrentValue()) * Mathf.Pow(10, i);
      }

      return (int)sum;
   }
}
