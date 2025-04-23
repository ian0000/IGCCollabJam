using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] CardObject _cardObject;

    public Vector2 tilePosition;

    public CardObject CardObject
    {
        get { return _cardObject; }
    }

    void Start()
    {
        foreach(var effect in _cardObject.effectPrefabs)
        {
            Instantiate(effect, transform);
        }
    }
}
