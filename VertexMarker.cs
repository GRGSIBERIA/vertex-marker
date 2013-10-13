using UnityEngine;
using System.Collections;
using System;

public class VertexMarker : MonoBehaviour
{
	public float size = 0.005f;
	public Color markerColor = Color.magenta;
	public GameObject targetObject;

	Mesh mesh;
	Material mat;
	Vector3[] vertices;
	Vector3[] normals;
	MayaCamera mayaCamera;

	void InitializeMembers()
	{
		mesh = targetObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		mat = new Material(Shader.Find("Diffuse"));
		vertices = mesh.vertices;
		normals = mesh.normals;
		mayaCamera = GetComponent<MayaCamera>();
	}

	void Awake()
	{
		InitializeMembers();
	}

	void RenderTriangles()
	{
		var vtx_size = (size * 0.5f) * (mayaCamera.Log10LookAtLength);
		var up = Camera.main.transform.up * vtx_size;
		var right = Camera.main.transform.right * vtx_size;
		var ray = Camera.main.transform.forward;

		var neg = up - right;
		var pos = up + right;

		/*
		 * Vector3の足し算 = 20cycle
		 * GL.Vertex呼び出しx4回 = 80cycle
		 * 80cycle * 30k頂点 = 2.4Mサイクル
		 * Dot(ray, normal)で1.2Mサイクルに削減
		 */
		for (int i = 0; i < vertices.Length; i++)
		{
			if (Vector3.Dot(ray, normals[i]) < 0f)
			{
				GL.Vertex(vertices[i] + neg);
				GL.Vertex(vertices[i] + pos);
				GL.Vertex(vertices[i] - neg);
				var last = vertices[i] - pos;
				GL.Vertex(last);
			}
		}
	}

	void OnPostRender()
	{
		GL.PushMatrix();
		mat.SetPass(0);
		GL.Begin(GL.QUADS);
		GL.Color(markerColor);
		GL.Clear(true, false, Camera.main.backgroundColor);
		RenderTriangles();
		GL.End();
		GL.PopMatrix();
	}
}
