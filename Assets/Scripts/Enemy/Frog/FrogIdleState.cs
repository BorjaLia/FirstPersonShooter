using UnityEngine;

public class FrogIdleState : IState
{
    private FrogEnemy frog;

    public FrogIdleState(FrogEnemy frog)
    {
        this.frog = frog;
    }

    public void Enter()
    {
        frog.nav.isStopped = true;
        frog.animator.SetTrigger("Idle");
    }

    public void UpdateLogic()
    {
        float distanceToPlayer = Vector3.Distance(frog.transform.position, frog.playerTarget.position);

        if (distanceToPlayer <= frog.jumpDistance)
        {
            frog.ChangeState(frog.JumpState);
        }
    }

    public void Exit()
    {
    }
}