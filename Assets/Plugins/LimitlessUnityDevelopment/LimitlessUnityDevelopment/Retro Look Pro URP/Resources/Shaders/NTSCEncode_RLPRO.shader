Shader "Hidden/Shader/NTSCEncode_RLPRO"
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
    SAMPLER(_MainTex);
	TEXTURE2D(_Mask);
	SAMPLER(sampler_Mask);
	float _FadeMultiplier;
#pragma shader_feature ALPHA_CHANNEL

	#define Pi2 float(6.283185307)
	#define Zero float4(0.0, 0.0, 0.0, 0.0)

	#define Half float4(0.5, 0.5, 0.5, 0.5)
	#define One float4(1.0, 1.0, 1.0, 1.0)
	#define Two float4(2.0, 2.0, 2.0, 2.0)
	#define A float4(0.5, 0.5, 0.5, 0.5)
	#define B float4(0.5, 0.5, 0.5, 0.5)
	#define P float(1.0)
	#define CCFrequency float(3.59754545)
	#define MinC float4(-1.1183, -1.1183, -1.1183, -1.1183)
	#define CRange float4(3.2366, 3.2366, 3.2366, 3.2366)
	float T;
	half val1;
	half val2;
	float4 CompositeSample(float2 UV) 
	{
		float ScanTime = 52.6 +T* val1;
		float2 InverseRes = 1.0/_ScreenParams.xy;
		float2 InverseP = float2(P, 0.0) * InverseRes;
		float2 C0 = UV;
		float2 C1 = UV + InverseP * 0.25;
		float2 C2 = UV + InverseP * 0.50;
		float2 C3 = UV + InverseP * 0.75;
		float4 Cx = float4(C0.x, C1.x, C2.x, C3.x);
		float4 Cy = float4(C0.y, C1.y, C2.y, C3.y);

		float3 Texel0 = tex2D(_MainTex, C0 ).rgb;
		float3 Texel1 = tex2D(_MainTex, C1 ).rgb;
		float3 Texel2 = tex2D(_MainTex, C2 ).rgb;
		float3 Texel3 = tex2D(_MainTex, C3 ).rgb;

		float4 T = A * Cy * float4(_ScreenParams.x, _ScreenParams.x, _ScreenParams.x, _ScreenParams.x) * Two + B + Cx;

		const float3 YTransform = float3(0.299, 0.587, 0.114);
		const float3 ITransform = float3(0.595716, -0.274453, -0.321263);
		const float3 QTransform = float3(0.211456, -0.522591, 0.311135);

		float Y0 = dot(Texel0, YTransform);
		float Y1 = dot(Texel1, YTransform);
		float Y2 = dot(Texel2, YTransform);
		float Y3 = dot(Texel3, YTransform);
		float4 Y = float4(Y0, Y1, Y2, Y3);

		float I0 = dot(Texel0, ITransform);
		float I1 = dot(Texel1, ITransform);
		float I2 = dot(Texel2, ITransform);
		float I3 = dot(Texel3, ITransform);
		float4 I = float4(I0, I1, I2, I3);

		float Q0 = dot(Texel0, QTransform);
		float Q1 = dot(Texel1, QTransform);
		float Q2 = dot(Texel2, QTransform);
		float Q3 = dot(Texel3, QTransform);
		float4 Q = float4(Q0, Q1, Q2, Q3);

		float4 W = float4(Pi2 * CCFrequency * ScanTime, Pi2 * CCFrequency * ScanTime, Pi2 * CCFrequency * ScanTime, Pi2 * CCFrequency * ScanTime);
		float4 Encoded = Y + I * cos(T * W) + Q * sin(T * W);
		return (Encoded - MinC) / CRange;
	}
	half Bsize;
	float4 NTSCCodec(float2 UV)
	{
		float YFrequency = 6.0;
		float IFrequency = 1.2;
		float QFrequency = 0.6;
		float NotchHalfWidth = 2.0;
		float ScanTime = 52.6 +T* val1;	
		float2 InverseRes = 1.0 / _ScreenParams.xy;
		float4 YAccum = Zero;
		float4 IAccum = Zero;
		float4 QAccum = Zero;
		float QuadXSize = _ScreenParams.x * Bsize;
		float TimePerSample = ScanTime / QuadXSize;

		float Fc_y1 = (CCFrequency - NotchHalfWidth) * TimePerSample;
		float Fc_y2 = (CCFrequency + NotchHalfWidth) * TimePerSample;
		float Fc_y3 = YFrequency * TimePerSample;
		float Fc_i = IFrequency * TimePerSample;
		float Fc_q = QFrequency * TimePerSample;
		float Pi2Length = Pi2 / 82.0;
		float4 NotchOffset = float4(0.0, 1.0, 2.0, 3.0);

		float4 W = float4(Pi2 * CCFrequency * ScanTime, Pi2 * CCFrequency * ScanTime, Pi2 * CCFrequency * ScanTime, Pi2 * CCFrequency * ScanTime);

		for (float n = -8.0; n < 8.0; n += 4.0)
		{
			float4 n4 = n/2 + NotchOffset;
			float4 CoordX = UV.x + InverseRes.x * n4 * val2;
			float4 CoordY = float4(UV.y, UV.y, UV.y, UV.y);
			float2 TexCoord = float2(CoordX.r, CoordY.r);
			float4 C = CompositeSample(TexCoord) * CRange + MinC;
			float4 WT = W * (CoordX + A * CoordY * Two * _ScreenParams.x + B);
			float4 SincYIn1 = Pi2 * Fc_y1 * n4;
			float4 SincYIn2 = Pi2 * Fc_y2 * n4;
			float4 SincYIn3 = Pi2 * Fc_y3 * n4;
			float4 SincY1 = sin(SincYIn1) / SincYIn1;
			float4 SincY2 = sin(SincYIn2) / SincYIn2;
			float4 SincY3 = sin(SincYIn3) / SincYIn3;

			if (SincYIn1.x == 0.0) SincY1.x = 1.0;
			if (SincYIn1.y == 0.0) SincY1.y = 1.0;
			if (SincYIn1.z == 0.0) SincY1.z = 1.0;
			if (SincYIn1.w == 0.0) SincY1.w = 1.0;
			if (SincYIn2.x == 0.0) SincY2.x = 1.0;
			if (SincYIn2.y == 0.0) SincY2.y = 1.0;
			if (SincYIn2.z == 0.0) SincY2.z = 1.0;
			if (SincYIn2.w == 0.0) SincY2.w = 1.0;
			if (SincYIn3.x == 0.0) SincY3.x = 1.0;
			if (SincYIn3.y == 0.0) SincY3.y = 1.0;
			if (SincYIn3.z == 0.0) SincY3.z = 1.0;
			if (SincYIn3.w == 0.0) SincY3.w = 1.0;
			float4 IdealY = (2.0 * Fc_y1 * SincY1 - 2.0 * Fc_y2 * SincY2) + 2.0 * Fc_y3 * SincY3;
			float4 FilterY = (0.54 + 0.46 * cos(Pi2Length * n4)) * IdealY;

			float4 SincIIn = Pi2 * Fc_i * n4;
			float4 SincI = sin(SincIIn) / SincIIn;
			if (SincIIn.x == 0.0) SincI.x = 1.0;
			if (SincIIn.y == 0.0) SincI.y = 1.0;
			if (SincIIn.z == 0.0) SincI.z = 1.0;
			if (SincIIn.w == 0.0) SincI.w = 1.0;
			float4 IdealI = 2.0 * Fc_i * SincI;
			float4 FilterI = (0.54 + 0.46 * cos(Pi2Length * n4)) * IdealI;

			float4 SincQIn = Pi2 * Fc_q * n4;
			float4 SincQ = sin(SincQIn) / SincQIn;
			if (SincQIn.x == 0.0) SincQ.x = 1.0;
			if (SincQIn.y == 0.0) SincQ.y = 1.0;
			if (SincQIn.z == 0.0) SincQ.z = 1.0;
			if (SincQIn.w == 0.0) SincQ.w = 1.0;
			float4 IdealQ = 2.0 * Fc_q * SincQ;
			float4 FilterQ = (0.54 + 0.46 * cos(Pi2Length * n4)) * IdealQ;

			YAccum = YAccum + C * FilterY;
			IAccum = IAccum + C * cos(WT) * FilterI;
			QAccum = QAccum + C * sin(WT) * FilterQ;
		}

		float Y = YAccum.r + YAccum.g + YAccum.b + YAccum.a;
		float I = (IAccum.r + IAccum.g + IAccum.b + IAccum.a) * 2.0;
		float Q = (QAccum.r + QAccum.g + QAccum.b + QAccum.a) * 2.0;

		float3 YIQ = float3(Y, I, Q);

		float3 OutRGB = float3(dot(YIQ, float3(1.0, 0.956, 0.621)), dot(YIQ, float3(1.0, -0.272, -0.647)), dot(YIQ, float3(1.0, -1.106, 1.703)));
		half4 colIn = tex2D(_MainTex, UV);
		float fd = 1;

		if (_FadeMultiplier > 0)
		{
#if ALPHA_CHANNEL
			float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, UV).a);
#else
			float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, UV).r);
#endif
			fd *= alpha_Mask;
		}

		return lerp(colIn,float4(OutRGB, colIn.a),fd);
	}

		float4 Frag0(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
		return NTSCCodec(i.uv );
	}

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "#NTSCEncode#"

			Cull Off ZWrite Off ZTest Always

            HLSLPROGRAM
                #pragma fragment Frag0
                #pragma vertex Vert
            ENDHLSL
        }

    }
    Fallback Off
}