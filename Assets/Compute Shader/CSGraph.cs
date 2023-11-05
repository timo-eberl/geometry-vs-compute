using UnityEngine;
using UnityEngine.Serialization;

public class CSGraph : MonoBehaviour {
	[SerializeField] ComputeShader _computeShader;

	[SerializeField] private Material _pointMaterial;

	[SerializeField] Mesh _pointMesh;

	[SerializeField, Range(10, 1000)] int _resolution = 100;

	ComputeBuffer positionsBuffer;

	static readonly int POSITIONS_ID = Shader.PropertyToID("_Positions");
	static readonly int RESOLUTION_ID = Shader.PropertyToID("_Resolution");
	static readonly int STEP_ID = Shader.PropertyToID("_Step");
	static readonly int TIME_ID = Shader.PropertyToID("_Time");

	void OnEnable() {
		// to store a position we need 3 float values, one float needs 4 Bytes
		positionsBuffer = new ComputeBuffer(_resolution * _resolution, 3 * 4);
	}

	void OnDisable() {
		positionsBuffer.Release();
		positionsBuffer = null;
	}

	void Update() {
		UpdateFunctionOnGPU();
	}

	void UpdateFunctionOnGPU() {
		float step = 2f / _resolution;

		_computeShader.SetInt(RESOLUTION_ID, _resolution);
		_computeShader.SetFloat(STEP_ID, step);
		_computeShader.SetFloat(TIME_ID, Time.time);

		_computeShader.SetBuffer(0, POSITIONS_ID, positionsBuffer);

		int groups = Mathf.CeilToInt(_resolution / 8f);
		_computeShader.Dispatch(0, groups, groups, 1);

		var rp = new RenderParams(_pointMaterial) {
			// bounds for frustum culling
			worldBounds = new(Vector3.zero, Vector3.one * (2f + step)),
			matProps = new(),
		};
		rp.matProps.SetFloat(STEP_ID, step);
		rp.matProps.SetBuffer(POSITIONS_ID, positionsBuffer);

		Graphics.RenderMeshPrimitives(rp, _pointMesh, 0, positionsBuffer.count);
	}
}
