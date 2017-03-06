Shader "Hidden/Wave Effect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScreenTex ("Texture", 2D) = "black" {}
		_NoiseTex ("NoiseTexture", 2D) = "black" {}
		_LayerTex ("LayerTexture", 2D) = "black" {}
		_BackTex ("BackTexture", 2D) = "white" {}
		_WaveScreenPos ("WaveScreenPos", Vector) = (0, 0, 0)
		_CamDelta ("CamDelta", Float) = 0
		_WaveParams ("WaveParams", Vector) = (0, 10, 0, 0)
		_ReverseUV ("ReverseUV", Int) = 0
		_Dead ("Dead", Int) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _ScreenTex;
			sampler2D _NoiseTex;
			sampler2D _LayerTex;
			sampler2D _BackTex;
			float2 _WaveScreenPos;
			float _CamDelta;
			float4 _WaveParams;
			int _ReverseUV;
			int _Dead;

/*
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec2 uv = fragCoord.xy / iResolution.xy;
	
	float w = (0.5 - (uv.x)) * (iResolution.x / iResolution.y);
    float h = 0.5 - uv.y;
	float distanceFromCenter = sqrt(w * w + h * h);
	
	float sinArg = distanceFromCenter * 10.0 - iGlobalTime
	 * 10.0;
	float slope = cos(sinArg) ;
	vec4 color = texture2D(iChannel0, uv + normalize(vec2(w, h)) * slope * 0.05);
	
	fragColor = color;
}
*/

			float ramp(float y, float start, float end) {
				float inside = step(start,y) - step(end,y);
				float fact = (y-start)/(end-start)*inside;
				return (1.-fact) * inside;
				
			}

			float noise2(float2 uv) {
				return tex2D(_NoiseTex, uv).r;
			}

			float stripes(float2 uv) {
				float noi = noise2(float2(uv.x * 0.5, uv.y * 1.0) + float2(1.0, 3.0));
				return ramp(fmod(uv.y*4. + _Time.y/2.+sin(_Time.y + sin(_Time.y*0.63)),1.),0.5,0.6)*noi;
			}

			fixed4 frag (v2f i) : SV_Target {
				float2 player = float2(_WaveScreenPos.x / _ScreenParams.x, _WaveScreenPos.y / _ScreenParams.y);

				float2 uv = i.uv;
				float inv_aspect = _ScreenParams.y / _ScreenParams.x;

				float2 rel = uv - player;
				rel.y *= inv_aspect;
				float dist = length(rel);
				float sinArg = dist * 40.0 - _Time.y * 15.0;
				float slope = cos(sinArg);
				float slope2 = cos(sinArg * 1.2);

				float waveProgress = clamp(lerp(1, 0, length(rel) / _WaveParams.y), 0, 1);

				fixed2 sampleUV;
				if (_Dead == 0) {
					sampleUV = uv + _WaveParams.x * pow(waveProgress, 2) * slope * 0.02;
				} else {
					sampleUV = uv;
				}

				fixed4 col = tex2D(_MainTex, sampleUV);
				
				if (_Dead == 0) {
					col.a *= _WaveParams.z * pow(waveProgress, 0.5f) * 2;
				}

				// Hack for platform compatibility
				if (_ReverseUV != 0) {
					sampleUV.y = 1 - sampleUV.y;
				}

				fixed4 back = tex2D(_BackTex, sampleUV + float2(0, _CamDelta));
				fixed4 back2 = tex2D(_ScreenTex, sampleUV);
				back = lerp(back, back2, back2.a);

				float m = min(1, waveProgress); //* _WaveParams.z);
				float modifier = m;
				if (modifier < 0.4) {
					modifier = 0;
				} else if (modifier < 0.6) {
					modifier = (modifier - 0.4) * 5;
				} else {
					modifier = 1;
				}
				modifier *= _WaveParams.z;

				float modifier2 = clamp(pow(waveProgress, 1.0) * _WaveParams.z, 0, 1);
				if (modifier2 < 0.25) {
					modifier2 = 0;
				} else if (modifier < 0.5) {
					modifier2 = (modifier2 - 0.25) * 4;
				} else {
					modifier2 = 1;
				}

				float field = slope2 + 0.3f * noise2(float2(waveProgress, atan2(rel.x, rel.y) * 1.3f));
				float ln = modifier2 * (field < 0.1f && field > -0.1f ? 1 : 0);
				float x = noise2(float2(waveProgress, atan2(rel.x, rel.y) / 2));
				if (x > 0.8f) {
					ln = 0;
				}

				modifier *= 1.1;

				if (_Dead == 0) {
					float4 layer = tex2D(_LayerTex, uv);
					layer.rgb *= layer.a;
					back.rgb -= layer.rgb * modifier;
					back.rgb += fixed3(ln, ln, ln);
				}

				float scanline = pow((sin(uv.y * 400 - _Time.y * 40)+1)*0.5, 6) * 0.1;

				return lerp(back, col, col.a) - float4(scanline, scanline, scanline, 0);
			}
			ENDCG
		}
	}
}
