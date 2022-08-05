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

float size = 0;
float2 target;

// Ps stands for pixel shader this is were we can effect the change of colors to our images.
// When we passed our effect to spritebatch.Begin( .., ...,, effect,..) this is what effects all the Draw calls we now make.
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 coords = input.TextureCoordinates;
    float2 diff = target - coords;

    if (pow(diff.x, 2) + pow(diff.y, 2) < pow(size, 2))
    {
        float4 imageColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
        return imageColor;
    }

    return float4(0, 0, 0, 1);
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL
        MainPS(); 
    }
};