using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParthenonBuilder : MonoBehaviour {
    public GameObject cubePrefab;
    public GameObject pillar;
    public float floorWidth;
    public float floorDepth;
    public float floorHeight;
    public float pillarRadius;
    public float pillarHeight;
    public int pillarCountWidth;
    public int pillarCountDepth;
    public int stairCount;
    public float roofHeight;
    public Material floorMaterial;
    public Material pillarMaterial;
    public Material roofMaterial;

    private float topWidth;
    private float topDepth;
    private void DestroyAll()
    {
        //하이알키 상에서 하위에 있는 애들을 돌면서 뽑아내는 그것 다만 여기에선 transform자체를 뽑아내는 것인지라 .gameObject를 추가해주는 것이다. 
        foreach (Transform t in transform.Cast<Transform>().ToArray())
        {
            DestroyImmediate(t.gameObject);//그 뽑아낸 걸 없애라. 
        }
    }

    [ContextMenu("Build")]
    private void Build()
    {
        DestroyAll();
        Stairs();
        Roof();
    }

    void Stairs()
    {
        var stairs = new GameObject("Stairs");
        stairs.transform.parent = transform;
        for (int i = 0; i < stairCount; i++)
        {
            topWidth = floorWidth * Mathf.Pow(0.9f, i);
            topDepth = floorDepth * Mathf.Pow(0.9f, i);
            var stair = Instantiate(cubePrefab, stairs.transform);
            stair.name = "Stair (" + (i + 1) + ")";
            var tr = stair.GetComponent<Transform>();
            tr.position = new Vector3(0, floorHeight/2 + floorHeight * i, 0);
            tr.localScale = new Vector3(topWidth, floorHeight, topDepth);
        }
        foreach (MeshRenderer m in stairs.GetComponentsInChildren<MeshRenderer>())
        {
            m.material = floorMaterial;
        }
    }

    void Roof()
    {
        var roof = Instantiate(cubePrefab, transform);
        roof.name = "Roof";
        roof.transform.position = new Vector3(0, roofHeight/2 + floorHeight*stairCount + pillarHeight, 0);
        roof.transform.localScale = new Vector3(topWidth, roofHeight, topDepth);
        roof.GetComponent<MeshRenderer>().material = roofMaterial;
    }
}
