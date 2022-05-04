using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeImageChanger : MonoBehaviour
{
	[SerializeField] SlimeController slimeController;

	AnimatorOverrideController overrideController;

	[SerializeField] Animator anim;

	bool animatorCore;
	bool changeAnimation;

	//アニメーションクリップ
	[SerializeField] AnimationClip coreSlime_Idle;
	[SerializeField] AnimationClip coreSlime_Haziku_prepare;
	[SerializeField] AnimationClip coreSlime_Haziku;
	[SerializeField] AnimationClip coreSlime_Fall;
	[SerializeField] AnimationClip coreSlime_Extend;
	[SerializeField] AnimationClip coreSlime_ExtendIdle;
	[SerializeField] AnimationClip coreSlime_Tearoff;
	[SerializeField] AnimationClip coreSlime_Move;
	[SerializeField] AnimationClip coreSlime_Landing;

	[SerializeField] AnimationClip noCoreSlime_Idle;
	[SerializeField] AnimationClip noCoreSlime_Haziku_prepare;
	[SerializeField] AnimationClip noCoreSlime_Haziku;
	[SerializeField] AnimationClip noCoreSlime_Fall;
	[SerializeField] AnimationClip noCoreSlime_Extend;
	[SerializeField] AnimationClip noCoreSlime_ExtendIdle;
	[SerializeField] AnimationClip noCoreSlime_Tearoff;
	[SerializeField] AnimationClip noCoreSlime_Move;
	[SerializeField] AnimationClip noCoreSlime_Landing;
	void Start()
	{
		animatorCore = true;
		changeAnimation = false;
	}

    private void Update()
    {
        if(slimeController.core)
        {
			if(!animatorCore)
            {
				animatorCore = true;
				ChangeAnimationToCore();
			}
        }
        else
        {
			if (animatorCore)
			{
				animatorCore = false;
				ChangeAnimationToNoCore();
			}
		}
    }

	void ChangeAnimationToNoCore()
	{
		if(!changeAnimation)
        {
			changeAnimation = true;
			overrideController = new AnimatorOverrideController();
			overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
			anim.runtimeAnimatorController = overrideController;
		}

		// ステートをキャッシュ
		AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[anim.layerCount];
		for (int i = 0; i < anim.layerCount; i++)
		{
			layerInfo[i] = anim.GetCurrentAnimatorStateInfo(i);
		}

		//アニメーションクリップを差し替える
		overrideController["Slime_Idle"] = noCoreSlime_Idle;
		overrideController["Slime_Haziku_prepare"] = noCoreSlime_Haziku_prepare;
		overrideController["Slime_Haziku"] = noCoreSlime_Haziku;
		overrideController["Slime_Fall"] = noCoreSlime_Fall;
		overrideController["Slime_Extend"] = noCoreSlime_Extend;
		overrideController["Slime_ExtendIdle"] = noCoreSlime_ExtendIdle;
		overrideController["Slime_Tearoff"] = noCoreSlime_Tearoff;
		overrideController["Slime_Move"] = noCoreSlime_Move;
		overrideController["Slime_Landing"] = noCoreSlime_Landing;

		anim.Update(0.0f);

		// ステートを戻す
		for (int i = 0; i < anim.layerCount; i++)
		{
			anim.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
		}
	}

	void ChangeAnimationToCore()
	{
		if (!changeAnimation)
		{
			changeAnimation = true;
			overrideController = new AnimatorOverrideController();
			overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
			anim.runtimeAnimatorController = overrideController;
		}

		// ステートをキャッシュ
		AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[anim.layerCount];
		for (int i = 0; i < anim.layerCount; i++)
		{
			layerInfo[i] = anim.GetCurrentAnimatorStateInfo(i);
		}

		//アニメーションクリップを差し替える
		overrideController["Slime_Idle"] = coreSlime_Idle;
		overrideController["Slime_Haziku_prepare"] = coreSlime_Haziku_prepare;
		overrideController["Slime_Haziku"] = coreSlime_Haziku;
		overrideController["Slime_Fall"] = coreSlime_Fall;
		overrideController["Slime_Extend"] = coreSlime_Extend;
		overrideController["Slime_ExtendIdle"] = coreSlime_ExtendIdle;
		overrideController["Slime_Tearoff"] = coreSlime_Tearoff;
		overrideController["Slime_Move"] = coreSlime_Move;
		overrideController["Slime_Landing"] = coreSlime_Landing;

		anim.Update(0.0f);

		// ステートを戻す
		for (int i = 0; i < anim.layerCount; i++)
		{
			anim.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
		}
	}
}
