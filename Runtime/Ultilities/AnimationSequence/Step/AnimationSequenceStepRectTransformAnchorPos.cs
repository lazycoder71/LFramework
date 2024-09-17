using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class AnimationSequenceStepRectTransformAnchorPos : AnimationSequenceStepRectTransform
    {
        public override string displayName { get { return $"{(_isSelf ? "RectTransform (This)" : _owner)}: DOAnchorPos"; } }

        protected override Tween GetTween(AnimationSequence animationSequence)
        {
            RectTransform owner = _isSelf ? animationSequence.rectTransform : _owner;

            float duration = _isSpeedBased ? Vector2.Distance(_value, owner.anchoredPosition) / _duration : _duration;
            Vector3 start = _changeStartValue ? _valueStart : owner.anchoredPosition;
            Vector3 end = _relative ? owner.anchoredPosition + (Vector2)_value : _value;

            Tween tween = owner.DOAnchorPos(end, duration, _snapping)
                               .ChangeStartValue(start);

            return tween;
        }

        protected override Tween GetResetTween(AnimationSequence animationSequence)
        {
            RectTransform owner = _isSelf ? animationSequence.rectTransform : _owner;

            return owner.DOAnchorPos(owner.anchoredPosition, 0.0f);
        }
    }
}