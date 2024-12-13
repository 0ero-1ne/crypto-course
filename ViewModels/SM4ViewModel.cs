using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CourseProject.Crypto;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CourseProject.ViewModels
{
    public partial class SM4ViewModel : ReactiveObject
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
        public ReactiveCommand<Unit, Unit> ClearFilePath { get; }
        public ReactiveCommand<Unit, Unit> ClearEncryptedFilePath { get; }
        public ReactiveCommand<Unit, Unit> ClearEncryptionKey { get; }
        public ReactiveCommand<Unit, Unit> ClearDecryptionKey { get; }
        public ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenEncryptedFileDialogCommand { get; }

        public SM4ViewModel()
        {
            EncryptCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (_filePath is null || _encryptionKey is null)
                {
                    await GetMsBox(
                        Messages.Messages.ENCRYPTION_FAIL,
                        Messages.Messages.ENCRYPT_INPUT_ERROR,
                        true
                    ).ShowAsync();

                    return;
                }

                if (!SM4KeyRegex().Match(EncryptionKey.ToUpper()).Success || EncryptionKey.Length != 16)
                {
                    await GetMsBox(
                        Messages.Messages.ENCRYPTION_FAIL,
                        Messages.Messages.SM4_KEY_ERROR,
                        true
                    ).ShowAsync();

                    return;
                }

                var timer = Stopwatch.StartNew();
                var encryptedData = SM4.Encrypt(File.ReadAllBytes(FilePath), EncryptionKey);
                FileWriter.FileWriter.WriteData(FileWriter.FileWriter.GetSM4EncryptedFilepath(FilePath)!, encryptedData);
                timer.Stop();

                FilePath = EncryptionKey = null!;

                await GetMsBox(
                    Messages.Messages.ENCRYPTION_SUCCESS,
                    $"File was encrypted in {timer.ElapsedMilliseconds} ms and saved with original file",
                    false
                ).ShowAsync();
            });

            DecryptCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (_encryptedFilePath is null || _decryptionKey is null)
                {
                    await GetMsBox(
                        Messages.Messages.DECRYPTION_FAIL,
                        Messages.Messages.DECRYPT_INPUT_ERROR,
                        true
                    ).ShowAsync();

                    return;
                }

                if (!SM4KeyRegex().Match(DecryptionKey.ToUpper()).Success || DecryptionKey.Length != 16)
                {
                    await GetMsBox(
                        Messages.Messages.DECRYPTION_FAIL,
                        Messages.Messages.SM4_KEY_ERROR,
                        true
                    ).ShowAsync();

                    return;
                }

                var timer = Stopwatch.StartNew();
                var decryptedData = SM4.Decrypt(File.ReadAllBytes(EncryptedFilePath), DecryptionKey);
                FileWriter.FileWriter.WriteData(FileWriter.FileWriter.GetSM4DecryptedFilepath(EncryptedFilePath)!, decryptedData);
                timer.Stop();

                EncryptedFilePath = DecryptionKey = null!;

                await GetMsBox(
                    Messages.Messages.DECRYPTION_SUCCESS,
                    $"File was decrypted in {timer.ElapsedMilliseconds} ms and saved with original file",
                    false
                ).ShowAsync();
            });

            GenerateKeyCommand = ReactiveCommand.Create(() =>
            {
                EncryptionKey = KeyGenerator.GenerateKey(16);
            });

            ClearFilePath = ReactiveCommand.Create(() =>
            {
                FilePath = null!;
            });

            ClearEncryptedFilePath = ReactiveCommand.Create(() =>
            {
                EncryptedFilePath = null!;
            });

            ClearEncryptionKey = ReactiveCommand.Create(() =>
            {
                EncryptionKey = null!;
            });

            ClearDecryptionKey = ReactiveCommand.Create(() =>
            {
                DecryptionKey = null!;
            });

            OpenFileDialogCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!;
                var mainWindow = desktop.MainWindow;
                var topLevel = TopLevel.GetTopLevel(mainWindow);
                var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new()
                {
                    Title = "Select file to encrypt",
                    AllowMultiple = false
                });

                if (files.Count >= 1)
                {
                    FilePath = files[0].Path.LocalPath;
                }
            });

            OpenEncryptedFileDialogCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!;
                var mainWindow = desktop.MainWindow;
                var topLevel = TopLevel.GetTopLevel(mainWindow);
                var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new()
                {
                    Title = "Select file to decrypt",
                    AllowMultiple = false
                });

                if (files.Count >= 1)
                {
                    if (Path.GetExtension(files[0].Path.LocalPath) == ".sm4")
                    {
                        EncryptedFilePath = files[0].Path.LocalPath;
                        return;
                    }

                    var _ = await GetMsBox("File extension error", Messages.Messages.FILE_DECRYPTION_FORMAT_ERROR_SM4, true).ShowAsync();
                }
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
        private static partial Regex SM4KeyRegex();
    }
}
