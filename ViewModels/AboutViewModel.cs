using ReactiveUI;

namespace CourseProject.ViewModels
{
    public class AboutViewModel : ReactiveObject
    {
        public string Header { get; } = "Welcome";
        public string Author { get; } = "Auhtor: 0ero_1ne";
        public string ParagraphOne { get; } = "This program allows you to encrypt your files";
        public string ParagraphTwo { get; } = "We provide two encryption methods: SM4 and ThreeFish";
    }
}
