using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Framework.Mayiboy.Logging;
using Framework.Mayiboy.Task;
using Mayiboy.Utils;

namespace Mayiboy.MainService
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            LogManager.DefaultLogger.Debug("MonotorMianService Start! [HostIP=127.0.0.1]");

            TaskGlobal.Log.OnDebug += LogManager.TaskLoger.Debug;
            TaskGlobal.Log.OnInfo += LogManager.TaskLoger.Info;
            TaskGlobal.Log.OnWarn += LogManager.TaskLoger.Warn;
            TaskGlobal.Log.OnError += LogManager.TaskLoger.Error;

            TaskGlobal.InitTask();
            TaskGlobal.StartAll();
        }
    }
}
