﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HitArea : MonoBehaviour
{
    [SerializeField]
    protected List<string> hitableTag;
    [SerializeField]
    protected float width;
    [SerializeField]
    protected float hight;
    [SerializeField]
    protected Vector3 hitAreaOffset;

    protected Vector2 hitBoxSize;

    public UnityEvent afterHitEvent;

    // public List<GameObject> reciveHItsObj;
    public List<ReciveHit> reciveHits = new List<ReciveHit> (); 
    //[SerializeField]
    public bool isActiveHit = true;

    //[SerializeField]
    //protected ParticleSystem beingHitEffect;
    [SerializeField]
    protected ParticleSystem doHitEffect;
    //[SerializeField]
    protected AudioSource bodyAudioSource;

    [SerializeField]
    protected AudioClip doHitAudioClip;
    public bool EffArea;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + hitAreaOffset, new Vector2(width, hight));

    }

    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnStart()
    {
        bodyAudioSource = GetComponent<AudioSource>();
        hitBoxSize = new Vector2(width, hight);
        //if (beingHitEffect != null)
        //{
        //    beingHitEffect = Instantiate(beingHitEffect);

        //}

        if (doHitEffect != null)
        {
            doHitEffect = Instantiate(doHitEffect);
            doHitEffect.transform.SetParent(transform.parent);
        }

        //reciveHIts = new List<ReciveHIt>();
        //foreach(var robj in reciveHItsObj)
        //{
        //    reciveHIts.Add(robj.GetComponent<ReciveHIt>());
        //}
    }

    public virtual void OnUpdate()
    {


        if (!isActiveHit) return;

        ActiveHitOnce();
    }

    public virtual void DoHitAction(GameObject hitObj)
    {
      

        if (doHitEffect != null)
        {
            doHitEffect.transform.position = transform.position;
            doHitEffect.transform.rotation = transform.rotation;
            doHitEffect.Play();

        }
        if (bodyAudioSource && doHitAudioClip)
        {
            bodyAudioSource.clip = doHitAudioClip;
            bodyAudioSource.Play();
        }

        ShowHitLog(hitObj);
    }

    public void ShowHitLog(GameObject hitObj)
    {
        Debug.Log(gameObject.name + " Hit " + hitObj.name);
    }

    public virtual void Hit(GameObject hitObj)
    {
        var x = hitObj.GetComponent<BaseBody>();

        if (x != null )
        {
            x.ReciveHitAction(this.gameObject);
        }
        DoHitAction(hitObj);
        afterHitEvent.Invoke();

    }
    public void ActiveHitOnce()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position + hitAreaOffset, hitBoxSize, 0);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != null && hitableTag.Contains(hitCollider.tag) && gameObject != hitCollider.gameObject)
            {
                Hit(hitCollider.gameObject);

                //isActiveHit = false;

            }

        }

    }


    //public virtual void ReciveHitAction(GameObject hitObj)
    //{
    //    var dmgObj = hitObj.GetComponent<DamageObject>();

    //    if (beingHitEffect != null)
    //    {
    //        if (!dmgObj || !dmgObj.EffArea)
    //        {
    //            beingHitEffect.transform.position = transform.position;
    //            beingHitEffect.transform.rotation = transform.rotation;
    //            beingHitEffect.Play();
    //            Debug.Log("Show " + beingHitEffect.name);
    //            if (bodyAudioSource)
    //            {
    //                bodyAudioSource.clip = beingHitAudioClip;
    //                bodyAudioSource.Play();
    //            }
    //        }

    //    }
    //    foreach (var rhit in reciveHIts)
    //    {
    //        rhit.ReciveHitObject(hitObj);
    //    }

    //    //ShowHitLog(hitObj);
    //}

    //public void AddReciveHitObserver(ReciveHit rh)
    //{
    //    reciveHits.Add(rh);
    //}
}
