using UnityEngine;

public class CSGraph : MonoBehaviour {
	[SerializeField] private ComputeShader _computeShader;

	[SerializeField] private Material _pointMaterial;

	[SerializeField] private Mesh _pointMesh;

	[SerializeField] private int _resolution = 1000;

	private ComputeBuffer _positionsBuffer;

	private static readonly int POSITIONS_ID = Shader.PropertyToID("_Positions");
	private static readonly int RESOLUTION_ID = Shader.PropertyToID("_Resolution");
	private static readonly int STEP_ID = Shader.PropertyToID("_Step");
	private static readonly int TIME_ID = Shader.PropertyToID("_Time");

	private void OnEnable() {
		// to store a position we need 3 float values
		_positionsBuffer = new ComputeBuffer(_resolution * _resolution, 3 * sizeof (float));

		UpdateGraph();
	}

	private void OnDisable() {
		_positionsBuffer.Release();
		_positionsBuffer = null;
	}

	private void Update() {
		RenderGraph();
	}

	private void UpdateGraph() {
		float step = 2f / _resolution;

		_computeShader.SetInt(RESOLUTION_ID, _resolution);
		_computeShader.SetFloat(STEP_ID, step);
		_computeShader.SetFloat(TIME_ID, Time.time);

		_computeShader.SetBuffer(0, POSITIONS_ID, _positionsBuffer);

		int groups = Mathf.CeilToInt(_resolution / 8f);
		_computeShader.Dispatch(0, groups, groups, 1);
	}

	private void RenderGraph() {
		float step = 2f / _resolution;

		var rp = new RenderParams(_pointMaterial) {
			// bounds for frustum culling
			worldBounds = new(Vector3.zero, Vector3.one * (2f + step)),
			matProps = new(),
		};
		rp.matProps.SetFloat(STEP_ID, step);
		rp.matProps.SetBuffer(POSITIONS_ID, _positionsBuffer);

		Graphics.RenderMeshPrimitives(rp, _pointMesh, 0, _positionsBuffer.count);
	}
}
