using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAdditionalHint : AdditionalHint
{
    GameObject mirror;

    public MirrorAdditionalHint(AvatarsController avatarsController)
        : base(avatarsController)
    {
        List<Transform> transforms = new List<Transform>();
        foreach (IAvatar avatar in avatarsController.TeachAssistants)
        {
            transforms.Add(avatar.Avatar);
        }

        mirror = MirrorController.InstantiateGameObject(transforms);

        foreach (IAvatar avatar in avatarsController.TeachAssistants)
        {
            Renderer[] renderers = avatar.Avatar.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (Renderer renderer in renderers)
                renderer.enabled = false;

            renderers = avatar.Avatar.parent.GetComponentsInChildren<MeshRenderer>();
            foreach (Renderer renderer in renderers)
                renderer.enabled = false;
        }
    }

    public override void Dispose()
    {
        UnityEngine.Object.Destroy(mirror);
    }
}
