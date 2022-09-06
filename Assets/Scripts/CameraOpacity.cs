using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utilities;

public class CameraOpacity : MonoBehaviour
{
    public GameObject player;
    public Shader shaderDifuse;
    public Shader shaderTransparent;
    public float targetAlpha;
    public float time;
    public GameObject o;
    public bool mustFadeBack = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shaderDifuse = Shader.Find("Diffuse");
        shaderTransparent = Shader.Find("Transparent/Diffuse");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 30))
        {
            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {
                mustFadeBack = true;
                //
                // if (hit.collider.gameObject != o && o != null)  
                // {
                //     FadeUp(o);
                // }
                //
                // o = hit.collider.gameObject;
                //
                // if (o.GetComponent<Renderer>().material.shader != shaderTransparent)
                // {
                //     o.GetComponent<Renderer>().material.shader = shaderTransparent;
                //     Color k = o.GetComponent<Renderer>().material.color;
                //     k.a = 0.5f;
                //     o.GetComponent<Renderer>().material.color = k;
                // }

                var mat = hit.collider.gameObject.GetComponent<Renderer>().material;
                StartCoroutine(FadeDown(mat));
            }
            else
            {
                if (mustFadeBack)
                {
                    mustFadeBack = false;
                    var mat = hit.collider.gameObject.GetComponent<Renderer>().material;
                    StartCoroutine(FadeUp(mat));
                }
            }
        }
    }

    private IEnumerator FadeUp(Material f)
    {
        // fade the material alpha to 1 over 1 second
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            f.color = new Color(f.color.r, f.color.g, f.color.b, Mathf.Lerp(1, targetAlpha, t));
            yield return null;
        }
    }

    private IEnumerator FadeDown(Material f)
    {
        // fade the material alpha to 0 over 1 second
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            f.color = new Color(f.color.r, f.color.g, f.color.b, Mathf.Lerp(1, targetAlpha, t));
            yield return null;
        }
    }
}