using UnityEngine;
using System.Collections;

/// <summary>
/// メッシュ付きのGameObjectの頂点にCubeのマーカーを付ける
/// </summary>
public class VertexMarker 
{
	public static void AssignMarker(GameObject root, Mesh mesh, Vector3 marker_size)
	{
		var primitive_mesh = GetPrimitivesSharedMesh();
		var marker_branch = CreateBranchedMarkerObject(root);
		StructureVertexMarkers(mesh, marker_branch, primitive_mesh, marker_size);
	}

	static void StructureVertexMarkers(Mesh root_mesh, GameObject marker_branch, Mesh primitive_mesh, Vector3 marker_size)
	{
		for (int i = 0; i < root_mesh.vertices.Length; i++)
		{
			var obj = new GameObject(i.ToString());
			var transform = obj.transform;
			transform.parent = marker_branch.transform;
			transform.localPosition = root_mesh.vertices[i];
			transform.localScale = marker_size;
			obj.AddComponent<MeshFilter>().sharedMesh = primitive_mesh;
			obj.AddComponent<MeshRenderer>().material = new Material("Unlit/Masked Colored");
		}
	}

	static Mesh GetPrimitivesSharedMesh()
	{
		var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
		return primitive.GetComponent<MeshFilter>().sharedMesh;
	}

	static GameObject CreateBranchedMarkerObject(GameObject root)
	{
		var marker_branch = new GameObject("markers");
		marker_branch.transform.parent = root.transform;
		return marker_branch;
	}
}
