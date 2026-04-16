using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets; // 필수
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations; // 필수

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

    // 인스턴스화된 객체를 관리 (해제용)
    private Dictionary<PartType, GameObject> spawnedParts = new();
    // 어드레서블 로딩 상태 및 메모리 해제용 핸들 
    private Dictionary<PartType, AsyncOperationHandle<GameObject>> partHandles = new();

    // 이제 프리팹 대신 '참조'를 인스펙터에 노출합니다.
    [SerializeField] AssetReference upper1Ref;
    [SerializeField] AssetReference upper2Ref;
    [SerializeField] AssetReference ear1MeshRef; // Mesh 전용 참조
    [SerializeField] AssetReference ear2MeshRef;

    public void EquipMesh(PartType type, AssetReference meshRef)
    {
        if (meshRef == null || !meshRef.RuntimeKeyIsValid()) return;

        if (meshRef.OperationHandle.IsValid())
        {
            Addressables.Release(meshRef.OperationHandle);
        }

        // AssetReference를 사용할 때는 어떤 타입으로 로드할지 <Mesh>를 명시해줘야 합니다.
        meshRef.LoadAssetAsync<Mesh>().Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                earMeshRenderer.sharedMesh = handle.Result;
            }
        };
    }

public void EquipPart(PartType type, AssetReference partRef)
{
    if (partRef == null || !partRef.RuntimeKeyIsValid()) return;

    // 1. 기존 파츠 및 핸들 해제 (메모리 누수 방지 핵심!)
    ReleasePart(type);

    // 2. 비동기 생성 시작
    var handle = partRef.InstantiateAsync(transform);
    partHandles[type] = handle;

    handle.Completed += (op) =>
    {
        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject newPartObj = op.Result;
                
            if (newPartObj.TryGetComponent<SkinnedMeshRenderer>(out var nSmr))
            {
                nSmr.rootBone = baseMeshRenderer.rootBone;
                nSmr.bones = baseMeshRenderer.bones;
            }

            spawnedParts[type] = newPartObj;
        }
    };
}

    private void ReleasePart(PartType type)
    {
        if (partHandles.ContainsKey(type))
        {
            // Addressables를 통해 생성된 객체는 반드시 Addressables.ReleaseInstance로 지워야 합니다.
            Addressables.ReleaseInstance(partHandles[type]);
            partHandles.Remove(type);
            spawnedParts.Remove(type);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipPart(PartType.Upper, upper1Ref);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipPart(PartType.Upper, upper2Ref);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipMesh(PartType.Ear, ear1MeshRef);
        if (Input.GetKeyDown(KeyCode.Alpha4)) EquipMesh(PartType.Ear, ear2MeshRef);
    }

    // 오브젝트 파괴 시 전체 메모리 해제
    private void OnDestroy()
    {
        foreach (var key in new List<PartType>(partHandles.Keys))
        {
            ReleasePart(key);
        }
    }
}