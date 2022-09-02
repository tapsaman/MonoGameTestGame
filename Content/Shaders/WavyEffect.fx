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

float calculateWave(float y, float yOffset, float waveWidth, float waveHeight)
{
    return sin(waveHeight * (y + yOffset)) * waveWidth;
}

float yOffset;
float waveWidth;
float waveHeight;

// Ps stands for pixel shader this is were we can effect the change of colors to our images.
// When we passed our effect to spritebatch.Begin( .., ...,, effect,..) this is what effects all the Draw calls we now make.
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float x = input.TextureCoordinates.x;
    float y = input.TextureCoordinates.y;
    
    x += calculateWave(y, yOffset, waveWidth, waveHeight);

    float2 coords = float2(x, y);
    float4 imageColor = tex2D(SpriteTextureSampler, coords);

    return imageColor * input.Color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL
        MainPS(); 
    }
};