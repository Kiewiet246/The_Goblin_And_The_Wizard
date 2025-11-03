using UnityEngine;

public class ZoomSc : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;
    [SerializeField] private Rigidbody rb;
    
    [Header("Zoom Variables")]
    [SerializeField] private float zoomSpeed;
     [SerializeField] private float zoomAmount;
    [SerializeField] private float minY, maxY;
    [SerializeField] private Transform cameraTransform;

    [Header("Tilt Variables")]
    [SerializeField] private bool tiltOnZoom = true;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float tiltAmount;
    [SerializeField] private float tiltMin, tiltMax;
    [SerializeField] private Transform lookTarget;

    void FixedUpdate()
    {
        if (inputController.zoom.y > 0)
        {
            ZoomIn();
            if (tiltOnZoom)
            {
                TiltUp();
            }
        }
        else if (inputController.zoom.y < 0)
        {
            ZoomOut();
            if (tiltOnZoom)
            {
                TiltDown();
            }
        }
    }

    private void ZoomIn()
    {
        float zoomAmount = cameraTransform.position.y -zoomSpeed * Time.deltaTime;
        float clampValue = Mathf.Clamp(zoomAmount, minY, maxY);
       // rb.MovePosition(rb.position - new Vector3(0, zoomAmount, 0));
        cameraTransform.position = new Vector3(cameraTransform.position.x, clampValue, cameraTransform.position.z);
    }

    private void ZoomOut()
    {
        zoomAmount = cameraTransform.position.y + zoomSpeed * Time.deltaTime;
        float clampValue = Mathf.Clamp(zoomAmount, minY, maxY);
       // rb.MovePosition(rb.position + new Vector3(0, zoomAmount, 0));
        cameraTransform.position = new Vector3(cameraTransform.position.x, clampValue, cameraTransform.position.z);
    }

    private void TiltUp()
    {
        cameraTransform.LookAt(lookTarget);
        // tiltAmount = cameraTransform.position.x - tiltSpeed * Time.deltaTime;
        // tiltAmount = Mathf.Clamp(tiltAmount, tiltMin, tiltMax);
        // cameraTransform.rotation = Quaternion.Euler(tiltAmount, 0f, 0f);
    }

    private void TiltDown()
    {
        cameraTransform.LookAt(lookTarget);
        // tiltAmount = cameraTransform.position.y + tiltSpeed * Time.deltaTime;
        // tiltAmount = Mathf.Clamp(tiltAmount, tiltMin, tiltMax);
        // cameraTransform.rotation = Quaternion.Euler(tiltAmount, 0f, 0f);;
    }
}
