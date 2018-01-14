using UnityEngine;

public class SpinScript : MonoBehaviour
{
    private bool doSpinning = true;

    public float spinSpeed;

    public void StartSpinning()
    {
        this.doSpinning = true;
    }

    public void StopSpinning()
    {
        this.doSpinning = false;
    }

    void Update()
    {
        this.transform.RotateAround(this.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
