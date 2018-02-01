using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParthenonBuilder : MonoBehaviour {
    public GameObject cubePrefab;
    public GameObject cylinderPrefab;
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
    [ContextMenu("Destroy")]
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
        Pillars();
        //Roof();
    }

    void Stairs()
    {
        var stairs = new GameObject("Stairs");
        stairs.transform.parent = transform;
        stairs.transform.position = new Vector3(0,0,0);
        for (int i = 0; i < stairCount; i++)
        {
            topWidth = floorWidth * Mathf.Pow(0.9f, i);
            topDepth = floorDepth * Mathf.Pow(0.9f, i);
            var stair = Instantiate(cubePrefab, stairs.transform);
            stair.name = "Stair (" + (i + 1) + ")";
            var tr = stair.GetComponent<Transform>();
            tr.position = new Vector3(0, floorHeight * i, 0);
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
    
    void Pillars()
    {
        var pillars = new GameObject("Pillars");
        pillars.transform.parent = transform;
        HPillars(new Vector3((pillarRadius - topWidth)/2, floorHeight*stairCount, (pillarRadius-  topDepth)/2), pillars.transform);
        HPillars(new Vector3((pillarRadius - topWidth) / 2, floorHeight*stairCount, (topDepth- pillarRadius) /2), pillars.transform);
       // VPillars(new Vector3((pillarRadius - topWidth) / 2, floorHeight * stairCount, (pillarRadius - topDepth) / 2), pillars.transform);
        //VPillars(new Vector3((topWidth - pillarRadius) / 2, floorHeight * stairCount, (pillarRadius - topDepth) / 2), pillars.transform);
    }
    void HPillars(Vector3 start, Transform tr)
    {
        var pillars = new GameObject("Pillars(H)");
        pillars.transform.parent = tr;
        float hSpace = (topWidth - pillarRadius) / (pillarCountWidth - 1);
        for (int i = 0; i < pillarCountWidth; i++)
        {
            var pillar = Instantiate(cylinderPrefab, pillars.transform);
            pillar.transform.position = start;
            pillar.transform.localScale = new Vector3(pillarRadius, pillarHeight, pillarRadius);
            start.x += hSpace;
        }
    }
    void VPillars(Vector3 start, Transform tr)
    {
        var pillars = new GameObject("Pillars(V)");
        pillars.transform.parent = tr;
        float vSpace = (topDepth - pillarRadius) / (pillarCountDepth - 1);
        for (int i = 0; i < pillarCountWidth; i++)
        {
            var pillar = Instantiate(cylinderPrefab, pillars.transform);
            pillar.transform.position = start;
            pillar.transform.localScale = new Vector3(pillarRadius, pillarHeight, pillarRadius);
            start.z += vSpace;
        }
    }
}
