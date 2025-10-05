using System.Runtime.CompilerServices;
using System.Text;

namespace Samurai.UnityFramework
{
    public static class Log
    {
        private static readonly StringBuilder _sb = new();
        
        public static void Debug(string message, string tag = null)
        {
            UnityEngine.Debug.Log(ConstructLog(message, tag));
        }

        public static void Debug(object obj, string tag = null)
        {
            if (obj is null)
            {
                return;
            }
            
            UnityEngine.Debug.Log(ConstructLog(obj.ToString(), tag));
        }
        
        public static void Warning(string message, string tag = null)
        {
            UnityEngine.Debug.LogWarning(ConstructLog(message, tag));
        }

        public static void Warning(object obj, string tag = null)
        {
            if (obj is null)
            {
                return;
            }
            
            UnityEngine.Debug.LogWarning(ConstructLog(obj.ToString(), tag));
        }
        
        public static void Error(string message, string tag = null)
        {
            UnityEngine.Debug.LogError(ConstructLog(message, tag));
        }

        public static void Error(object obj, string tag = null)
        {
            if (obj is null)
            {
                return;
            }
            
            UnityEngine.Debug.LogError(ConstructLog(obj.ToString(), tag));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ConstructLog(string message, string tag)
        {
            _sb.Clear();
            if (tag is not null)
            {
                _sb.Append('[');
                _sb.Append(tag);
                _sb.Append(']');
                _sb.Append(' ');
            }

            _sb.Append(message);
            return _sb.ToString();
        }
    }
}