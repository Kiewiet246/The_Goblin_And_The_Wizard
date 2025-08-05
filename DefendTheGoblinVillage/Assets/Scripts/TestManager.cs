using UnityEngine;

public class TestManager : MonoBehaviour
{
    public GridManager gridManager;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            gridManager.ClearGrid();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            gridManager.CreateGrid();
        }
    }
}
