using UnityEngine;

public abstract class PartData : ScriptableObject
{
    [Header("Common Info")]
    public string partName;   // 파츠 이름
    public Sprite icon;       // UI 아이콘
}
