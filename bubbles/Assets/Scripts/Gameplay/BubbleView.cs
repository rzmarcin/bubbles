using Bubbles.Config;
using Bubbles.Core;
using Bubbles.Core.Abstract;
using Bubbles.UI;
using UnityEngine;
using Zenject;

namespace Bubbles.Gameplay {
    public class BubbleView : MonoBehaviour, IBubbleView {
        [SerializeField] private float suffixOffset = .1f;
        [Header("Cached components")]
        [SerializeField] private SpriteFont font;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer text;
        [SerializeField] private SpriteRenderer suffix;
        
        private BubbleConfig config;
        private BubbleConfigItem current;
        
        public BubbleConfigItem Current => current;

        [Inject]
        private void Construct(BubbleConfig config) {
            this.config = config; //get config from global context!
        }

        public void Refresh(IBubbleScore score) {
            current = config.Get(score.Value);
            background.color = current.Background;
            var valueStr = Mathf.Pow(2, score.Value % 10).ToString("0");
            text.sprite = font[valueStr];
            suffix.sprite = GetSuffix(score);
            Vector3 offset = Vector3.zero;
            if (score.Value >= 10) {
                offset = Vector3.right * (score.ValueString.Length - 1) * suffixOffset / 2f;
            }

            text.transform.localPosition = -offset;
            suffix.transform.localPosition = offset;
        }

        private Sprite GetSuffix(IBubbleScore score) {
            var check = score.Value / 10;
            string suffix;
            if (check < 1) {
                suffix = null;
            } else if (check < 2) {
                suffix = "k";
            } else if (check < 3) {
                suffix = "M";
            } else if (check < 4) {
                suffix = "G";
            } else if (check < 5) {
                suffix = "T";
            } else if (check < 6) {
                suffix = "P";
            } else {
                suffix = "*";
            }

            return font[suffix];
        }
    }
}