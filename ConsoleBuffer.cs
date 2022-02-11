namespace console_draw;

internal class ConsoleBuffer
{
    CharInfo[] buffer;
    CharInfo fillChar = new CharInfo((byte)' ', ConsoleColor.Black);
    bool isEmpty = false;

    public ConsoleBuffer(int cols, int rows)
    {
        buffer = new CharInfo[cols * rows];
        Clear();
    }

    public int Length => buffer.Length;

    internal void Clear(CharInfo? fillChar = null)
    {
        if (isEmpty && (fillChar is null || fillChar.Value == buffer.FirstOrDefault()))
            return;

        if (fillChar is null)
            fillChar = this.fillChar;

        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = fillChar.Value;
        }
        isEmpty = true;
    }

    internal void Clear(int x, int y)
    {
        Set(x * y, fillChar);
    }

    internal void Set(int x, int y, CharInfo charInfo)
    {
        Set(x + (x * y - 1), charInfo);
    }

    internal void Set(int x, CharInfo charInfo)
    {
        isEmpty = false;

        if (buffer.Length > x)
            buffer[x] = charInfo;
    }

    internal CharInfo[] Get() => buffer;
}