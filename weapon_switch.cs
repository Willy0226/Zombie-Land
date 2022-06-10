using UnityEngine;

public class weapon_switch : MonoBehaviour
{
    public string currWeaponName;
    public int selectedWeapon = 0;
    public int startingWeapon = 0;
    private int[] inv = { 0, -1 }; //-1 means null

    // Start is called before the first frame update
    public void Start()
    {
        inv[0] = startingWeapon;
        SelectWeapon();
    }

    // Update is called once per frame
    public void Update()
    {
        int previousSelected = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = 1;
            else
                selectedWeapon--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }

        if (previousSelected != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    public void SelectWeapon()
    {
        int i = 0;
        var currWeapon = inv[selectedWeapon];
        if (currWeapon != -1)
        {
            foreach (Transform weapon in transform)
            {
                if (i == currWeapon)
                {
                    weapon.gameObject.SetActive(true);
                    currWeaponName = weapon.name;
                }
                else
                    weapon.gameObject.SetActive(false);
                i++;
            }
        }
        else
        {
            return;
        }
    }

    public void pickUp(ref GameObject Item)
    {
        int i = 0;
        int y = 0;

        foreach (Transform weapon in transform)
        {
            if (y == inv[selectedWeapon])
            {
                if (inv[1] != -1)
                    weapon.gameObject.SetActive(false);
                else
                    inv[1] = y;
                break;
            }
            y++;
        }

        y = 0;

        foreach (Transform weapon in transform)
        {
            if (Item.name == weapon.gameObject.name)
            {
                Debug.Log("GUN FOUND");
                weapon.gameObject.SetActive(true);
                inv[selectedWeapon] = i;
                break;
            }
            i++;
        }
        SelectWeapon();
    }
}
