Shader "Unlit/Graph Point" {

	Properties { }

	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma nolightmap nodirlightmap nodynlightmap novertexlight
			// disable async compilation, because the cyan dummy shader does not work with instancing
			#pragma editor_sync_compilation
			#pragma target 4.5

			float3 _Color;

#if SHADER_TARGET >= 45
			StructuredBuffer<float3> _Positions;
			float _Step;
#endif

			struct MeshData {
				float4 vertex : POSITION;
			};

			struct FragmentData {
				float4 vertex : SV_POSITION;
				float3 world_pos : TEXCOORD0;
			};

			FragmentData vert(MeshData mesh_data, uint instance_id : SV_InstanceID) {

#if SHADER_TARGET >= 45
				float3 position = _Positions[instance_id];

				unity_ObjectToWorld = 0.0;
				unity_ObjectToWorld._m03_m13_m23_m33 = float4(position, 1.0);
				unity_ObjectToWorld._m00_m11_m22 = _Step;
#endif

				FragmentData output;
				output.vertex = UnityObjectToClipPos(mesh_data.vertex);
				output.world_pos = mul(unity_ObjectToWorld, mesh_data.vertex);
				return output;
			}

			float4 frag(FragmentData input) : SV_Target {
				return float4(saturate(input.world_pos * 0.5 + 0.5), 1);
			}
			ENDCG
		}
	}
}
