using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IMetaEssence
{
    [Header("Button F")]
    [SerializeField] private int sizeCheck;
    [SerializeField] private LayerMask layer;
    [Header("Inital")]
    public GUIHandler handlerGUI;
    public Movement movement;
    public WeaponManager weaponManager;
    public LightManager lightManager;
    public OutFitManager outfit;
    public Detector detector;
    public Health health;
    public Energy energy;
    [Space]
    [SerializeField] private DialogCall[] calls;
    [SerializeField] private Quest[] quests;

    private bool _isInterecrion = false;
    private Coroutine _coroutine;

    public Dictionary<string, Sprite> visualEnemy { get; set; } = new ();
    public string Name { get; set; }
    public TypeGroup TypeG { get; set; }

    public void Initialization()
    {
        weaponManager.inv = handlerGUI.inventory;
        /*
        if (handlerGUI.input != null)
        {
            handlerGUI.input.OnMove += movement.Move;

            handlerGUI.input.OnShoot += weaponManager.SetIsShoot;
            handlerGUI.input.OnSetNumWeapon += weaponManager.SetNumberWeapon;
            handlerGUI.input.OnResetOutfit += weaponManager.SetGunList;
            handlerGUI.input.OnRload += weaponManager.StartReload;

            handlerGUI.input.OnResetOutfit += lightManager.OnResetOutfit;
            handlerGUI.input.OnLight += lightManager.OnSetLight;

            handlerGUI.input.OnResetOutfit += outfit.OnResetOutfit;

            handlerGUI.input.OnCastBolt += weaponManager.OnCastBolt;
        } */ 
    }

    private void Awake()
    {
        TypeG = TypeGroup.Stalker;
        weaponManager.Meta = this;

        transform.position = new Vector2(SaveHeandler.SessionSave.pos.x,
                                         SaveHeandler.SessionSave.pos.y);

        if (SaveHeandler.SessionSave.pos.flipX < 0)
        {
            movement.toRight = false;
            Vector3 scaler = transform.localScale;
            scaler.x = scaler.x * -1;
            transform.localScale = scaler;
        }

        if (SaveHeandler.SessionSave.GetSwitchObject("StartCall") && SceneManager.GetActiveScene().buildIndex == 3)
        {
            handlerGUI.Call.OpenDialog(calls[0]);
            SaveHeandler.SessionSave.SetSwitchObject("StartCall", false);
        }
        if (SaveHeandler.SessionSave.GetSwitchObject("ProvodnikCall-Cerkov") && SceneManager.GetActiveScene().buildIndex == 4)
        {
            handlerGUI.Call.OpenDialog(calls[1]);
            SaveHeandler.SessionSave.SetSwitchObject("ProvodnikCall-Cerkov", false);
        }
    }

    private void OnEnable()
    {
        if (handlerGUI.input != null)
        {
            handlerGUI.input.OnMove += movement.Move;

            handlerGUI.input.OnShoot += weaponManager.SetIsShoot;
            handlerGUI.input.OnSetNumWeapon += weaponManager.SetNumberWeapon;
            handlerGUI.input.OnResetOutfit += weaponManager.SetGunList;
            handlerGUI.input.OnRload += weaponManager.StartReload;

            handlerGUI.input.OnResetOutfit += lightManager.OnResetOutfit;
            handlerGUI.input.OnLight += lightManager.OnSetLight;

            handlerGUI.input.OnResetOutfit += outfit.OnResetOutfit;

            handlerGUI.input.OnCastBolt += weaponManager.OnCastBolt;
        }

        handlerGUI.Detector.OnSetChecking += detector.Checking;
        handlerGUI.inventory.Disct.OnUse += Use;
        health.Deid += handlerGUI.Dide;
        health.OnChangeValueHealth += UpdateHealth;
        health.SetDebaff += OnDebuff;
        health.OnEndInitilization += UpdateHealth;
        SaveHeandler.OnSaveSession += SaveSessinon;
    }

    private void OnDisable()
    {
        if (handlerGUI.input != null)
        {
            handlerGUI.input.OnMove -= movement.Move;

            handlerGUI.input.OnShoot -= weaponManager.SetIsShoot;
            handlerGUI.input.OnSetNumWeapon -= weaponManager.SetNumberWeapon;
            handlerGUI.input.OnResetOutfit -= weaponManager.SetGunList;
            handlerGUI.input.OnRload -= weaponManager.StartReload;

            handlerGUI.input.OnResetOutfit -= lightManager.OnResetOutfit;
            handlerGUI.input.OnLight -= lightManager.OnSetLight;

            handlerGUI.input.OnResetOutfit -= outfit.OnResetOutfit;

            handlerGUI.input.OnCastBolt -= weaponManager.OnCastBolt;
        }

        handlerGUI.Detector.OnSetChecking -= detector.Checking;
        handlerGUI.inventory.Disct.OnUse -= Use;
        health.Deid -= handlerGUI.Dide;
        health.OnChangeValueHealth -= UpdateHealth;
        health.SetDebaff -= OnDebuff;
        health.OnEndInitilization -= UpdateHealth;
        SaveHeandler.OnSaveSession -= SaveSessinon;
    }

    private void Update()
    {
        if (weaponManager._numWeapon <= 1 && weaponManager._guns[weaponManager._numWeapon] != null && weaponManager._flagWeapon)
            handlerGUI.UpdateAmmos(handlerGUI.inventory.GetCountAmmos(weaponManager._guns[weaponManager._numWeapon]),
                                                                      weaponManager._guns[weaponManager._numWeapon].currentAmmos);
        else
            handlerGUI.UpdateAmmos(-1, -1);

        float reason;

        handlerGUI.UpdateMass(outfit.MaxMass, handlerGUI.inventory.allMass);
        if ((reason = handlerGUI.inventory.allMass - outfit.MaxMass) > 0)
            movement.debufSpeed = reason / 10;
        else
            movement.debufSpeed = 0;

        handlerGUI.UpdateEnergy(energy.energy);

        if (_coroutine == null)
            _coroutine = StartCoroutine(Check());
        
        if (weaponManager._flagWeapon)
        {
            if (weaponManager._numWeapon < 2)
            {
                handlerGUI.UpdateButtons(weaponManager._guns[weaponManager._numWeapon].currentAmmos > 0, 
                    weaponManager._guns[weaponManager._numWeapon].currentAmmos < weaponManager._guns[weaponManager._numWeapon].ammo &&
                    handlerGUI.inventory.GetCountAmmos(weaponManager._guns[weaponManager._numWeapon]) > 0,
                    lightManager.HaveLight);
            }
            else
            {
                handlerGUI.UpdateButtons(true, false, lightManager.HaveLight);
            }
        }
        else
        {
            handlerGUI.UpdateButtons(false, false, lightManager.HaveLight);
        }

        if (SaveHeandler.SessionSave.GetSwitchObject("PriceNasos") && !SaveHeandler.SessionSave.GetSwitchObject("PriceNasos-End"))
        {
            handlerGUI.Call.OpenDialog(calls[2]);
            handlerGUI.questManager.AddQuest(quests[2]);
            SaveHeandler.SessionSave.SetSwitchObject("PriceNasos-End", true);
        } 
    }

    private void OnTouchMultiButton()
    {
        Collider2D[] _list = Physics2D.OverlapCircleAll(transform.position, sizeCheck, layer);

        if (_list.Length > 0)
        {
            foreach (Collider2D col in _list)
            {
                NPC _npc = col.gameObject.GetComponentInParent<NPC>();
                DialogList _dialogList = col.gameObject.GetComponentInParent<DialogList>();

                if (_npc != null)
                {
                    if (_npc.backpack)
                    {
                        handlerGUI.OpenNPCPack();
                        handlerGUI.inventory.OnBackpackNPC(_npc);
                    }
                    else if (_dialogList)
                    {
                        handlerGUI.SetDialog(_dialogList, _dialogList.startDialog);
                    }
                }
            }
        }
    }

    IEnumerator Check()
    {
        while (true)
        {
            RaycastHit2D _hit = Physics2D.BoxCast(transform.position, new Vector2(sizeCheck, 3f), 0f, Vector2.zero, 0f, layer);

            if (_hit)
            {
                if (!_isInterecrion)
                {
                    Collider2D _col = _hit.collider;

                    NPC _npc = _col.gameObject.GetComponentInParent<NPC>();
                    DialogList _dialogList = _col.gameObject.GetComponentInParent<DialogList>();
                    Entry _entry = _col.gameObject.GetComponent<Entry>();

                    if (_entry)
                        handlerGUI.ButtonInterection(null, null, null, _entry);
                    else if (_npc)
                    {
                        if (_npc.backpack)
                            handlerGUI.ButtonInterection(null, null, _npc);
                        else if (_dialogList)
                        {
                            if (_dialogList.AutoOpenDialog)
                                handlerGUI.SetDialog(_dialogList, _dialogList.startDialog);
                            else
                                handlerGUI.ButtonInterection(_dialogList, _dialogList.startDialog);
                        }
                            
                    }

                    _isInterecrion = true;
                }
            }
            else
            {
                handlerGUI.ButtonInterection();
                _isInterecrion = false;
            }
                

            yield return new WaitForEndOfFrame();
        }
    }

    private void SaveSessinon()
    {
        SaveHeandler.SessionSave.pos.y = 0.8f;

        if (!_isInterecrion)
        {
            SaveHeandler.SessionSave.pos.x = transform.position.x;

            SaveHeandler.SessionSave.pos.flipX = (int)transform.localScale.x;
            SaveHeandler.SessionSave.idScene = SceneManager.GetActiveScene().buildIndex;
        }
    }

    private void Use(ObjectItem _item)
    {
        if (_item.item.medicObject)
            handlerGUI.Health.SetMedicMenu(health.listBodyParths);
    }

    private void UpdateHealth()
    {
        handlerGUI.UpdateHealth(health.HealthAll);
        handlerGUI.Health.SetOverlyBody(health.listBodyParths);
        handlerGUI.Health.SetBodyMenu(health.listBodyParths);
    }

    private void OnDebuff(TypeBodyParth _type, float _value)
    {
        if (_type == TypeBodyParth.Leg)
            movement.debufSpeed += _value;
    }
}
