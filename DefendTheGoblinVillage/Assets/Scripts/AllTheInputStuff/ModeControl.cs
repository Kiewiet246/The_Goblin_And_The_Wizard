using UnityEngine;

public class ModeControl : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;

    [Header("Mode Variables")] [SerializeField]
    private bool oncePressed;

    public bool isBuildMode = true;

    void FixedUpdate()
    {
        if (inputController.changePhase)
        {
            if (!oncePressed)
            {
                oncePressed = true;
                ChangeMode();
            }
        }
        else
        {
            oncePressed = false;
        }
    }

    private void ChangeMode()
    {
        if (isBuildMode)
        {
            isBuildMode = false;
        }

        else
        {
            isBuildMode = true;
        }
    }
}
