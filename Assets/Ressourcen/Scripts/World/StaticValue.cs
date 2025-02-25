using System.Collections.Generic;
using UnityEngine;

public static class StaticValue
{
    public static string SessionToken;

    public static int[] time = new int[2] { 6, 0 };
    public static float lightLevel = 0.69f;

    public static int countKills = 0;
    public static float countDm = 0;

    public static Dictionary<string, bool> baseSwitcherObject = new();

    static StaticValue()
    {
        baseSwitcherObject.Add("Karatel", true); //������ ������������ �� ��������
        baseSwitcherObject.Add("StartCall", true); // ��������� �������� �� �����
        baseSwitcherObject.Add("ProvodnikCall-Cerkov", true); // ������� ��������� ����� �������� �� �������
        baseSwitcherObject.Add("InScene", false); //�������� ����� ����� � �������
        baseSwitcherObject.Add("SvarogS", true); //������ �������� �� ��������
        baseSwitcherObject.Add("MninBoss-1", true); //���� ���� � ������
        baseSwitcherObject.Add("MninBoss-1-End", true); //����� �����������
        baseSwitcherObject.Add("NewQuest-Lebed", false); //����� ������� ����� ������
        baseSwitcherObject.Add("BaseNarciss", true); //������ �������� - �����
        baseSwitcherObject.Add("FightNarciss", false); //������ �������� - �������
        baseSwitcherObject.Add("PriceNasos", false); //call � �������� �� �������� �������� ������
        baseSwitcherObject.Add("PriceNasos-End", false); //call � �������� �� �������� �������� ������ - END
    }
}
