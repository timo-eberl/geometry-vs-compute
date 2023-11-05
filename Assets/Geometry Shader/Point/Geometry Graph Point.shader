Shader "Unlit/Geometry Graph Point" {

	Properties { }

	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom

			// disable async compilation, because the cyan dummy shader does not work with instancing
			#pragma editor_sync_compilation
			#pragma require geometry
			#pragma target 4.5

			float3 _Color;
			float _Step;

			struct MeshData {
				float4 vertex : POSITION;
			};

			struct VertexToGeometryData {
				float4 vertex : SV_POSITION;
				float3 world_pos : TEXCOORD0;
			};

			struct FragmentData {
				float4 vertex : SV_POSITION;
				float3 world_pos : TEXCOORD0;
			};

			VertexToGeometryData vert(MeshData mesh_data, uint instance_id : SV_InstanceID) {
				VertexToGeometryData output;
				output.vertex = mesh_data.vertex;
				output.world_pos = mul(unity_ObjectToWorld, mesh_data.vertex);
				return output;
			}

			[maxvertexcount(27)]
			void geom(
				point VertexToGeometryData points[1], inout TriangleStream<FragmentData> tri_stream
			) {
				float3 root = points[0].vertex.xyz;
				float c = _Step * 0.5; // half edge length

				/*
				// blender cube
				float3 vertices[8];
				vertices[0] = root + float3(+c, +c, -c);
				vertices[1] = root + float3(+c, -c, -c);
				vertices[2] = root + float3(+c, +c, +c);
				vertices[3] = root + float3(+c, -c, +c);
				vertices[4] = root + float3(-c, +c, -c);
				vertices[5] = root + float3(-c, -c, -c);
				vertices[6] = root + float3(-c, +c, +c);
				vertices[7] = root + float3(-c, -c, +c);
				uint3 faces[12];
				faces[0]  = uint3(4,2,0); // 531
				faces[1]  = uint3(2,7,3); // 384
				faces[2]  = uint3(6,5,7); // 768
				faces[3]  = uint3(1,7,5); // 286
				faces[4]  = uint3(0,3,1); // 142
				faces[5]  = uint3(4,1,5); // 526
				faces[6]  = uint3(4,6,2); // 573
				faces[7]  = uint3(2,6,7); // 378
				faces[8]  = uint3(6,4,5); // 756
				faces[9]  = uint3(1,3,7); // 248
				faces[10] = uint3(0,2,3); // 134
				faces[11] = uint3(4,0,1); // 512
				*/

				// own cube, because I didn't get the Blender cube to work ;-;
				float3 vertices[8];
				vertices[0] = root + float3(-c, -c, +c);
				vertices[1] = root + float3(+c, -c, +c);
				vertices[2] = root + float3(+c, -c, -c);
				vertices[3] = root + float3(-c, -c, -c);
				vertices[4] = root + float3(-c, +c, +c);
				vertices[5] = root + float3(+c, +c, +c);
				vertices[6] = root + float3(+c, +c, -c);
				vertices[7] = root + float3(-c, +c, -c);
				// I literally drew a cube on a piece of paper and connected the lines
				// so this is probably not very optimized
				uint indices[27] = {0,4,5,0,1,5,6,7,5,4,7,3,4,0,3,2,0,1,5,6,1,2,6,7,2,3,7};

				for (int i = 0; i < 27; ++i) {
					FragmentData vert;
					vert.world_pos = vertices[indices[i]];
					vert.vertex = UnityObjectToClipPos(float4(vertices[indices[i]], 0));
					tri_stream.Append(vert);
				}
			}

			float4 frag(FragmentData input) : SV_Target {
				return float4(saturate(input.world_pos * 0.5 + 0.5), 1);
			}
			ENDCG
		}
	}
}
