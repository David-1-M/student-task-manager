using StudentTaskManager.Models;
using StudentTaskManager.ViewModels;

namespace StudentTaskManager.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;

        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadTasksCommand.ExecuteAsync(null);
    }

    private async void CollectionView_SelectionChanged(
    object sender,
    SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TaskItem task)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(EditTaskPage)}?TaskId={task.Id}");

            ((CollectionView)sender).SelectedItem = null;
        }
    }

}