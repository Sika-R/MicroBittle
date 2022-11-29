// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'



Shader "SimpleStylizedGrass/GrassBuiltInInDirect"
{
    Properties
    {
        _Color ("Grass Color", Color) = (0.0588,0.294,0.282,1)
        _ColorTip ("Grass Tip Color", Color) = (0.152,0.56,0.611,1)
        _TipOffset ("TipOffset", Range(-1,1)) = 0.1
        _Emission("Emission", Float) = 0.3
        _Opacity ("Opacity", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.12
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _WindTexture("Wind Noise Texture",2D) = "black" {}
        _WindDensity("Wind Density",Float)=0.39
        _WindSpeed("Wind Speed",Float)=5.0
        _WindStrength("WindStrength",Float)=0.6
        _TranslucentStrength("Translucent Strength",Range(0,1)) = 0.3
        _TranslucentDiffuse("Translucent Strength Diffuse",Range(0,1)) = 0.1
        _GrassDetails ("Grass Details", 2D) = "white" {}
        _GrassDetailScale("Grass Detail Scale",Float) =  100
        [HideInInspector]_Cutoff("AlphaCutOff", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "LightMode" = "ForwardBase"}
        LOD 200
        Cull off   

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow vertex:vert noambient noforwardadd 
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:setup

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 4.0

        #define UNITY_MATRIX_M     unity_ObjectToWorld
        #define UNITY_MATRIX_I_M   unity_WorldToObject


struct Input
{
      float2 uv_Opacity;
      float3 vertColor;
      float3 WorldPosition;
      half3 WorldSpaceNormal;
      half3 WorldSpaceTangent;
      half3 WorldSpaceBiTangent;
};

sampler2D _Opacity;
sampler2D _GrassDetails;
sampler2D _WindTexture;

half _Glossiness;
half _Metallic;
fixed4 _Color;
fixed4 _ColorTip;
float _Emission;
float _TipOffset;
float _GrassDetailScale;
float _TranslucentStrength;
float _TranslucentDiffuse;
float _WindDensity;
float _WindStrength;
float _WindSpeed;
float _Cutoff=0.5;


float4x4 _MeshMatrix;
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
   StructuredBuffer<float3> positionBuffer;
   StructuredBuffer<float4> rotationBuffer;
   StructuredBuffer<uint> _VisibleInstanceOnlyTransformIDBuffer;
#endif

float4x4 inverse(float4x4 m) {
    float n11 = m[0][0], n12 = m[1][0], n13 = m[2][0], n14 = m[3][0];
    float n21 = m[0][1], n22 = m[1][1], n23 = m[2][1], n24 = m[3][1];
    float n31 = m[0][2], n32 = m[1][2], n33 = m[2][2], n34 = m[3][2];
    float n41 = m[0][3], n42 = m[1][3], n43 = m[2][3], n44 = m[3][3];

    float t11 = n23 * n34 * n42 - n24 * n33 * n42 + n24 * n32 * n43 - n22 * n34 * n43 - n23 * n32 * n44 + n22 * n33 * n44;
    float t12 = n14 * n33 * n42 - n13 * n34 * n42 - n14 * n32 * n43 + n12 * n34 * n43 + n13 * n32 * n44 - n12 * n33 * n44;
    float t13 = n13 * n24 * n42 - n14 * n23 * n42 + n14 * n22 * n43 - n12 * n24 * n43 - n13 * n22 * n44 + n12 * n23 * n44;
    float t14 = n14 * n23 * n32 - n13 * n24 * n32 - n14 * n22 * n33 + n12 * n24 * n33 + n13 * n22 * n34 - n12 * n23 * n34;

    float det = n11 * t11 + n21 * t12 + n31 * t13 + n41 * t14;
    float idet = 1.0f / det;

    float4x4 ret;

    ret[0][0] = t11 * idet;
    ret[0][1] = (n24 * n33 * n41 - n23 * n34 * n41 - n24 * n31 * n43 + n21 * n34 * n43 + n23 * n31 * n44 - n21 * n33 * n44) * idet;
    ret[0][2] = (n22 * n34 * n41 - n24 * n32 * n41 + n24 * n31 * n42 - n21 * n34 * n42 - n22 * n31 * n44 + n21 * n32 * n44) * idet;
    ret[0][3] = (n23 * n32 * n41 - n22 * n33 * n41 - n23 * n31 * n42 + n21 * n33 * n42 + n22 * n31 * n43 - n21 * n32 * n43) * idet;

    ret[1][0] = t12 * idet;
    ret[1][1] = (n13 * n34 * n41 - n14 * n33 * n41 + n14 * n31 * n43 - n11 * n34 * n43 - n13 * n31 * n44 + n11 * n33 * n44) * idet;
    ret[1][2] = (n14 * n32 * n41 - n12 * n34 * n41 - n14 * n31 * n42 + n11 * n34 * n42 + n12 * n31 * n44 - n11 * n32 * n44) * idet;
    ret[1][3] = (n12 * n33 * n41 - n13 * n32 * n41 + n13 * n31 * n42 - n11 * n33 * n42 - n12 * n31 * n43 + n11 * n32 * n43) * idet;

    ret[2][0] = t13 * idet;
    ret[2][1] = (n14 * n23 * n41 - n13 * n24 * n41 - n14 * n21 * n43 + n11 * n24 * n43 + n13 * n21 * n44 - n11 * n23 * n44) * idet;
    ret[2][2] = (n12 * n24 * n41 - n14 * n22 * n41 + n14 * n21 * n42 - n11 * n24 * n42 - n12 * n21 * n44 + n11 * n22 * n44) * idet;
    ret[2][3] = (n13 * n22 * n41 - n12 * n23 * n41 - n13 * n21 * n42 + n11 * n23 * n42 + n12 * n21 * n43 - n11 * n22 * n43) * idet;

    ret[3][0] = t14 * idet;
    ret[3][1] = (n13 * n24 * n31 - n14 * n23 * n31 + n14 * n21 * n33 - n11 * n24 * n33 - n13 * n21 * n34 + n11 * n23 * n34) * idet;
    ret[3][2] = (n14 * n22 * n31 - n12 * n24 * n31 - n14 * n21 * n32 + n11 * n24 * n32 + n12 * n21 * n34 - n11 * n22 * n34) * idet;
    ret[3][3] = (n12 * n23 * n31 - n13 * n22 * n31 + n13 * n21 * n32 - n11 * n23 * n32 - n12 * n21 * n33 + n11 * n22 * n33) * idet;

    return ret;
}

float4x4 quaternion_to_matrix(float4 quat)
{
    float4x4 m = float4x4(float4(0, 0, 0, 0), float4(0, 0, 0, 0), float4(0, 0, 0, 0), float4(0, 0, 0, 0));

    float x = quat.x, y = quat.y, z = quat.z, w = quat.w;
    float x2 = x + x, y2 = y + y, z2 = z + z;
    float xx = x * x2, xy = x * y2, xz = x * z2;
    float yy = y * y2, yz = y * z2, zz = z * z2;
    float wx = w * x2, wy = w * y2, wz = w * z2;

    m[0][0] = 1.0 - (yy + zz);
    m[0][1] = xy - wz;
    m[0][2] = xz + wy;

    m[1][0] = xy + wz;
    m[1][1] = 1.0 - (xx + zz);
    m[1][2] = yz - wx;

    m[2][0] = xz - wy;
    m[2][1] = yz + wx;
    m[2][2] = 1.0 - (xx + yy);

    m[3][3] = 1.0;

    return m;
}

void setup()
        {
        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            int index=_VisibleInstanceOnlyTransformIDBuffer[unity_InstanceID];
            float3 data =positionBuffer[index];

            //unity_ObjectToWorld=data;

            unity_ObjectToWorld._11_21_31_41 = float4(1, 0, 0, 0);
            unity_ObjectToWorld._12_22_32_42 = float4(0,1, 0, 0);
            unity_ObjectToWorld._13_23_33_43 = float4(0, 0,1, 0);
            unity_ObjectToWorld._14_24_34_44 = float4(data.xyz, 1);

            float4x4 rotationMatrix=quaternion_to_matrix(rotationBuffer[_VisibleInstanceOnlyTransformIDBuffer[unity_InstanceID]]);

            unity_ObjectToWorld=mul(mul(unity_ObjectToWorld,rotationMatrix),_MeshMatrix);


            unity_WorldToObject = inverse(unity_ObjectToWorld);
            //unity_WorldToObject._14_24_34 *= -1;
            //unity_WorldToObject._11_22_33 = 1.0f / unity_WorldToObject._11_22_33;
        #endif
        }



float4x4 GetObjectToWorldMatrix()
{
    return UNITY_MATRIX_M;
}

float4x4 GetWorldToObjectMatrix()
{
    return UNITY_MATRIX_I_M;
}

float3 TransformObjectToWorldDir(float3 dirOS)
{
    // Normalize to support uniform scaling
    return normalize(mul((float3x3)GetObjectToWorldMatrix(), dirOS));
}

float GetOddNegativeScale()
{
    return unity_WorldTransformParams.w;
}

float3 TransformWorldToObject(float3 positionWS)
{
    return mul(GetWorldToObjectMatrix(), float4(positionWS, 1.0)).xyz;
}

float3 TransformObjectToWorld(float3 positionOS)
{
    return mul(GetObjectToWorldMatrix(), float4(positionOS, 1.0)).xyz;
}

// Transforms normal from object to world space
float3 TransformObjectToWorldNormal(float3 normalOS)
{
#ifdef UNITY_ASSUME_UNIFORM_SCALING
    return UnityObjectToWorldDir(normalOS);
#else
    // Normal need to be multiply by inverse transpose
    // mul(IT_M, norm) => mul(norm, I_M) => {dot(norm, I_M.col0), dot(norm, I_M.col1), dot(norm, I_M.col2)}
    return normalize(mul(normalOS, (float3x3)GetWorldToObjectMatrix()));
#endif
}



    half GetNoiseValue(half2 UV,half Scale)
    {
        return tex2Dlod(_WindTexture,half4(UV.x*Scale/100.0,UV.y*Scale/100.0,0,0));
    }
    float3 GetVertexPosition(appdata_full v,float3 worldPosition)
    {
        float2 Offset=float2(_Time.y*_WindSpeed,0);
        float2 UV=float2(worldPosition.x,worldPosition.z)+Offset;
        float NoiseValue=GetNoiseValue(UV,_WindDensity)-0.5;
        NoiseValue*=_WindStrength;

        float3 newWorldPosition=float3(worldPosition.x+NoiseValue,worldPosition.y-(abs(NoiseValue)*0.6),worldPosition.z);

        float BendStrength=v.color.g;

        float3 LerpedPosition=lerp(worldPosition,newWorldPosition,BendStrength);
        return TransformWorldToObject(LerpedPosition);
    }


    void vert (inout appdata_full v,out Input o) {

        UNITY_INITIALIZE_OUTPUT(Input,o);
        o.vertColor=v.color.rgb;
        o.WorldPosition=TransformObjectToWorld(v.vertex);
        v.vertex.xyz=GetVertexPosition(v,o.WorldPosition);

        float3 normalWS=TransformObjectToWorldNormal(v.normal);
        float3 tangentWS = TransformObjectToWorldDir(v.tangent.xyz);

        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        float3 unnormalizedNormalWS = normalWS;
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);

        // use bitangent on the fly like in hdrp
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        float crossSign = (v.tangent.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        float3 bitang = crossSign * cross(normalWS.xyz, tangentWS.xyz);

        o.WorldSpaceNormal =            renormFactor*normalWS.xyz;		// we want a unit length Normal Vector node in shader graph

        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        o.WorldSpaceTangent =           renormFactor*tangentWS.xyz;
        o.WorldSpaceBiTangent =         renormFactor*bitang;
    }

        
    float3 TransformWorldToTangent(float3 dirWS, float3x3 worldToTangent)
    {
        return mul(worldToTangent, dirWS);
    }

    fixed3 GetMainColor(Input IN){
        //return _Color;
        //Details
        float3 worldPos=IN.WorldPosition;
        half3 scaledPos=worldPos/_GrassDetailScale;
        half Detail=tex2D(_GrassDetails,half2(scaledPos.x,scaledPos.z));

        //Grass tips
        half tip=IN.vertColor.g+_TipOffset;

        half clampedAndScaledtip=clamp((tip)*Detail,0,1);

        fixed4 c=lerp(_Color,_ColorTip,clampedAndScaledtip);

        //Translucent
        half3 LightDirection=normalize(UnityWorldSpaceLightDir(worldPos));
        half3 LightColor=clamp(_LightColor0,half3(0,0,0),half3(1,1,1));
        half LightStrength=clamp(dot(LightDirection,IN.WorldSpaceNormal),0,1);

        float3 cameraPos=_WorldSpaceCameraPos.xyz;  
        float3 camToPositionDirection=normalize(cameraPos-worldPos);
        half camOnLightDirectionStrength=clamp(dot(LightDirection,camToPositionDirection),0,1);
            
        half TranslucentResult=(1-(LightStrength*camOnLightDirectionStrength))*_TranslucentStrength;

        half TranslucentDiffuse=clamp(lerp(Detail,1,_TranslucentDiffuse)*tip,0,1);

        //Finalize
        fixed3 finalColor=lerp(c.rgb,LightColor,TranslucentResult*TranslucentDiffuse);

        return finalColor;
    }

    void surf (Input IN, inout SurfaceOutputStandard o)
    {

        fixed4 opacity=tex2D (_Opacity, IN.uv_Opacity);

        clip(opacity.a - _Cutoff);

        o.Albedo = GetMainColor(IN);
        o.Emission = o.Albedo*_Emission;
        // Metallic and smoothness come from slider variables
        o.Metallic = _Metallic;
        o.Smoothness = _Glossiness;
        o.Alpha = opacity.a;

        float3x3 matrixTangentSpace = float3x3(IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, IN.WorldSpaceNormal);
        float3 normalInTangentSpace=TransformWorldToTangent(float3(0, 1, 0), matrixTangentSpace);
            
        o.Normal= normalInTangentSpace;
    }


        ENDCG
    }
        Fallback "VertexLit"

}
