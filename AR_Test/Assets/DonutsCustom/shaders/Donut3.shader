// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Donut3"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_GlossRMaskG("Gloss(R) Mask (G)", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_BaseColorAdd("Base Color Add", Color) = (0,0,0,0)
		_BaseColorMul("Base Color Mul", Color) = (1,1,1,0)
		_AddMul("Add / Mul", Range( 0 , 1)) = 0
		_TopColorMul("Top Color Mul", Color) = (0.6509434,0.6509434,0.6509434,0)
		_Top2Multiply("Top 2 Multiply", Color) = (0,0,0,0)
		_BittenColorMul("Bitten Color Mul", Color) = (0,0,0,0)
		_BaseGloss("Base Gloss", Range( -1 , 2)) = 2
		_TopGloss("Top Gloss", Range( -1 , 1)) = 1
		_BittenGloss("Bitten Gloss", Range( -1 , 1)) = 1
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
		uniform float4 _TopColorMul;
		uniform float4 _BittenColorMul;
		uniform float4 _Top2Multiply;
		uniform float _BaseGloss;
		uniform float _BittenGloss;
		uniform float _TopGloss;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

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
			float temp_output_97_0 = step( 0.33 , tex2DNode25.b );
			float4 lerpResult71 = lerp( tex2DNode2 , ( _TopColorMul * tex2DNode2 ) , temp_output_97_0);
			float4 lerpResult74 = lerp( lerpResult67 , lerpResult71 , temp_output_97_0);
			float4 lerpResult78 = lerp( lerpResult74 , ( _BittenColorMul * tex2DNode2 ) , tex2DNode25.g);
			float4 lerpResult96 = lerp( lerpResult78 , ( _Top2Multiply * tex2DNode2 ) , tex2DNode25.a);
			o.Albedo = lerpResult96.rgb;
			float temp_output_100_0 = ( 1.0 - tex2DNode25.r );
			float4 temp_cast_1 = (temp_output_100_0).xxxx;
			float4 temp_cast_2 = (temp_output_100_0).xxxx;
			float4 lerpResult108 = lerp( temp_cast_1 , CalculateContrast(_BaseGloss,temp_cast_2) , ( 1.0 - tex2DNode25.g ));
			float4 temp_cast_3 = (tex2DNode25.r).xxxx;
			float4 temp_cast_4 = (tex2DNode25.r).xxxx;
			float4 lerpResult107 = lerp( temp_cast_3 , CalculateContrast(_BittenGloss,temp_cast_4) , tex2DNode25.g);
			float4 lerpResult110 = lerp( lerpResult108 , lerpResult107 , tex2DNode25.g);
			float4 temp_cast_5 = (temp_output_100_0).xxxx;
			float4 temp_cast_6 = (temp_output_100_0).xxxx;
			float4 lerpResult109 = lerp( temp_cast_5 , CalculateContrast(_TopGloss,temp_cast_6) , tex2DNode25.b);
			float4 lerpResult111 = lerp( lerpResult110 , lerpResult109 , tex2DNode25.b);
			o.Smoothness = lerpResult111.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1072;92;608;936;2091.038;1883.298;4.774914;False;False
