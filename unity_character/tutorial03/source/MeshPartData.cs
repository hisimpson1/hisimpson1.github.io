using UnityEngine;

[CreateAssetMenu(menuName = "Character/Part/Mesh")]
public class MeshPartData : PartData, IEquippablePart
{
    public Mesh mesh;
    public PartType type; // Ear, Hair, Face, Tail 등

    public PartType Type => type;

    public void Equip(CharacterCustomizer customizer)
    {
        customizer.EquipMesh(this);
    }
}