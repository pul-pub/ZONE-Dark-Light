using UnityEngine;

public enum TypeGroup 
{
    [InspectorName("�������")] millitary,
    [InspectorName("������")] scientist,
    [InspectorName("��������")] stalker,
    [InspectorName("������ ����")] clearSky,
    [InspectorName("�������")] bandut,
    [InspectorName("����")] dolg,
    [InspectorName("�������")] svoboda,
    [InspectorName("�������")] monolit,
    [InspectorName("�������")] Mutant
};

public interface IMetaEssence
{
    string Name { get; }
    TypeGroup Group { get; }
    bool IsDide { get; }
    ViewEssence Visual { get; }
}
