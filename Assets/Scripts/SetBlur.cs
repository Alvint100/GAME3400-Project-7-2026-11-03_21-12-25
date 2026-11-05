using System;
using UnityEngine;
using UnityEngine.UI;

public class SetBlur : MonoBehaviour
{
    private static readonly int BlurValue = Shader.PropertyToID("_Blur");
    [SerializeField] private Material blackScreenMaterial;
    public float blurValue;

    public void Update()
    {
        blackScreenMaterial.SetFloat(BlurValue, blurValue);
    }

    public void DestroySelf()
    {
        gameObject.SetActive(false);
    }


}
