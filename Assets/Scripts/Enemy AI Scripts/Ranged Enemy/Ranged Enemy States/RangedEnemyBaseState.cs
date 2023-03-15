using UnityEngine;

public abstract class RangedEnemyBaseState
{
    public abstract void EnterState(RangedEnemyController rangedEnemy);

    public abstract void UpdateState(RangedEnemyController rangedEnemy);
}
