using UnityEngine;

public class MysteryDoorRise : MonoBehaviour
{
    private bool doSlideUp = false;
    private float amountToSlide = 10f;
    private float speed = 1f;

    public void SlideUp()
    {
        this.doSlideUp = true;
    }

    void Update()
    {
        if (this.doSlideUp)
        {
            if (amountToSlide > 0)
            {
                var delta = speed * Time.deltaTime;
                this.transform.Translate(Vector3.up * delta);

                this.amountToSlide -= delta;
            }
            else
            {
                // all done
                Destroy(this);
            }
        }
    }
}
