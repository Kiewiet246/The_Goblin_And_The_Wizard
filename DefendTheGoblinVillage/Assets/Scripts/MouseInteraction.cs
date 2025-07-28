using System;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    public GridManager gridManager;
    public MouseState mouseState;
    public enum MouseState
    {
        Normal,
        ChangeStructure,
        ChangeTerrain,
        ChangeHeight
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButton();
        }

        if (Input.GetMouseButtonUp(1))
        {
            RightMouseButton();
        }
    }

    private void RightMouseButton()
    {
        Vector2 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.collider != null)
        {


            switch (mouseState)
            {
                case MouseState.Normal:
                    gridManager.AssignGoalLoc(hit.collider.gameObject.GetComponentInParent<TileInfo>());
                    break;
                case MouseState.ChangeStructure:
                    hit.collider.gameObject.GetComponentInParent<TileInfo>().CallWall();
                    break;
                case MouseState.ChangeTerrain:
                    break;
                case MouseState.ChangeHeight:
                    hit.collider.gameObject.GetComponentInParent<TileInfo>().RemoveTile();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void LeftMouseButton()
    {
        Vector2 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.collider != null)
        {


            switch (mouseState)
            {
                case MouseState.Normal:
                    gridManager.AssignStartLoc(hit.collider.gameObject.GetComponentInParent<TileInfo>());
                    break;
                case MouseState.ChangeStructure:
                    hit.collider.gameObject.GetComponentInParent<TileInfo>().CallTower();
                    break;
                case MouseState.ChangeTerrain:
                    hit.collider.gameObject.GetComponentInParent<TileInfo>().ChangeTheTerrain();
                    break;
                case MouseState.ChangeHeight:
                    hit.collider.gameObject.GetComponentInParent<TileInfo>().AddTile();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void ChangeToNormal()
    {
        mouseState = MouseState.Normal;
    }

    public void ChangeToChangeStructure()
    {
        mouseState = MouseState.ChangeStructure;
    }

    public void ChangeToChangeTerrain()
    {
        mouseState = MouseState.ChangeTerrain;
    }

    public void ChangeToChangeHeight()
    {
        mouseState = MouseState.ChangeHeight;
    }
}
