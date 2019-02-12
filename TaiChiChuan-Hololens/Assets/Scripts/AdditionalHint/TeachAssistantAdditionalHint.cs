using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TeachAssistantAdditionalHint : AdditionalHint
{
    protected WeightVisualizationStrategy coachWeightVisualizationStrategy;
    protected WeightVisualizationStrategy userWeightVisualizationStrategy;

    public TeachAssistantAdditionalHint(AvatarsController avatarsController)
        : base(avatarsController)
    {
        coachWeightVisualizationStrategy = null;
        userWeightVisualizationStrategy = null;

        foreach (IAvatar avatar in avatarsController.TeachAssistants)
        {
            Renderer[] renderers = avatar.Avatar.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (Renderer renderer in renderers)
                renderer.enabled = true;

            renderers = avatar.Avatar.parent.GetComponentsInChildren<MeshRenderer>();
            foreach (Renderer renderer in renderers)
                renderer.enabled = true;
        }
    }

    public override void Dispose()
    {
        if (coachWeightVisualizationStrategy != null)
            coachWeightVisualizationStrategy.Dispose();

        if (userWeightVisualizationStrategy != null)
            userWeightVisualizationStrategy.Dispose();
    }
}
