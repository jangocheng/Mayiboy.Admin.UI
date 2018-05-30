using System;
using Framework.Mayiboy.Ioc;
using Mayiboy.Contract;

namespace Mayiboy.Logic.Task
{
    public class DemoTask
    {
        public static void Show(string Patch, string taskName)
        {
            Console.WriteLine("Patch:{0};TaskName:{1}", Patch, taskName);

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            IUserInfoService _userinfoService = ServiceLocater.GetService<IUserInfoService>();
        }

        public static void LoopShow()
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}