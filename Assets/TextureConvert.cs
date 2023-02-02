using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureConvert : MonoBehaviour
{
    public RenderTexture renderTexture;

    private void Start()
    {
        Texture2D myTexture = renderTexture.toTexture2D();
    }
}
