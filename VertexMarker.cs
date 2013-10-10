using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 頂点を表示したいGameObjectにアサインする
/// ただし，対象のGameObjectにはSkinnedMeshRendererがあること
/// </summary>
public class VertexMarker : MonoBehaviour
{
	public GameObject marker;
	public float marker_size = 0.01f;

	float prev_marker_size;
	Mesh target_mesh;
	Mesh marker_mesh;
	Material non_select_color;
	Vector3[] original_vertices;

	void Start()
	{
		var target = gameObject;

		target_mesh = target.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		marker_mesh = CopyMesh();
		non_select_color = marker.renderer.material;

		AssignMarker(target, marker_size);
		ChangeMarkerSize();

		prev_marker_size = marker_size;
	}

	void Update()
	{
		if (marker_size != prev_marker_size)
		{

		}
		prev_marker_size = marker_size;
	}

	void ChangeMarkerSize()
	{
		for (int i = 0; i < marker_mesh.vertexCount; i++)
			marker_mesh.vertices[i] = original_vertices[i] * marker_size;
	}

	Mesh CopyMesh()
	{
		var mesh = marker.GetComponent<MeshFilter>().sharedMesh;
		var new_mesh = new Mesh();

		new_mesh.vertices = new Vector3[mesh.vertexCount];
		original_vertices = new Vector3[mesh.vertexCount];
		for (int i = 0; i < mesh.vertexCount; i++)
			new_mesh.vertices[i] = mesh.vertices[i];
		for (int i = 0; i < mesh.vertexCount; i++)
			original_vertices[i] = mesh.vertices[i];

		new_mesh.triangles = new int[mesh.triangles.Length];
		for (int i = 0; i < mesh.triangles.Length; i++)
			new_mesh.triangles[i] = mesh.triangles[i];

		new_mesh.normals = new Vector3[mesh.normals.Length];
		for (int i = 0; i < mesh.normals.Length; i++)
			new_mesh.normals[i] = mesh.normals[i];

		new_mesh.uv = new Vector2[mesh.uv.Length];
		for (int i = 0; i < mesh.uv.Length; i++)
			new_mesh.uv[i] = mesh.uv[i];

		return new_mesh;
	}

	void AssignMarker(GameObject root, float marker_size)
	{
		var marker_branch = CreateBranchedMarkerObject(root);
		StructureVertexMarkers(marker_branch.transform, marker_size);
	}

	public void InstantiateMarker(int number, GameObject marker, Transform branch, Vector3 mesh_vtx, float size)
	{
		var obj = new GameObject(number.ToString());
		var transform = obj.transform;
		transform.parent = branch;
		transform.localPosition = mesh_vtx;
		obj.AddComponent<MeshFilter>().sharedMesh = marker_mesh;
		var renderer = obj.AddComponent<MeshRenderer>();
		renderer.materials[0] = non_select_color;
		renderer.castShadows = false;
		renderer.receiveShadows = false;
	}

	void StructureVertexMarkers(Transform marker_branch_transform, float marker_size)
	{
		for (int i = 0; i < target_mesh.vertices.Length; i++)
		{
			InstantiateMarker(i, marker, marker_branch_transform, target_mesh.vertices[i], marker_size);
		}
	}

	GameObject CreateBranchedMarkerObject(GameObject root)
	{
		var marker_branch = new GameObject("markers");
		marker_branch.transform.parent = root.transform;
		return marker_branch;
	}
}
