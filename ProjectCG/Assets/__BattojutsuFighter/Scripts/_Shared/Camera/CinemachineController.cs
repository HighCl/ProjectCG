public class CinemachineController: BaseCinemachineController
{
    protected override void OnInit()
    {
        if (_perlinNoise != null)
        {
            _perlinNoise.FrequencyGain = 5;
        }
    }
}