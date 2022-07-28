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

// Ps stands for pixel shader this is were we can effect the change of colors to our images.
// When we passed our effect to spritebatch.Begin( .., ...,, effect,..) this is what effects all the Draw calls we now make.
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = input.Color;
    float2 coords = input.TextureCoordinates;
    
    if (!any(color)) return color;

    float step = 1.0/7;

    if      (coords.x < (step * 1)) color = float4(1, 0, 0, 1);
    else if (coords.x < (step * 2)) color = float4(1, .5, 0, 1);
    else if (coords.x < (step * 3)) color = float4(1, 1, 0, 1);
    else if (coords.x < (step * 4)) color = float4(0, 1, 0, 1);
    else if (coords.x < (step * 5)) color = float4(0, 0, 1, 1);
    else if (coords.x < (step * 6)) color = float4(.3, 0, .8, 1);
    else                            color = float4(1, .8, 1, 1);

    return color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL
        MainPS(); 
    }
};