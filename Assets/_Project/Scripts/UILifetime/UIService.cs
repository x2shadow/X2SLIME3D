
using UnityEngine.SceneManagement;

namespace X2SLIME3D
{
    public class UIService
    {
       public void Hello()
       {
            UnityEngine.Debug.Log("UI HELLO");
       }

       public void Restart()
       {
            SceneManager.LoadScene(0);  // Boot Scene
       }
    }
}
