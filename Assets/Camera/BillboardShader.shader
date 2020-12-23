Shader "Custom/Billboard"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};


			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				// ①Meshの原点をModelView変換
				float3 viewPos = UnityObjectToViewPos(float3(0, 0, 0));

				// ②スケールと回転（平行移動なし）だけModel変換して、View変換はスキップ
				float3 scaleRotatePos = mul((float3x3)unity_ObjectToWorld, v.vertex);

				// ③View行列からX軸の回転だけ抽出した行列を生成
				float3x3 ViewRotateX = float3x3(
					UNITY_MATRIX_V._m00, 0, 0,
					UNITY_MATRIX_V._m10, 1, 0,
					UNITY_MATRIX_V._m20, 0, -1// Zの符号を反転して右手系に変換
				);
				viewPos += mul(ViewRotateX, scaleRotatePos);

				// ④最後にプロジェクション変換
				o.pos = mul(UNITY_MATRIX_P, float4(viewPos, 1));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}