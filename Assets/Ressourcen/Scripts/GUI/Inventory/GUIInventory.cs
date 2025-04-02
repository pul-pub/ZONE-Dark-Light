using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIInventory : Inventory
{
    [SerializeField] private DataBase data;
    [SerializeField] private GUIDiscriptionItem discriptionItem;
    [SerializeField] private MobileInput input;
    [Header("---------  Parents  ----------")]
    [SerializeField] private Transform[] parentCells;
    [SerializeField] private List<ObjectCell> cells = new();
    [Header("----------  Screens  -----------")]
    [SerializeField] private List<GUIButton> buttons;
    [Space]
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject leftScreen;
    [SerializeField] private GameObject outfitScreen;
    [SerializeField] private GameObject npcScreen;
    [SerializeField] private GameObject boadyParthsScreen;
    [Header("----------  Items  -----------")]
    [SerializeField] private UnityEngine.Object objectItem;
    [Space]
    [SerializeField] private Transform parentDrag;
    [Header("----------  Value  -----------")]
    [SerializeField] private TextMeshProUGUI mass;

    private void Awake()
    {
        _data = data;
        _parentBase = parentCells;
        _parentDrag = parentDrag;
        _objectItem = objectItem;

        if (cells.Count == 0)
            GetAllCells();
        else
            foreach (ObjectCell cell in cells)
                _cellObjs.Add(cell);
    }

    private void OnEnable()
    {
        SaveHeandler.SaveSession += Save;
        SaveHeandler.LoadSession += Load;
        OpenDiscription += discriptionItem.OnOpenDiscription;
        ChangeOutfit += input.OnUpdateOutFit;

        foreach (GUIButton but in buttons)
            but.Click += OnOpenIventory;
    }

    private void OnDisable()
    {
        SaveHeandler.SaveSession -= Save;
        SaveHeandler.LoadSession -= Load;
        OpenDiscription -= discriptionItem.OnOpenDiscription;
        ChangeOutfit -= input.OnUpdateOutFit;

        foreach (GUIButton but in buttons)
            but.Click -= OnOpenIventory;
    }

    private void Start()
    {
        UpdateAllItems();
        OnUpdateOutFit();
    }

    private void Update()
    {
        mass.text = System.Math.Round(WeightInventory, 3, System.MidpointRounding.ToEven).ToString();
    }

    public void AddPackToInv(NPCBackpack _pack)
    {
        foreach (ObjectItem item in _pack.DeathPack)
            AddNPCItem(item.Item, item.Count);
    }

    public void OnOpenIventory(string _screen)
    {
        if (_screen != "")
        {
            mainScreen.SetActive(true);
            leftScreen.SetActive(_screen != "PAK");
            outfitScreen.SetActive(_screen == "OTF");
            npcScreen.SetActive(_screen == "PAK");
            boadyParthsScreen.SetActive(_screen == "PRT");
        }
        else
        {
            mainScreen.SetActive(false);
            leftScreen.SetActive(false);
            outfitScreen.SetActive(false);
            npcScreen.SetActive(false);
            boadyParthsScreen.SetActive(false);
        }

        UpdateStatusItems(_screen);
    }

    private void GetAllCells()
    {
        foreach (Transform tr in parentCells) 
            foreach (ObjectCell cell in tr.parent.GetComponentsInChildren<ObjectCell>())
                _cellObjs.Add(cell);
    }

    public void OnGiveItems(List<StaticItem> _list)
    {
        foreach (StaticItem si in _list)
        {
            IItem _new = data.GetItem(si.id).CloneItem();

            _new.Value = si.value;
            _new.Condition = si.cond;

            AppItem(_new, si.count);
        }  
    }
}
