using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Point : MonoBehaviour
{
    private bool canAfford = false;
    private bool isWeapon = false;
    private bool pickUp = false;
    private GameObject weapon;
    public weapon_switch wep;
    public TMP_Text toastText;
    public TMP_Text pointsText;
    private bool isAmmo = false;

    private PlayerGameManager PM;

    // starting set up
    void Start()
    {
        PM = GameObject.Find("GameManager").GetComponent<PlayerGameManager>();

    }

    private void Update()
    {
        pointsText.text = "$" + PM.money;
        if (Input.GetKeyDown(KeyCode.F) && isWeapon == true && canAfford == true)
        {
            PM.money -= weapon.gameObject.GetComponent<Weapon>().price;
            wep.pickUp(ref weapon);
        }
        if (Input.GetKeyDown(KeyCode.F) && isAmmo == true && canAfford == true)
        {
            Debug.Log("Bought ammo");
            PM.money -= weapon.gameObject.GetComponent<Weapon>().ammoPrice;
            weapon.gameObject.GetComponent<Weapon>().fillMaxAmmo();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            weapon =
                wep.gameObject
                    .GetComponent<weapon_switch>()
                    .transform.Find(other.gameObject.name).gameObject;
            var selPrice = weapon.gameObject.GetComponent<Weapon>().price;
            var ammoPrice = weapon.gameObject.GetComponent<Weapon>().ammoPrice;
            Debug.Log(weapon.name);
            if (weapon.gameObject.name == wep.currWeaponName)
            {
                toastText.text = "Press F to buy ammo ($" + ammoPrice + ")";
                isAmmo = true;
                if (ammoPrice > PM.money)
                    canAfford = false;
                else
                    canAfford = true;
            }
            else
            {
                isWeapon = true;
                toastText.text = "Press F to purchase ($" + selPrice + ")";
                if (selPrice > PM.money)
                    canAfford = false;
                else
                    canAfford = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        toastText.text = "";
        isWeapon = false;
        isAmmo = false;
        canAfford = false;
    }
}
