
using R3;

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

      public void SetMusicVolume()
      {
         UnityEngine.Debug.Log("Music Volume");
      }

      public void SetSoundVolume()
      {
         UnityEngine.Debug.Log("Sound Volume");
      }

      public void Restart()
      {
         OnLevelRestarted.OnNext(Unit.Default);
      }
   }
}
