using UnityEngine;

public class CandyConveyorMover : MonoBehaviour
{
    public CommonDataSettings commonData;

    private float speed;
    [SerializeField] private bool isMoveOnConveyor;
    private Rigidbody candyRigidBody;
    void Start()
    {
        isMoveOnConveyor = false;
        speed = commonData.conveyorSpeed;
        candyRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(isMoveOnConveyor)
            transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision toConveyor)
    {
        if (toConveyor.gameObject.CompareTag("Conveyor"))
        {
            candyRigidBody.angularVelocity = Vector3.zero;
            isMoveOnConveyor = true;
        }
    }

    private void OnCollisionExit(Collision fromConveyor)
    {
        if (fromConveyor.gameObject.CompareTag("Conveyor"))
        {
            candyRigidBody.velocity = Vector3.zero;
            isMoveOnConveyor = false;
        }
    }
}