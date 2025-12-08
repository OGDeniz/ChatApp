using ChatApp.PageModels;
using ChatApp.Services;

using System.Collections.Specialized;

namespace ChatApp.Pages;

public partial class ChatPage : ContentPage
{
    private ChatPageModel _viewModel;

    public ChatPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel == null)
        {
            // Hole ChatService aus DI
            var chatService = IPlatformApplication.Current.Services.GetService<ChatService>();

            _viewModel = new ChatPageModel(chatService);
            BindingContext = _viewModel;

            // Auto-Scroll bei neuer Nachricht
            _viewModel.Messages.CollectionChanged += Messages_CollectionChanged;
        }
    }

    private async void Messages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            await Task.Delay(100);
            await MessagesScrollView.ScrollToAsync(0, MessagesContainer.Height, true);
        }
    }

}