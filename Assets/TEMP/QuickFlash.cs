﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFlash : MonoBehaviour
{

    //TEMPORARY COMPONENT FOR VISUALIZING DAMAGE
    HealthSystem hs;
    SpriteRenderer sp_rend;
    Color df_color;

    private void Awake()
    {
        hs = GetComponent<HealthSystem>();
        sp_rend = GetComponent<SpriteRenderer>();

        hs.onDamageTaken += Hs_onDamageTaken;
        df_color = sp_rend.color;
    }

    private void Hs_onDamageTaken()
    {
        sp_rend.color = Color.white;
        Invoke("ChangeBack", 0.1f);
    }

    public void ChangeBack()
    {
        sp_rend.color = df_color;
    }

    private void OnDisable()
    {
        hs.onDamageTaken -= Hs_onDamageTaken;
    }
}