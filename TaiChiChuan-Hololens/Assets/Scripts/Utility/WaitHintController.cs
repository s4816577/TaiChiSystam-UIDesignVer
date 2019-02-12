using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitHintController : MonoBehaviour
{
    private const float DISTANCE2CAMERA = 1.4f;

    private const int NUM_OF_CAPSULE = 8;
    private const float RADIUS = 0.1f;
    private GameObject weightHintCapsule;

    private float time = 0.0f;
    private float degree = 0.0f;
    private const float DEGREE_PER_SECOND = 1.0f;

    static public GameObject InstantiateGameObject()
    {
        GameObject g = new GameObject();
        g.name = "WaitHint";
        g.AddComponent<WaitHintController>();

        return g;
    }

    // Use this for initialization
    void Start()
    {
        weightHintCapsule = ResourcePool.GetInstance().GetWaitHintCapsulePrefab();
        GameObject prefab = Instantiate(weightHintCapsule);
        MeshRenderer[] originRenderer = prefab.GetComponentsInChildren<MeshRenderer>();
        UnityEngine.Object.Destroy(prefab);

        for (int i = 0; i < NUM_OF_CAPSULE; ++i)
        {
            GameObject capsule = UnityEngine.Object.Instantiate(weightHintCapsule, this.transform);

            capsule.transform.position = transform.position + Vector3.up * RADIUS;
            capsule.transform.RotateAround(transform.position, transform.forward, i * 360.0f / NUM_OF_CAPSULE);

            MeshRenderer[] newRenderer = capsule.GetComponentsInChildren<MeshRenderer>();
            Material newMaterial = new Material(originRenderer[0].material)
            {
                color = new Color(1.0f / NUM_OF_CAPSULE * i, 1.0f / NUM_OF_CAPSULE * i, 1.0f / NUM_OF_CAPSULE * i)
            };
            foreach (MeshRenderer renderer in newRenderer)
            {
                renderer.material = newMaterial;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position + DISTANCE2CAMERA * Camera.main.transform.forward;
        transform.forward = Camera.main.transform.forward;

        degree += Time.deltaTime * 100;
        transform.Rotate(-Vector3.forward * degree);
    }
}
