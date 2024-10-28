using System.Speech.Recognition;

namespace Jenny_V2.Services.Core
{

    public class VoiceActivationService
    {
        private readonly ZeroShotService _zeroShotService;
        private readonly ChatGPTService _chatGPTService;
        private readonly SpeechRecognizerService _speechRecognizerService;

        private const double _minConfidence = .85;

        public VoiceActivationService(
            ZeroShotService zeroShotService,
            ChatGPTService chatGPTService,
            SpeechRecognizerService speechRecognizerService
            )
        {
            _chatGPTService = chatGPTService;
            _zeroShotService = zeroShotService;
            _speechRecognizerService = speechRecognizerService;
        }

        public void OnRegocnizedHeyJenny(SpeechRecognizedEventArgs args)
        {
            if (args.Result.Confidence < _minConfidence) return;

            VoiceActivationStop();
            _speechRecognizerService.StartSpeechRegonition();
            _chatGPTService.GetAIResponse("greet the user and ask if you can be of assistance, in a polite and consice manner");
        }

        public void ToggleVoiceActivation()
        {
            if (_speechRecognizerService.IsRegonizing) VoiceActivationStop();
            else VoiceActivationStart();
        }

        public void VoiceActivationStop()
        {
            if (_zeroShotService.OnSpeechRegonised != null)
                _zeroShotService.OnSpeechRegonised -= OnRegocnizedHeyJenny;
            _zeroShotService.ListenAsyncStop();
        }

        public void VoiceActivationStart()
        {
            if (_zeroShotService.OnSpeechRegonised == null)
                _zeroShotService.OnSpeechRegonised += OnRegocnizedHeyJenny;

            _zeroShotService.AddPossibilities(new List<string>() { "Hey Jenny" });
            _zeroShotService.ListenAsyncStart();
        }

    }
}
