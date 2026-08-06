using StudentTaskManager.ViewModels;

namespace StudentTaskManager.Views;

[QueryProperty(nameof(TaskId), "TaskId")]
public partial class EditTaskPage : ContentPage
{
    private readonly EditTaskViewModel _viewModel;

    public EditTaskPage(EditTaskViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    public string TaskId
    {
        set
        {
            if (int.TryParse(value, out int id))
            {
                _viewModel.LoadTaskCommand.Execute(id);
            }
        }
    }
}