using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int HP { get; private set; }
    public int ATK { get; private set; }

    public void Init(int hp, int atk)
    {
        HP = hp;
        ATK = atk;
        Debug.Log($"Initialized Enemy: {HP} HP / {ATK} ATK");
    }
}