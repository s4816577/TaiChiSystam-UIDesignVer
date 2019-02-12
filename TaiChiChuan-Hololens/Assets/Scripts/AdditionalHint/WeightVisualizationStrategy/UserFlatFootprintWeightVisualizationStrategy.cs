using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserFlatFootprintWeightVisualizationStrategy : WeightVisualizationStrategy
{
    public UserFlatFootprintWeightVisualizationStrategy(List<TeachAssistantAvatar> TeachAssistants)
        : base()
    {
        foreach (TeachAssistantAvatar avatar in TeachAssistants)
        {
			UserFlatFootprintController.InstantiateGameObject(
                 weightVisualization, avatar.Avatar.parent, avatar.LeftFoot, avatar.RightFoot);
        }
    }
}
