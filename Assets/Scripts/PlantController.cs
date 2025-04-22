using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] CardObject _cardObject;

    public Vector2 tilePosition;

    void Start()
    {
        if (_cardObject.effectPrefab)
        {
            Instantiate(_cardObject.effectPrefab, transform);
        }
    }
}
