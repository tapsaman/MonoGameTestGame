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
    float2 coords = input.TextureCoordinates;

    // Random based on texture Y
    float yRandom = rand(float2(0, coords.y) + seed);

    // Random based on texture XY
    float xyRandom = rand(coords  + seed);

    if (yRandom < 0.2)
    {
        coords.x += (yRandom - 0.1) / 2;
    }

    float4 imageColor = tex2D(SpriteTextureSampler, coords) * input.Color;

    // Return empty colors (transparent)
    if (!any(imageColor))
        return imageColor;

    // Default color 3/4 times
    if (xyRandom > 0.25)
        return imageColor;
    
    // Radnom noise 1/4 times
    float c = xyRandom * 4;
    return float4(c, c, c, 1);
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL
        MainPS(); 
    }
};