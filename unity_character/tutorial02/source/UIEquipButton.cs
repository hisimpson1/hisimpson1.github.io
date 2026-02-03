using UnityEngine;
using UnityEngine.UI;

public class UIEquipButton : MonoBehaviour
{
    [Header("Target Customizer")]
    [SerializeField] private CharacterCustomizer customizer;

    [Header("Part to Equip: wear")]
    [SerializeField] private PartData upperPartData;

    [Header("Part to Equip: attach")]
    [SerializeField] private PartData earPartData;

    [Header("UI Button")]
    [SerializeField] private Button equipButton;

    private void Awake()
    {
        if (equipButton != null)
        {
            equipButton.onClick.AddListener(OnEquipClicked);
        }
    }

    private void OnEquipClicked()
    {
        if (customizer != null && earPartData != null)
            customizer.Equip(earPartData);

        if (customizer != null && upperPartData != null)
            customizer.Equip(upperPartData);
    }
}