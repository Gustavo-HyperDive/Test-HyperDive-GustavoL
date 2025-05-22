void GetMainLight_half(half3 WorldPos, out half3 Direction, out half3 Color, out half Attenuation)
{
    #ifdef SHADERGRAPH_PREVIEW
        Direction = normalize(half3(1.0f, 1.0f, 0.0f));
        Color = 1.0f;
        Attenuation = 1.0f;
    #else
        Light mainLight = GetMainLight();
        Direction = mainLight.direction;
        Color = mainLight.color;
        Attenuation = mainLight.distanceAttenuation;
    #endif
}
