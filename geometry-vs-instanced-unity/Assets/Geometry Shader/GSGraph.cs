using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GSGraph : MonoBehaviour {
	[SerializeField] private ComputeShader _computeShader;

	[SerializeField] private int _resolution = 1000;

	private MeshRenderer _meshRenderer;
	private MeshFilter _meshFilter;
	private Mesh _graphMesh;
	private Vector3[] _graphVertices;
	private int[] _graphIndices;

	private ComputeBuffer _positionsBuffer;
	private ComputeBuffer _indicesBuffer;

	private static readonly int POSITIONS_ID = Shader.PropertyToID("_Positions");
	private static readonly int INDICES_ID = Shader.PropertyToID("_Indices");
	private static readonly int RESOLUTION_ID = Shader.PropertyToID("_Resolution");
	private static readonly int STEP_ID = Shader.PropertyToID("_Step");
	private static readonly int TIME_ID = Shader.PropertyToID("_Time");

	private void OnEnable() {
		_meshFilter = GetComponent<MeshFilter>();
		_meshRenderer = GetComponent<MeshRenderer>();

		int verticesCount = _resolution * _resolution;
		_positionsBuffer = new ComputeBuffer(verticesCount, Marshal.SizeOf(typeof (Vector3)));
		_indicesBuffer = new ComputeBuffer(verticesCount, sizeof (int));

		_graphMesh = new() {
			name = "Graph",
			// set the index buffer to 32 bit, so up to 4 billion vertices are supported
			// instead of 65536 (default)
			indexFormat = UnityEngine.Rendering.IndexFormat.UInt32,
		};

		_graphVertices = new Vector3[verticesCount];
		_graphIndices = new int[verticesCount];

		UpdateGraphMesh();
	}

	private void OnDisable() {
		_positionsBuffer.Release();
		_positionsBuffer = null;
		_indicesBuffer.Release();
		_indicesBuffer = null;
	}

	private void Update() {
	}

	private void UpdateGraphMesh() {
		float step = 2f / _resolution;

		_meshRenderer.material.SetFloat(STEP_ID, step);

		_computeShader.SetInt(RESOLUTION_ID, _resolution);
		_computeShader.SetFloat(STEP_ID, step);
		_computeShader.SetFloat(TIME_ID, Time.time);

		_computeShader.SetBuffer(0, POSITIONS_ID, _positionsBuffer);
		_computeShader.SetBuffer(0, INDICES_ID, _indicesBuffer);

		int groups = Mathf.CeilToInt(_resolution / 8f);
		_computeShader.Dispatch(0, groups, groups, 1);

		_positionsBuffer.GetData(_graphVertices);
		_indicesBuffer.GetData(_graphIndices);

		_graphMesh.vertices = _graphVertices;
		_graphMesh.SetIndices(_graphIndices, MeshTopology.Points, 0);

		_meshFilter.mesh = _graphMesh;
	}
}
