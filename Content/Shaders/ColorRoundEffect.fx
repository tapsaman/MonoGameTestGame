#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float divider = 0.01;

float roundTo(float value, float divider)
{
    return round(value / divider) * divider;
}

float3 roundTo(float3 value, float divider)
{
    return float3(
        roundTo(value.r, divider),
        roundTo(value.g, divider),
        roundTo(value.b, divider)
    );
}

// Ps stands for pixel shader this is were we can effect the change of colors to our images.
// When we passed our effect to spritebatch.Begin( .., ...,, effect,..) this is what effects all the Draw calls we now make.
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 imageColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    imageColor = float4(roundTo(imageColor.rgb, divider), imageColor.a);
    
    return imageColor;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL
        MainPS(); 
    }
};