using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BBG.PictureColoring;

public class SpriteConverter : MonoBehaviour
{
    public Image[] imageToMerge = null;
    private GameObject finalSpriteRenderer;
    [SerializeField] private Material[] matToMerge = null;
    [SerializeField] private Material imageMat = null;

    public void Merge()
    {
        Resources.UnloadUnusedAssets();
        var newTex = new Texture2D(1024, 1024);

        for (int x = 0; x < newTex.width; x++)
        {
            for (int y = 0; y < newTex.height; y++)
            {
                newTex.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }

        for (int i = 0; i < imageToMerge.Length; i++)
        {
            for (int x = 0; x < imageToMerge[i].sprite.texture.width; x++)
            {
                for (int y = 0; y < imageToMerge[i].sprite.texture.width; y++)
                {
                    var color = imageToMerge[i].sprite.texture.GetPixel(x, y).a == 0 ? newTex.GetPixel(x, y) : imageToMerge[i].sprite.texture.GetPixel(x, y);
                    newTex.SetPixel(x, y, color);
                }
            }
            matToMerge[i] = new Material(Shader.Find("UI/Default"));
            matToMerge[i].mainTexture = imageToMerge[i].gameObject.GetComponent<PictureImage>().material.mainTexture;
        }

        //newTex.Apply();
        var finalImage = Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height), new Vector2(0.5f, 0.5f));
        finalImage.name = "okayge";
        finalSpriteRenderer.GetComponent<Image>().sprite = finalImage;
        finalSpriteRenderer.GetComponent<Image>().material.mainTexture = matToMerge[0].mainTexture;
    }
}
