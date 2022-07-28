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

float seed = 0;

float rand(float2 co){
  return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
}

float calculateWave(float y, float yOffset)
{
    return sin(2 * (y + yOffset)) / 16;
}

// Ps stands for pixel shader this is were we can effect the change of colors to our images.
// When we passed our effect to spritebatch.Begin( .., ...,, effect,..) this is what effects all the Draw calls we now make.
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float r = rand(input.TextureCoordinates + seed);

    if (r < 0.02)
    {
        return float4(1,0,0,0);
    }

    float2 coords = input.TextureCoordinates;
    float x;
    float y = coords.y;
    float threshold = seed % 1 + r / 50;

    if (y > threshold && y < threshold + 0.07)
    {
        if (y - threshold < 0.005)
            return float4(1,0,0,0);
        
        x = coords.x + r / 32 - 0.2;
    }
    else
    {
        x = coords.x + calculateWave(y, seed / 20) + r / 100;
    }

    float4 imageColor = tex2D(SpriteTextureSampler, float2(x, y));
    float combined = imageColor.r + imageColor.g + imageColor.b;
    
    if (combined < 0.4)
    {
        return float4(1,0,0,1);
    }
    else if (combined < 1)
    {
        return float4(0,0,0,1);
    }
    else
    {
        return float4(r, r, r, 1);
    }
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL
        MainPS(); 
    }
};