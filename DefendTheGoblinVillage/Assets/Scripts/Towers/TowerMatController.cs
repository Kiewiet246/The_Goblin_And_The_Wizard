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
    
    [SerializeField] private float flashDuration = 0.1f;
    
    [SerializeField] private bool startFlash;

    [SerializeField] private float flashTimer;
    
    public bool canBuild = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void BuildTowerMat()
    {
        meshRenderer.material = buildingMat;
        gameObject.GetComponent<Collider>().isTrigger = true;
    }

    void FixedUpdate()
    {
        if (startFlash)
        {
            float diff = Time.time - flashTimer;

            if (diff > flashDuration)
            {
                startFlash = false;
                meshRenderer.material = buildingMat;
                canBuild = true;
            }
        }
        else
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

    public void FlashTowerMat()
    {
        canBuild = false;
        meshRenderer.material = cannotBuildMat;
        startFlash = true;
        flashTimer = Time.time;
    }
}
