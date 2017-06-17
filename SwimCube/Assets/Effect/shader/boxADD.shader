// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33383,y:32686,varname:node_3138,prsc:2|emission-2504-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5179,x:31500,y:33023,ptovrint:False,ptlb:01,ptin:_01,varname:node_5179,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:2362,x:31500,y:33118,ptovrint:False,ptlb:02,ptin:_02,varname:node_2362,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:2032,x:31500,y:33227,ptovrint:False,ptlb:03,ptin:_03,varname:node_2032,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Append,id:9654,x:31761,y:33092,varname:node_9654,prsc:2|A-5179-OUT,B-2362-OUT,C-2032-OUT;n:type:ShaderForge.SFN_Dot,id:4016,x:31979,y:33168,varname:node_4016,prsc:2,dt:0|A-9654-OUT,B-4463-OUT;n:type:ShaderForge.SFN_NormalVector,id:4463,x:31761,y:33239,prsc:2,pt:False;n:type:ShaderForge.SFN_Color,id:1025,x:31979,y:32995,ptovrint:False,ptlb:color,ptin:_color,varname:node_1025,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_VertexColor,id:604,x:32712,y:33054,varname:node_604,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7509,x:32691,y:33342,ptovrint:False,ptlb:node_7509,ptin:_node_7509,varname:node_7509,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_ValueProperty,id:3600,x:31451,y:32466,ptovrint:False,ptlb:04,ptin:_04,varname:_02,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_ValueProperty,id:6583,x:31451,y:32561,ptovrint:False,ptlb:05,ptin:_05,varname:_03,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_ValueProperty,id:9458,x:31451,y:32670,ptovrint:False,ptlb:06,ptin:_06,varname:_04,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Append,id:7443,x:31712,y:32535,varname:node_7443,prsc:2|A-3600-OUT,B-6583-OUT,C-9458-OUT;n:type:ShaderForge.SFN_Dot,id:5899,x:31930,y:32611,varname:node_5899,prsc:2,dt:0|A-7443-OUT,B-1405-OUT;n:type:ShaderForge.SFN_NormalVector,id:1405,x:31712,y:32682,prsc:2,pt:False;n:type:ShaderForge.SFN_OneMinus,id:2036,x:32160,y:32614,varname:node_2036,prsc:2|IN-5899-OUT;n:type:ShaderForge.SFN_Multiply,id:4423,x:32458,y:32646,varname:node_4423,prsc:2|A-9455-RGB,B-2036-OUT;n:type:ShaderForge.SFN_Color,id:9455,x:32160,y:32426,ptovrint:False,ptlb:node_9455,ptin:_node_9455,varname:node_9455,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:8887,x:32350,y:33097,varname:node_8887,prsc:2|A-1025-RGB,B-4016-OUT;n:type:ShaderForge.SFN_Add,id:6132,x:32729,y:32900,varname:node_6132,prsc:2|A-4423-OUT,B-8887-OUT;n:type:ShaderForge.SFN_Multiply,id:2504,x:32961,y:32985,varname:node_2504,prsc:2|A-6132-OUT,B-604-RGB,C-7509-OUT;proporder:5179-2362-2032-1025-7509-3600-6583-9458-9455;pass:END;sub:END;*/

Shader "Shader Forge/boxADD" {
    Properties {
        _01 ("01", Float ) = 0.2
        _02 ("02", Float ) = 0.2
        _03 ("03", Float ) = 0.2
        _color ("color", Color) = (0.5,0.5,0.5,1)
        _node_7509 ("node_7509", Float ) = 0.5
        _04 ("04", Float ) = 0.5
        _05 ("05", Float ) = 0.5
        _06 ("06", Float ) = 0.5
        _node_9455 ("node_9455", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            uniform float _01;
            uniform float _02;
            uniform float _03;
            uniform float4 _color;
            uniform float _node_7509;
            uniform float _04;
            uniform float _05;
            uniform float _06;
            uniform float4 _node_9455;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float3 normalDir : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 emissive = (((_node_9455.rgb*(1.0 - dot(float3(_04,_05,_06),i.normalDir)))+(_color.rgb*dot(float3(_01,_02,_03),i.normalDir)))*i.vertexColor.rgb*_node_7509);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
