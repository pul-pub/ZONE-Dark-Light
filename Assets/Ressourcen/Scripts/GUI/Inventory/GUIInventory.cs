using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIInventory : Inventory
{
    [SerializeField] private DataBase data;
    [SerializeField] private GUIDiscriptionItem discriptionItem;
    [SerializeField] private MobileInput input;
    [SerializeField] private GUIBuyDialogScreen buy;
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
    [SerializeField] private GameObject ScreenBuy;
    [Header("----------  Items  -----------")]
    [SerializeField] private UnityEngine.Object objectItem;
    [Space]
    [SerializeField] private Transform parentDrag;
    [Header("----------  Value  -----------")]
    [SerializeField] private TextMeshProUGUI mass;
    [SerializeField] private TextMeshProUGUI money;

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
        OpenStoreDialogScreen += buy.SetDialogScreen;
        buy.AddItem += EndNPCStor;

        foreach (GUIButton but in buttons)
            but.Click += OnOpenIventory;
    }

    private void OnDisable()
    {
        SaveHeandler.SaveSession -= Save;
        SaveHeandler.LoadSession -= Load;
        OpenDiscription -= discriptionItem.OnOpenDiscription;
        ChangeOutfit -= input.OnUpdateOutFit;
        OpenStoreDialogScreen -= buy.SetDialogScreen;
        buy.AddItem -= EndNPCStor;

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
        money.text = Money.ToString();
    }

    public void AddPackToInv(List<ObjectItem> _pack, string _conf = "")
    {
        foreach (ObjectItem item in _pack)
            AddNPCItem(item.Item, item.Count, _conf);
    }

    public void OnOpenIventory(string _screen)
    {
        mainScreen.SetActive(_screen != "");
        leftScreen.SetActive(_screen == "OTF" || _screen == "PRT");
        outfitScreen.SetActive(_screen == "OTF");
        npcScreen.SetActive(_screen == "PAK" || _screen == "STR");
        boadyParthsScreen.SetActive(_screen == "PRT");

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
