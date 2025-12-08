using ChatApp.Models;
using CommunityToolkit.Mvvm.Input;

namespace ChatApp.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}