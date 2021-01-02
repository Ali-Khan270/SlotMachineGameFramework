using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class AnimationModel
{
    public string Name;
    public GameObject Model;
    public Transform FinalPosition;
    public float AnimationTime;
        
}

public class AnimationView : MonoBehaviour
{

    [Header("AnimationModel")]
    public List<AnimationModel> mAnimationItem = new List<AnimationModel>();

    [Header("StartPosition")]
    public List<Transform> mStartPosition;

    
    public void PlayAnimation(string aName,string aType = "")
    {
            foreach (AnimationModel animmodel in mAnimationItem)
            {
                if (aName.Equals(animmodel.Name))
                {
                    if (aType.Equals(""))
                    {
                        StartAnimation(animmodel);
                    }
                    if (aType.Equals("potions"))
                    {
                        StartAnimation(animmodel);
                    }
                    if(aType.Equals("aor"))
                    {
                        StartAnimation(animmodel, "aor");
                    }
                }
            }
    }

    

    void StartAnimation(AnimationModel aModel, string aAnimationType = "")
    {
        foreach(Transform transform in mStartPosition)
        {
            GameObject obj = Instantiate(aModel.Model, transform);

            switch (aAnimationType)
            {
                case "":
                    obj.transform.DOMove(aModel.FinalPosition.position, aModel.AnimationTime).SetEase(Ease.Linear).OnComplete(() => {
                        //executes whenever coin reach target position
                        Destroy(obj);
                    });
                    break;

                case "potions":
                    obj.transform.DOMove(aModel.FinalPosition.position, aModel.AnimationTime).SetEase(Ease.Linear).OnComplete(() => {
                        //executes whenever coin reach target position
                        Destroy(obj);
                    });
                    break;

                case "aor":

                    break;

            }

            
        }
    }
}
