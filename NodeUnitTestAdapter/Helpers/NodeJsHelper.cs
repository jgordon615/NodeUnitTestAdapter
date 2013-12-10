using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeUnitTestAdapter.Helpers
{
    public static class NodeJsHelper
    {
        private static readonly string[] _pathsToTry;
        private static string _nodeJsLocation;

        static NodeJsHelper()
        {
            var paths = new List<string>();

            paths.AddRange(
             new[] {
                @"%ProgramFiles%\nodejs\node.exe",
                @"%ProgramFiles(x86)%\nodejs\node.exe",
                @"%ProgramW6432%\nodejs\node.exe",
                @"%SystemDrive%\Program Files\nodejs\node.exe",
                @"%SystemDrive%\Program Files (x86)\nodejs\node.exe",
                @"%SystemDrive%\nodejs\node.exe",
                @"%USERPROFILE%\Desktop\node.exe"
            });

            var envs = Environment.GetEnvironmentVariable("path");
            var envsSplit = envs.Split(';');
            foreach (var env in envsSplit)
            {
                paths.Add(Path.Combine(env, "node.exe"));
            }

            _pathsToTry = paths.ToArray();
        }

        public static string LocateNodeJs()
        {
            if (string.IsNullOrEmpty(_nodeJsLocation))
            {
                foreach (var path in _pathsToTry)
                {
                    var p = Environment.ExpandEnvironmentVariables(path);
                    if (File.Exists(p))
                    {
                        _nodeJsLocation = p;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(_nodeJsLocation))
            {
                string message = "Could not locate node.exe. Tried:\n" + string.Join("\n", _pathsToTry);
                throw new FileNotFoundException(message);
            }

            return _nodeJsLocation;
        }
    }
}
