using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachFlatFootprintWeightVisualizationStrategy : WeightVisualizationStrategy
{
    public CoachFlatFootprintWeightVisualizationStrategy(List<CoachAvatar> coachAvatars)
        : base()
    {
        foreach (CoachAvatar avatar in coachAvatars)
        {
            CoachFlatFootprintController.InstantiateGameObject(
                 weightVisualization, avatar.Avatar.parent, avatar.LeftFoot, avatar.RightFoot);
        }
    }
}
