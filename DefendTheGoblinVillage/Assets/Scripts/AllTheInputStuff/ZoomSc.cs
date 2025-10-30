using UnityEngine;

public class ZoomSc : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;


    void FixedUpdate()
    {
        if (inputController.zoom.y > 0)
        {
            ZoomIn();
        }
        else if (inputController.zoom.y < 0)
        {
            ZoomOut();
        }
    }

    private void ZoomIn()
    {
        Debug.Log("Going In");
    }

    private void ZoomOut()
    {
        Debug.Log("Going up");
    }
}
