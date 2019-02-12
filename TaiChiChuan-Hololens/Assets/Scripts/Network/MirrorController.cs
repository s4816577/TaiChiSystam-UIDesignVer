using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Windows;

public class MirrorController : MonoBehaviour
{
    static List<Transform> parameter;
    List<Transform> mirrorPositions;
    private int positionsInd = 0;
    private readonly Vector3 mirrorShift = new Vector3(0.0f, 0.3f, 0.0f);

    private GameObject plane;
    private Texture2D planeMainTexture;

    private const int BUFFER_SIZE = 2;
    private List<byte[]> imageBuffers = new List<byte[]>();
    private int bufferIndex = 0;


    static public GameObject InstantiateGameObject(List<Transform> mirrorPositions)
    {
        parameter = mirrorPositions;
        GameObject g = Instantiate(ResourcePool.GetInstance().GetMirrorPrefab());
        MirrorController controller = g.AddComponent<MirrorController>();

        return g;
    }

    private void Awake()
    {
        mirrorPositions = parameter;
        this.transform.position = mirrorPositions[positionsInd].position + mirrorShift;
        this.transform.forward = this.transform.position - Camera.main.transform.position;

        plane = this.transform.Find("Plane").gameObject;
        planeMainTexture = new Texture2D(480, 480);

        for (int i = 0; i < BUFFER_SIZE; ++i)
            imageBuffers.Add(null);

        UdpNetworkServer.GetInstance().OnImageReceivedEvent += OnImageReceived;
    }

    private void OnImageReceived(byte[] imageBytes)
    {
        imageBuffers[bufferIndex] = imageBytes;
        bufferIndex = (bufferIndex + 1) % BUFFER_SIZE;
    }

    private void Update()
    {
        UpdatePosition();

        if (bufferIndex == -1)
            return;

        planeMainTexture.LoadImage(imageBuffers[bufferIndex]);
        plane.GetComponent<Renderer>().material.mainTexture = planeMainTexture;
        plane.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
    }

    private void UpdatePosition()
    {
        //if (this.gameObject.activeSelf)
        //{
        float currentAngle = Vector3.Angle(Camera.main.transform.forward, this.transform.position - Camera.main.transform.position);

        int newInd1 = (positionsInd + 1) % mirrorPositions.Count;
        float newAngle1 = Vector3.Angle(Camera.main.transform.forward, mirrorPositions[newInd1].position - Camera.main.transform.position);

        int newInd2 = (positionsInd - 1 + mirrorPositions.Count) % mirrorPositions.Count;
        float newAngle2 = Vector3.Angle(Camera.main.transform.forward, mirrorPositions[newInd2].position - Camera.main.transform.position);

        if (newAngle1 < currentAngle)
        {
            currentAngle = newAngle1;
            positionsInd = newInd1;
        }
        if (newAngle2 < currentAngle)
        {
            currentAngle = newAngle2;
            positionsInd = newInd2;
        }

        this.transform.position = mirrorPositions[positionsInd].position + mirrorShift;
        this.transform.forward = this.transform.position - Camera.main.transform.position;
        //}
    }

    private void OnDestroy()
    {
        UdpNetworkServer.GetInstance().OnImageReceivedEvent -= OnImageReceived;
    }
}