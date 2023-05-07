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
    int weapon_ammo,weapon_ammo_MAX,weapon_spareAmmo, weapon_spareAmmo_MAX;

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
        subShot.Set(this);
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
        if (weapon_ammo == 0) { ReLoad(); return; }
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
    void ReLoad()
    {
        int ammoDiff = weapon_ammo_MAX - weapon_ammo;//リロードする数
        if (ammoDiff <= weapon_spareAmmo)
        {
            weapon_ammo = weapon_ammo_MAX;
            weapon_spareAmmo -= ammoDiff;
        }
        else
        {
            weapon_ammo = weapon_spareAmmo;
            weapon_spareAmmo = 0;
        }
    }

    #region SubShots
    [System.Serializable]
    public class SubShot
    {
        WeaponInfo wI;
        public enum SubShotType {shotGun,streng,burst}
        public SubShotType subShotType;

        public int oneShotCount;//バーストかショットガンモードにおける射撃弾数です
        public int rate;//一秒間に発射する回数です
        public int damage;//一発当たりのダメージです
        public int accuracy;//精度です、サブショットのため固定です

        public void Set(WeaponInfo winfo)
        {
            Debug.Log($"初期化完了{winfo}");
            wI = winfo;
        }

        public void OnShot()
        {
            Debug.Log($"射撃{subShotType}\n起動したオブジェクト{wI.weaponName}");
            switch (subShotType)
            {
                case SubShotType.shotGun: ShotGunMode(); break;
                case SubShotType.streng: strengMode(); break;
                case SubShotType.burst: break;
            }
        }

        #region subShots
        void strengMode()
        {
                wI.audio.PlayOneShot(wI.shotSE);
                var shotDirTmp = wI.weaponObject.transform.eulerAngles;
                Quaternion shotDir = Quaternion.Euler(shotDirTmp);
                GameObject bullet = MonoBehaviour.Instantiate(wI.bulletPrefab, wI.muzzlePos, shotDir);
                bullet.GetComponent<Bullet>().Shot(damage*3, wI.speed);
            
        }
        void ShotGunMode()
        {
            wI.audio.PlayOneShot(wI.shotSE);
            for (int i = 0; i < oneShotCount - 1; i++)
            {
                var shotDirTmp = wI.weaponObject.transform.eulerAngles;
                shotDirTmp.x += Random.Range(-accuracy, accuracy);
                shotDirTmp.y += Random.Range(-accuracy, accuracy);
                Quaternion shotDir = Quaternion.Euler(shotDirTmp);
                GameObject bullet = MonoBehaviour.Instantiate(wI.bulletPrefab, wI.muzzlePos, shotDir);
                bullet.GetComponent<Bullet>().Shot(damage, wI.speed);
            }
        }
        void burstMode()
        {
            wI.audio.PlayOneShot(wI.shotSE);
            for (int i = 0; i < oneShotCount - 1; i++)
            {
                var shotDirTmp = wI.weaponObject.transform.eulerAngles;
                shotDirTmp.x += Random.Range(-accuracy, accuracy);
                shotDirTmp.y += Random.Range(-accuracy, accuracy);
                Quaternion shotDir = Quaternion.Euler(shotDirTmp);
                GameObject bullet = MonoBehaviour.Instantiate(wI.bulletPrefab, wI.muzzlePos, shotDir);
                bullet.GetComponent<Bullet>().Shot(damage, wI.speed);
            }
        }
        


        #endregion

    }
    public SubShot subShot=new SubShot();
    public void OnSubShot()
    {
        subShot.OnShot();
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
    bool isFade,isFind;
    Material normalMaterial, fadeMaterial;
    GameObject playerObject,playerCam, enemyObject;
    GameObject target_UI;
    Player playerScript;
    Rigidbody rb;
    SkinnedMeshRenderer smr;

    public void Set(GameObject tmpEnemyObject,EnemyInfo tmpInfo,GameObject targetUI)
    {
        #region ステータスの代入
        name = tmpInfo.name;
        hitPoint = tmpInfo.hitPoint;
        defense = tmpInfo.defense;
        speed = tmpInfo.speed;
        attack = tmpInfo.attack;
        #endregion

        target_UI = targetUI;
        enemyObject = tmpEnemyObject;
        rb = enemyObject.GetComponent<Rigidbody>();
        smr = enemyObject.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();

        string loadtmp = string.Format("EnemyMaterial/{0}/normal",name);
        normalMaterial = Resources.Load<Material>(loadtmp);
        loadtmp = string.Format("EnemyMaterial/{0}/fade", name);
        fadeMaterial = Resources.Load<Material>(loadtmp);

        playerObject = GameObject.FindWithTag("Player").gameObject;
        playerCam = GameObject.Find("PlayerCamera").gameObject;
        playerScript = playerObject.GetComponent<Player>();

        isFind = true;

    }
    public void OnFadeSwitch()
    {
        if (playerScript.onGhostView) { smr.material = normalMaterial; target_UI.SetActive(true); }
        else{ smr.material = fadeMaterial; target_UI.SetActive(false); }
    }
    public void OnMove()
    {
        Debug.Log($"移動:{isFind}");
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
