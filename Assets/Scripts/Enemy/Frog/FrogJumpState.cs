using UnityEngine;

public class FrogJumpState : IState
{
    private FrogEnemy frog;

    public FrogJumpState(FrogEnemy frog)
    {
        this.frog = frog;
    }

    public void Enter()
    {
    }

    public void UpdateLogic()
    {
    }

    public void Exit()
    {
    }
}