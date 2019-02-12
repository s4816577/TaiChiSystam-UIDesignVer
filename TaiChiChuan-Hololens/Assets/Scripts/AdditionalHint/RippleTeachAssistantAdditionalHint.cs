using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleTeachAssistantAdditionalHint : TeachAssistantAdditionalHint
{
    public RippleTeachAssistantAdditionalHint(AvatarsController avatarsController)
        : base(avatarsController)
    {
        coachWeightVisualizationStrategy = new CoachCircleRippleWeightVisualizationStrategy(avatarsController.Coaches);

        UdpNetworkServer.GetInstance();
        userWeightVisualizationStrategy = new UserCircleRippleWeightVisualizationStrategy(avatarsController.Coaches, avatarsController.transform);
        //userWeightVisualizationStrategy = new UserUnderFootCircleRippleWeightVisualizationStrategy(avatarsController.ReferenceCoach);
    }
}
