using UnityEngine;

public class SpellAflections : MonoBehaviour
{
    [Header("Components")]
    public SpellManager spellManager;
    
    [Header("Stacks")]
    public float fireStack;
    public float iceStack;
    public float poisonStack;

    public GameObject burnFVX;
    public GameObject freezeFVX;
    public GameObject poisonFVX;
}
