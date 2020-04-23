Shader "ChemLab/TransparentBlend"
{
	Properties
	{
		_Albedo("Albedo", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+1" "IgnoreProjector" = "True" }
		Cull Back
		Blend DstColor Zero
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard addshadow fullforwardshadows 
		struct Input
		{
			half filler;
		};

		uniform float4 _Albedo;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Albedo.rgb;
			o.Alpha = _Albedo.a;
		}

		ENDCG
	}
	Fallback "Diffuse"
}