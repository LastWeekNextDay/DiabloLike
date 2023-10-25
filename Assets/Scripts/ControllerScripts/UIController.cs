using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameObject _weaponSlotFull;
    private GameObject _abilitySlotFull;
    private GameObject _movementSlotFull;
    private GameObject _HPFull;
    private GameObject _MPFull;
    private TextMeshProUGUI _HPText;
    private TextMeshProUGUI _MPText;

    private RawImage _weaponTexture;
    private RawImage _abilityTexture;
    private RawImage _movementTexture;

    private GameObject _canvas;
    private List<GameObject> _damageNumbers;
    private GameObject _damageNumber;
    private GameObject _damageEffect;
    
    private GameObject _invObj;
    private bool _invActive = false;

    private GameObject _invLoaded;
    private SceneController _sceneController;
    private PlayerController _player;
    // Start is called before the first frame update
    void Start()
    {
        _sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        _weaponSlotFull = GameObject.Find("WeaponSlotFull");
        _abilitySlotFull = GameObject.Find("AbilitySlotFull");
        _movementSlotFull = GameObject.Find("MovementSlotFull");
        _invLoaded = Resources.Load<GameObject>("Misc/Inventory");
        _HPFull = GameObject.Find("HPFull");
        _HPText = _HPFull.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        _MPFull = GameObject.Find("MPFull");
        _MPText = _MPFull.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        _weaponTexture = _weaponSlotFull.transform.GetChild(1).gameObject.GetComponent<RawImage>();
        _abilityTexture = _abilitySlotFull.transform.GetChild(1).gameObject.GetComponent<RawImage>();
        _movementTexture = _movementSlotFull.transform.GetChild(1).gameObject.GetComponent<RawImage>();
        _canvas = GameObject.Find("Canvas");
        _damageNumbers = new List<GameObject>();
        _damageNumber = Resources.Load<GameObject>("Misc/DamageNumber");
        _damageEffect = Resources.Load<GameObject>("Misc/DamageEffect");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeaponSlot();
        UpdateAbilitySlot();
        UpdateMovementSlot();
        UpdateHPandMP();
        UpdateWorldUIElementsToLookAtCamera();
        UpdateCooldownUI();
    }

    public void ToggleInventory()
    {
        if (_sceneController.GameplayAllowed)
        {
            if (_invActive)
            {
                Destroy(_invObj);
                _invActive = false;
                _player.CanMove = true;
                _player.CanAttack = true;
            } else
            {
                _invObj = Instantiate(_invLoaded, _canvas.transform);
                _invActive = true;
                _player.CanMove = false;
                _player.CanAttack = false;
            }
        }
    }

    void UpdateWeaponSlot()
    {
        if (_weaponTexture.texture != GameObject.Find("Player").GetComponent<Character>().EquippedWeapon.GetComponent<Weapon>().Texture)
            _weaponTexture.texture = GameObject.Find("Player").GetComponent<Character>().EquippedWeapon.GetComponent<Weapon>().Texture;
    }

    void UpdateAbilitySlot()
    {
        if (_abilityTexture.texture != GameObject.Find("Player").GetComponent<Character>().EquippedAbility.GetComponent<Ability>().Texture)
            _abilityTexture.texture = GameObject.Find("Player").GetComponent<Character>().EquippedAbility.GetComponent<Ability>().Texture;
    }

    void UpdateHPandMP()
    {
        if (_HPText.text != GameObject.Find("Player").GetComponent<Character>().Health.ToString())
            _HPText.text = GameObject.Find("Player").GetComponent<Character>().Health.ToString();
        _HPFull.transform.GetChild(0).gameObject.GetComponent<Slider>().value = (float)GameObject.Find("Player").GetComponent<Character>().Health / (float)GameObject.Find("Player").GetComponent<Character>().MaxHealth;


        if (_MPText.text != GameObject.Find("Player").GetComponent<Character>().Mana.ToString())
            _MPText.text = GameObject.Find("Player").GetComponent<Character>().Mana.ToString();
        _MPFull.transform.GetChild(0).gameObject.GetComponent<Slider>().value = (float)GameObject.Find("Player").GetComponent<Character>().Mana / (float)GameObject.Find("Player").GetComponent<Character>().MaxMana;
    }

    void UpdateCooldownUI()
    {
        if (GameObject.Find("Player").GetComponent<Character>().EquippedAbility != null)
        {
            float abilityCooldown = GameObject.Find("Player").GetComponent<Character>().EquippedAbility.GetComponent<Ability>().Cooldown;
            float abilityCurrentCooldown = GameObject.Find("Player").GetComponent<Character>().EquippedAbility.GetComponent<Ability>().CooldownTimer;
            float abilityCooldownRatio = abilityCurrentCooldown / abilityCooldown;
            _abilitySlotFull.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(65, 65 * abilityCooldownRatio);
        }
        if (GameObject.Find("Player").GetComponent<Character>().EquippedMovementAbility != null)
        {
            float movementCooldown = GameObject.Find("Player").GetComponent<Character>().EquippedMovementAbility.GetComponent<Ability>().Cooldown;
            float movementCurrentCooldown = GameObject.Find("Player").GetComponent<Character>().EquippedMovementAbility.GetComponent<Ability>().CooldownTimer;
            float movementCooldownRatio = movementCurrentCooldown / movementCooldown;
            _movementSlotFull.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(65, 65 * movementCooldownRatio);
        }
    }

    void UpdateMovementSlot()
    {
        if (_movementTexture.texture != GameObject.Find("Player").GetComponent<Character>().EquippedMovementAbility.GetComponent<Ability>().Texture)
            _movementTexture.texture = GameObject.Find("Player").GetComponent<Character>().EquippedMovementAbility.GetComponent<Ability>().Texture;
    }

    public void ShowDamageNumber(int damage, GameObject receiver, GameObject starter)
    {
        if (starter.GetComponent<PlayerController>() == null)
        {
            if (receiver.GetComponent<PlayerController>() != null)
            {
                CalculateAndDisplayDamageEffect(damage);
            }
            return;
        }
        float height = receiver.GetComponent<BoxCollider>().size.y;
        Vector3 pos = receiver.transform.position;
        pos.y += height;
        GameObject damageNumber = Instantiate(_damageNumber, pos, Quaternion.identity);
        damageNumber.GetComponent<TextMeshPro>().text = damage.ToString();
        damageNumber.GetComponent<TextMeshPro>().color = Color.red;
        damageNumber.transform.SetParent(_canvas.transform);
        _damageNumbers.Add(damageNumber);
        StartCoroutine("FadeThenRemoveDamageNumber", damageNumber);
    }

    void CalculateAndDisplayDamageEffect(int damage)
    {
        GameObject effect = Instantiate(_damageEffect, GameObject.Find("Canvas").transform);
        effect.transform.SetAsFirstSibling();
        float hurtRatio = (float)damage / (float)GameObject.Find("Player").GetComponent<Character>().Health;
        effect.GetComponent<DamageEffectScript>().HitDamageEffectThreshold = false;
        StartCoroutine(FadeThenRemoveDamageEffect(hurtRatio, effect));
    }

    IEnumerator FadeThenRemoveDamageEffect(float endPointRatio, GameObject effect)
    {
        while (effect.GetComponent<RawImage>().color.a < endPointRatio && effect.GetComponent<DamageEffectScript>().HitDamageEffectThreshold == false)
        {
            effect.GetComponent<RawImage>().color += new Color(0, 0, 0, 0.01f);
            if (effect.GetComponent<RawImage>().color.a >= endPointRatio)
            {
                effect.GetComponent<DamageEffectScript>().HitDamageEffectThreshold = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        while (effect.GetComponent<RawImage>().color.a > 0 && effect.GetComponent<DamageEffectScript>().HitDamageEffectThreshold)
        {
            effect.GetComponent<RawImage>().color -= new Color(0, 0, 0, 0.01f);
            yield return null;
        }
        Destroy(effect);
    }

    IEnumerator FadeThenRemoveDamageNumber(GameObject damageNumber)
    {
        while (damageNumber.GetComponent<TextMeshPro>().fontSize > 0)
        {
            damageNumber.GetComponent<TextMeshPro>().fontSize -= 0.1f;
            yield return null;
        }
        _damageNumbers.Remove(damageNumber);
        Destroy(damageNumber);
    }

    void UpdateWorldUIElementsToLookAtCamera()
    {
        foreach (GameObject damageNumber in _damageNumbers)
        {
            damageNumber.transform.LookAt(Camera.main.transform);
            damageNumber.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }
}
