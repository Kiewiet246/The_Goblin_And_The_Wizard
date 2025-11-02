using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class RotateSC : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;
    [SerializeField] private Rigidbody rb;
    
    [Header("Rotate Variables")]
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Transform lookTarget;
    public float newAngle;
    public bool rotating = false;


    void FixedUpdate()
    {
        if (inputController.rotateLeft)
        {
            RotateClockwise();
        }
        else if (inputController.rotateRight)
        {
            RotateAntiClockwise();
        }

        else
        {
            rotating = false;
           // newAngle = 0;
        }
        //transform.LookAt(lookTarget);
    }

    private void RotateAntiClockwise()
    {
        rotating = true;
        newAngle = rb.rotation.eulerAngles.y + rotateSpeed * Time.deltaTime;
        //transform.forward = Quaternion.Euler(0, newAngle, 0) * transform.forward;
        rb.rotation = Quaternion.AngleAxis(newAngle, UnityEngine.Vector3.up);
        //newRotation = Quaternion.AngleAxis(newAngle, UnityEngine.Vector3.up);
    }

    private void RotateClockwise()
    { 
        rotating = true;
        newAngle = rb.rotation.eulerAngles.y - rotateSpeed * Time.deltaTime;
        rb.rotation = Quaternion.AngleAxis(newAngle, UnityEngine.Vector3.up);
       // newRotation = Quaternion.AngleAxis(newAngle, UnityEngine.Vector3.up);
        //rb.rotation *= Quaternion.Euler(0, -rotateSpeed * Time.deltaTime, 0);
    }
}
