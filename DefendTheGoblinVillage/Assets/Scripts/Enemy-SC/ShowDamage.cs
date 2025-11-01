using System;
using System.Collections;
using UnityEngine;

public class ShowDamage : MonoBehaviour
{
    [SerializeField] private Material normalMat;

    [SerializeField] private Material flashMat;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float flashDuration;
    
    [SerializeField] private bool startFlash;

    [SerializeField] private float flashTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (startFlash)
        {
            float diff = Time.time - flashTimer;

            if (diff > flashDuration)
            {
                startFlash = false;
                meshRenderer.material = normalMat;
            }
        }
    }

    public void FlashDamage()
    {
       // StartCoroutine(Flash());
       TimerFlash();
    }

    private void TimerFlash()
    {
        startFlash = true;
        flashTimer = Time.time;
        meshRenderer.material = flashMat;
    }

    IEnumerator Flash()
    {
        meshRenderer.material = flashMat;
        yield return new WaitForSeconds(flashDuration);
        meshRenderer.material = normalMat;
    }
}
