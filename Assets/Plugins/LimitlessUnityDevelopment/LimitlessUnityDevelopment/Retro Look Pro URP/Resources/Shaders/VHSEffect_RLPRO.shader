Shader "Hidden/Shader/VHSEffect_RLPRO"
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
#pragma shader_feature ALPHA_CHANNEL
	sampler2D _Mask;
	float _FadeMultiplier;

	float MaskCut(float2 uv)
	{
		if (_FadeMultiplier > 0)
		{
#if ALPHA_CHANNEL
			float alpha_Mask = step(0.0001, tex2D(_Mask, uv).a);
#else
			float alpha_Mask = step(0.0001, tex2D(_Mask, uv).r);
#endif
			return alpha_Mask;
		}
		else return 1;
	}

	#define half4_one float4(1.0, 1.0, 1.0, 1.0)
	float iterations;
	float smoothSize;
	float _StandardDeviation;
	float _OffsetNoiseX;
	float _OffsetNoiseY;
	SAMPLER(_SecondaryTex);
	half _Stripes;
	float4 _MainTex_ST;
	float4 _SecondaryTex_ST;
	#define E 2.71828182846
	float _Intensity;
	float _TexIntensity;
	float _TexCut;
	float _OffsetColor;
	float2 _OffsetColorAngle;
	float _OffsetPosY;
	float _OffsetDistortion;
	float tileX = 0;
	float tileY = 0;
	float smooth1 = 0;
	float Time;
		        struct Attributes
        {
            float4 positionOS       : POSITION;
            float2 uv               : TEXCOORD0;
        };

        struct Varyings
        {
            float2 uv        : TEXCOORD0;
            float4 positionCS : SV_POSITION;
            UNITY_VERTEX_OUTPUT_STEREO
        };
        Varyings Vert(Attributes input)
        {
            Varyings output = (Varyings)0;
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
            output.positionCS = vertexInput.positionCS;
            output.uv = input.uv;

            return output;
        }
	#define unity_ColorSpaceLuminance half4(0.0396819152, 0.458021790, 0.00609653955, 1.0) 
	float smoothCut(float colR) {
		if (smooth1)
			return saturate(colR - _TexCut);
		else
			return ceil(colR - _TexCut);
	}
	inline half luminance(half3 rgb)
	{
		return dot(rgb, unity_ColorSpaceLuminance.rgb);
	}
	struct Attributes1
	{
		uint vertexID : SV_VertexID;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};
    struct Varyings1
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
		float2 texcoordStereo   : TEXCOORD1;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings1 Vert1(Attributes1 input)
    {
        Varyings1 output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
		output.texcoordStereo = output.texcoord + float2(_OffsetNoiseX - 0.2f, _OffsetNoiseY);
		output.texcoordStereo *= float2(tileY, tileX);
        return output;
    }
	float4 box(sampler2D tex, float2 uv, float4 size)
	{
		float4 col = 0;
		float sum = 0;
		for (float index = 0; index < iterations; index++) 
		{
			float offset = (index / (10 - 1) - 0.5) * size.x;
			float2 uv1 = uv + float2(0, offset);
			float stDevSquared = _StandardDeviation * _StandardDeviation;
			float gauss = (1 / sqrt(2 * PI * stDevSquared)) * pow(E, -((offset * offset) / (2 * stDevSquared)));
			sum += gauss;
			col += tex2D(tex, uv1) * gauss;
		}
		col = col / sum;
		return col;
	}

		float4 FragDivide(Varyings1 input) : SV_Target
	{
		half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;

		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = col2 / col;
		col2 = lerp(col, half4(col2.rgb, col.a), _TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragSubtract(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = saturate(col - col2);
		col2 = lerp(col, half4(col2.rgb, col.a), _TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragMultiply(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = saturate(col * col2);
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragColorBurn(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = half4_one - (half4_one - col) / col2;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLinearBurn(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = col2 + col - half4_one;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragDarkerColor(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = luminance(col.rgb) < luminance(col2.rgb) ? col : col2;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLighten(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = max(col, col2);
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragScreen(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = half4_one - ((half4_one - col2) * (half4_one - col));
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragColorDodge(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = col / (half4_one - col2);
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLinearDodge(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = col + col2;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLighterColor(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = luminance(col.rgb) > luminance(col2.rgb) ? col : col2;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragOverlay(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 ress = check * (half4_one - ((half4_one - 2.0 * (col - 0.5)) * (half4_one - col2)));
		ress += (half4_one - check) * (2.0 * col * col2);
		col2 = ress;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragSoftLight(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * (2.0 * col * col2 + col * col - 2.0 * col * col * col2);
		result += (half4_one - check) * (2.0 * sqrt(col) * col2 - sqrt(col) + 2.0 * col - 2.0 * col * col2);
		col2 = result;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragHardLight(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * (half4_one - ((half4_one - 2.0 * (col - 0.5)) * (half4_one - col2)));
		result += (half4_one - check) * (2.0 * col * col2);
		col2 = result;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragVividLight(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * (col / (half4_one - 2.0 * (col2 - 0.5)));
		result += (half4_one - check) * (half4_one - (half4_one - col) / (2.0 * col2));
		col2 = result;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragLinearLight(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * (col + (2.0 * (col2 - 0.5)));
		result += (half4_one - check) * (col + 2.0 * col2 - half4_one);
		col2 = result;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragPinLight(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		float4 check = step(0.5, col2);
		float4 result = check * max(2.0 * (col2 - 0.5), col);
		result += (half4_one - check) * min(2 * col2, col);
		col2 = result;
		col2 = lerp(col, half4(col2.rgb, col.a),_TexIntensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragHardMix(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		float4 result = float4(0.0, 0.0, 0.0, 0.0);
		result.r = col2.r > 1.0 - col.r ? 1.0 : 0.0;
		result.g = col2.g > 1.0 - col.g ? 1.0 : 0.0;
		result.b = col2.b > 1.0 - col.b ? 1.0 : 0.0;
		result.a = col2.a > 1.0 - col.a ? 1.0 : 0.0;
		col2 = result;
		col2 = lerp(col, half4(col2.rgb, col.a),_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragDifference(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = abs(col - col2);
		col2 = lerp(col, half4(col2.rgb, col.a),_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragExclusion(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = col + col2 - (2.0 * col * col2);
		col2 = lerp(col, half4(col2.rgb, col.a),_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}
		float4 FragDarken(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = min(col, col2);
		col2 = lerp(col, half4(col2.rgb, col.a),_Intensity);
		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}

	float4 Frag(Varyings1 input) : SV_Target
	{
				half alpha_Mask = MaskCut(input.texcoord);
		_OffsetColor *= alpha_Mask;
		_TexIntensity *= alpha_Mask;
		_OffsetPosY *= alpha_Mask;
		_OffsetDistortion *= alpha_Mask;
		input.texcoord = float2(frac(input.texcoord.x), frac(input.texcoord.y + _OffsetPosY));
		input.texcoord.x = _OffsetDistortion > 0 ? frac(input.texcoord.x + cos((input.texcoord.y + Time) * 100) * _OffsetDistortion*0.1) : frac(input.texcoord.x);
		half2 positionSS = input.texcoord  ;
		float4 col = tex2D(_MainTex, positionSS);
		half  amount = _OffsetColor * (distance(positionSS, half2(0.5, 0.5))) * 2;
		col.r = tex2D(_MainTex, positionSS + (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).r;
		col.b = tex2D(_MainTex, positionSS - (amount * half2(_OffsetColorAngle.y, _OffsetColorAngle.x))).b;
		float4 col2 = tex2D(_SecondaryTex, input.texcoordStereo);
		col2 = box(_SecondaryTex, input.texcoordStereo, smoothSize);
		col2 = lerp(col,half4(col2.rgb,col.a),_TexIntensity);

		return lerp(col, col2, smoothCut(col2.r)) * (1 - ceil(saturate(abs(input.texcoord.y - 0.5) - _Stripes)));
	}

    ENDHLSL

    SubShader
    {
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment Frag
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragDarken
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragMultiply
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragColorBurn
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragLinearBurn
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragDarkerColor
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragLighten
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragScreen
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragColorDodge
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragLinearDodge
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragLighterColor
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragOverlay
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragSoftLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragHardLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragVividLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragLinearLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragPinLight
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragHardMix
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragDifference
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragExclusion
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragSubtract
			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM
				#pragma vertex Vert1
				#pragma fragment FragDivide
			ENDHLSL
		}
    }
    Fallback Off
}