using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSC : MonoBehaviour
{
    AudioClip shotSE;
    WeaponInfo weaponInfo = new WeaponInfo();
    [SerializeField] int weaponID;
    [SerializeField] GameObject muzzleObject;


    private void Awake()
    {
        weaponInfo = GameObject.Find("GameManager").GetComponent<GameManager>().GetWInfo(weaponID);
        weaponInfo.Set(gameObject);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        weaponInfo.Update(muzzleObject.transform.position);
    }

    public bool isSubShot;

    #region Input
    public void InputKey(string key, bool on)
    {
        switch (key)
        {
            case "Main": weaponInfo.isShot = on; break;
            case "Sub": weaponInfo.OnSubShot(); break;
            case "ReLoad":weaponInfo.ReLoad(); break;
        }
    }


    #endregion

}
