#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler s0;

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

float time;

// Ps stands for pixel shader this is were we can effect the change of colors to our images.
// When we passed our effect to spritebatch.Begin( .., ...,, effect,..) this is what effects all the Draw calls we now make.
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 coords = input.TextureCoordinates / 2;
    coords.y += 0.5;

    float4 texelColorFromLoadedImage = tex2D(SpriteTextureSampler, coords);
    float4 theColorWeGaveToSpriteBatchDrawAsaParameter = input.Color;
    float4 color = texelColorFromLoadedImage * theColorWeGaveToSpriteBatchDrawAsaParameter;

    float threshold = time / 20;
    float high = 1 - threshold;
    float low  = threshold;

    if      (color.r > high) color.r = 1;
    else if (color.r < low) color.r = 0;

    if      (color.g > high) color.g = 1;
    else if (color.g < low) color.g = 0;

    if      (color.b > high) color.b = 1;
    else if (color.b < low) color.b = 0;

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