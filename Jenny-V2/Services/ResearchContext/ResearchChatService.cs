namespace Jenny_V2.Services.ResearchContext
{
    public class ResearchChatService
    {
        private readonly DictationService _dictationService;
        private readonly MainWindow _mainWindow;

        public ResearchChatService(
            DictationService dictationService,
            MainWindow mainWindow
            )
        {
            _dictationService = dictationService;
            _mainWindow = mainWindow;
        }

        public void OpenChat()
        {

        }
    }
}
