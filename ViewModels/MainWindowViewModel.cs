namespace CourseProject.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        ThreeFishViewModel = new();
        SM4ViewModel = new();
        AboutViewModel = new();
    }

    public ThreeFishViewModel ThreeFishViewModel { get; }
    public SM4ViewModel SM4ViewModel { get; }
    public AboutViewModel AboutViewModel { get; }
}
