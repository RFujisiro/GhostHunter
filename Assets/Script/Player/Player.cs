using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    #region 変数
    enum HaveItem {mainWeapon,subWeapon,item1,item2};
    HaveItem haveItem;
    public bool onGhostView;

    #region コンポーネント
    PlayerInput input;
    CharacterController controller;
    Animator amine;
    #endregion

    Vector3 moveDirection;


    Vector2 tmpCamKey;
    Vector2 tmpKey;
    float bodyAngle;
    [SerializeField] float cameraSpeed;

    #region bool
    bool isDash =false;
    bool isSquat = false;
    bool isMap = false;
    bool isMenu = false;
    #endregion
    

    int speed = 10;

    public GameObject bodyObject;
    #region UI
    [SerializeField] GameObject map;
    [SerializeField] GameObject menu;


    #endregion
    [SerializeField] GameObject weaponObject_Main, weaponObject_Sub;
    WeaponSC weaponSC_Main, weaponSC_Sub;

    #endregion
    
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        amine = transform.GetChild(0).GetComponent<Animator>();

        weaponSC_Main = weaponObject_Main.GetComponent<WeaponSC>();
        weaponSC_Sub = weaponObject_Sub.GetComponent<WeaponSC>();

        weaponObject_Main.SetActive(haveItem == HaveItem.mainWeapon);
        weaponObject_Sub.SetActive(haveItem == HaveItem.subWeapon);


    }
    // Update is called once per frame
    void Update()
    {
        if (isMenu) return;
        Move();
        CameraMove();
    }
    private void Move()
    {
        if (controller.isGrounded)
        {
            Vector2 inputtmp = input.currentActionMap["Move"].ReadValue<Vector2>();

            if (inputtmp == Vector2.zero) isDash = false;

            moveDirection = new Vector3(inputtmp.x, 0.0f, inputtmp.y);
            moveDirection = transform.TransformDirection(moveDirection);
            float tmpSpeed = speed;
            tmpSpeed *= isDash == true ? 2 : 1;
            tmpSpeed /= isSquat == true ? 3 : 1;

            moveDirection = moveDirection * tmpSpeed;
              

        }
        moveDirection.y = moveDirection.y - (20 * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

    }
    private void CameraMove()
    {
        tmpCamKey = input.currentActionMap["Camera"].ReadValue<Vector2>()* cameraSpeed;
        bodyAngle -= tmpCamKey.y;
        if (bodyAngle > 70) bodyAngle = 70;
        if (bodyAngle < -70) bodyAngle = -70;


        bodyObject.transform.localEulerAngles = new Vector3(bodyAngle, 0, 0);
        transform.eulerAngles += new Vector3(0, tmpCamKey.x, 0);

    }

    #region public関数
    public void UpdateAmmo(int ammo,int spareAmmo,string type)
    {
    }


    #endregion
    #region Input
    public void MainAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log($"射撃");
            if (haveItem == HaveItem.mainWeapon)
                weaponSC_Main.InputKey("Main", true);
            else
                weaponSC_Sub.InputKey("Main", true);
        }
        if (context.canceled)
        {
            if (haveItem == HaveItem.mainWeapon)
                weaponSC_Main.InputKey("Main", false);
            else
                weaponSC_Sub.InputKey("Main", false);

        }
    }
    public void SubAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //  Debug.Log($"サブ射撃:plyaer");
            if (haveItem == HaveItem.mainWeapon)
                weaponSC_Main.InputKey("Sub", true);
            else
                weaponSC_Sub.InputKey("Sub", true);

        }

    }
    public void ReLoad(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (haveItem == HaveItem.mainWeapon)
                weaponSC_Main.InputKey("ReLoad", true);
            else
                weaponSC_Sub.InputKey("ReLoad", true);
        }
    }
    public void WeaponSwitch(InputAction.CallbackContext context)
    {
        if(haveItem==HaveItem.subWeapon)
            haveItem = HaveItem.mainWeapon;
        else
            haveItem = HaveItem.subWeapon;

        weaponObject_Main.SetActive(haveItem == HaveItem.mainWeapon);
        weaponObject_Sub.SetActive(haveItem == HaveItem.subWeapon);
    }
    public void ItemSelect(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (context.started)
            {
                Vector2 key = input.currentActionMap["ItemSelect"].ReadValue<Vector2>();
                switch (key)
                {
                    case Vector2 v when v.Equals(Vector2.up):
                        //アイテム3
                        break;
                    case Vector2 v when v.Equals(Vector2.down):
                        onGhostView = !onGhostView;
                        break;
                    case Vector2 v when v.Equals(Vector2.right):
                        //アイテム１
                        break;
                    case Vector2 v when v.Equals(Vector2.left):
                        //アイテム２
                        break;
                }
            }
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDash = !isDash;
        }
    }
    public void squat(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSquat = !isSquat;
            amine.SetBool("IsSpuat", isSquat);
            Debug.Log("しゃがむ");

        }
    }
    public void Map(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isMap = !isMap;
            map.SetActive(isMap);
        }
    }
    public void Start(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isMenu = !isMenu;
            menu.SetActive(isMenu);
            if (isMenu) Time.timeScale=0;
            else Time.timeScale = 1;
        }
    }
    
    #endregion

}
