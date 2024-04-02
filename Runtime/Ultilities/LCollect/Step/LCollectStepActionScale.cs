using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class LCollectStepActionScale : LCollectStepAction
    {
        [SerializeField] Vector3 _value = Vector3.one;

        protected override Tween GetTween(LCollectItem item)
        {
            return item.transformCached.DOScale(_value, _duration)
                                       .SetEase(_ease);
        }
    }
}
