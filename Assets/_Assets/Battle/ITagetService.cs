using System;
using System.Collections.Generic;

public interface ITargetService
{
    public List<BattleCharacter> GetTargetsForsTeam(int teamId, bool hostileTargets);
    public TargetingComponent GetTargetingComponent();
}
