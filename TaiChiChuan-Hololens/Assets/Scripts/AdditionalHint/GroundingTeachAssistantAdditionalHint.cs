using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingTeachAssistantAdditionalHint : TeachAssistantAdditionalHint
{
    public GroundingTeachAssistantAdditionalHint(AvatarsController avatarsController)
        : base(avatarsController)
    {
        coachWeightVisualizationStrategy = new CoachFlatFootprintWeightVisualizationStrategy(avatarsController.Coaches);

        //UdpNetworkServer.GetInstance();
		//no network option supported in base version
        userWeightVisualizationStrategy = new UserFlatFootprintWeightVisualizationStrategy(avatarsController.TeachAssistants);
    }
}
