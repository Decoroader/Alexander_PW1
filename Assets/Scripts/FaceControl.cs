using UnityEngine;

public class FaceControl : MonoBehaviour
{
    public Animator comAnimator;

    void Start()
    {
        comAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider candy)
	{
        if (candy.gameObject.CompareTag("DinamicObject"))
            comAnimator.SetTrigger("om_nom_trg");
    }
}
