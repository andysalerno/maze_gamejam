using UnityEngine;

public abstract class APlayerInteractionAction : MonoBehaviour
{
    public abstract float DistanceActionable { get; }

    public abstract void Action(PlayerInteract source);
}
