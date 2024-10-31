using DG.Tweening;
using UnityEngine;

namespace LFramework.AnimationSequence
{
    public class AnimationSequenceStepTransformScale : AnimationSequenceStepTransform
    {
        public override string DisplayName { get { return $"{(_isSelf ? "Transform (This)" : _owner.name)}: DOScale"; } }

        protected override Tween GetTween(AnimationSequence animationSequence)
        {
            Transform owner = _isSelf ? animationSequence.Transform : _owner;

            float duration = _isSpeedBased ? Vector3.Distance(_value, owner.localScale) / _duration : _duration;

            Tweener tween = owner.DOScale(_relative ? owner.localScale + _value : _value, duration);

            if (_changeStartValue)
                tween.ChangeStartValue(_relative ? owner.localScale + _valueStart : _valueStart);

            return tween;
        }

        protected override Tween GetResetTween(AnimationSequence animationSequence)
        {
            Transform owner = _isSelf ? animationSequence.Transform : _owner;

            return owner.DOScale(owner.localScale, 0.0f);
        }
    }
}