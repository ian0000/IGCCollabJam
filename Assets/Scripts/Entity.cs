using UnityEngine;

[CreateAssetMenu(menuName = "CardObject")]
public class CardObject : ScriptableObject
{
    public string cardName;
    [TextArea] public string description;
    public Texture2D portrait;
    public int seedCount;
    public int attack;
    public int health;
    // public Effect effect; // TODO: What to do for these?
}
