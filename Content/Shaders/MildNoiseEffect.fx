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
    float4 imageColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    float r = rand(input.TextureCoordinates + seed);
    
    if (r < 0.5)
    {
        return imageColor;
    }

    float high = 0.078432;
    float low =  0.078431;
    

    if (imageColor.r > low && imageColor.r < high
     && imageColor.g > low && imageColor.g < high
     && imageColor.b > low && imageColor.b < high)
    {
        return float4(1,0,0,1);
    }

    if (r > 0.6 || input.TextureCoordinates.y < 0.72 + r / 10)
    {
        return imageColor;
    }

    float redHigh =     0.141177;
    float redLow =      0.141176;
    float greenHigh =   0.298040;
    float greenLow =    0.298039;
    float blueHigh =    0.141177;
    float blueLow =     0.141176;

    if (imageColor.r > redLow && imageColor.r < redHigh
     && imageColor.g > greenLow && imageColor.g < greenHigh
     && imageColor.b > blueLow && imageColor.b < blueHigh)
    {
        return float4(0,0,0,1);
    }
    
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