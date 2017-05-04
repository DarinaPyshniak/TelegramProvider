using System.Windows;

namespace TelegramProvider {
    public partial class MainWindow : Window {
        private readonly SmsProviderTelegram _provider = new SmsProviderTelegram();
        private readonly string _receiverPhoneNumder = "XXXXXXXXXXXX";
        public MainWindow() {
            InitializeComponent();
            DataContext = this;
        }

        public string Message { get; set; }

        private async void ButtonSendClick(object sender, RoutedEventArgs e) {
            var isConnected = await _provider.Connect();
            if (!isConnected)
                return;
            var id = await _provider.GetUserIdByPhoneNumber(_receiverPhoneNumder);
            _provider.SendMessage(id, Message);
            Message = string.Empty;
        }
    }
}