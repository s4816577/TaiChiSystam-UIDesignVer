using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHintController : MonoBehaviour {

    private GameObject leftSphere;
    private GameObject rightSphere;

    private void Awake()
    {
        leftSphere = transform.Find("Left").gameObject;
        rightSphere = transform.Find("Right").gameObject;
    }

    // Use this for initialization
    private void Start ()
    {

	}
	
	// Update is called once per frame
	private void Update ()
    {

	}

    public void SetLeft()
    {
        leftSphere.SetActive(true);
        rightSphere.SetActive(false);
    }

    public void SetRight()
    {
        leftSphere.SetActive(false);
        rightSphere.SetActive(true);
    }
}
