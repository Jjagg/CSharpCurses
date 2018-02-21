using CSharpCurses.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpCurses
{
    
    public static class ConsoleApp
    {
        public static ConsoleState StartState;

        public static Screen Screen { get; }
        private static readonly Stack<ConsoleState> StateStack = new Stack<ConsoleState>();

        static ConsoleApp()
        {
            Screen = new Screen();
            BufferManager.Initialize();
        }

        private static void StartRun()
        {
            StartState = ConsoleState.Create();
            Console.CursorVisible = false;

            //Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.UTF8; // for some reason this works as unicode??
            Console.TreatControlCAsInput = true;

            BufferManager.Initialize();
        }

        private static CancellationTokenSource _cts;

        public static void Run(TimeSpan delay, Action update, Action<ConsoleKeyInfo> keyDown)
        {
            StartRun();
            try
            {
                Console.Clear();
                Screen.StartRun();

                using (_cts = new CancellationTokenSource())
                {
                    var ct = _cts.Token;
                    Task.Run(() => InputLoop(keyDown, ct)).ContinueWith(t => _cts.Cancel());
                    UpdateLoop(delay, update, ct);
                    _cts.Cancel();
                }
            }
            finally
            {
                _cts = null;
                Console.WriteLine();
                Reset();
                Console.Clear();
            }
        }

        private static void InputLoop(Action<ConsoleKeyInfo> keyDown, CancellationToken ct)
        {
            while (true)
            {
                Wait(TimeSpan.FromSeconds(5), ct);
                if (ct.IsCancellationRequested)
                    return;
                while (Console.KeyAvailable)
                {
                    var k = Console.ReadKey(true);
                    if (k.Key == ConsoleKey.C && (k.Modifiers & ConsoleModifiers.Control) > 0)
                        return;
                    keyDown?.Invoke(k);
                }
            }
        }

        private static void UpdateLoop(TimeSpan delay, Action update, CancellationToken ct)
        {
            while (true)
            {
                update?.Invoke();
                Screen.Update();
                Screen.Draw();
                BufferManager.Flush();

                if (Wait(delay, ct))
                    break;
            }
        }

        private static bool Wait(TimeSpan t, CancellationToken ct)
        {
            try
            {
                Task.Delay(t).Wait(ct);
            }
            catch (OperationCanceledException)
            {
                return true;
            }
            return false;
        }

        public static void Reset()
        {
            SetState(StartState);
            Console.CursorVisible = true;
        }

        public static void PushState()
        {
            var cs = ConsoleState.Create();
            StateStack.Push(cs);
        }

        public static void PopState()
        {
            if (StateStack.Any())
                SetState(StateStack.Pop());
        }
        
        public static void SetState(ConsoleState state)
        {
            Console.ForegroundColor = state.Foreground;
            Console.BackgroundColor = state.Background;
            Console.SetCursorPosition(state.CursorX, state.CursorY);
        }

        public static void Quit()
        {
            _cts?.Cancel();
        }
    }
}
