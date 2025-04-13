using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public string description;

    protected abstract void DoEffect();
}
