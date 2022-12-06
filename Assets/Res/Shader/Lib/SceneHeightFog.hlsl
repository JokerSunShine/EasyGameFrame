#ifndef _SCENEHEIGHTFOG_INCLUDE
    #define _SCENEHEIGHTFOG_INCLUDE
    //场景高度雾
    half3 _SceneHeightFogColor;
    float _SceneHeightFogThreshold;
    float _SceneHeightFogFearther;
    float _SceneHeightFogIntensity;
    #define SCENE_HIGHT_FOG(c,hight) c.rgb = lerp(c.rgb,lerp(_SceneHeightFogColor,c.rgb,smoothstep(_SceneHeightFogThreshold-_SceneHeightFogFearther,_SceneHeightFogThreshold+_SceneHeightFogFearther,hight)),_SceneHeightFogIntensity);
#endif