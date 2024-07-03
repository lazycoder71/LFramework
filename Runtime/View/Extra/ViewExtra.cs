using DG.Tweening;
using System;

namespace LFramework.View
{
    [Serializable]
    public class ViewExtra
    {
        public virtual string displayName { get; }

        public void Apply(View view)
        {
            Tween tween = GetTween(view, 1.0f);

            view.sequence.Join(tween);
        }

        protected virtual Tween GetTween(View popup, float duration)
        {
            return null;
        }
    }
}