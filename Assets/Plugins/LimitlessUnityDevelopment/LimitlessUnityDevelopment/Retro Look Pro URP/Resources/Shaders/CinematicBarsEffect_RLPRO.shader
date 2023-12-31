Shader "Hidden/Shader/CinematicBarsEffect_RLPRO"
{
     Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

    HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    SAMPLER(_MainTex);
	half _Stripes;
	half _Fade;
            struct Attributes
        {
            float4 positionOS       : POSITION;
            float2 uv               : TEXCOORD0;
        };

        struct Varyings
        {
            float2 uv        : TEXCOORD0;
            float4 vertex : SV_POSITION;
            UNITY_VERTEX_OUTPUT_STEREO
        };
        Varyings Vert(Attributes input)
        {
            Varyings output = (Varyings)0;
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
            output.vertex = vertexInput.positionCS;
            output.uv = input.uv;

            return output;
        }
    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        half2 positionSS = input.uv;
        float4 outColor = float4(0,0,0,1);
		float4 outColor2 = tex2D(_MainTex, positionSS);		
		outColor = lerp(outColor, outColor2, (1 - ceil(saturate(abs(input.uv.y - 0.5) - _Stripes))));
		return lerp(outColor2, outColor, _Fade);        
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "#CinematicBars#"
			Cull Off ZWrite Off ZTest Always

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}