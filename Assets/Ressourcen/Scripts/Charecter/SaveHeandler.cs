using MessagePack;
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public static class SaveHeandler
{
    public static event Action OnSaveSession;

    public static Ids charecters;
    public static Character SessionSave;

    static SaveHeandler()
    {
        Debug.Log("Start work SaveHandler");

        if (!File.Exists(Application.persistentDataPath + "/ListCharecters.json"))
        {
            charecters = new Ids();
            ExportSeves();
        }

        ImportSeves();
    }


    public static void StartSession()
    {
        SessionSave = charecters.keysCharecters[StaticValue.SessionToken];
    }
    public static void SaveSession() => OnSaveSession?.Invoke();
    public static void SaveProgress(MonoBehaviour _parent) => _parent.StartCoroutine(Save());


    public static void NewCharecter(string _name, int _faceID, int[] _charecteristic)
    {
        Character _character = new Character();

        _character.pos = new PlayerPos(1, -47, 0.8f);
        _character.idScene = 1;

        _character.name = _name;
        _character.idFace = _faceID;
        _character.characteristics.Add("Сила", _charecteristic[1]);
        _character.characteristics.Add("Ловкость", _charecteristic[2]);
        _character.characteristics.Add("Интелект", _charecteristic[0]);

        _character.money = 0;

        #region ADD_ITEM
        SavesItem _si = new SavesItem();
        _si.idItem = 140;
        _si.count = 1;
        _si.cellsId = new int[1] { 104 };
        _si.conditionItem.Add("Armor", 80);
        _character.items.Add(_si);

        _si = new SavesItem();
        _si.idItem = 110;
        _si.count = 1;
        _si.cellsId = new int[1] { 0 };
        _si.customPropertyItem.Add("Light", 60);
        _character.items.Add(_si);
        #endregion

        _character.idActivQuest = 0;
        _character.idQuests.Add(0);

        _character.switcherObject = StaticValue.baseSwitcherObject;

        _character.time = new int[2] { 6, 0 };
        _character.isRain = false;

        _character.endTimeSession = DateTime.Now.ToString();

        string _newId = GenerateUniqueId();
        charecters.keysCharecters.Add(_newId, _character);
        StaticValue.SessionToken = _newId;

        ExportSeves();
    }

    private static void ImportSeves()
    {
        string _json = File.ReadAllText(Application.persistentDataPath + "/ListCharecters.json");
        byte[] _bytes = MessagePackSerializer.ConvertFromJson(_json);
        charecters = MessagePackSerializer.Deserialize<Ids>(_bytes);
    }

    private static void ExportSeves()
    {
        byte[] _bytes = MessagePackSerializer.Serialize(charecters);
        string _json = MessagePackSerializer.ConvertToJson(_bytes);
        File.WriteAllText(Application.persistentDataPath + "/ListCharecters.json", _json);
    }


    private static IEnumerator Save()
    {
        SaveSession();
        yield return new WaitForEndOfFrame();
        charecters.keysCharecters[StaticValue.SessionToken] = SessionSave.Clone();
        yield return new WaitForEndOfFrame();
        ExportSeves();
    }

    private static string GenerateUniqueId()
    {
        byte[] randomBytes = new byte[32];

        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }

        string uniqueId = Convert.ToBase64String(randomBytes);
        uniqueId = uniqueId.Replace('+', '-').Replace('/', '_');
        uniqueId = uniqueId.TrimEnd('=');

        return uniqueId;
    }
}
