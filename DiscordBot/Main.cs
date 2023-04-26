﻿using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        private static string[] messages; // 投稿するメッセージを格納する配列

        private static DiscordSocketClient client; // Discordサーバーとの接続を管理するクライアント
        private static ulong serverId = 695915901492396043; // 投稿するサーバーのID
        private static ulong channelId = 1100284988190109716; // 投稿するチャンネルのID
        private static TimeSpan interval = TimeSpan.FromHours(1); // 投稿する間隔
        private static int targetHour = 4; // 投稿する整数時刻

        static void Main(string[] args)
        {
            // メッセージを外部ファイルから読み込む
            messages = File.ReadAllLines("messages.txt").Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            client = new DiscordSocketClient(); // Discordサーバーとの接続を管理するクライアントを作成する

            client.Log += LogAsync; // クライアントのログ出力時に呼び出されるメソッドを登録する
            client.Ready += ReadyAsync; // クライアントがDiscordサーバーと接続し、準備ができた時に呼び出されるメソッドを登録する

            client.LoginAsync(TokenType.Bot, "MTEwMDI4Mjg4NzI1OTA0OTk5NQ.G4GsJr.rOuFbddmEqQP3iZ8k0QIgnqupjUHAqOxZhRDTQ"); // ボットのトークンを指定してログインする
            client.StartAsync(); // クライアントを起動する

            Task.Delay(-1).GetAwaiter().GetResult(); // プログラムが終了しないように待機する
        }

        private static Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.ToString()); // ログメッセージをコンソールに出力する
            return Task.CompletedTask;
        }

        private static Task ReadyAsync()
        {
            var server = client.GetGuild(serverId); // 投稿するサーバーを取得する
            if (server == null)
            {
                Console.WriteLine($"Cannot find server with id: {serverId}");
                return Task.CompletedTask;
            }

            var channel = server.GetTextChannel(channelId); // 投稿するチャンネルを取得する
            if (channel == null)
            {
                Console.WriteLine($"Cannot find channel with id: {channelId}");
                return Task.CompletedTask;
            }

            // 定期的にメッセージを投稿する
Task.Run(async () =>
{
    while (true)
    {
        DateTime now = DateTime.Now;
        if (now.Hour == 4 && now.Minute == 0 && now.Second == 0)
        {
            await channel.SendMessageAsync("午前4時です。特定の文章を送信します。");
        }
        else
        {
            int index = new Random().Next(messages.Length); // メッセージのインデックスをランダムに取得する
            await channel.SendMessageAsync(messages[index]); // メッセージを投稿する
        }

        await Task.Delay(interval); // 次の投稿まで待機する
    }
});

            return Task.CompletedTask;
        }
    }
}

