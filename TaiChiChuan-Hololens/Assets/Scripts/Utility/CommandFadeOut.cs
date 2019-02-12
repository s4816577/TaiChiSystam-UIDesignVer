using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFadeOut : MonoBehaviour
{
    private TextMesh textMesh;
    private const float FADE_OUT_TIME = 3.0f;
    private float time = 0.0f;

    void Awake()
    {
        textMesh = gameObject.GetComponent<TextMesh>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > FADE_OUT_TIME)
            Destroy(gameObject);

        Color color = textMesh.color;
        color.a = 1.0f - (time / FADE_OUT_TIME);
        textMesh.color = color;
    }

    public void SetCommandText(string str)
    {
        textMesh.text = str;
    }
}
