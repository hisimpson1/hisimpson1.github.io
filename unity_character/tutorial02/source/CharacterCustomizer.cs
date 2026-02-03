using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterCustomizer : MonoBehaviour
{
    [Header("Base Character")]
    [SerializeField] private SkinnedMeshRenderer baseMeshRenderer;

    [Header("Mesh Swap Renderers")]
    [SerializeField] private SkinnedMeshRenderer earRenderer;
    [SerializeField] private SkinnedMeshRenderer hairRenderer;
    [SerializeField] private SkinnedMeshRenderer faceRenderer;
    [SerializeField] private SkinnedMeshRenderer tailRenderer;

    private Dictionary<PartType, SkinnedMeshRenderer> equippedWearables = new();

    // 공통 Equip 메서드
    public void Equip(PartData partData)
    {
        if (partData == null) return;

        switch (partData.Category)
        {
            case PartCategory.Wearable:
                EquipWearable(partData as WearablePartData);
                break;

            case PartCategory.Mesh:
                EquipMesh(partData as MeshPartData);
                break;
        }
    }

    #region 내부 장착 로직

    private void EquipWearable(WearablePartData partData)
    {
        if (partData == null || partData.prefab == null)
            return;

        if (equippedWearables.TryGetValue(partData.type, out var oldPart))
        {
            Destroy(oldPart.gameObject);
        }

        var newPart = Instantiate(partData.prefab, transform);
        newPart.rootBone = baseMeshRenderer.rootBone;
        newPart.bones = baseMeshRenderer.bones;

        equippedWearables[partData.type] = newPart;
    }

    private void EquipMesh(MeshPartData partData)
    {
        if (partData == null || partData.mesh == null)
            return;

        SkinnedMeshRenderer targetRenderer = partData.type switch
        {
            PartType.Ear => earRenderer,
            PartType.Hair => hairRenderer,
            PartType.Face => faceRenderer,
            PartType.Tail => tailRenderer,
            _ => null
        };

        if (targetRenderer != null)
        {
            targetRenderer.sharedMesh = partData.mesh;
        }
    }

    #endregion
}
