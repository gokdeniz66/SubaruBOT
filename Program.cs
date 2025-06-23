using SubaruBOT;
using DotNetEnv;

class Program
{
    static async Task Main(string[] args)
    {
        Env.Load();
        var bot = new Bot();
        await bot.StartAsync();
    }
}