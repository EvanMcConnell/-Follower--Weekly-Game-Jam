using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class palletteSwap : MonoBehaviour
{
    public Color In0;
    public Color In1;
    public Color In2;
    public Color In3;
    public Color Out0;
    public Color Out1;
    public Color Out2;
    public Color Out3;

    public Material mat;

    void OnEnable()
    {
        Shader shader = Shader.Find("Unlit/palletteSwap");
        if (mat == null)
        {
            mat = new Material(shader);
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        mat.SetColor("_In0", In0);
        mat.SetColor("_Out0", Out0);
        mat.SetColor("_In1", In0);
        mat.SetColor("_Out1", Out2);
        mat.SetColor("_In2", In2);
        mat.SetColor("_Out2", Out2);
        mat.SetColor("_In3", In3);
        mat.SetColor("_Out3", Out3);

        Graphics.Blit(src, dst, mat);
    }
}
