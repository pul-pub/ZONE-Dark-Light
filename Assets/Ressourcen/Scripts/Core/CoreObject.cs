using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoreObject : MonoBehaviour, IMetaEssence
{
    public string Name { get => nameEssence; }
    public TypeGroup Group { get => group; }
    public bool IsDide { get => isDide; }
    public ViewEssence Visual 
    {
        get
        {
            ViewEssence viw = new ViewEssence();

            if (AutoSetViwe)
            {
                Dictionary<string, Sprite> _outfit = outFitManager?.GetImages();
                Dictionary<string, Sprite> _weapon = weaponManager?.GetImages();

                viw.Light = lightManager?.GetImages();
                viw.Mask = _outfit["MAS"];
                viw.Face = _outfit["FAC"];

                viw.Body = _outfit["BOD"];
                viw.Body2 = null;
                viw.Backpack = _outfit["PAK"];
                viw.Gun = _weapon["GUN"];
                viw.Pistol = _weapon["PIS"];

                viw.Hand = _outfit["HND"];

                viw.Leg = _outfit["LEG"];

                Debug.Log(_weapon["GUN"]);
            }
            else
                viw.Body = sprBody;

            return viw;
        } 
    }

    [Header("-----------  Base  -----------")]
    [SerializeField] private string nameEssence;
    [SerializeField] private TypeGroup group;
    [SerializeField] private bool isDide = false;
    [SerializeField] private bool AutoSetViwe = true;
    [SerializeField] private Sprite sprBody;
    [Header("--------  Interface  --------")]
    [SerializeField] private GameObject parentInput;
    [SerializeField] private GameObject parentPack;
    [Header("---------  Modules  ---------")]
    [SerializeField] private Movement movement;
    [SerializeField] private ModuleDirectionHands viwe;
    [SerializeField] private Health health;
    [SerializeField] private Energy energy;
    [SerializeField] private OutFitManager outFitManager;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private EventAnimationHands handlerHands;
    [SerializeField] private LightManager lightManager;
    [SerializeField] private Detector detector;
    [SerializeField] private BoltManager bolt;
    [Header("---------  Weapon  ---------")]
    [SerializeField] private bool isOptionsWeapon = false;
    [ConditionallyVisible(nameof(isOptionsWeapon))]
    [SerializeField] private bool upWeapon = false;
    [ConditionallyVisible(nameof(isOptionsWeapon))]
    [SerializeField] private int numWeapon = 0;
    [Header("----------  Death  ---------")]
    [SerializeField] private bool isOptionsDeath = false;
    [ConditionallyVisible(nameof(isOptionsDeath))]
    [SerializeField] private Vector3 rootationDeath;
    [ConditionallyVisible(nameof(isOptionsDeath))]
    [SerializeField] private Vector2 offsetColiderDeath;

    private IInput input;
    private IPack pack;

    private void Awake()
    {
        if ((input = parentInput.GetComponent<IInput>()) == null)
            input = parentInput.GetComponentInChildren<IInput>();
        if ((pack = parentPack.GetComponent<IPack>()) == null)
            pack = parentPack.GetComponentInChildren<IPack>();
    }

    private void OnEnable()
    {
        if (movement)
            input.Move += movement.Move;
        if (viwe)
            input.Viwe += viwe.OnSetDirection;
        if (health)
        {
            health.Death += OnDeath;
            health.Death += pack.CreateDeathPack;
        }
        if (weaponManager)
        {
            input.Shoot += weaponManager.OnShoot;
            input.Reload += weaponManager.StartReload;
            input.SwitchWeapon += weaponManager.OnSwitchWeapon;
            input.CastBolt += weaponManager.OnCastelBolt;
            pack.ChangeOutfit += weaponManager.OnUpdateOutFit;
            if (handlerHands)
            {
                handlerHands.TakeBolt += weaponManager.OnEndCastelBolt;
                handlerHands.EndReload += weaponManager.EndReload;
            }    
        }
        if (outFitManager)
            pack.ChangeOutfit += outFitManager.OnResetOutfit;
        if (lightManager)
        {
            pack.ChangeOutfit += lightManager.OnResetOutfit;
            input.SwitchLight += lightManager.OnSetLight;
        } 
        if (bolt)
        {
            input.CastBolt += bolt.OnStartCastelBolt;
            if (handlerHands)
                handlerHands.TakeBolt += bolt.OnCastelBolt;
        }  
    }

    private void OnDisable()
    {
        if (movement)
            input.Move -= movement.Move;
        if (viwe)
            input.Viwe -= viwe.OnSetDirection;
        if (health)
        {
            health.Death -= OnDeath;
            health.Death -= pack.CreateDeathPack;
        }  
        if (weaponManager)
        {
            input.Shoot -= weaponManager.OnShoot;
            input.Reload -= weaponManager.StartReload;
            input.SwitchWeapon -= weaponManager.OnSwitchWeapon;
            input.CastBolt -= weaponManager.OnCastelBolt;
            pack.ChangeOutfit -= weaponManager.OnUpdateOutFit;
            if (handlerHands)
            {
                handlerHands.TakeBolt -= weaponManager.OnEndCastelBolt;
                handlerHands.EndReload -= weaponManager.EndReload;
            }
        }
        if (outFitManager)
            pack.ChangeOutfit -= outFitManager.OnResetOutfit;
        if (lightManager)
        {
            pack.ChangeOutfit -= lightManager.OnResetOutfit;
            input.SwitchLight -= lightManager.OnSetLight;
        }
        if (bolt)
        {
            input.CastBolt -= bolt.OnStartCastelBolt;
            if (handlerHands)
                handlerHands.TakeBolt -= bolt.OnCastelBolt;
        }
    }

    private void Start()
    {
        if (weaponManager)
            weaponManager.Meta = this;

        input.Initialization(Group);
        pack.Initialization();

        weaponManager?.Initialization(numWeapon, upWeapon, pack);
    }

    private void OnDeath(IMetaEssence _meta)
    {
        isDide = true;
        if (isOptionsDeath)
        {
            transform.rotation = Quaternion.Euler(rootationDeath);
            foreach (BoxCollider2D box in gameObject.GetComponentsInChildren<BoxCollider2D>())
                box.offset += offsetColiderDeath;

            viwe?.OnSetDirection(Vector2.left);
            movement?.Move(Vector2.zero);
            weaponManager?.Initialization(0, false, pack);
        }
        if (name == "MninBoss-1")
            SaveHeandler.SessionNow.SetSwitchObject("MninBoss-1", false);

        if (movement)
            input.Move -= movement.Move;
        if (viwe)
            input.Viwe -= viwe.OnSetDirection;
        if (health)
        {
            health.Death -= OnDeath;
            health.Death -= pack.CreateDeathPack;
        }
        if (weaponManager)
        {
            input.Shoot -= weaponManager.OnShoot;
            input.Reload -= weaponManager.StartReload;
            input.SwitchWeapon -= weaponManager.OnSwitchWeapon;
            input.CastBolt -= weaponManager.OnCastelBolt;
            pack.ChangeOutfit -= weaponManager.OnUpdateOutFit;
            if (handlerHands)
            {
                handlerHands.TakeBolt -= weaponManager.OnEndCastelBolt;
                handlerHands.EndReload -= weaponManager.EndReload;
            }
        }
        if (outFitManager)
            pack.ChangeOutfit -= outFitManager.OnResetOutfit;
        if (lightManager)
        {
            pack.ChangeOutfit -= lightManager.OnResetOutfit;
            input.SwitchLight -= lightManager.OnSetLight;
        }
        if (bolt)
        {
            input.CastBolt -= bolt.OnStartCastelBolt;
            if (handlerHands)
                handlerHands.TakeBolt -= bolt.OnCastelBolt;
        }
    }
}

#if UNITY_EDITOR
[Serializable]
public class ConditionallyVisibleAttribute : PropertyAttribute
{
    public string ConditionPropertyName { get; private set; }

    public ConditionallyVisibleAttribute(string conditionPropertyName)
    {
        ConditionPropertyName = conditionPropertyName;
    }
}

[Serializable]
[CustomPropertyDrawer(typeof(ConditionallyVisibleAttribute))]
public class ConditionallyVisibleDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var condAttr = (ConditionallyVisibleAttribute)attribute;
        var conditionProperty = property.serializedObject.FindProperty(condAttr.ConditionPropertyName);

        if (conditionProperty != null && conditionProperty.boolValue)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var condAttr = (ConditionallyVisibleAttribute)attribute;
        var conditionProperty = property.serializedObject.FindProperty(condAttr.ConditionPropertyName);

        if (conditionProperty != null && conditionProperty.boolValue)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        return 0;
    }
}
#endif
