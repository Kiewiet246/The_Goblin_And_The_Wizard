using UnityEngine;

public class BuildingTowers : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TowerManager towerManager;
    
    [Header("Building Tower Variables")]
    [SerializeField] RaycastHit hit = new RaycastHit();
    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private TileInfo tileInfo;
    
    [Header("Tower Information")]
    [SerializeField] private LayerMask towerLayer;

    public TowerController towerController;
    
    public void CastRayOnClick()
    {
        Debug.Log("Click Left");
        Vector2 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        
        Physics.Raycast(ray, out hit, Mathf.Infinity, towerLayer);
        if (hit.collider != null)
        {
            TowerSelected();
            return;
        }
        
        Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer);
        if (hit.collider != null)
        {
            if (towerController != null)
            {
                towerController.HideRange();
                towerController = null;
            }
            
            TileSelected();
        }
    }

    private void TowerSelected()
    {
        Debug.Log("Tower Selected");
        if (hit.collider.gameObject.GetComponent<TowerController>() != null)
        {
            if (towerController != null)
            {
                towerController.HideRange();
                TowerController tower = hit.collider.gameObject.GetComponent<TowerController>();
                if (tower == towerController)
                {
                    HidetowerRange();
                }
                    
                else if (tower != towerController)
                {
                    //tower.HideRange();
                    towerController = tower;
                    towerController.ShowRange();
                }
            }
            else
            {
                towerController = hit.collider.gameObject.GetComponent<TowerController>();
                towerController.ShowRange();
            }
        } 
    }

    private void HidetowerRange()
    {
        towerController.HideRange();
        towerController = null;
    }

    private void TileSelected()
    {
        if (towerController != null)
        {
            HidetowerRange();
        }
        Debug.Log("Tile Selected");
        if (hit.collider.gameObject.GetComponentInParent<TileInfo>() != null)
        {
            Debug.Log(hit.collider.gameObject.transform.parent.gameObject.name);
            tileInfo = hit.collider.gameObject.GetComponentInParent<TileInfo>();
            towerController = towerManager.CreateTower(tileInfo);
            towerController.ShowRange();
        }
    }
}
