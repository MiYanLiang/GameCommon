Shader "Change Brightness Saturation And Contrast" {
    Properties {
        //Graphics.Blit函数传入的src
        _MainTex ("Source Texture", 2D) = "white" {}
        //亮度值
        _Brightness ("Brightness", Float) = 1
       //饱和度
        _Saturation("Saturation", Float) = 1
       //对比度
        _Contrast("Contrast", Float) = 1
    }
    SubShader {
        Pass {  
            //关闭深度写入
            ZTest Always Cull Off ZWrite Off
            
            CGPROGRAM  
            #pragma vertex vert  
            #pragma fragment frag  
              
            #include "UnityCG.cginc"  
            //CG中代码块中声明对应的变量
            sampler2D _MainTex;  
            half _Brightness;
            half _Saturation;
            half _Contrast;
              
            struct v2f {
                float4 pos : SV_POSITION;
                half2 uv: TEXCOORD0;
            };
            //顶点着色器，坐标转换以及获取uv值  
            v2f vert(appdata_img v) {
                v2f o;
                
                o.pos = UnityObjectToClipPos(v.vertex);
                
                o.uv = v.texcoord;
                         
                return o;
            }
            //片元着色器
            fixed4 frag(v2f i) : SV_Target {
                //纹理采样
                fixed4 renderTex = tex2D(_MainTex, i.uv);  
                  
                // 亮度值调整
                fixed3 finalColor = renderTex.rgb * _Brightness;
                
                // 该像素对应的亮度值
                fixed luminance = 0.2125 * renderTex.r + 0.7154 * renderTex.g + 0.0721 * renderTex.b;
                //使用该亮度值创建一个饱和度为0的颜色
                fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
                //将之前的颜色与该颜色进行插值运算，得到调整饱和度后的颜色
                finalColor = lerp(luminanceColor, finalColor, _Saturation);
                
                // 创建一个对比度度为0的颜色
                fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
                //将之前的颜色与该颜色进行插值运算，得到调整对比度后的颜色
                finalColor = lerp(avgColor, finalColor, _Contrast);
                 //返回最终颜色   
                return fixed4(finalColor, renderTex.a);  
            }  
              
            ENDCG
        }  
    }
    
    Fallback Off
}