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
    public static event Action SaveSession;
    public static event Action LoadSession;

    public static Ids ListCharecters;
    public static Save Settings;
    public static Character SessionNow;

    static SaveHeandler()
    {
        var resolver = CompositeResolver.Create(
            DynamicGenericResolver.Instance, 
            StandardResolver.Instance        
        );
        var options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
        MessagePackSerializer.DefaultOptions = options;

        Debug.Log("Start import parameters");

        if (!File.Exists(Application.persistentDataPath + "/ListCharecters") ||
            !File.Exists(Application.persistentDataPath + "/Save"))
        {
            ListCharecters = new Ids();
            Settings = new Save();
            ExportSeves();
            ExportSettings();
        }

        ImportSeves();
        ImportSettings();

        Debug.Log("Start work SaveHandler");
    }

    public static void StartSession() => SessionNow = ListCharecters.keysCharecters[StaticValue.SessionToken].Clone();
    public static void Save() => SaveSession?.Invoke();
    public static void Load() => LoadSession?.Invoke();
    public static void SaveProgress(MonoBehaviour _parent) => _parent.StartCoroutine(DilaySave());


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

        #region BODY PARTH
        _character.hpBodyParth.Add("body", 155);
        _character.hpBodyParth.Add("head", 35);
        _character.hpBodyParth.Add("armL", 60);
        _character.hpBodyParth.Add("armR", 60);
        _character.hpBodyParth.Add("leg", 130);
        #endregion

        _character.money = 1000;

        #region ADD ITEM
        SavesItem _si = new SavesItem();
        _si.idItem = "ARM001";
        _si.count = 1;
        _si.cellsId = new int[1] { 104 };
        _si.condition = 80;
        _character.items.Add(_si);

        _si = new SavesItem();
        _si.idItem = "LIT001";
        _si.count = 1;
        _si.cellsId = new int[1] { 0 };
        _si.value = 60;
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
        ListCharecters.keysCharecters.Add(_newId, _character);
        StaticValue.SessionToken = _newId;

        ExportSeves();
    }

    private static void ImportSeves()
    {
        byte[] _byteList = File.ReadAllBytes(Application.persistentDataPath + "/ListCharecters");
        try
        {
            ListCharecters = MessagePackSerializer.Deserialize<Ids>(_byteList);
        }
        catch
        {
            ListCharecters = MessagePackSerializer.Deserialize<Ids>(DecryptData(_byteList));
        }
    }

    private static void ExportSeves()
    {
        byte[] _byteList = MessagePackSerializer.Serialize(ListCharecters);
        File.WriteAllBytes(Application.persistentDataPath + "/ListCharecters", _byteList);
    }

    public static void ImportSettings()
    {
        byte[] _byteSettingst = File.ReadAllBytes(Application.persistentDataPath + "/Save");
        Settings = MessagePackSerializer.Deserialize<Save>(_byteSettingst);
    }

    public static void ExportSettings()
    {
        byte[] _byteSettingst = MessagePackSerializer.Serialize(Settings);
        File.WriteAllBytes(Application.persistentDataPath + "/Save", _byteSettingst);
    }

    private static IEnumerator DilaySave()
    {
        Save();
        SessionNow.endTimeSession = DateTime.Now.ToString();
        yield return new WaitForEndOfFrame();
        ListCharecters.keysCharecters[StaticValue.SessionToken] = SessionNow.Clone();
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
