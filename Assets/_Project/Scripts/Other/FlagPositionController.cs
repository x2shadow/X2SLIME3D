using UnityEngine;

public class FlagPositionController : MonoBehaviour
{
    public Transform flagAnchor;
    public Transform flag;

    void Start()
    {
        UpdateFlagPosition();
    }

    public void UpdateFlagPosition()
    {
        if(flagAnchor != null && flag != null)
        {
            flag.position = flagAnchor.position;
        }
    }
}
