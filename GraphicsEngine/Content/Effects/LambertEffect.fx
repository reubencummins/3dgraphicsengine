float4x4 World;
float4x4 View;
float4x4 Projection;
float4 AmbientLight;
float4 LightPosition;

struct VertexShaderInput
{
    float4 Position : SV_POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION0;
	float3 Color : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return float4(1,1,1, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}
