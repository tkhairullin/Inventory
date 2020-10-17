// конфигуратор
using UnityEngine;

[CreateAssetMenu(fileName ="New Pickable Item", menuName ="Items/Pickable")]
public class PickableItem : ScriptableObject
{
    public string Name;
    public int ID;
    public float Weight;
    public ObjectType Type;
    public enum ObjectType { RedType, GreenType, BlueType}
    public GameObject Model;
}
