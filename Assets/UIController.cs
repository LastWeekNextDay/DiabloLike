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
    // Start is called before the first frame update
    void Start()
    {
        _weaponSlotFull = GameObject.Find("WeaponSlotFull");
        _abilitySlotFull = GameObject.Find("AbilitySlotFull");
        _movementSlotFull = GameObject.Find("MovementSlotFull");
        _HPFull = GameObject.Find("HPFull");
        _HPText = _HPFull.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        _MPFull = GameObject.Find("MPFull");
        _MPText = _MPFull.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        _weaponTexture = _weaponSlotFull.transform.GetChild(1).gameObject.GetComponent<RawImage>();
        _abilityTexture = _abilitySlotFull.transform.GetChild(1).gameObject.GetComponent<RawImage>();
        _movementTexture = _movementSlotFull.transform.GetChild(1).gameObject.GetComponent<RawImage>();
        _canvas = GameObject.Find("Canvas");
        _damageNumbers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeaponSlot();
        UpdateAbilitySlot();
        UpdateHPandMP();
        UpdateWorldUIElementsToLookAtCamera();
    }


    void UpdateWeaponSlot()
    {
        if (_weaponTexture.texture != GameObject.Find("Player").GetComponent<Character>().EquippedWeapon.Texture)
            _weaponTexture.texture = GameObject.Find("Player").GetComponent<Character>().EquippedWeapon.Texture;   
    }

    void UpdateAbilitySlot()
    {
        if (_abilityTexture.texture != GameObject.Find("Player").GetComponent<Character>().EquippedAbility.Texture)
            _abilityTexture.texture = GameObject.Find("Player").GetComponent<Character>().EquippedAbility.Texture;
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

    void UpdateMovementSlot()
    {
        if (_movementTexture.texture != GameObject.Find("Player").GetComponent<Character>().EquippedMovementAbility.Texture)
            _movementTexture.texture = GameObject.Find("Player").GetComponent<Character>().EquippedMovementAbility.Texture;
    }

    public void ShowDamageNumber(int damage, GameObject charObj, Character starter)
    {
        if (starter.isPlayer == false)
        {
            return;
        }
        GameObject obj = Resources.Load<GameObject>("DamageNumber");
        
        float height = charObj.GetComponent<BoxCollider>().size.y;
        Vector3 pos = charObj.transform.position;
        pos.y += height;
        GameObject damageNumber = Instantiate(obj, pos, Quaternion.identity);
        damageNumber.GetComponent<TextMeshPro>().text = damage.ToString();
        damageNumber.GetComponent<TextMeshPro>().color = Color.red;
        damageNumber.transform.SetParent(_canvas.transform);
        _damageNumbers.Add(damageNumber);
        StartCoroutine("FadeThenRemoveDamageNumber", damageNumber);
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
