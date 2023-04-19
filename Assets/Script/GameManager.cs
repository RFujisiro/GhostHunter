using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerInfo
{
    int mainWeapon_ID, subWeapon_ID;
    int item1_ID, item2_ID,item3_DI;
    int parts1_ID, parts2_ID, parts3_DI;
}

class EquipmentSelect
{
    //trueならその武器を持っている
    bool[] mainWeapon, subWeapon;
    //そのアイテムを持っている数
    int[] item;
    //改造パーツのレベル、0なら未取得
    int[] parts;
}
[System.Serializable]
public class Shop
{
    [System.Serializable]
    public struct Weapon
    {
        public int price;
        public string text;
    }
    public Weapon[] weapons;
    [System.Serializable]
    public struct Item
    {
       public int price;
       public string text;

    }
    public Item[] item;
    [System.Serializable]
    public struct Parts
    {
        public int price;
        public string text;
    }
    public Parts[] parts;

}

[System.Serializable]
public class WeaponInfo
{
    public string weaponName;
    public int weaponID;
    public float damage, speed, rate, accuracy_MAX;

    AudioSource audio;
    AudioClip shotSE;
    public GameObject weaponObject, bulletPrefab;


    public Vector3 muzzlePos;
    float mainShotCoolDown, mainShotCoolDown_MAX;
    float accuracy;

    public bool isShot = false;

    public void Set(GameObject weaponObjectTmp,int id)
    {
        weaponID = id;
        weaponObject = weaponObjectTmp;
        audio = weaponObject.GetComponent<AudioSource>();
        shotSE = Resources.Load<AudioClip>("Sound/EnergyShot");
        bulletPrefab = Resources.Load<GameObject>("Prefab/Weapon/Bullet");
        mainShotCoolDown_MAX = 1 / rate;
    }

    public void Update()
    {
        if (mainShotCoolDown > 0)
            mainShotCoolDown -= Time.deltaTime;
        if (mainShotCoolDown < 0) mainShotCoolDown = 0;
        if(isShot&&mainShotCoolDown==0)Shot();
        if(!isShot&&accuracy>0)accuracy-=Time.deltaTime;
        if(!isShot&&accuracy<0)accuracy=0;
    }
    public void Shot()
    {
        if (mainShotCoolDown > 0) return;
        audio.pitch = 1.5f;
        audio.PlayOneShot(shotSE);
        Vector3 shotDirTmp = weaponObject.transform.eulerAngles;
        shotDirTmp.x += Random.Range(-accuracy, accuracy);
        shotDirTmp.y += Random.Range(-accuracy, accuracy);
        Quaternion shotDir = Quaternion.Euler(shotDirTmp);
        GameObject bullet = MonoBehaviour.Instantiate(bulletPrefab, muzzlePos, shotDir);
        bullet.GetComponent<Bullet>().Shot(damage, speed);
        mainShotCoolDown = mainShotCoolDown_MAX;
        if (accuracy < accuracy_MAX) accuracy += 0.1f;
    }

    #region SubShots

    public void OnSubShot()
    {
        switch (weaponID)
        {
            case 1:SubShot_EnergyLifle();break;
        }
    }

    void SubShot_EnergyLifle()
    {
        audio.PlayOneShot(shotSE);
        for (int i = 0; i < 10; i++)
        {
            var shotDirTmp = weaponObject.transform.eulerAngles;
            shotDirTmp.x += Random.Range(-5, 5);
            shotDirTmp.y += Random.Range(-5, 5);
            Quaternion shotDir = Quaternion.Euler(shotDirTmp);
            GameObject bullet = MonoBehaviour.Instantiate(bulletPrefab, muzzlePos, shotDir);
            bullet.GetComponent<Bullet>().Shot(damage, speed);
        }

    }

    #endregion
}

#region Enemys
[System.Serializable]
public class EnemyInfo
{
    public int enemyID;

    public string name;
    public int hitPoint, defense, speed, attack;
    public bool isFade,isFind;
    Material normalMaterial, fadeMaterial;
    GameObject playerObject, enemyObject;
    Player playerScript;
    Rigidbody rb;
    SkinnedMeshRenderer smr;

    public void Set(GameObject tmpEnemyObject)
    {
        enemyObject = tmpEnemyObject;
        rb = enemyObject.GetComponent<Rigidbody>();
        smr = enemyObject.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();

        string loadtmp = string.Format("EnemyMaterial/{0}/normal",name);
        normalMaterial = Resources.Load<Material>(loadtmp);
        loadtmp = string.Format("EnemyMaterial/{0}/fade", name);
        fadeMaterial = Resources.Load<Material>(loadtmp);

        playerObject = GameObject.FindWithTag("Player").gameObject;
        playerScript = playerObject.GetComponent<Player>();

    }
    public void OnFadeSwitch()
    {
        if (playerScript.onGhostView) smr.material = normalMaterial;
        else smr.material = fadeMaterial;
    }
    public void OnMove()
    {
        if (!isFind) return;
        enemyObject.transform.LookAt(playerObject.transform.position);
        rb.velocity = enemyObject.transform.forward*speed;
    }
}


#endregion
public class GameManager : MonoBehaviour
{
    public WeaponInfo[] weaponInfo;
    public EnemyInfo[] enemyInfo;
    public Shop Shop;

    // Start is called before the first frame update
    void Start()
    {
        var enemyInfo_save=JsonUtility.ToJson(enemyInfo[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public WeaponInfo GetWInfo(int ID)
    {
        return weaponInfo[ID];
    }

    public EnemyInfo GetEnemyInfo(int id)
    {
        return enemyInfo[id];
    }
}
