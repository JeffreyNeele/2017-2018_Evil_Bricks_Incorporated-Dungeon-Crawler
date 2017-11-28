
sampler s0;

float4 PixelShader(float2 coords: TEXCOORD0) : Color0
{
	float4 color = tex2D(s0, coords);
	color.gb = color.r;
	return color;
}
technique Techniquetest
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShader();
	};
};