using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectGrey : MonoBehaviour
{
    public Material grayMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, grayMaterial);
    }
}
