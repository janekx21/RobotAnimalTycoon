using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : Building {
    public override void Start() {
        base.Start();
        if(colliders.Length == 0) {
            var b = new GameObject("c", typeof(BuildingCollider),typeof(BoxCollider),typeof(Rigidbody));
            b.transform.parent = transform;
            b.transform.localPosition = Vector3.zero;
            b.GetComponent<Rigidbody>().isKinematic = true;
            var c = b.GetComponent<BoxCollider>();
            c.isTrigger = true;
            c.size = repr.GetComponent<MeshFilter>().mesh.bounds.size;
            c.center = repr.GetComponent<MeshFilter>().mesh.bounds.center;
            b.transform.rotation = repr.transform.rotation;

            var bc = b.GetComponent<BuildingCollider>();
            bc.origen = this;
            colliders = new BuildingCollider[1] {bc};
        }
    }
}
