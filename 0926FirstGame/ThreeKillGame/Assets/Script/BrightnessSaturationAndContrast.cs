using UnityEngine;
using System.Collections;

public class BrightnessSaturationAndContrast : PostEffectsBase
{
    //绑定的shader
    public Shader briSatConShader;
    private Material briSatConMaterial;
    public Material material
    {
        get
        {
            //调用基类的CheckShaderAndCreateMaterial方法绑定shader与Material
            briSatConMaterial = CheckShaderAndCreateMaterial(briSatConShader, briSatConMaterial);
            return briSatConMaterial;
        }
    }
    //亮度值
    [Range(0.0f, 3.0f)]
    public float brightness = 1.0f;

    //饱和度
    [Range(0.0f, 3.0f)]
    public float saturation = 1.0f;
    //对比度
    [Range(0.0f, 3.0f)]
    public float contrast = 1.0f;
    //重写OnRenderImage方法
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            //设置shader中的各个值
            material.SetFloat("_Brightness", brightness);
            material.SetFloat("_Saturation", saturation);
            material.SetFloat("_Contrast", contrast);
            //将源纹理通过material处理，复制到目标纹理中
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}