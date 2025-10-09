using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMedium : EnemyBase
{
    protected override void Initialize()
    {
        m_maxHealth = 3;
        base.Initialize();
    }
}
