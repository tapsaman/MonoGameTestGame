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

float Min(float a, float b)
{
    return a < b ? a : b;
}


float Max(float a, float b)
{
    return a > b ? a : b;
}

int paramColorIter;

// Ps stands for pixel colorr this is were we can effect the change of colors to our images.
// When we passed our effect to spritebatch.Begin( .., ...,, effect,..) this is what effects all the Draw calls we now make.
float4 MainPS(VertexShaderOutput input) : COLOR
{
    /*float4 color = tex2D(s0, input.TextureCoordinates);
    float4 invertedColor = float4(color.a - color.rgb, color.a);
    return invertedColor;*/
    float4 texelColorFromLoadedImage = tex2D(SpriteTextureSampler, input.TextureCoordinates);
     // we can clip low alpha pixels on the colorr if we like directly .. we wont however here.
    // clip(texelColorFromLoadedImage.a - 0.01f);
    float4 theColorWeGaveToSpriteBatchDrawAsaParameter = input.Color;
    float4 color = texelColorFromLoadedImage * theColorWeGaveToSpriteBatchDrawAsaParameter;

    float combined = color.r + color.g + color.b;
    float main = Min(1, combined);
    float other = Max(0, (combined - main) / 2);

    if (paramColorIter == 0)
    {
        color.r = main;
        color.gb = other;
    }
    else if (paramColorIter == 1)
    {
        color.g = main;
        color.rb = other;
    }
    else if (paramColorIter == 2)
    {
        color.b = main;
        color.rg = other;
    }

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