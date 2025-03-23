using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour
{
    public event Action<string> ShowInteractionButton;

    public event Action<DialogList> OpenDialog;
    public event Action<NPCBackpack> OpenNPCPack;
    public event Action<Entry> StartEntrying;
    public event Action<int, int> UpdateAmmos;

    [Header("---------  Modules  ---------")]
    [SerializeField] private GUIHandler gui;
    [SerializeField] private MobileInput input;
    [Space]
    [SerializeField] private Health health;
    [SerializeField] private Energy energy;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private LightManager lightManager;
    [SerializeField] private OutFitManager outFitManager;
    [SerializeField] private ModuleDirectionHands moduleDirection;
    [Header("----  Interaction Button  ----")]
    [SerializeField] private int sizeCheck;
    [SerializeField] private LayerMask layer;

    private bool _isInterecrion = false;
    private int _allAmmo = 0;

    private NPCBackpack _targetPack;
    private DialogList _targetDialogList;
    private Entry _targetEntry;
    
    private void OnEnable()
    {
        SaveHeandler.SaveSession += health.Save;
        SaveHeandler.SaveSession += energy.Save;
        SaveHeandler.SaveSession += weaponManager.Save;
        SaveHeandler.SaveSession += lightManager.Save;
        SaveHeandler.SaveSession += Save;
        SaveHeandler.SaveSession += moduleDirection.Save;

        SaveHeandler.LoadSession += health.Load;
        SaveHeandler.LoadSession += energy.Load;
        SaveHeandler.LoadSession += weaponManager.Load;
        SaveHeandler.LoadSession += lightManager.Load;
        SaveHeandler.LoadSession += outFitManager.Load;
        SaveHeandler.LoadSession += Load;
        SaveHeandler.LoadSession += moduleDirection.Load;

        StartEntrying += OnEntry;

        ShowInteractionButton += gui.OnSetIntButton;
        OpenDialog += gui.OnOpenDialog;
        OpenNPCPack += gui.OnOpenNPCPack;
        UpdateAmmos += gui.OnUpdateAmmos;
        input.PressInteractionButton += OnPressIntButton;
        health.Death += gui.OnDeath;
    }

    private void OnDisable()
    {
        SaveHeandler.SaveSession -= health.Save;
        SaveHeandler.SaveSession -= energy.Save;
        SaveHeandler.SaveSession -= weaponManager.Save;
        SaveHeandler.SaveSession -= lightManager.Save;
        SaveHeandler.SaveSession -= Save;
        SaveHeandler.SaveSession -= moduleDirection.Save;

        SaveHeandler.LoadSession -= health.Load;
        SaveHeandler.LoadSession -= energy.Load;
        SaveHeandler.LoadSession -= weaponManager.Load;
        SaveHeandler.LoadSession -= lightManager.Load;
        SaveHeandler.LoadSession -= outFitManager.Load;
        SaveHeandler.LoadSession -= Load;
        SaveHeandler.LoadSession -= moduleDirection.Load;

        StartEntrying -= OnEntry;

        ShowInteractionButton -= gui.OnSetIntButton;
        OpenDialog -= gui.OnOpenDialog;
        OpenNPCPack -= gui.OnOpenNPCPack;
        UpdateAmmos -= gui.OnUpdateAmmos;
        input.PressInteractionButton -= OnPressIntButton;
        health.Death -= gui.OnDeath;
    }

    private void Start()
    {
        SaveHeandler.Load();

        StartCoroutine(Check());
    }

    private void Update()
    {
        UpdateAmmos?.Invoke(weaponManager.GetAllItems(), weaponManager.GetAmmosNow());
    }

    private void OnPressIntButton(string _settings)
    {
        if (_settings != "")
        {
            switch (_settings)
            {
                case "ETR":
                    StartEntrying?.Invoke(_targetEntry);
                    break;
                case "PAK":
                    OpenNPCPack?.Invoke(_targetPack);
                    break;
                case "DLG":
                    OpenDialog?.Invoke(_targetDialogList);
                    break;
            }
        }
    }

    IEnumerator Check()
    {
        
        while (true)
        {
            RaycastHit2D[] _hits = Physics2D.BoxCastAll(transform.position, new Vector2(sizeCheck, 9f), 0f, Vector2.zero, 0f, layer);

            if (_hits.Length > 0)
            {
                foreach (RaycastHit2D _hit in _hits)
                {
                    Collider2D _col = _hit.collider;

                    CoreObject _npc = _col.gameObject.GetComponentInParent<CoreObject>();
                    DialogList _dl = _col.gameObject.GetComponentInParent<DialogList>();
                    Entry _entry = _col.gameObject.GetComponent<Entry>();

                    if((_entry || (_npc && _dl) || (_npc && _npc.IsDide)) && !_isInterecrion)
                    {
                        if (_entry)
                            ShowInteractionButton?.Invoke("ETR");
                        else if (_npc)
                        {
                            if (_npc.IsDide)
                                ShowInteractionButton?.Invoke("PAK");
                            else if (_dl)
                            {
                                if (_dl.AutoOpenDialog)
                                    OpenDialog?.Invoke(_dl);
                                else
                                    ShowInteractionButton?.Invoke("DLG");
                            }
                        }

                        _targetPack = _npc && _npc.IsDide ? _npc.gameObject.GetComponent<NPCBackpack>() : null;
                        _targetDialogList = _dl;
                        _targetEntry = _entry;

                        _isInterecrion = true;
                    }
                }
            }
            else
            {
                if (_isInterecrion)
                    ShowInteractionButton?.Invoke("");
                _isInterecrion = false;
            }
                

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnEntry(Entry _ent)
    {
        SaveHeandler.Save();
        SaveHeandler.SessionNow.pos.x = _ent.meta.posTo.x;
        SaveHeandler.SessionNow.idScene = _ent.meta.locationToID;
        SceneManager.LoadScene(_ent.meta.locationToID, LoadSceneMode.Single);
    }

    private void Save()
    {
        SaveHeandler.SessionNow.pos.y = 0.8f;

        if (!_isInterecrion)
        {
            SaveHeandler.SessionNow.pos.x = transform.position.x;

            SaveHeandler.SessionNow.pos.flipX = (int)transform.localScale.x;
            SaveHeandler.SessionNow.idScene = SceneManager.GetActiveScene().buildIndex;
        }
    }

    private void Load()
    {
        transform.position = new Vector2(SaveHeandler.SessionNow.pos.x,
                                         SaveHeandler.SessionNow.pos.y);

        if (SaveHeandler.SessionNow.pos.flipX < 0)
        {
            //movement.toRight = false;
            Vector3 scaler = transform.localScale;
            scaler.x = scaler.x * -1;
            transform.localScale = scaler;
        }
    }
}
