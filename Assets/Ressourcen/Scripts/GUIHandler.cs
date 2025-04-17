using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIHandler : MonoBehaviour
{
    [SerializeField] private MapObject map;
    [Header("---------  Modules  ---------")]
    [SerializeField] public MobileInput input;
    [SerializeField] private GUIInventory inventory;
    [SerializeField] private GUIDiscriptionItem disct;
    [SerializeField] private GUIHealth Health;
    [SerializeField] public GUIDialogs dialogSystem;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private GUIDideScreen dideScreen;
    [SerializeField] private GUICall call;
    [Header("----------  Overly  -----------")]
    [SerializeField] private TextMeshProUGUI titeleQuest;
    [SerializeField] private TextMeshProUGUI smalDiscriptQuest;
    [SerializeField] private RectTransform markerQuest;
    [Space]
    [SerializeField] private RectTransform transAmmos;
    [SerializeField] private Vector3 posActiv;
    [SerializeField] private Vector3 posUnactiv;
    [SerializeField] private TextMeshProUGUI allAmmos;
    [SerializeField] private TextMeshProUGUI curentAmmos;
    [Header("-----------  Calls  ------------")]
    [SerializeField] private DialogCall[] callObjs;
    [SerializeField] private Quest[] startQuest;
    [SerializeField] private Quest[] endQuest;
    [Header("----------  Screen  -----------")]
    [SerializeField] private GameObject screenEntry;

    private Camera _cam;
    private Vector3 _questPoint;

    private void OnEnable()
    {
        questManager.UpdateQuestTitles += OnUpdateQuest;
        SaveHeandler.SaveSession += questManager.Save;
        SaveHeandler.LoadSession += questManager.Load;
        dialogSystem.DeathPlayer += dideScreen.OnDeath;
        dialogSystem.AddNewQuest += questManager.AddQuest;
        dialogSystem.AddItems += inventory.OnGiveItems;
        dialogSystem.GiveMoney += inventory.OnGiveMoney;
        dialogSystem.OpenStor += OnOpenStor;
        disct.OnUse += Health.Use;
    }

    private void OnDisable()
    {
        questManager.UpdateQuestTitles -= OnUpdateQuest;
        SaveHeandler.SaveSession -= questManager.Save;
        SaveHeandler.LoadSession -= questManager.Load;
        dialogSystem.DeathPlayer -= dideScreen.OnDeath;
        dialogSystem.AddNewQuest -= questManager.AddQuest;
        dialogSystem.AddItems -= inventory.OnGiveItems;
        dialogSystem.GiveMoney -= inventory.OnGiveMoney;
        dialogSystem.OpenStor -= OnOpenStor;
        disct.OnUse -= Health.Use;
    }

    private void Start()
    {
        _cam = Camera.main;

        if (SaveHeandler.SessionNow.GetSwitchObject("StartCall") && SceneManager.GetActiveScene().buildIndex == 3)
        {
            call.OpenDialog(callObjs[0]);
            SaveHeandler.SessionNow.SetSwitchObject("StartCall", false);
        }
        if (SaveHeandler.SessionNow.GetSwitchObject("ProvodnikCall-Cerkov") && SceneManager.GetActiveScene().buildIndex == 4)
        {
            call.OpenDialog(callObjs[1]);
            SaveHeandler.SessionNow.SetSwitchObject("ProvodnikCall-Cerkov", false);
        }
    }

    private void Update()
    {
        if (SaveHeandler.SessionNow.GetSwitchObject("PriceNasos") && !SaveHeandler.SessionNow.GetSwitchObject("PriceNasos-End"))
        {
            call.OpenDialog(callObjs[2]);
            questManager.SetEndQuest(endQuest[1]);
            questManager.AddQuest(startQuest[1]);
            SaveHeandler.SessionNow.SetSwitchObject("PriceNasos-End", true);
        }

        if (!SaveHeandler.SessionNow.GetSwitchObject("MninBoss-1") &&
            SaveHeandler.SessionNow.GetSwitchObject("MninBoss-1-End"))
        {
            questManager.SetEndQuest(endQuest[0]);
            questManager.AddQuest(startQuest[0]);
            SaveHeandler.SessionNow.SetSwitchObject("MninBoss-1-End", false);
        }

        if (markerQuest.gameObject.activeSelf)
        {
            Vector2 _o = _cam.WorldToScreenPoint(_questPoint);

            if (_o.x < Screen.width && _o.x > 0)
                markerQuest.position = new Vector3(_o.x, _o.y, 0);
            else
            {
                if (_o.x > 0)
                    markerQuest.position = new Vector3(Screen.width, 1000, 0);
                else
                    markerQuest.position = new Vector3(0, 1000, 0);
            }
        }
    }

    public void OnUpdateAmmos(int _all, int _cur)
    {
        allAmmos.text = _all != -1 ? _all.ToString() : "--";
        curentAmmos.text = _cur != -1 ? _cur.ToString() : "--";
    }
    public void OnUpdateQuest(Quest _now)
    {
        smalDiscriptQuest.gameObject.SetActive(_now || (_now && titeleQuest.text != _now.textTitell));
        titeleQuest.gameObject.SetActive(_now || (_now && titeleQuest.text != _now.textTitell));
        markerQuest.gameObject.SetActive(_now || (_now && titeleQuest.text != _now.textTitell));

        if (_now != null)
        {
            transAmmos.localPosition = posActiv;
            smalDiscriptQuest.text = _now.textDiscription;
            titeleQuest.text = _now.textTitell;

            if (SceneManager.GetActiveScene().buildIndex == _now.idScene)
                _questPoint = new Vector3(_now.position.x, (_now.position.y == 0 ? 2.5f : _now.position.y), 0);
            else
            {
                EntryMeta _ent;

                if (_ent = map.FindPath(SceneManager.GetActiveScene().buildIndex, _now.idScene))
                    _questPoint = new Vector3(_ent.posFrom.x, 2.5f, 0);
            }
        }
        else
            transAmmos.localPosition = posUnactiv;
    }
    public void OnSetIntButton(string _type) => input.OnShowInteractionBut(_type);
    public void OnOpenNPCPack(NPCBackpack _pack)
    {
        inventory.OnOpenIventory("PAK");
        inventory.AddPackToInv(_pack.DeathPack, "PAK");
    }
    public void OnOpenStor(ShopObject _pack)
    {
        inventory.OnOpenIventory("STR");
        inventory.AddPackToInv(_pack.GetListItems(), "STR");
    }
    public void OnOpenDialog(DialogList _list)
    {
        CoreObject core = _list.gameObject.GetComponent<CoreObject>();
        Quest quest = questManager.FindActivQuest(core?.Name);

        if (quest)
        {
            dialogSystem.SetDialog(core, quest.startDialog);
            questManager.SetEndQuest(quest);
        }
        else
            dialogSystem.SetDialog(core, _list.startDialog);
    }
    public void OnDeath(IMetaEssence _essence) => dideScreen.OnDeath(_essence);
    public void OnEentry(Entry entry) => screenEntry.SetActive(true);
}
