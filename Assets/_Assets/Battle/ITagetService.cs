using System;
using System.Collections.Generic;

public interface ITargetService
{
    public List<BattleCharacter> GetTargetsForTeam(int teamId, bool hostileTargets);
    public TargetingComponent GetTargetingComponent();
}
