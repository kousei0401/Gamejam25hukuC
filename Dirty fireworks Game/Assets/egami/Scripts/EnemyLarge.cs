using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLarge : EnemyBase
{
    protected override void Initialize()
    {
        m_maxHealth = 5;
        base.Initialize();
    }
}
