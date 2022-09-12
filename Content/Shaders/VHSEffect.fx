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

float rand(float2 co){
    return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
}

float calculateWave(float y, float yOffset)
{
    return sin(2 * (y + yOffset)) / 16;
}

float RoundTo(float value, float divider)
{
    return round(value / divider) * divider;
}

float3 RoundTo(float3 value, float divider)
{
    return float3(
        RoundTo(value.r, divider),
        RoundTo(value.g, divider),
        RoundTo(value.b, divider)
    );
}

float seed = 0;
float scanlineY = 0;
float scanlineHeight = 0.07;
float yOffset = 0;

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 coords = input.TextureCoordinates;

    // Random based on texture XY
    float xyRandom = rand(coords  + seed);

    /*if (xyRandom < 0.005)
    {
        return float4(0,0,0,0);
    }*/

    // Random based on texture Y
    float yRandom = rand(float2(0, coords.y) + seed);

    if (yRandom < 0.2)
    {
        // Apply horizontal distortion
        coords.x += (yRandom - 0.1) / 2;
    }
    else if (yRandom < 0.201)
    {
        // Occasional white line
        return float4(0.9, 0.9, 0.9, 1);
    }

    float threshold = (scanlineY % 1 + xyRandom / 20);

    if (threshold + scanlineHeight < 1
        ? (coords.y > threshold && coords.y < threshold + scanlineHeight)
        : (coords.y > threshold || coords.y < (threshold + scanlineHeight) % 1)
    )
    {
        // Apply scanline

        //if (coords.y - threshold < 0.005)
        //    return float4(1,0,0,0);

        if (yRandom > 0.9)
        {
            return yRandom < 0.95 ? float4(0.9, 0.9, 0.9, 1) : float4(0, 0, 0, 1);
        }
        if (xyRandom > 0.8)
        {

            return xyRandom > 0.88 ? float4(0.9, 0.9, 0.9, 1) : float4(0, 0, 0, 1);
        }
        
        coords.x += xyRandom / 32 - 0.2;
    }

    coords.y -= yOffset;

    float4 imageColor = tex2D(SpriteTextureSampler, coords);
    imageColor = float4(RoundTo(imageColor.rgb, 0.45), imageColor.a);

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