using StudentTaskManager.ViewModels;

namespace StudentTaskManager.Views;

public partial class AddTaskPage : ContentPage
{
    public AddTaskPage(AddTaskViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}