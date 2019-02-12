using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUnderFootCircleRippleWeightVisualizationStrategy : WeightVisualizationStrategy
{
    public UserUnderFootCircleRippleWeightVisualizationStrategy(CoachAvatar avatar)
        : base()
    {
        UserUnderFootCircleRippleController.InstantiateGameObject(
            weightVisualization, avatar, avatar.LeftFoot, avatar.RightFoot);
    }
}
