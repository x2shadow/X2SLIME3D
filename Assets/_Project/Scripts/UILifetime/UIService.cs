
using System;
using R3;
using UnityEngine.SceneManagement;

namespace X2SLIME3D
{
   public class UIService
   {
      public BehaviorSubject<int> OnLevelUpdated = new BehaviorSubject<int>(0);
      public Subject<Unit> OnLevelRestarted = new Subject<Unit>();

      public void UpdateLevel(int currentLevel)
      {
         OnLevelUpdated.OnNext(currentLevel);
      }

      public void Hello()
      {
         UnityEngine.Debug.Log("UI HELLO");
      }

      public void Restart()
      {
         OnLevelRestarted.OnNext(Unit.Default);
      }
   }
}
