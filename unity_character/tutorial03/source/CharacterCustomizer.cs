using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    [Header("Base Character")]
    [SerializeField] private SkinnedMeshRenderer baseMeshRenderer;

    [Header("Mesh Swap Renderers")]
    [SerializeField] private SkinnedMeshRenderer earRenderer;
    [SerializeField] private SkinnedMeshRenderer hairRenderer;
    [SerializeField] private SkinnedMeshRenderer faceRenderer;
    [SerializeField] private SkinnedMeshRenderer tailRenderer;

    // 현재 장착된 착용형 파츠 관리
    private Dictionary<PartType, SkinnedMeshRenderer> equippedWearables = new();

    // 공통 Equip 메서드
    public void Equip(IEquippablePart part)
    {
        if (part == null) 
            return;
        part.Equip(this);
    }

    #region 내부 장착 로직 (Wearable / Mesh)

    // 착용형 파츠 장착
    public void EquipWearable(WearablePartData partData)
    {
        if (partData == null || partData.prefab == null)
            return;

        if (equippedWearables.TryGetValue(partData.Type, out var oldPart))
        {
            Destroy(oldPart.gameObject);
        }

        var newPart = Instantiate(partData.prefab, transform);
        newPart.rootBone = baseMeshRenderer.rootBone;
        newPart.bones = baseMeshRenderer.bones;

        equippedWearables[partData.Type] = newPart;
    }

    // 메쉬 교체형 파츠 장착
    public void EquipMesh(MeshPartData partData)
    {
        if (partData == null || partData.mesh == null)
            return;

        SkinnedMeshRenderer targetRenderer = partData.Type switch
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
