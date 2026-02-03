using UnityEngine;

[CreateAssetMenu(menuName = "Character/Part/Wearable")]
public class WearablePartData : PartData
{
    public SkinnedMeshRenderer prefab;

    public override PartCategory Category => PartCategory.Wearable;
}
