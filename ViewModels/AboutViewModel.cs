using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CourseProject.Crypto;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reflection;
using System.Text;

namespace CourseProject.ViewModels
{
    public class AboutViewModel : ReactiveObject
    {
        public string Header { get; } = "Welcome";
        public string Author { get; } = "Auhtor: 0ero_1ne";
        public string ParagraphOne { get; } = "This program allows you to encrypt your files";
        public string ParagraphTwo { get; } = "We provide two encryption methods: SM4 and ThreeFish";
        public string Compare { get; } = "The button below will generate a CSV files for you\nbased on which you can conduct a comparative analysis of the algorithms.";

        public ReactiveCommand<Unit, Unit> CreateСomparisonFiles { get; }

        public AboutViewModel()
        {
            CreateСomparisonFiles = ReactiveCommand.Create(() =>
            {
                try
                {
                    int[] byteCounts = [100_000, 1_000_000, 10_000_000, 100_000_000];
                    var threefishKey = KeyGenerator.GenerateKey(32);
                    var sm4Key = KeyGenerator.GenerateKey(16);

                    Dictionary<int, (long, long)> threefishEncryptionTime = [];
                    Dictionary<int, (long, long)> threefishDecryptionTime = [];
                    Dictionary<int, (long, long)> sm4EncryptionTime = [];
                    Dictionary<int, (long, long)> sm4DecryptionTime = [];
                    Dictionary<string, string> avalancheEffectThreeFish = [];
                    Dictionary<string, string> avalancheEffectSM4 = [];

                    for (int i = 0; i < byteCounts.Length; i++)
                    {
                        var message = new byte[byteCounts[i]];
                        new Random().NextBytes(message);

                        var timer = System.Diagnostics.Stopwatch.StartNew();
                        var encryptedThreeFishMessage = ThreeFish.Encrypt(message, threefishKey);
                        timer.Stop();
                        threefishEncryptionTime[byteCounts[i]] = (timer.ElapsedMilliseconds, encryptedThreeFishMessage!.Length);

                        timer.Restart();
                        var decrpytedThreeFishMessage = ThreeFish.Decrypt(encryptedThreeFishMessage!, threefishKey);
                        timer.Stop();
                        threefishDecryptionTime[byteCounts[i]] = (timer.ElapsedMilliseconds, byteCounts[i]);

                        timer.Restart();
                        var encryptedSM4Message = SM4.Encrypt(message, sm4Key);
                        timer.Stop();
                        sm4EncryptionTime[byteCounts[i]] = (timer.ElapsedMilliseconds, encryptedSM4Message!.Length);

                        timer.Restart();
                        var decrpytedSM4Message = SM4.Decrypt(encryptedSM4Message!, sm4Key);
                        timer.Stop();
                        sm4DecryptionTime[byteCounts[i]] = (timer.ElapsedMilliseconds, byteCounts[i]);
                    }

                    var avalancheMessage = "Hello";
                    var avalancheEffect = Encoding.UTF8.GetBytes(avalancheMessage);

                    var encryptedThreeFish = ThreeFish.Encrypt(avalancheEffect, threefishKey);
                    avalancheEffectThreeFish[avalancheMessage] = Convert.ToHexString(encryptedThreeFish!);
                    var encryptedSM4 = SM4.Encrypt(avalancheEffect, sm4Key);
                    avalancheEffectSM4[avalancheMessage] = Convert.ToHexString(encryptedSM4!);

                    avalancheMessage = "Hellp";
                    avalancheEffect = Encoding.UTF8.GetBytes(avalancheMessage);

                    encryptedThreeFish = ThreeFish.Encrypt(avalancheEffect, threefishKey);
                    avalancheEffectThreeFish[avalancheMessage] = Convert.ToHexString(encryptedThreeFish!);
                    encryptedSM4 = SM4.Encrypt(avalancheEffect, sm4Key);
                    avalancheEffectSM4[avalancheMessage] = Convert.ToHexString(encryptedSM4!);

                    avalancheMessage = "Hellr";
                    avalancheEffect = Encoding.UTF8.GetBytes(avalancheMessage);

                    encryptedThreeFish = ThreeFish.Encrypt(avalancheEffect, threefishKey);
                    avalancheEffectThreeFish[avalancheMessage] = Convert.ToHexString(encryptedThreeFish!);
                    encryptedSM4 = SM4.Encrypt(avalancheEffect, sm4Key);
                    avalancheEffectSM4[avalancheMessage] = Convert.ToHexString(encryptedSM4!);

                    avalancheMessage = "Hells";
                    avalancheEffect = Encoding.UTF8.GetBytes(avalancheMessage);

                    encryptedThreeFish = ThreeFish.Encrypt(avalancheEffect, threefishKey);
                    avalancheEffectThreeFish[avalancheMessage] = Convert.ToHexString(encryptedThreeFish!);
                    encryptedSM4 = SM4.Encrypt(avalancheEffect, sm4Key);
                    avalancheEffectSM4[avalancheMessage] = Convert.ToHexString(encryptedSM4!);

                    // operations time to file

                    var timeOperationsFileName = "./../../../time.csv";
                    File.WriteAllText(timeOperationsFileName, "Operation;Algorithm;Bytes;Time;Size\n");

                    for (int i = 0; i < byteCounts.Length; i++)
                    {
                        File.AppendAllText(
                            timeOperationsFileName,
                            $"Encryption;Threefish;{byteCounts[i]};{threefishEncryptionTime[byteCounts[i]].Item1};{threefishEncryptionTime[byteCounts[i]].Item2}\n"
                        );

                        File.AppendAllText(
                            timeOperationsFileName,
                            $"Encryption;SM4;{byteCounts[i]};{sm4EncryptionTime[byteCounts[i]].Item1};{sm4EncryptionTime[byteCounts[i]].Item2}\n"
                        );
                    }

                    for (int i = 0; i < byteCounts.Length; i++)
                    {
                        File.AppendAllText(
                            timeOperationsFileName,
                            $"Decryption;Threefish;{byteCounts[i]};{threefishDecryptionTime[byteCounts[i]].Item1};{threefishDecryptionTime[byteCounts[i]].Item2}\n"
                        );

                        File.AppendAllText(
                            timeOperationsFileName,
                            $"Decryption;SM4;{byteCounts[i]};{sm4DecryptionTime[byteCounts[i]].Item1};{sm4DecryptionTime[byteCounts[i]].Item2}\n"
                        );
                    }


                    // avalanche effect to file
                    var avalancheEffectFileName = "./../../../avalanche.csv";
                    File.WriteAllText(avalancheEffectFileName, "Algorithm;Original;Encrypted HEX\n");

                    foreach (var item in avalancheEffectThreeFish)
                    {
                        File.AppendAllText(avalancheEffectFileName, $"Threefish;{item.Key};{item.Value}\n");
                    }

                    foreach (var item in avalancheEffectSM4)
                    {
                        File.AppendAllText(avalancheEffectFileName, $"SM4;{item.Key};{item.Value}\n");
                    }

                    GetMsBox($"Files {timeOperationsFileName} and {avalancheEffectFileName} where successfully generated").ShowAsync();
                }
                catch (Exception e)
                {
                    GetMsBox(e.Message + "\n" + e.StackTrace + "\n" + e.Source).ShowAsync();
                }
            });
        }

        private static IMsBox<ButtonResult> GetMsBox(string text)
        {
            Bitmap logo = new(AssetLoader.Open(new Uri($"avares://{Assembly.GetExecutingAssembly().GetName().Name}/Assets/avalonia-logo.ico")));
            return MessageBoxManager.GetMessageBoxStandard
            (
                new MsBox.Avalonia.Dto.MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "ThreeFish/SM4 Encryptor",
                    ContentHeader = "CVS files",
                    ContentMessage = text,
                    Icon = Icon.Info,
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
    }
}
