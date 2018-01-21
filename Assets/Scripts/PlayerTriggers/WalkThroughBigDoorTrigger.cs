using UnityEngine;

public class WalkThroughBigDoorTrigger : APlayerTrigger
{
    private Light mysteryLight;

    private const float finalIntensity = 3f;
    private const float dimDurationMs = 250;

    private float dimDiff;
    private float dimTimeMs;

    private bool doDim = false;

    void Start()
    {
        this.mysteryLight = this.GetComponentInParent<Light>();

        this.dimDiff = this.mysteryLight.intensity - finalIntensity;
        this.dimTimeMs = dimDurationMs;
    }

    public void Update()
    {
        if (this.doDim)
        {
            this.DimMysteryLight();
        }
    }

    private void DimMysteryLight()
    {
        var deltaTimeMs = Time.deltaTime * 1000;
        if (this.dimTimeMs > 0)
        {
            var dimSlice = (deltaTimeMs / dimDurationMs) * this.dimDiff;

            this.mysteryLight.intensity -= dimSlice;

            this.dimTimeMs -= deltaTimeMs;
        }
        else
        {
            this.mysteryLight.intensity = Mathf.Max(this.mysteryLight.intensity, finalIntensity);
            Destroy(this);
        }
    }

    public override void Interact(GameObject player)
    {
        this.doDim = true;
    }
}
