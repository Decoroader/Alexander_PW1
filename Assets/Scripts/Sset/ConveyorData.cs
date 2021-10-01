using UnityEngine;

public class ConveyorData : MonoBehaviour
{
    public float convLong;
    void Start()
    {
        convLong = transform.localScale.x;
    }
}
