// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Donut1"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_GlossRMaskG("Gloss(R) Mask (G)", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_BaseColorAdd("Base Color Add", Color) = (0,0,0,0)
		_BaseColorMul("Base Color Mul", Color) = (1,1,1,0)
		_AddMul("Add / Mul", Range( 0 , 1)) = 0
		_Gloss("Gloss", Range( 0 , 3)) = 3
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _BaseColorAdd;
		uniform sampler2D _GlossRMaskG;
		uniform float4 _GlossRMaskG_ST;
		uniform float4 _BaseColorMul;
		uniform float _AddMul;
		uniform float _Gloss;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode2 = tex2D( _Albedo, uv_Albedo );
			float2 uv_GlossRMaskG = i.uv_texcoord * _GlossRMaskG_ST.xy + _GlossRMaskG_ST.zw;
			float4 tex2DNode25 = tex2D( _GlossRMaskG, uv_GlossRMaskG );
			float4 lerpResult65 = lerp( ( tex2DNode2 + _BaseColorAdd ) , tex2DNode2 , tex2DNode25.g);
			float4 lerpResult66 = lerp( ( tex2DNode2 * _BaseColorMul ) , tex2DNode2 , tex2DNode25.g);
			float4 lerpResult67 = lerp( lerpResult65 , lerpResult66 , _AddMul);
			o.Albedo = lerpResult67.rgb;
			float lerpResult71 = lerp( ( _Gloss * tex2DNode25.r ) , ( 1.0 - tex2DNode25.r ) , tex2DNode25.g);
			o.Smoothness = lerpResult71;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1072;92;608;936;1208.417;847.8753;1.646906;False;False
Node;AmplifyShaderEditor.SamplerNode;2;-1114.416,-777.1204;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;b8e4f8f7ff5cea847a6fb93703ade6ec;b8e4f8f7ff5cea847a6fb93703ade6ec;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;37;-1130.216,-537.1205;Float;False;Property;_BaseColorAdd;Base Color Add;3;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;34;-1107.507,-314.5036;Float;False;Property;_BaseColorMul;Base Color Mul;4;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-828.215,-544.9196;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;25;-867.7307,-4.723419;Float;True;Property;_GlossRMaskG;Gloss(R) Mask (G);1;0;Create;True;0;0;False;0;0be0d60a335d9f841a4e3dff9606ba03;0be0d60a335d9f841a4e3dff9606ba03;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-756.3428,293.9419;Float;True;Property;_Gloss;Gloss;6;0;Create;True;0;0;False;0;3;0.9;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-813.8381,-324.1113;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;66;-373.6187,-288.1793;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;72;-450.6652,40.99338;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-255.1418,242.1613;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-275.9851,-7.779438;Float;False;Property;_AddMul;Add / Mul;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;65;-371.2528,-541.1837;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;26;-244.1728,533.7112;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;da77b0fb54025a54db44c8ea9ba3bda6;da77b0fb54025a54db44c8ea9ba3bda6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;67;65.68421,-333.1115;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;71;127.2969,120.6723;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1082.39,-15.55192;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Donut1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;36;0;2;0
WireConnection;36;1;37;0
WireConnection;29;0;2;0
WireConnection;29;1;34;0
WireConnection;66;0;29;0
WireConnection;66;1;2;0
WireConnection;66;2;25;2
WireConnection;72;0;25;1
WireConnection;27;0;28;0
WireConnection;27;1;25;1
WireConnection;65;0;36;0
WireConnection;65;1;2;0
WireConnection;65;2;25;2
WireConnection;67;0;65;0
WireConnection;67;1;66;0
WireConnection;67;2;70;0
WireConnection;71;0;27;0
WireConnection;71;1;72;0
WireConnection;71;2;25;2
WireConnection;0;0;67;0
WireConnection;0;1;26;0
WireConnection;0;4;71;0
ASEEND*/
//CHKSM=4AA2CC9DA3CBCDD9148AA92630DA13D7F5F8C16C