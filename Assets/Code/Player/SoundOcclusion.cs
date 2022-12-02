using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Code.Player
{
    public class SoundOcclusion : MonoBehaviour
    {
        [Header("FMOD Event")]
        [SerializeField]
        public EventReference eventReference;
        private EventInstance _audio;
        private EventDescription _audioDes;
        private StudioListener _listener;
        private PLAYBACK_STATE _pb;

        [Header("Occlusion Options")]
        [SerializeField]
        [Range(0f, 10f)]
        private float soundOcclusionWidening = 1f;
        [SerializeField]
        [Range(0f, 10f)]
        private float playerOcclusionWidening = 1f;
        [SerializeField]
        private LayerMask occlusionLayer;

        private bool _audioIsVirtual;
        private float _maxDistance;
        private float _listenerDistance;
        private float _lineCastHitCount;
        private Color _colour;

        private void Start()
        {
            _audioDes = RuntimeManager.GetEventDescription(eventReference);
            _audioDes.getMinMaxDistance(out float min, out _maxDistance);

            _listener = FindObjectOfType<StudioListener>();
        }
    
        private void FixedUpdate()
        {
            _audio.isVirtual(out _audioIsVirtual);
            _audio.getPlaybackState(out _pb);
            _listenerDistance = Vector3.Distance(transform.position, _listener.transform.position);

            if (!_audioIsVirtual && _pb == PLAYBACK_STATE.PLAYING && _listenerDistance <= _maxDistance)
                OccludeBetween(transform.position, _listener.transform.position);

            _lineCastHitCount = 0f;
        }

        private void OccludeBetween(Vector3 sound, Vector3 listener)
        {
            Vector3 soundLeft = CalculatePoint(sound, listener, soundOcclusionWidening, true);
            Vector3 soundRight = CalculatePoint(sound, listener, soundOcclusionWidening, false);

            Vector3 soundAbove = new Vector3(sound.x, sound.y + soundOcclusionWidening, sound.z);
            Vector3 soundBelow = new Vector3(sound.x, sound.y - soundOcclusionWidening, sound.z);

            Vector3 listenerLeft = CalculatePoint(listener, sound, playerOcclusionWidening, true);
            Vector3 listenerRight = CalculatePoint(listener, sound, playerOcclusionWidening, false);

            Vector3 listenerAbove = new Vector3(listener.x, listener.y + playerOcclusionWidening * 0.5f, listener.z);
            Vector3 listenerBelow = new Vector3(listener.x, listener.y - playerOcclusionWidening * 0.5f, listener.z);

            CastLine(soundLeft, listenerLeft);
            CastLine(soundLeft, listener);
            CastLine(soundLeft, listenerRight);

            CastLine(sound, listenerLeft);
            CastLine(sound, listener);
            CastLine(sound, listenerRight);

            CastLine(soundRight, listenerLeft);
            CastLine(soundRight, listener);
            CastLine(soundRight, listenerRight);
        
            CastLine(soundAbove, listenerAbove);
            CastLine(soundBelow, listenerBelow);

            if (playerOcclusionWidening == 0f || soundOcclusionWidening == 0f)
            {
                _colour = Color.blue;
            }
            else
            {
                _colour = Color.green;
            }

            SetParameter();
        }

        private Vector3 CalculatePoint(Vector3 a, Vector3 b, float m, bool posOrNeg)
        {
            float x;
            float z;
            float n = Vector3.Distance(new Vector3(a.x, 0f, a.z), new Vector3(b.x, 0f, b.z));
            float mn = (m / n);
            if (posOrNeg)
            {
                x = a.x + (mn * (a.z - b.z));
                z = a.z - (mn * (a.x - b.x));
            }
            else
            {
                x = a.x - (mn * (a.z - b.z));
                z = a.z + (mn * (a.x - b.x));
            }
            return new Vector3(x, a.y, z);
        }

        private void CastLine(Vector3 start, Vector3 end)
        {
            Physics.Linecast(start, end, out var hit, occlusionLayer);

            if (hit.collider)
            {
                _lineCastHitCount++;
                Debug.DrawLine(start, end, Color.red);
            }
            else
                Debug.DrawLine(start, end, _colour);
        }

        private void SetParameter()
        {
            _audio.setParameterByName("Occlusion", _lineCastHitCount / 11);
        }
        
        private void OnDestroy()
        {
            _audio.release();
        }
    }
}