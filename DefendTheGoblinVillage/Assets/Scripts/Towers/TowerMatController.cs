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
    }
    
}
