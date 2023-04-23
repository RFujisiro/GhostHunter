using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject TargetUI;
    public int id;
    private EnemyInfo enemyInfo=new EnemyInfo();
    private void Awake()
    {
        Debug.Log($"id={id}:name={gameObject.name}");
        EnemyInfo tmpinfo= GameObject.FindWithTag("GameManager").gameObject.GetComponent<GameManager>().GetEnemyInfo(id);
        enemyInfo.Set(gameObject,tmpinfo, TargetUI);
    }
    private void FixedUpdate()
    {
        enemyInfo.OnFadeSwitch();
        enemyInfo.OnMove();
    }

    public void OnDamage(float damage)
    {
        Debug.Log($"�_���[�W���󂯂܂���\n�_���[�W���󂯂���:{enemyInfo.name}\nID{enemyInfo.enemyID}");
        enemyInfo.hitPoint -= (int)damage;
        if(enemyInfo.hitPoint <= 0)Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}
