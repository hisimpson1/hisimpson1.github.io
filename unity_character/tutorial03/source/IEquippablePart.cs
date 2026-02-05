public interface IEquippablePart
{
    PartType Type { get; }
    void Equip(CharacterCustomizer customizer);
}
