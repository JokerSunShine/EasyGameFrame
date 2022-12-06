#ifndef _TERRAINAMBIENT_INCLUDE

    TEXTURE2D(_Terrain_AmbientTex);//需代码传入
    SAMPLER(sampler_Terrain_AmbientTex);
    float _GlobalAmbientIntensity; //需添加一个全局变暗的效果，这里先简单实现，后续有需求改动再说

    #define GetTerrainAmbientColor(worldPos) SAMPLE_TEXTURE2D(_Terrain_AmbientTex,sampler_Terrain_AmbientTex,worldPos.xz * 0.00390625) * _GlobalAmbientIntensity
#endif
