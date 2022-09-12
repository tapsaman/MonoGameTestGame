#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float distance;
float spriteTop;
float spriteBottom;
float4x4 viewProjection;

sampler TextureSampler : register(s0);

struct VertexInput {
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};
struct PixelInput {
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

float rand(float2 co){
    return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
}

static const int N_WAVES = 2;

PixelInput SpriteVertexShader(VertexInput v) {
    PixelInput output;

    output.Position = mul(v.Position, viewProjection);
    output.Color = v.Color;
    output.TexCoord = v.TexCoord;

    return output;
}

float4 SpritePixelShader(PixelInput p) : SV_TARGET {
    float2 coords = p.TexCoord;

    float xRandom = rand(float2(coords.x, 10)) - 0.5;
    //coords.y += xRandom * 0.01;
    coords.y = coords.y + xRandom * distance * 0.015;

    if (coords.y < spriteTop || coords.y > spriteBottom)
        return float4(0, 0, 0, 0);

    float4 diffuse = tex2D(TextureSampler, coords);
    float c = 1 - distance / 8;
    float v = 1 - distance / 2;
    return diffuse * p.Color * float4(c, c, c, v);
}

technique SpriteBatch {
    pass {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}