using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCircleRippleWeightVisualizationStrategy : WeightVisualizationStrategy
{
    public UserCircleRippleWeightVisualizationStrategy(List<CoachAvatar> coachAvatars, Transform rotationCenter)
        : base()
    {
        foreach (CoachAvatar avatar in coachAvatars)
        {
            UserCircleRippleController.InstantiateGameObject(
                 weightVisualization, avatar.Avatar.parent, avatar.LeftFoot, avatar.RightFoot, rotationCenter, 360.0f / coachAvatars.Count / 2);
        }
    }
}
