using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingButton : MonoBehaviour {

    Material material;

    private const string EMISSION = "_EMISSION";

	// Use this for initialization
	void Start () {
        var renderer = this.GetComponent<Renderer>();

        this.material = renderer.material;
	}
	
	// Update is called once per frame
	void Update () {

        // float deltaTimeMs = Time.deltaTime * 1000;
        float onOff = Mathf.PingPong(Time.time, 1);

        if (onOff > 0.50f)
        {
            this.material.EnableKeyword(EMISSION);
        }
        else
        {
            this.material.DisableKeyword(EMISSION);
        }
	}
}
