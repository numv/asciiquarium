namespace console_draw;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
public record struct CharInfo
{
    [FieldOffset(0)] public CharUnion Char;
    [FieldOffset(2)] public ushort Attributes;

    public ConsoleColor ForegroundColor => (ConsoleColor)((this.Attributes & 0x0F));

    public ConsoleColor BackgroundColor => (ConsoleColor)((this.Attributes & 0xF0) >> 4);

    public CharInfo(char character, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        this.Char = new CharUnion() { UnicodeChar = character };
        this.Attributes = (ushort)((int)(foreground ?? 0) | (((ushort)(background ?? 0)) << 4));
    }

    public CharInfo(byte character, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        this.Char = new CharUnion() { AsciiChar = character };
        this.Attributes = (ushort)((int)(foreground ?? 0) | (((ushort)(background ?? 0)) << 4));
    }

    public static bool Equals(CharInfo first, CharInfo second)
    {
        return first.Char.UnicodeChar == second.Char.UnicodeChar
            && first.Char.AsciiChar == second.Char.AsciiChar
            && first.Attributes == second.Attributes;
    }
}

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
public struct CharUnion
{
    [FieldOffset(0)] public char UnicodeChar;
    [FieldOffset(0)] public byte AsciiChar;
}