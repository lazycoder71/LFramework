using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework.AnimationSequence
{
    public class AnimationSequenceStepTransformRotate : AnimationSequenceStepTransform
    {
        [VerticalGroup("Value")]
        [SerializeField] private RotateMode _rotateMode = RotateMode.Fast;

        public override string DisplayName { get { return $"{(_isSelf ? "Transform (This)" : _owner.name)}: DOLocalRotate"; } }

        protected override Tween GetTween(AnimationSequence animationSequence)
        {
            Transform owner = _isSelf ? animationSequence.Transform : _owner;

            float duration = _isSpeedBased ? Vector3.Distance(_value, owner.localEulerAngles) / _duration : _duration;

            Tweener tween = owner.DOLocalRotate(_relative ? owner.localEulerAngles + _value : _value, duration, _rotateMode);

            if (_changeStartValue)
                tween.ChangeStartValue(_relative ? owner.localEulerAngles + _valueStart : _valueStart);

            return tween;
        }

        protected override Tween GetResetTween(AnimationSequence animationSequence)
        {
            Transform owner = _isSelf ? animationSequence.Transform : _owner;

            return owner.DOLocalRotate(owner.localEulerAngles, 0.0f);
        }
    }
}