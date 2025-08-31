using IKitaplik.BlazorUI.Components.Components;
using MudBlazor;

namespace IKitaplik.BlazorUI.Helpers
{
    public class MessageBoxService
    {
        private readonly IDialogService _dialogService;

        public MessageBoxService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task<bool> ShowConfirmDialog(string title, string message, string confirmText = "Sil", string cancelText = "Vazgeç")
        {
            var parameters = new DialogParameters
            {
                { "Title", title },
                { "Content", message },
                { "ConfirmButtonText", confirmText },
                { "CancelButtonText", cancelText }
            };

            var options = new DialogOptions { CloseOnEscapeKey = true,FullWidth = true,MaxWidth = MaxWidth.Small,BackdropClick = false};
            var dialog = await _dialogService.ShowAsync<MessageBoxComponent>("", parameters, options);
            var result = await dialog.Result;

            return !result.Canceled && (bool)result.Data;
        }
    }

}
