using UnityEngine;

[CreateAssetMenu(menuName = "Character/Part/Wearable")]
public class WearablePartData : PartData, IEquippablePart
{
    public SkinnedMeshRenderer prefab;
    // Upper, Pants, Gloves, Shoes 등
    public PartType type; // 인스펙터에서 지정

    public PartType Type => type; // 인터페이스 구현

    public void Equip(CharacterCustomizer customizer)
    {
        customizer.EquipWearable(this);
    }
}
