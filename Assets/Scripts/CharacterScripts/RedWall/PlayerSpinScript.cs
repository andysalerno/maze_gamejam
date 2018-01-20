using UnityEngine;

public class SpinScript : MonoBehaviour
{
    void Update()
    {
        this.transform.RotateAround(this.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
