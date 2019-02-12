using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachCircleRippleWeightVisualizationStrategy : WeightVisualizationStrategy
{
    public CoachCircleRippleWeightVisualizationStrategy(List<CoachAvatar> coachAvatars)
        : base()
    {
        foreach (CoachAvatar avatar in coachAvatars)
        {
            CoachCircleRippleController.InstantiateGameObject(
                 weightVisualization, avatar.Avatar.parent, avatar.LeftFoot, avatar.RightFoot);
        }
    }
}
