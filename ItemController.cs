using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject myUILabel;
    void Start()
    {
        GameObject globalUILabel = GameObject.Find("UI press E");
        myUILabel = Instantiate(globalUILabel);
        FollowItem labelObject = (FollowItem)myUILabel.GetComponent(typeof(FollowItem));
        labelObject.target = this.gameObject;
        myUILabel.transform.SetParent(transform, true);
        

        //if (this.gameObject.tag == "Door")
        //{
        //    Vector3 pos = myUILabel.transform.position;
        //    pos.z += 5f;
        //    myUILabel.transform.position = pos;
        //} else
        //{
        //    myUILabel.transform.position = transform.position;
        //}

        //Vector3 scaleVec = new Vector3(1f, 1f, 1f);
        //myUILabel.transform.localScale = scaleVec;
        //myUILabel.transform.localScale = 1 / transform.localScale;
    }

    //void Update()
    //{
    //    if (this.gameObject.active)
    //    {
    //        myUILabel.SetActive(true);
    //        //myUILabel.enabled = true;
    //    }
    //    else
    //    {
    //        myUILabel.SetActive(false);
    //        //myUILabel.enabled = false;
    //    }
    //}
}
