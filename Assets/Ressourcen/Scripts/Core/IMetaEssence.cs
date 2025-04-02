using UnityEngine;

public enum TypeGroup 
{
    [InspectorName("Военные")] millitary,
    [InspectorName("Учёные")] scientist,
    [InspectorName("Сталкеры")] stalker,
    [InspectorName("Чистое Небо")] clearSky,
    [InspectorName("Бандиты")] bandut,
    [InspectorName("Долг")] dolg,
    [InspectorName("Свобода")] svoboda,
    [InspectorName("Монолит")] monolit,
    [InspectorName("Мутанты")] Mutant
};

public interface IMetaEssence
{
    string Name { get; }
    TypeGroup Group { get; }
    bool IsDide { get; }
    ViewEssence Visual { get; }
}
