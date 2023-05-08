using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject TargetUI;
    int id;
    private EnemyInfo enemyInfo=new EnemyInfo();
    [SerializeField] bool isMove;
    private void Awake()
    {
        EnemyInfo tmpinfo= GameObject.FindWithTag("GameManager").gameObject.GetComponent<GameManager>().GetEnemyInfo(id);
        enemyInfo.Set(gameObject,tmpinfo, TargetUI);
    }
    private void FixedUpdate()
    {
        enemyInfo.OnFadeSwitch();
        if(isMove)
        enemyInfo.OnMove();
    }

    public void OnDamage(float damage)
    {
        Debug.Log($"ダメージを受けました\nダメージを受けた個体:{enemyInfo.name}\nID{enemyInfo.enemyID}");
        enemyInfo.hitPoint -= (int)damage;
        if(enemyInfo.hitPoint <= 0)Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}
