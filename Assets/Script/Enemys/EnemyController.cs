using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int id;
    private EnemyInfo enemyInfo=new EnemyInfo();
    private void Awake()
    {
        Debug.Log($"id={id}:name={gameObject.name}");
        enemyInfo=GameObject.FindWithTag("GameManager").gameObject.GetComponent<GameManager>().GetEnemyInfo(id);
        enemyInfo.Set(gameObject);
    }
    private void FixedUpdate()
    {
        enemyInfo.OnFadeSwitch();
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
