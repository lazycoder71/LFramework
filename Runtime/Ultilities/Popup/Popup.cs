using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace LFramework
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class Popup : MonoCached
    {
        [Title("Config")]
        [SerializeField] float _openDuration = 0.2f;
        [SerializeField] float _closeDuration = 0.2f;
        [SerializeField] bool _isLocked = false;
        [SerializeField] bool _deactiveOnClosed = false;

        [Space]

        [FoldoutGroup("Animation")]
        [ListDrawerSettings(ListElementLabelName = "displayName", AddCopiesLastElement = true, ElementColor = "orange")]
        [SerializeReference] PopupAnimation[] _animations;

        [Space]

        [FoldoutGroup("Event")]
        [SerializeField] UnityEvent _onOpenStart;
        [FoldoutGroup("Event")]
        [SerializeField] UnityEvent _onOpenEnd;
        [FoldoutGroup("Event")]
        [SerializeField] UnityEvent _onCloseStart;
        [FoldoutGroup("Event")]
        [SerializeField] UnityEvent _onCloseEnd;

        bool _isOpening = false;

        Sequence _sequence;

        CanvasGroup _canvasGroup;

        public float openDuration { get { return _openDuration; } }
        public float closeDuration { get { return _closeDuration; } }
        public bool isLocked { get { return _isLocked; } set { _isLocked = value; } }
        public CanvasGroup canvasGroup { get { return _canvasGroup; } }

        public UnityEvent onOpenStart { get { return _onOpenStart; } }
        public UnityEvent onOpenEnd { get { return _onOpenEnd; } }
        public UnityEvent onCloseStart { get { return _onCloseStart; } }
        public UnityEvent onCloseEnd { get { return _onCloseEnd; } }

        #region MonoBehaviour

        void Awake()
        {
            // Get canvas group component and disable UI constrol at begin
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void OnDestroy()
        {
            // Kill tweens
            _sequence?.Kill();
        }

        private void Update()
        {
            if (!_isLocked)
                InputCheck();
        }

        private void OnEnable()
        {
            _isOpening = true;

            PopupManager.PushToStack(this);

            ConstructSequence();

            _sequence.Restart();
            _sequence.Play();
        }

        private void OnDisable()
        {
            // Pop this popup out of stack
            PopupManager.PopFromStack(this);

            // Trigger closed event
            _onCloseEnd?.Invoke();
        }

        #endregion

        #region Function -> Public

        public void Close()
        {
            if (_isLocked)
                return;

            ProcessClose();
        }

        public void CloseForced()
        {
            ProcessClose();
        }

        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;

            _canvasGroup.interactable = enabled;
        }

        #endregion

        #region Function -> Private

        private void ConstructSequence()
        {
            if (_sequence != null)
                return;

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            if (!_animations.IsNullOrEmpty())
            {
                for (int i = 0; i < _animations.Length; i++)
                    _animations[i].Apply(this, _sequence);
            }
            else
            {
                _sequence.AppendCallback(null);
            }

            _sequence.SetAutoKill(false);
            _sequence.OnStepComplete(Sequence_OnStepComplete);
        }

        private void Sequence_OnStepComplete()
        {
            if (_isOpening)
            {
                SetEnabled(true);

                _onOpenEnd?.Invoke();
            }
            else
            {
                _onCloseEnd?.Invoke();

                if (_deactiveOnClosed)
                    gameObjectCached.SetActive(false);
                else
                    Destroy(gameObjectCached);
            }
        }

        private void ProcessClose()
        {
            // Can't close when it is transiting
            if (_sequence.IsPlaying())
                return;

            // Set is opening flag
            _isOpening = false;

            // Disable popup at this moment
            SetEnabled(false);

            // On close callback
            _onCloseStart?.Invoke();

            if (_openDuration > 0.0f && _closeDuration > 0.0f)
            {
                // Set sequence time scale to match close duration
                _sequence.timeScale = _openDuration / _closeDuration;
            }

            // Play sequence backward when close
            _sequence.PlayBackwards();
        }

        private void InputCheck()
        {
#if UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
#endif
        }

        #endregion
    }
}