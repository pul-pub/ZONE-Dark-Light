using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "Data", fileName = "Data")]
public class DataBase : ScriptableObject
{
    [SerializeField] private List<IItem> Items;

    [SerializeField] private List<Gun> Guns;
    [SerializeField] private List<KnifeObject> Knifes;
    [SerializeField] private List<DetectorObject> Detectors;
    [SerializeField] private List<ArmorObject> Armors;
    [SerializeField] private List<ArtifactObject> Artifacts;
    [SerializeField] private List<BackpackObject> Backpacks;
    [SerializeField] private List<LightObject> Lights;
    [SerializeField] private List<MedicObject> Medics;
    [SerializeField] private List<AmmoObject> Ammos;

    [SerializeField] private List<Sprite> Faces = new();
    [SerializeField] private List<Quest> Quests;

#if UNITY_EDITOR
    private void OnEnable()
    {
        DetekrotChange.OnChange += OnChenge;
    }

    private void OnDisable()
    {
        DetekrotChange.OnChange -= OnChenge;
    }

    private void OnChenge()
    {
        string[] classNames = new string[11]
        {
            "Gun", "KnifeObject", "DetectorObject", "ArmorObject", "ArtifactObject", "BackpackObject",
            "LightObject", "MedicObject", "AmmoObject", "Quest", "ScriptableObject"
        };

        Items = new();

        Guns = new();
        Knifes = new();
        Detectors = new();
        Armors = new();
        Artifacts = new();
        Backpacks = new();
        Lights = new();
        Medics = new();
        Ammos = new();

        Quests = new();

        foreach (string name in classNames)
        {
            string[] guids = AssetDatabase.FindAssets("t:" + name);
            foreach (string guid in guids)
            {
                if (name == "ScriptableObject")
                {
                    ScriptableObject _s = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid));
                    if (_s is IItem _item)
                        Items.Add(_item);
                }

                switch (name)
                {
                    case "Gun":
                        {
                            Gun _g = AssetDatabase.LoadAssetAtPath<Gun>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_g)
                                Guns.Add(_g);
                            break;
                        }
                    case "KnifeObject":
                        {
                            KnifeObject _k = AssetDatabase.LoadAssetAtPath<KnifeObject>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_k)
                                Knifes.Add(_k);
                            break;
                        }
                    case "DetectorObject":
                        {
                            DetectorObject _d = AssetDatabase.LoadAssetAtPath<DetectorObject>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_d)
                                Detectors.Add(_d);
                            break;
                        }
                    case "ArmorObject":
                        {
                            ArmorObject _a = AssetDatabase.LoadAssetAtPath<ArmorObject>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_a)
                                Armors.Add(_a);
                            break;
                        }
                    case "ArtifactObject":
                        {
                            ArtifactObject _a = AssetDatabase.LoadAssetAtPath<ArtifactObject>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_a)
                                Artifacts.Add(_a);
                            break;
                        }
                    case "BackpackObject":
                        {
                            BackpackObject _b = AssetDatabase.LoadAssetAtPath<BackpackObject>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_b)
                                Backpacks.Add(_b);
                            break;
                        }
                    case "LightObject":
                        {
                            LightObject _l = AssetDatabase.LoadAssetAtPath<LightObject>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_l)
                                Lights.Add(_l);
                            break;
                        }
                    case "MedicObject":
                        {
                            MedicObject _m = AssetDatabase.LoadAssetAtPath<MedicObject>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_m)
                                Medics.Add(_m);
                            break;
                        }
                    case "AmmoObject":
                        {
                            AmmoObject _a = AssetDatabase.LoadAssetAtPath<AmmoObject>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_a)
                                Ammos.Add(_a);
                            break;
                        }
                    case "Quest":
                        {
                            Quest _q = AssetDatabase.LoadAssetAtPath<Quest>(AssetDatabase.GUIDToAssetPath(guid));
                            if (_q)
                                Quests.Add(_q);
                            break;
                        }
                }
            }
        }

        Debug.Log(
            $"Loaded {Items.Count} items implementing IItem\n" +
            $"Loaded {Guns.Count} items implementing Gun\n" +
            $"Loaded {Knifes.Count} items implementing Knif\n" +
            $"Loaded {Detectors.Count} items implementing Detector\n" +
            $"Loaded {Armors.Count} items implementing Armors\n" +
            $"Loaded {Artifacts.Count} items implementing Artifacts\n" +
            $"Loaded {Backpacks.Count} items implementing Pack\n" +
            $"Loaded {Lights.Count} items implementing Lights\n" +
            $"Loaded {Medics.Count} items implementing Medic\n" +
            $"Loaded {Ammos.Count} items implementing Ammos\n" +
            $"Loaded {Quests.Count} items implementing Quest");
    }
#endif

    public IItem GetItem(string _id)
    {
        char[] _listID = _id.ToCharArray();
        string _typeItem = _listID[0].ToString() + _listID[1].ToString() + _listID[2].ToString();

        switch (_typeItem)
        {
            case "GUN":
                return GetGun(_id);
            case "KNF":
                return GetKnife(_id);
            case "DET":
                return GetDetector(_id);
            case "ARM":
                return GetArmor(_id);
            case "ART":
                return GetArtifact(_id);
            case "PAK":
                return GetBackpack(_id);
            case "LIT":
                return GetLight(_id);
            case "MED":
                return GetMedics(_id);
            case "AMO":
                return GetAmmos(_id);
        }

        return null;
    }

    public IItem GetTestItem(string _id) => Items.Find(x => x.Id == _id);

    public Gun GetGun(string _id) => Guns.Find(g => g.Id == _id);
    public KnifeObject GetKnife(string _id) => Knifes.Find(k => k.Id == _id);
    public DetectorObject GetDetector(string _id) => Detectors.Find(d => d.Id == _id);
    public ArmorObject GetArmor(string _id) => Armors.Find(a => a.Id == _id);
    public ArtifactObject GetArtifact(string _id) => Artifacts.Find(a => a.Id == _id);
    public BackpackObject GetBackpack(string _id) => Backpacks.Find(b => b.Id == _id);
    public LightObject GetLight(string _id) => Lights.Find(l => l.Id == _id);
    public MedicObject GetMedics(string _id) => Medics.Find(m => m.Id == _id);
    public AmmoObject GetAmmos(string _id) => Ammos.Find(a => a.Id == _id);

    public Sprite GetFace(int _id) => Faces[_id];
    public int GetFaceLen() => Faces.Count;
    public Quest GetQuest(int _id) => Quests.Find(q => q.Id == _id);
}
