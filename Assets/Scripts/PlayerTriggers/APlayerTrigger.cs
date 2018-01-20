using UnityEngine;

public abstract class APlayerTrigger : MonoBehaviour
{
    public abstract void Interact(GameObject player);

    private const string PLAYER_TAG = "Player";

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OncollisionEnter called");
        if (other.gameObject.tag == PLAYER_TAG)
        {
            Debug.Log($"Firing PlayerTrigger: {this.GetType().Name}");
            this.Interact(other.gameObject);
        }
    }
}
