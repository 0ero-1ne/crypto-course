using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CourseProject.Crypto;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CourseProject.ViewModels
{
    public partial class ThreeFishViewModel : ReactiveObject
    {
        private string? _filePath = null;
        public string FilePath
        {
            get => _filePath ?? "No file selected";
            set => this.RaiseAndSetIfChanged(ref _filePath, value);
        }

        private string? _encryptedFilePath = null;
        public string EncryptedFilePath
        {
            get => _encryptedFilePath ?? "No file selected";
            set => this.RaiseAndSetIfChanged(ref _encryptedFilePath, value);
        }

        private string? _encryptionKey = null;
        public string EncryptionKey
        {
            get => _encryptionKey ?? "";
            set => this.RaiseAndSetIfChanged(ref _encryptionKey, value);
        }

        private string? _decryptionKey = null;
        public string DecryptionKey
        {
            get => _decryptionKey ?? "";
            set => this.RaiseAndSetIfChanged(ref _decryptionKey, value);
        }

        public ReactiveCommand<Unit, Unit> EncryptCommand { get; }
        public ReactiveCommand<Unit, Unit> DecryptCommand { get; }
        public ReactiveCommand<Unit, Unit> GenerateKeyCommand { get; }

        public ThreeFishViewModel()
        {
            EncryptCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (_filePath is null || _encryptionKey is null)
                {
                    var _ = await GetMsBox("Encryption error", Messages.Messages.ENCRYPT_INPUT_ERROR, true).ShowAsync();
                    return;
                }

                if (!ThreeFishKeyRegex().Match(EncryptionKey).Success || EncryptionKey.Length != 32)
                {
                    var _ = await GetMsBox("Encryption error", Messages.Messages.TF_KEY_ERROR, true).ShowAsync();
                    return;
                }

                var res = await GetMsBox("Encryption success", Messages.Messages.ENCRYPTION_SUCCESS, false).ShowAsync();
            });

            DecryptCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (_encryptedFilePath is null || _decryptionKey is null)
                {
                    var _ = await GetMsBox("Decryption error", Messages.Messages.DECRYPT_INPUT_ERROR, true).ShowAsync();
                    return;
                }

                if (!ThreeFishKeyRegex().Match(DecryptionKey).Success || DecryptionKey.Length != 32)
                {
                    var _ = await GetMsBox("Decryption error", Messages.Messages.TF_KEY_ERROR, true).ShowAsync();
                    return;
                }

                var res = await GetMsBox("Decryption success", Messages.Messages.DECRYPTION_SUCCESS, false).ShowAsync();
            });

            GenerateKeyCommand = ReactiveCommand.Create(() =>
            {
                EncryptionKey = KeyGenerator.GenerateKey(32);
            });
        }

        private static IMsBox<ButtonResult> GetMsBox(string header, string text, bool isError)
        {
            Bitmap logo = new(AssetLoader.Open(new Uri($"avares://{Assembly.GetExecutingAssembly().GetName().Name}/Assets/avalonia-logo.ico")));
            return MessageBoxManager.GetMessageBoxStandard
            (
                new MsBox.Avalonia.Dto.MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "ThreeFish/SM4 Encryptor",
                    ContentHeader = header,
                    ContentMessage = text,
                    Icon = isError ? Icon.Error : Icon.Success,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    CanResize = false,
                    Width = 400,
                    SizeToContent = SizeToContent.Height,
                    ShowInCenter = true,
                    Topmost = true,
                    WindowIcon = new WindowIcon(logo)
                }
            );
        }

        [GeneratedRegex(@"[0-9A-Za-z@#!`'""\\|/?$[\]{};:%^&*()=_+.,~<>-]")]
        private static partial Regex ThreeFishKeyRegex();
    }
}
