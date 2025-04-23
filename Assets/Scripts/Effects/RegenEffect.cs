using System;
using UnityEngine;

public class RegenEffect : MonoBehaviour
{
    [SerializeField] int _turnsToRegen;
    [SerializeField] int _maxHealth;

    PlantController _parent;
    int _turnCount;

    void Start()
    {
        _parent = GetComponentInParent<PlantController>();
        GameManager.turnEnded += HandleTurnIncrement;
    }

    void HandleTurnIncrement()
    {
        _turnCount++;
        if (_turnCount == _turnsToRegen)
        {
            _turnCount = 0;
            if (_parent.health < _maxHealth)
            {
                _parent.health++;
            }
        }
    }
}
