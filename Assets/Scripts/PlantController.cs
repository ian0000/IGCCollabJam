using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] CardObject _cardObject;

    public Vector2 tilePosition;
    public int attack;
    public int health;

    public CardObject CardObject
    {
        get { return _cardObject; }
    }

    void Start()
    {
        attack = _cardObject.attack;
        health = _cardObject.health;
        foreach(var effect in _cardObject.effectPrefabs)
        {
            Instantiate(effect, transform);
        }
    }
}
