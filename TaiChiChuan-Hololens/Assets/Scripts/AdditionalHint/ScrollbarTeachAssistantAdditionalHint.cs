using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollbarTeachAssistantAdditionalHint : TeachAssistantAdditionalHint
{
    public ScrollbarTeachAssistantAdditionalHint(AvatarsController avatarsController)
        : base(avatarsController)
    {
        coachWeightVisualizationStrategy = new CoachBalanceBarWeightVisualizationStrategy();

        UdpNetworkServer.GetInstance();
        userWeightVisualizationStrategy = new UserBalanceBarWeightVisualizationStrategy();
    }
}
