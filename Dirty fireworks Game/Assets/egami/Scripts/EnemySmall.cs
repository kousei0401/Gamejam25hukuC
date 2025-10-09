using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmall : EnemyBase
{

    protected override void Initialize()
    {
        m_maxHealth = 1;
        base.Initialize();
    }
}
