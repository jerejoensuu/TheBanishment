using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI collectableText;

        public void SetCollectableText(int collectablesInPossession, int collectablesNeeded)
        {
            collectableText.text = collectablesInPossession + "/" + collectablesNeeded;
        }

    }
}
