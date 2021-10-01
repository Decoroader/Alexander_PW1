using UnityEngine;

public class ControllerOfConveyor : MonoBehaviour
{
    public CommonDataSettings commonData;
    public ConveyorData conveyorData;
    [SerializeField] private Vector3 startPos;

    private float speed;
    [SerializeField] private float maxConveyorShift;
    void Start()
    {
        speed = commonData.conveyorSpeed;
        startPos = transform.position;

        maxConveyorShift = startPos.x - conveyorData.convLong;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < maxConveyorShift)
            transform.position = startPos;
    }   
}
