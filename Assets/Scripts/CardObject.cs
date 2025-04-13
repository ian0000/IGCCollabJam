using UnityEngine;

public enum CardType
{
    PLANT,
    TRAP,
    FERTILIZER,
}

[CreateAssetMenu(menuName = "CardObject")]
public class CardObject : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    [TextArea] public string description;
    public Sprite portrait;
    public int seedCount;
    public int attack;
    public int health;
    public Effect effect;

    public void DoEffect()
    {
        Instantiate(effect);
    }
}
