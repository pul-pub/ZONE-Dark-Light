using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using Struct;

public static class SaveHeandler
{
    public static event Action OnSaveSession;
    public static event Action OnEndInit;

    public static Ids charecters;
    public static Character SessionSave;


    static SaveHeandler()
    {
        var resolver = CompositeResolver.Create(
            DynamicGenericResolver.Instance, 
            StandardResolver.Instance        
        );
        var options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
        MessagePackSerializer.DefaultOptions = options;

        Debug.Log("Start import parameters");

        if (!File.Exists(Application.persistentDataPath + "/ListCharecters"))
        {
            charecters = new Ids();
            ExportSeves();
        }

        ImportSeves();
        OnEndInit?.Invoke();

        Debug.Log("Start work SaveHandler");
    }


    public static void StartSession()
    {
        SessionSave = charecters.keysCharecters[StaticValue.SessionToken].Clone();
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

        _si = new SavesItem();
        _si.idItem = 151;
        _si.count = 1;
        _si.cellsId = new int[2] { 2, 3 };
        _si.customPropertyItem.Add("Gun", 25);
        _character.items.Add(_si);
        #endregion

        _character.idActivQuest = 0;
        _character.idQuests.Add(0);

        _character.switcherObject = new();
        foreach (string k in StaticValue.baseSwitcherObject.Keys)
            _character.switcherObject.Add(k, StaticValue.baseSwitcherObject[k]);

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
        byte[] _data = File.ReadAllBytes(Application.persistentDataPath + "/ListCharecters");
        charecters = MessagePackSerializer.Deserialize<Ids>(DecryptData(_data));
    }

    private static void ExportSeves()
    {
        byte[] _data = MessagePackSerializer.Serialize(charecters);
        File.WriteAllBytes(Application.persistentDataPath + "/ListCharecters", EncryptData(_data));
    }


    private static IEnumerator Save()
    {
        SaveSession();
        yield return new WaitForEndOfFrame();
        charecters.keysCharecters[StaticValue.SessionToken] = SessionSave.Clone();
        yield return new WaitForEndOfFrame();
        ExportSeves();
    }

    private static string GenerateUniqueId(int _leg = 32)
    {
        byte[] randomBytes = new byte[_leg];

        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }

        string uniqueId = Convert.ToBase64String(randomBytes);
        uniqueId = uniqueId.Replace('+', '-').Replace('/', '_');
        uniqueId = uniqueId.TrimEnd('=');

        return uniqueId;
    }
    private static byte[] EncryptData(byte[] dataToEncrypt)
    {
        byte[] _data;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.GenerateKey();
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    csEncrypt.FlushFinalBlock();
                    
                    _data = msEncrypt.ToArray();
                }
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] _byts = sha256.ComputeHash(_data);
                string _hash = BitConverter.ToString(_byts);
                AesCryptoParametrsStructure _aes = new AesCryptoParametrsStructure
                {
                    key = aesAlg.Key,
                    iv = aesAlg.IV
                };

                SaveEncryptParam(_aes, _hash);
            }
        }

        return _data;
    }
    private static byte[] DecryptData(byte[] dataToDecrypt) 
    {
        AesCryptoParametrsStructure _param = LoadEncryptParam(dataToDecrypt);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _param.key;
            aesAlg.IV = _param.iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(dataToDecrypt))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (MemoryStream msOutput = new MemoryStream())
                    {
                        csDecrypt.CopyTo(msOutput);
                        return msOutput.ToArray();
                    }
                }
            }
        }
    }

    private static void SaveEncryptParam(AesCryptoParametrsStructure _param, string _key)
    {
        string _json = JsonUtility.ToJson(_param);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString(_key, _json);
    }

    private static AesCryptoParametrsStructure LoadEncryptParam(byte[] dataToDecrypt)
    {
        string _key;

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] _byts = sha256.ComputeHash(dataToDecrypt);
            _key = BitConverter.ToString(_byts);
        }

        if (_key == null)
            return null;

        string _json = PlayerPrefs.GetString(_key);
        return JsonUtility.FromJson<AesCryptoParametrsStructure>(_json);
    }
}
