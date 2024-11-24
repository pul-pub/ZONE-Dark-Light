using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class SaveHeandler
{
    public Dictionary<string, Character> characters = new Dictionary<string, Character>();

    private Ids _ids;

    public SaveHeandler()
    {
        if (File.Exists(Application.persistentDataPath + "/ids.json"))
            ImportSeves();
        else
            FristOpen();
    }

    public void NewCharecter()
    {
        string _newId = GenerateUniqueId();
        characters.Add(_newId, new Character());

        string[] id_list = new string[_ids.ids.Length + 1];
        if (_ids.ids[0] != null && _ids.ids[0] != "")
        {
            for (int i = 0; i < _ids.ids.Length; i++)
            {
                id_list[i] = _ids.ids[i];
                id_list[_ids.ids.Length] = _newId;
            }
            _ids.ids = id_list;
        }
        else
        {
            _ids.ids[0] = _newId;
        }

        ExportSeves();
    }

    private void ImportSeves()
    {
        string _json = File.ReadAllText(Application.persistentDataPath + "/ids.json");
        _ids = JsonUtility.FromJson<Ids>(_json);

        characters = new Dictionary<string, Character>();
        foreach (string _key in _ids.ids)
        {
            if (_key != null && _key != "")
            {
                _json = File.ReadAllText(Application.persistentDataPath + "/" + _key + ".json");
                characters.Add(_key, JsonUtility.FromJson<Character>(_json));
            }
        }
    }

    private void ExportSeves()
    {
        string _json = JsonUtility.ToJson(_ids, true);
        File.WriteAllText(Application.persistentDataPath + "/ids.json", _json);

        if (characters.Count > 0)
        {
            foreach (string _key in characters.Keys)
            {
                _json = JsonUtility.ToJson(characters[_key], true);
                File.WriteAllText(Application.persistentDataPath + "/" + _key + ".json", _json);
            }
        }
    }

    private void FristOpen()
    {
        _ids = new Ids();
        _ids.ids = new string[1] { null };

        string _json = JsonUtility.ToJson(_ids, true);
        File.WriteAllText(Application.persistentDataPath + "/ids.json", _json);
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
