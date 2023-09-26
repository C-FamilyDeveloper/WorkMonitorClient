using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using static WorkMonitorClient.Models.PInvoke.NativeMethods;
namespace WorkMonitorClient.Models.Services
{
    public class HookService
    {
        private LowLevelMouseProc procMouse; 
        private LowLevelKeyboardProc procKeyboard; 
        private IntPtr hookIDMouse = IntPtr.Zero;
        private IntPtr hookIDKeyboard = IntPtr.Zero;
        private const int WHKEYBOARDLL = 13;
        private const int WHMOUSELL = 14;
        private Stopwatch fullwatch = new();
        private Stopwatch workwatch = new();
        
        public HookService() 
        {
            procKeyboard = HookCallbackKeyboard;
            procMouse = HookCallbackMouse;
        }
        private  IntPtr SetHook(LowLevelMouseProc proc)
        {
            using Process curProcess = Process.GetCurrentProcess();
            using ProcessModule curModule = curProcess.MainModule;            
            return SetWindowsHookEx(WHMOUSELL, proc,
                GetModuleHandle(curModule.ModuleName), 0);            
        }
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using Process curProcess = Process.GetCurrentProcess();
            using ProcessModule curModule = curProcess.MainModule;            
            return SetWindowsHookEx(WHKEYBOARDLL, proc,
                GetModuleHandle(curModule.ModuleName), 0);            
        }

        private IntPtr HookCallbackMouse(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && 
                   (wParam == (IntPtr)MouseMessages.WMLBUTTONDOWN  ||
                    wParam == (IntPtr)MouseMessages.WMLBUTTONUP ||
                    wParam == (IntPtr)MouseMessages.WMLBUTTONDBLCLK ||
                    wParam == (IntPtr)MouseMessages.WMRBUTTONDOWN  ||
                    wParam == (IntPtr)MouseMessages.WMRBUTTONUP ||
                    wParam == (IntPtr)MouseMessages.WMRBUTTONDBLCLK ||
                    wParam == (IntPtr)MouseMessages.WMMBUTTONDOWN ||
                    wParam == (IntPtr)MouseMessages.WMMBUTTONUP ||
                    wParam == (IntPtr)MouseMessages.WMMBUTTONDBLCLK ||
                    wParam == (IntPtr)MouseMessages.WMMOUSEMOVE ||
                    wParam == (IntPtr)MouseMessages.WMMOUSEWHEEL))
            {
                workwatch.Start();
            }
            else
            {
                workwatch.Stop();
            }
            return CallNextHookEx(hookIDMouse, nCode, wParam, lParam);
        }


        private IntPtr HookCallbackKeyboard(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 &&
                   (wParam == (IntPtr)KeyBoardMessages.WMKEYDOWN ||
                    wParam == (IntPtr)KeyBoardMessages.WMKEYUP))
            {
                workwatch.Start();
            }
            else
            {
                workwatch.Stop();
            }
            return CallNextHookEx(hookIDKeyboard, nCode, wParam, lParam);
        }

        public void Start()
        {
            fullwatch.Start();
            hookIDMouse = SetHook(procMouse);
            hookIDKeyboard = SetHook(procKeyboard);
        }

        public MonitorTime Restart()
        {
            TimeSpan timeSpan = workwatch.Elapsed;
            TimeSpan timeSpanFull = fullwatch.Elapsed;
            fullwatch.Reset();
            fullwatch.Start();
            workwatch.Reset();
            return new() { FullTime = timeSpanFull, WorkTime = timeSpan };         
        }
        public MonitorTime Stop()
        {
            UnhookWindowsHookEx(hookIDMouse);
            UnhookWindowsHookEx(hookIDKeyboard);
            workwatch.Stop();
            fullwatch.Stop();
            return new() { FullTime = fullwatch.Elapsed, WorkTime = workwatch.Elapsed };      
        }
    }
}
