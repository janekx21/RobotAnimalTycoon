using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour {

	public MeshFilter line;

	public Vector3 posA;
	public Vector3 dirA;
	public Vector3 dirB;
	public Vector3 posB;
	Vector3[] baseMesh;

	public Transform start;
	public Transform end;

	private void Awake() {
		baseMesh = line.mesh.vertices.Clone() as Vector3[];

	}

	public void GenerateTransforms() {
		start = new GameObject("start").transform;
		start.position = posA;
		start.rotation = Quaternion.LookRotation(dirA - posA, Vector3.up);
		start.localScale = new Vector3(0, 0, (dirA - posA).magnitude);
		start.parent = transform;

		end = new GameObject("end").transform;
		end.position = posB;
		end.rotation = Quaternion.LookRotation(posB - dirB, Vector3.up);
		end.localScale = new Vector3(0, 0, -(dirB - posB).magnitude);
		end.parent = transform;
	}

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateModel() {
		var vertex = baseMesh.Clone() as Vector3[];
		//var copy = vertex.Clone();
		for (int i = 0; i < vertex.Length; i++) {
			Vector3 off = new Vector3(vertex[i].x, vertex[i].y, 0);
			vertex[i] = benziner(vertex[i].z) + 
				Quaternion.Lerp(start.rotation, end.rotation, vertex[i].z) * off;
		}
		line.mesh.SetVertices(new List<Vector3>(vertex));
		//line.mesh.UploadMeshData(false);
		
		line.mesh.RecalculateNormals();
		line.mesh.RecalculateTangents();
		line.mesh.RecalculateBounds();
	}

	Vector3 benziner(float t) {
		Vector3 a = start.position - transform.position;
		Vector3 b = start.position + start.rotation * (Vector3.forward* start.localScale.z) - transform.position;
		Vector3 c = end.position + end.rotation*(Vector3.forward*end.localScale.z) - transform.position;
		Vector3 d = end.position - transform.position;

		var e = Vector3.Lerp(a, b, t);
		var f = Vector3.Lerp(b, c, t);
		var g = Vector3.Lerp(c, d, t);

		var h = Vector3.Lerp(e, f, t);
		var i = Vector3.Lerp(f, g, t);

		return Vector3.Lerp(h, i, t);
	}

	private void OnDrawGizmosSelected() {
		int max = 5;
		if (start && end) {
			Vector3[] poses = new Vector3[max + 1];
			Gizmos.color = Color.red;
			Gizmos.matrix = transform.localToWorldMatrix;
			for (int i = 0; i < max + 1; i++) {
				poses[i] = benziner((float)i / (float)max);
				Vector3 other = benziner((float)i / (float)max + .01f);
				Vector3 off = other - poses[i];
				Gizmos.DrawLine(poses[i], poses[i] + off.normalized);
				Gizmos.DrawLine(poses[i], poses[i] + Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, off, Vector3.up), Vector3.up) * Vector3.right);
			}
			Gizmos.color = Color.green;
			for (int i = 0; i < max; i++) {
				Gizmos.DrawLine(poses[i], poses[i + 1]);
			}
		}
	}
}
