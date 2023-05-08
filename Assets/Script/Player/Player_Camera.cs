using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
    Vector3 camPos,playerPos,dir;
    public float range,range_MAX;
    LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = ~(1  <<LayerMask.NameToLayer("Player"));
        camPos = transform.position;
        playerPos = playerObject.transform.position;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit,hit2;

       dir = playerPos-camPos;
        range =Vector3.Distance(camPos,playerPos);
        #if UNITY_EDITOR
       // Debug.Log($"ÉJÉÅÉâÇÃãﬂÇ√Ç≠å¸Ç´{dir}");
        Debug.DrawLine(camPos, playerPos, Color.red);
        Debug.DrawLine(camPos, camPos + (-dir / 10), Color.blue);
        #endif

        if (Physics.Linecast(camPos, playerPos,out hit, layerMask))
        {
            Debug.Log($"ê⁄ãﬂ");
            camPos += dir / 10;
        }else if (Physics.Linecast(camPos, camPos + (-dir / 10), out hit2, layerMask)|| range > range_MAX)
        {
            Debug.Log($"í‚é~");
        }else
        {
                #if UNITY_EDITOR
            Debug.Log($"ñﬂÇÈ");
#endif
            camPos -= dir/10;
        }

    }
    //void FixedUpdate()
    //{
    //    RaycastHit hit,hit2;

    //    camPos = transform.position;
    //    playerPos = playerObject.transform.position;
    //    dir = playerPos-camPos;
    //    range =Vector3.Distance(camPos,playerPos);
    //    #if UNITY_EDITOR
    //   // Debug.Log($"ÉJÉÅÉâÇÃãﬂÇ√Ç≠å¸Ç´{dir}");
    //    Debug.DrawLine(camPos, playerPos, Color.red);
    //    Debug.DrawLine(camPos, camPos + (-dir / 10), Color.blue);
    //    #endif

    //    if (Physics.Linecast(camPos, playerPos,out hit, layerMask))
    //    {
    //        Debug.Log($"ê⁄ãﬂ");
    //        transform.position += dir / 10;
    //    }else if (Physics.Linecast(camPos, camPos + (-dir / 10), out hit2, layerMask)|| range > range_MAX)
    //    {
    //        Debug.Log($"í‚é~");
    //    }else
    //    {
    //            #if UNITY_EDITOR
    //        Debug.Log($"ñﬂÇÈ");
    //            #endif
    //            transform.position -= dir/10;
    //    }

    //}
}
