using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PhysicsProp : MonoBehaviour
{

    [SerializeField] private float strength = 5.0f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void TakeHit(Vector3 dir)
    {
        if (rb)
        {
            rb.AddForce(dir.normalized * strength, ForceMode.VelocityChange);
            Debug.Log("Prop took hit");
        }
        else Debug.Log("Couldnt get rigidbody!");
    }
}
