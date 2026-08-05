using StudentTaskManager.Services;

namespace StudentTaskManager
{
    public partial class App : Application
    {
        public App(DatabaseService databaseService)
        {
            InitializeComponent();

            MainPage = new AppShell();

            Task.Run(async () =>
            {
                await databaseService.InitializeAsync();
            });
        }
    }
}
