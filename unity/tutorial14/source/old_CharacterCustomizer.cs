using System.Collections.Generic;
using UnityEngine;

public enum PartType
{
    Upper,
    Pants,
    Gloves,
    Shoes,
    Ear
}

public class CharacterCustomizer : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer baseMeshRenderer;
    [SerializeField] SkinnedMeshRenderer earMeshRenderer;

    private Dictionary<PartType, SkinnedMeshRenderer> equippedParts = new();

    [SerializeField] SkinnedMeshRenderer upper1;
    [SerializeField] SkinnedMeshRenderer upper2;
    [SerializeField] Mesh ear1;
    [SerializeField] Mesh ear2;


public void EquipMesh(PartType type, Mesh partPrefab)
{
	earMeshRenderer.sharedMesh = partPrefab;
}

public void EquipPart(PartType type, SkinnedMeshRenderer partPrefab)
{
	// 기존 파츠 제거
	if (equippedParts.ContainsKey(type))
	{
		Destroy(equippedParts[type].gameObject);
	}

	// 새 파츠 생성
	var newPart = Instantiate(partPrefab, transform);
	newPart.rootBone = baseMeshRenderer.rootBone;
	newPart.bones = baseMeshRenderer.bones;

	equippedParts[type] = newPart;
}
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipPart(PartType.Upper, upper1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipPart(PartType.Upper, upper2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipMesh(PartType.Ear, ear1);
        if (Input.GetKeyDown(KeyCode.Alpha4)) EquipMesh(PartType.Ear, ear2);
    }
}