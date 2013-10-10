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
	public Color marker_color;
	public float marker_size = 0.01f;

	float prev_marker_size;
	Mesh target_mesh;

	void Start()
	{
		var target = gameObject;
		target_mesh = target.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		AssignMarker(target, marker_size);
		prev_marker_size = marker_size;
	}

	void Update()
	{
		if (marker_size != prev_marker_size)
		{

		}
		prev_marker_size = marker_size;
	}

	void AssignMarker(GameObject root, float marker_size)
	{
		var marker_branch = CreateBranchedMarkerObject(root);
		StructureVertexMarkers(marker_branch.transform, marker_size);
	}

	public void InstantiateMarker(int number, GameObject marker, Transform branch, Vector3 mesh_vtx, float size)
	{
		var obj = GameObject.Instantiate(marker) as GameObject;
		var transform = obj.transform;
		obj.name = number.ToString();
		transform.parent = branch;
		transform.localPosition = mesh_vtx;
		transform.localScale = Vector3.one * size;
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