Node;AmplifyShaderEditor.SamplerNode;2;-1105.598,-849.1423;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;af93ae6e50e793243875b2e4f0555f72;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;37;-1130.216,-537.1205;Float;False;Property;_BaseColorAdd;Base Color Add;3;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;34;-1182.821,-321.0527;Float;False;Property;_BaseColorMul;Base Color Mul;4;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;73;-1272.248,-106.3138;Float;False;Property;_TopColorMul;Top Color Mul;6;0;Create;True;0;0;False;0;0.6509434,0.6509434,0.6509434,0;0.1411765,0.04705882,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;-1797.742,211.6899;Float;True;Property;_GlossRMaskG;Gloss(R) Mask (G);1;0;Create;True;0;0;False;0;None;6e7b39f83b3bf214192c5563e8f4cd0f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-813.8381,-324.1113;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-983.2203,123.0669;Float;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;False;0;0.33;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-828.215,-544.9196;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;65;-493.2352,-525.3986;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-486.3568,-122.9242;Float;False;Property;_AddMul;Add / Mul;5;0;Create;True;0;0;False;0;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;97;-715.2877,122.5905;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-894.7329,-15.25526;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;66;-547.2554,-275.025;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;100;-1379.425,585.9289;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-1700.921,1073.42;Float;True;Property;_BittenGloss;Bitten Gloss;11;0;Create;True;0;0;False;0;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-1711.201,682.5402;Float;True;Property;_BaseGloss;Base Gloss;9;0;Create;True;0;0;False;0;2;2;-1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;67;-174.0714,-357.1838;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;105;-1185.863,1205.897;Float;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;71;-637.1949,-28.72361;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;104;-1150.979,674.6463;Float;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;103;-1081.155,523.8287;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-1698.174,1485.032;Float;True;Property;_TopGloss;Top Gloss;10;0;Create;True;0;0;False;0;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;76;48.2254,-281.8446;Float;False;Property;_BittenColorMul;Bitten Color Mul;8;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;160.5964,149.6807;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;74;-22.43352,-59.85494;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;94;-794.8994,242.3615;Float;False;Property;_Top2Multiply;Top 2 Multiply;7;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;108;-794.4868,553.2405;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;107;-804.1262,889.0327;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;106;-914.2015,1472.982;Float;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;78;433.0738,156.0189;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-416.8437,200.6654;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;110;-496.1937,749.9113;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;109;-514.7435,1404.261;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;111;-139.2259,1027.425;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;96;502.6255,290.5434;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;26;97.64522,578.6241;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;None;e7998b6ee8bcc2a429b37407caecb103;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;920.619,191.7566;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Donut3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;29;0;2;0
WireConnection;29;1;34;0
WireConnection;36;0;2;0
WireConnection;36;1;37;0
WireConnection;65;0;36;0
WireConnection;65;1;2;0
WireConnection;65;2;25;2
WireConnection;97;0;98;0
WireConnection;97;1;25;3
WireConnection;72;0;73;0
WireConnection;72;1;2;0
WireConnection;66;0;29;0
WireConnection;66;1;2;0
WireConnection;66;2;25;2
WireConnection;100;0;25;1
WireConnection;67;0;65;0
WireConnection;67;1;66;0
WireConnection;67;2;70;0
WireConnection;105;1;25;1
WireConnection;105;0;99;0
WireConnection;71;0;2;0
WireConnection;71;1;72;0
WireConnection;71;2;97;0
WireConnection;104;1;100;0
WireConnection;104;0;101;0
WireConnection;103;0;25;2
WireConnection;77;0;76;0
WireConnection;77;1;2;0
WireConnection;74;0;67;0
WireConnection;74;1;71;0
WireConnection;74;2;97;0
WireConnection;108;0;100;0
WireConnection;108;1;104;0
WireConnection;108;2;103;0
WireConnection;107;0;25;1
WireConnection;107;1;105;0
WireConnection;107;2;25;2
WireConnection;106;1;100;0
WireConnection;106;0;102;0
WireConnection;78;0;74;0
WireConnection;78;1;77;0
WireConnection;78;2;25;2
WireConnection;95;0;94;0
WireConnection;95;1;2;0
WireConnection;110;0;108;0
WireConnection;110;1;107;0
WireConnection;110;2;25;2
WireConnection;109;0;100;0
WireConnection;109;1;106;0
WireConnection;109;2;25;3
WireConnection;111;0;110;0
WireConnection;111;1;109;0
WireConnection;111;2;25;3
WireConnection;96;0;78;0
WireConnection;96;1;95;0
WireConnection;96;2;25;4
WireConnection;0;0;96;0
WireConnection;0;1;26;0
WireConnection;0;4;111;0
ASEEND*/
//CHKSM=77F9E646B973D4AE1A2E4CF849E7306DAF4B5E81