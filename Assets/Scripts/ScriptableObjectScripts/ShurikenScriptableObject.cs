using UnityEngine;


[CreateAssetMenu(fileName = "ShurikenData", menuName = "ScriptableObjects/Shuriken")]
public class ShurikenScriptableObject : ScriptableObject
{
    public GameObject OnCollideWithShurikenPrefab;
    public GameObject ThrowableShurikenPrefab;
    public Sprite ShurikenSprite;
    public float CoolDown;
    public int Capacity;
    public float ShurikenSpeed;
    public float CoolDownToTakeAgain;
}
