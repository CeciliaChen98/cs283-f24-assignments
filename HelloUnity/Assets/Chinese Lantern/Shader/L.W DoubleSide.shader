// Upgrade NOTE: upgraded instancing buffer 'LWDoubleSidedPBR' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "L.W/DoubleSided/PBR"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_DiffuseA("Diffuse(A)", 2D) = "white" {}
		[Normal]_Normal("Normal", 2D) = "bump" {}
		_NormalIntensity("Normal Intensity", Float) = 0
		_Metallicsmoothness("Metallicsmoothness", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_smoothness("smoothness", Range( 0 , 1)) = 0
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_Emission_Strengh("Emission_Strengh", Range( 0 , 3)) = 0
		_Emission("Emission", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		Stencil
		{
			Ref 1
			CompFront Always
			PassFront Replace
		}
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform sampler2D _Normal;
		uniform sampler2D _DiffuseA;
		uniform sampler2D _Emission;
		uniform float4 _EmissionColor;
		uniform float _Emission_Strengh;
		uniform sampler2D _Metallicsmoothness;
		uniform float _Metallic;
		uniform float _smoothness;
		uniform float _Cutoff = 0.5;

		UNITY_INSTANCING_BUFFER_START(LWDoubleSidedPBR)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Normal_ST)
#define _Normal_ST_arr LWDoubleSidedPBR
			UNITY_DEFINE_INSTANCED_PROP(float4, _DiffuseA_ST)
#define _DiffuseA_ST_arr LWDoubleSidedPBR
			UNITY_DEFINE_INSTANCED_PROP(float4, _Emission_ST)
#define _Emission_ST_arr LWDoubleSidedPBR
			UNITY_DEFINE_INSTANCED_PROP(float4, _Metallicsmoothness_ST)
#define _Metallicsmoothness_ST_arr LWDoubleSidedPBR
			UNITY_DEFINE_INSTANCED_PROP(float, _NormalIntensity)
#define _NormalIntensity_arr LWDoubleSidedPBR
		UNITY_INSTANCING_BUFFER_END(LWDoubleSidedPBR)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float _NormalIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_NormalIntensity_arr, _NormalIntensity);
			float4 _Normal_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Normal_ST_arr, _Normal_ST);
			float2 uv_Normal = i.uv_texcoord * _Normal_ST_Instance.xy + _Normal_ST_Instance.zw;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, uv_Normal ), _NormalIntensity_Instance );
			float4 _DiffuseA_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_DiffuseA_ST_arr, _DiffuseA_ST);
			float2 uv_DiffuseA = i.uv_texcoord * _DiffuseA_ST_Instance.xy + _DiffuseA_ST_Instance.zw;
			float4 tex2DNode82 = tex2D( _DiffuseA, uv_DiffuseA );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult20 = dot( ase_worldNormal , ase_worldViewDir );
			float FaceSign48 = (1.0 + (sign( dotResult20 ) - -1.0) * (0.0 - 1.0) / (1.0 - -1.0));
			float4 temp_cast_0 = (FaceSign48).xxxx;
			float4 lerpResult89 = lerp( tex2DNode82 , temp_cast_0 , float4( 0,0,0,0 ));
			o.Albedo = lerpResult89.rgb;
			float4 _Emission_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Emission_ST_arr, _Emission_ST);
			float2 uv_Emission = i.uv_texcoord * _Emission_ST_Instance.xy + _Emission_ST_Instance.zw;
			o.Emission = ( tex2D( _Emission, uv_Emission ) * _EmissionColor * _Emission_Strengh ).rgb;
			float4 _Metallicsmoothness_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Metallicsmoothness_ST_arr, _Metallicsmoothness_ST);
			float2 uv_Metallicsmoothness = i.uv_texcoord * _Metallicsmoothness_ST_Instance.xy + _Metallicsmoothness_ST_Instance.zw;
			float4 tex2DNode67 = tex2D( _Metallicsmoothness, uv_Metallicsmoothness );
			o.Metallic = ( tex2DNode67 * _Metallic ).r;
			o.Smoothness = ( tex2DNode67.a * _smoothness );
			o.Alpha = 1;
			clip( tex2DNode82.a - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;1446.174;821.6213;1.723414;True;True
Node;AmplifyShaderEditor.CommentaryNode;49;-1804.416,-305.1648;Inherit;False;1094.131;402.4268;Comment;6;20;22;23;48;19;41;Face Sign (0 = Front, 1 = Back);1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;41;-1754.416,-255.165;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;19;-1729.196,-83.68282;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;20;-1496.163,-160.0436;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SignOpNode;22;-1328.61,-148.431;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;23;-1166.107,-166.5917;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;109;-561.649,-866.405;Inherit;False;748.9426;357.8324;Comment;3;84;82;89;BaseColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;108;-555.9188,-99.43094;Inherit;False;551.9392;511.8566;Comment;4;73;75;66;77;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;107;-561.7862,452.215;Inherit;False;829.189;334.8873;Comment;5;80;78;81;67;79;MetallicSmoothness;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;48;-944.2804,-170.2457;Float;False;FaceSign;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;110;-560.5884,-478.8936;Inherit;False;756.7582;351.8393;Comment;3;86;87;111;Norrmal;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-202.537,561.7681;Inherit;False;Property;_Metallic;Metallic;5;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-206.7416,672.1024;Inherit;False;Property;_smoothness;smoothness;6;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;82;-511.649,-816.405;Inherit;True;Property;_DiffuseA;Diffuse(A);1;0;Create;True;0;0;False;0;False;-1;None;9a182c63f21c3df4ca0be851050cb902;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;111;-449.6584,-339.4479;Inherit;False;InstancedProperty;_NormalIntensity;Normal Intensity;3;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;67;-511.7863,502.2151;Inherit;True;Property;_Metallicsmoothness;Metallicsmoothness;4;0;Create;True;0;0;False;0;False;-1;None;a87188c01e297ba48a5e576deea3b955;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;75;-481.6874,297.4256;Inherit;False;Property;_Emission_Strengh;Emission_Strengh;8;0;Create;True;0;0;False;0;False;0;0.6;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;73;-414.3848,134.8434;Float;False;Property;_EmissionColor;Emission Color;7;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;1,0.03836783,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;66;-505.9189,-49.43093;Inherit;True;Property;_Emission;Emission;9;0;Create;True;0;0;False;0;False;-1;None;a4c5ca373691ea14087d0ba2faa09550;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;84;-449.4373,-623.5726;Inherit;False;48;FaceSign;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;89;3.293631,-742.8693;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;88.82653,502.4824;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-172.9799,172.972;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-461.9703,-234.3696;Inherit;False;48;FaceSign;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;86;-162.4944,-423.9208;Inherit;True;Property;_Normal;Normal;2;1;[Normal];Create;True;0;0;False;0;False;-1;None;d03b4fa4c3ce37a47a07c17848ee3422;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;98.40273,621.3049;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1538.37,-268.2133;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;L.W/DoubleSided/PBR;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;7;False;-1;3;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;1,0.4344827,0,0;VertexScale;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;41;0
WireConnection;20;1;19;0
WireConnection;22;0;20;0
WireConnection;23;0;22;0
WireConnection;48;0;23;0
WireConnection;89;0;82;0
WireConnection;89;1;84;0
WireConnection;78;0;67;0
WireConnection;78;1;79;0
WireConnection;77;0;66;0
WireConnection;77;1;73;0
WireConnection;77;2;75;0
WireConnection;86;5;111;0
WireConnection;81;0;67;4
WireConnection;81;1;80;0
WireConnection;0;0;89;0
WireConnection;0;1;86;0
WireConnection;0;2;77;0
WireConnection;0;3;78;0
WireConnection;0;4;81;0
WireConnection;0;10;82;4
ASEEND*/
//CHKSM=B2FCE6C8DB1962B29979CCE262953AFEE8349FE7