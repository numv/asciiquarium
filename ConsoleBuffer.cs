namespace asciiquarium;

internal class ConsoleBuffer
{
    CharInfo[] buffer;
    CharInfo fillChar = new CharInfo(' ', ConsoleColor.Black);
    bool isEmpty = false;
    int cols, rows;


    public ConsoleBuffer(int cols, int rows)
    {
        this.cols = cols;
        this.rows = rows;
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
        Set(x, y, fillChar);
    }

    internal void Set(int x, int y, CharInfo? charInfo)
    {
        if (charInfo == null)
            return;
        if (x < 0 || x >= cols
            || y < 0 || y >= rows)
            return;
        Set(x + (cols * y), charInfo);
    }

    internal void Set(int x, CharInfo? charInfo)
    {
        isEmpty = false;
        if (charInfo is null)
        {
            charInfo = fillChar;
        }

        if (buffer.Length > x)
            buffer[x] = charInfo.Value;
    }

    internal CharInfo[] Get() => buffer;
}
