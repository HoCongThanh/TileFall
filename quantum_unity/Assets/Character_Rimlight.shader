Shader "MyShader/Unlit/Character_Rimlight"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Tex", 2D) = "white" {}
		[Toggle]_Isadditivecolor("Is additive color", Float) = 0
		_Lightdirection("Light direction", Vector) = (-1.2,0,1.5,0)
		_Rimlightcolor("Rimlight color", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True" "Queue"="Transparent" "CanUseSpriteAtlas"="True" }
		Cull Off

		Pass
		{
			ColorMask 0
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA
			CGPROGRAM
			
			#pragma target 3.0 
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				half3 ase_normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
			};
			uniform half _Isadditivecolor;
			uniform half4 _Color;
			uniform half3 _Lightdirection;
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform half4 _Rimlightcolor;


			v2f vert(appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.texcoord.xy = v.texcoord.xy;
				o.texcoord.zw = v.texcoord1.xy;

				half3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;

				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i ) : SV_Target
			{
				fixed4 finalColor;
				half3 ase_worldNormal = i.ase_texcoord1.xyz;
				half dotResult55 = dot( _Lightdirection , ase_worldNormal );
				half smoothstepResult74 = smoothstep( 0.0 , 1.0 , dotResult55);
				float2 uv_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				half4 tex2DNode1 = tex2D( _MainTex, uv_MainTex );
				half dotResult24 = dot( ase_worldNormal , half3(0.4,0.5,-0.5) );
				half4 temp_output_81_0 = ( ( smoothstepResult74 * tex2DNode1 * _Rimlightcolor ) + tex2DNode1 + ( dotResult24 * tex2DNode1 * half4(0.4,0.4,0.4,1) ) );
				

				finalColor = (( _Isadditivecolor )?( ( _Color + temp_output_81_0 ) ):( ( _Color * temp_output_81_0 ) ));
				finalColor.a = ( _Color.a * tex2DNode1.a );
				return finalColor;
			}
			ENDCG
		}
	}
	
	
	
}