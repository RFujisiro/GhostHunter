using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    enum HaveItem {mainWeapon,subWeapon,item1,item2};
    HaveItem haveItem;
    public bool onGhostView;

    PlayerInput input;
    CharacterController controller;
    Vector3 moveDirection;
    public Vector2 tmpCamKey;
    public Vector2 tmpKey;
    float bodyAngle;

    public GameObject bodyObject;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraMove();
    }

    private void Move()
    {
        bool isGround = Physics.Linecast(transform.position, (Vector3.down / 10 + transform.position));
        Debug.DrawLine(transform.position, (Vector3.down / 10 + transform.position), Color.yellow);
        if (controller.isGrounded)
        {
            Vector2 inputtmp = input.currentActionMap["Move"].ReadValue<Vector2>();
            tmpKey = inputtmp;
            moveDirection = new Vector3(inputtmp.x, 0.0f, inputtmp.y)*2;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * 5;

        }
        moveDirection.y = moveDirection.y - (20 * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

    }
    private void CameraMove()
    {
        tmpCamKey = input.currentActionMap["Camera"].ReadValue<Vector2>();
        bodyAngle -= tmpCamKey.y;
        if (bodyAngle > 70) bodyAngle = 70;
        if (bodyAngle < -70) bodyAngle = -70;


        bodyObject.transform.localEulerAngles = new Vector3(bodyAngle, 0, 0);
        transform.eulerAngles += new Vector3(0, tmpCamKey.x, 0);

    }

    #region Input
    public WeaponSC[] weaponSC;
    public void MainAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            weaponSC[0].InputKey("Main", true);
            weaponSC[1].InputKey("Main", true);
        }
        if (context.canceled)
        {
            weaponSC[0].InputKey("Main", false);
            weaponSC[1].InputKey("Main", false);
        }
    }
    public void SubAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            weaponSC[0].InputKey("Sub", true);
            weaponSC[1].InputKey("Sub", true);
        }

    }
    public void WeaponSwitch(InputAction.CallbackContext context)
    {
        if(haveItem==HaveItem.subWeapon)
            haveItem = HaveItem.mainWeapon;
        else
            haveItem = HaveItem.subWeapon;

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

    #endregion

}
