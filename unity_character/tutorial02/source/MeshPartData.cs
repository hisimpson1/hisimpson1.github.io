using UnityEngine;

[CreateAssetMenu(menuName = "Character/Part/Mesh")]
public class MeshPartData : PartData
{
    public Mesh mesh;
    public override PartCategory Category => PartCategory.Mesh;
}
