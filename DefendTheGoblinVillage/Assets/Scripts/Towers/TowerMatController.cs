using System;
using UnityEngine;

public class TowerMatController : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField] private MeshRenderer meshRenderer;
    [Header("Materials")]
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material buildingMat;
    [SerializeField] private Material cannotBuildMat;
    
    public bool canBuild = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void BuildTowerMat()
    {
        meshRenderer.material = buildingMat;
        gameObject.GetComponent<Collider>().isTrigger = true;
    }

    void FixedUpdate()
    {
        if (canBuild)
        {
            meshRenderer.material = buildingMat;
        }
        else
        {
            meshRenderer.material = cannotBuildMat;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        canBuild = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            canBuild = false;
        }
        else
        {
            canBuild = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canBuild = true;
    }
}
