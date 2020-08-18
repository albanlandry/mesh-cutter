using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    public GameObject weaponPrefabs;
    public Transform weaponSlot;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1")) {
            // Instantiate the weapon inside the weapon slot
            Instantiate(weaponPrefabs, weaponSlot);
            weaponPrefabs.transform.position = weaponSlot.position;
            weaponPrefabs.transform.localRotation = weaponSlot.localRotation;
        }
    }
}
