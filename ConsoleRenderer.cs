namespace asciiquarium;

using Microsoft.Win32.SafeHandles;

using System;
using System.Runtime.InteropServices;

internal class ConsoleRenderer
{
    SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
    public short AreaLeft { get; private set; }
    public short AreaTop { get; private set; }
    public short AreaRight { get; private set; }
    public short AreaBottom { get; private set; }
    public short AreaWidth { get; private set; }
    public short AreaHeight { get; private set; }

    int maxAreaWidth, maxAreaHeight;
    ConsoleBuffer? buffer;
    bool clearBeforeNextFrame = true;
    public bool AutoResizeToWindow { get; set; } = false;

    public delegate void AfterUpdate();

    public delegate void Update(ref ConsoleBuffer buffer, out bool clearOnNextFrame);

    public event AfterUpdate? OnUpdateCompleted;

    public event Update? OnUpdate;

    public void SetSize(int width, int height)
    {
        SetSize(Convert.ToInt16(width), Convert.ToInt16(height));
    }
    public void SetSize(short width, short height)
    {
        maxAreaWidth = Console.WindowWidth;
        maxAreaHeight = Console.WindowHeight;

        AreaWidth = width;
        AreaHeight = height;
        AreaLeft = Convert.ToInt16((maxAreaWidth - width) / 2d);
        AreaTop = Convert.ToInt16((maxAreaHeight - height) / 2d);
        AreaRight = Convert.ToInt16(width + AreaLeft);
        AreaBottom = Convert.ToInt16(height + AreaTop);
        buffer = new ConsoleBuffer(width, height);
        Console.Clear();
    }

    public async Task DoUpdateAsync()
    {
        if (buffer is null)
        {
            throw new ArgumentException($"You need to call {nameof(ConsoleRenderer)}.{nameof(SetSize)}() before the first Update!");
        }

        if (AutoResizeToWindow && (maxAreaHeight != Console.WindowHeight
            || maxAreaWidth != Console.WindowWidth))
        {
            SetSize(Convert.ToInt16(Console.WindowWidth), Convert.ToInt16(Console.WindowHeight));
        }

        //low: update rect if console got resized

        if (OnUpdate is not null)
        {
            if (clearBeforeNextFrame)
                buffer.Clear();

            OnUpdate?.Invoke(ref buffer, out clearBeforeNextFrame);
        }
        else
        {
            buffer.Clear(new CharInfo((byte)' ', ConsoleColor.Black, ConsoleColor.DarkMagenta));
        }

        RenderFrame();
        OnUpdateCompleted?.Invoke();
    }

    [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern SafeFileHandle CreateFile(
                string fileName,
                [MarshalAs(UnmanagedType.U4)] uint fileAccess,
                [MarshalAs(UnmanagedType.U4)] uint fileShare,
                IntPtr securityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                [MarshalAs(UnmanagedType.U4)] int flags,
                IntPtr template);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool WriteConsoleOutputW(
        SafeFileHandle hConsoleOutput,
        CharInfo[] lpBuffer,
        Coord dwBufferSize,
        Coord dwBufferCoord,
        ref SmallRect lpWriteRegion);

    private void RenderFrame()
    {
        if (buffer is null)
            return;

        SmallRect rect = new SmallRect()
        {
            Left = AreaLeft,
            Top = AreaTop,
            Right = AreaRight,
            Bottom = AreaBottom
        };
        bool b = WriteConsoleOutputW(h, buffer.Get(),
                      new Coord() { X = AreaWidth, Y = AreaHeight },
                      new Coord() { X = 0, Y = 0 },
                      ref rect);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Coord
    {
        public short X;
        public short Y;

        public Coord(short X, short Y)
        {
            this.X = X;
            this.Y = Y;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    private struct SmallRect
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }
}
