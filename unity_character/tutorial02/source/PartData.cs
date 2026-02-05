using UnityEngine;

public abstract class PartData : ScriptableObject
{
    public string partName;
    public Sprite icon;

    //C# expression-bodied property 문법, Read-Only 속성
    public abstract PartCategory Category { get; }  // Wearable / Mesh

    public PartType type;          // Upper, Pants, Ear, Hair 등
}
