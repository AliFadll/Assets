using UnityEngine;
using TMPro;
using System.Collections;

namespace itsmakingthings_daynightcycle
{
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Skyboxes")]
        public Material daySkybox;
        public Material middaySkybox;
        public Material sunsetSkybox;
        public Material nightSkybox;

        [Header("Time Settings")]
        public float startTimeOfDay = 12f;
        public float cycleSpeed = 0.1f;
        public float TimeOfDay => _timeOfDay;
        private float _timeOfDay;
        private bool _isTimeRunning = true;

        [Header("Reset Settings")]
        public float resetSpeed = 1f;
        public bool forwardsOnly = false;
        private Coroutine _timeResetCoroutine;

        [Header("Directional Light Settings")]
        public Light sunLight;
        public Transform rotationPivot;
        [Range(0, 259)] public float rotationOffsetY = 0f;

        [Header("UI Settings")]
        public TextMeshProUGUI timeText;

        [Header("Fog Control")]
        public bool enableFogControl = true;

        [Header("Water Settings")]
        public Renderer waterRenderer;
        public string waterColorProperty = "_WaterColor";
        private MaterialPropertyBlock _waterPropertyBlock;

        [System.Serializable]
        public class TimeSettings
        {
            public Color ambientColor;
            public Color sunColor;
            public Color backgroundColor;
            public float sunIntensity;
            [Range(0f,1f)] public float shadowStrength;
            public Color fogColor;
            public float fogDensity;
            public Color waterColor;
        }

        [Header("Daybreak Settings")]
        public TimeSettings daybreak;
        [Header("Midday Settings")]
        public TimeSettings midday;
        [Header("Sunset Settings")]
        public TimeSettings sunset;
        [Header("Night Settings")]
        public TimeSettings night;

        private static readonly System.Text.StringBuilder _timeStringBuilder = new System.Text.StringBuilder();
        private float _lastTimeUpdated = -1f;

        void Start()
        {
            _timeOfDay = startTimeOfDay;
            UpdateTimeUI();
            UpdateLighting(Camera.main);
        }

        void Update()
        {
            if (_isTimeRunning)
            {
                UpdateTime();
            }
        }

        void LateUpdate()
        {
            // Always update sceneCamera to the active camera
            if (Camera.main != null)
            {
                UpdateLighting(Camera.main);
            }
        }

        private void UpdateTime()
        {
            _timeOfDay += cycleSpeed * Time.deltaTime;
            if (_timeOfDay >= 24f) _timeOfDay = 0f;

            int currentMinute = Mathf.FloorToInt((_timeOfDay - Mathf.FloorToInt(_timeOfDay)) * 60);
            if (currentMinute != _lastTimeUpdated)
            {
                _lastTimeUpdated = currentMinute;
                UpdateTimeUI();
            }
        }

        private void UpdateTimeUI()
        {
            if (timeText == null) return;

            int hours = Mathf.FloorToInt(_timeOfDay);
            int minutes = Mathf.FloorToInt((_timeOfDay - hours) * 60);

            _timeStringBuilder.Clear();
            _timeStringBuilder.Append(hours.ToString("00"));
            _timeStringBuilder.Append(":");
            _timeStringBuilder.Append(minutes.ToString("00"));

            timeText.text = _timeStringBuilder.ToString();
        }

        private void UpdateLighting(Camera sceneCamera)
        {
            if (sunLight == null || rotationPivot == null || sceneCamera == null) return;

            // Rotate sun
            float timePercent = _timeOfDay / 24f;
            float xRotation = (timePercent * 360f) - 90f;
            rotationPivot.localRotation = Quaternion.Euler(new Vector3(xRotation, rotationOffsetY, 0));

            // Determine time period
            TimeSettings from, to;
            float blend;

            if (_timeOfDay < 6f)       { from = night; to = daybreak; blend = _timeOfDay / 6f; }
            else if (_timeOfDay < 12f) { from = daybreak; to = midday; blend = (_timeOfDay - 6f) / 6f; }
            else if (_timeOfDay < 18f) { from = midday; to = sunset; blend = (_timeOfDay - 12f) / 6f; }
            else                       { from = sunset; to = night; blend = (_timeOfDay - 18f) / 6f; }

            // Apply lighting and fog
            RenderSettings.ambientLight = Color.Lerp(from.ambientColor, to.ambientColor, blend);
            sunLight.color = Color.Lerp(from.sunColor, to.sunColor, blend);
            sunLight.intensity = Mathf.Lerp(from.sunIntensity, to.sunIntensity, blend);
            sunLight.shadowStrength = Mathf.Lerp(from.shadowStrength, to.shadowStrength, blend);
            sceneCamera.backgroundColor = Color.Lerp(from.backgroundColor, to.backgroundColor, blend);

            if (enableFogControl && RenderSettings.fog)
            {
                RenderSettings.fogColor = Color.Lerp(from.fogColor, to.fogColor, blend);
                RenderSettings.fogDensity = Mathf.Lerp(from.fogDensity, to.fogDensity, blend);
            }

            // Update water color
            if (waterRenderer != null && waterRenderer.sharedMaterial.HasProperty(waterColorProperty))
            {
                if (_waterPropertyBlock == null)
                    _waterPropertyBlock = new MaterialPropertyBlock();

                waterRenderer.GetPropertyBlock(_waterPropertyBlock);
                Color waterColor = Color.Lerp(from.waterColor, to.waterColor, blend);
                _waterPropertyBlock.SetColor(waterColorProperty, waterColor);
                waterRenderer.SetPropertyBlock(_waterPropertyBlock);
            }

            // Update skybox
            if (_timeOfDay < 6f)        RenderSettings.skybox = nightSkybox;
            else if (_timeOfDay < 12f)  RenderSettings.skybox = daySkybox;
            else if (_timeOfDay < 18f)  RenderSettings.skybox = middaySkybox;
            else                         RenderSettings.skybox = sunsetSkybox;
        }

        public void StopTime() => _isTimeRunning = false;
        public void StartTime() => _isTimeRunning = true;

        public void ResetTimeSmoothly(float targetTime)
        {
            if (_timeResetCoroutine != null) StopCoroutine(_timeResetCoroutine);
            _timeResetCoroutine = StartCoroutine(SmoothTimeReset(targetTime));
        }

        private IEnumerator SmoothTimeReset(float targetTime)
        {
            float originalTime = _timeOfDay;
            float elapsedTime = 0f;

            if (forwardsOnly && originalTime > targetTime) targetTime += 24f;
            bool crossesMidnight = (originalTime > targetTime) && (Mathf.Abs(originalTime - targetTime) > 12f);
            float adjustedTargetTime = crossesMidnight ? targetTime + 24f : targetTime;

            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * resetSpeed;
                _timeOfDay = Mathf.Lerp(originalTime, adjustedTargetTime, elapsedTime) % 24f;
                if (Camera.main != null) UpdateLighting(Camera.main);
                yield return null;
            }

            _timeOfDay = targetTime % 24f;
            if (Camera.main != null) UpdateLighting(Camera.main);
            UpdateTimeUI();
        }

        // Shortcut methods
        public void SetToDaybreak() => SetTimeInstantly(6f);
        public void SetToMidday() => SetTimeInstantly(12f);
        public void SetToSunset() => SetTimeInstantly(18f);
        public void SetToNight() => SetTimeInstantly(0f);

        public void SetTimeInstantly(float targetTime)
        {
            _timeOfDay = targetTime % 24f;
            if (Camera.main != null) UpdateLighting(Camera.main);
            UpdateTimeUI();
        }
    }
}
