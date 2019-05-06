using static System.Console;

namespace System
{
    public static class ConsoleUtilities
    {
        public static int ReadInt32()
            => int.Parse(ReadLine());

        public static bool TryReadInt32(out int result)
            => int.TryParse(ReadLine(), out result);

        public static long ReadInt64()
            => long.Parse(ReadLine());

        public static bool TryReadInt64(out long result)
            => long.TryParse(ReadLine(), out result);

        public static short ReadInt16()
            => short.Parse(ReadLine());

        public static bool TryReadInt16(out short result)
            => short.TryParse(ReadLine(), out result);

        public static byte ReadByte()
            => byte.Parse(ReadLine());

        public static bool TryReadByte(out byte result)
            => byte.TryParse(ReadLine(), out result);

        public static sbyte ReadSByte()
            => sbyte.Parse(ReadLine());

        public static bool TryReadSByte(out sbyte result)
            => sbyte.TryParse(ReadLine(), out result);

        public static ushort ReadUInt16()
            => ushort.Parse(ReadLine());

        public static bool TryReadUInt16(out ushort result)
            => ushort.TryParse(ReadLine(), out result);

        public static uint ReadUInt32()
            => uint.Parse(ReadLine());

        public static bool TryReadUInt32(out uint result)
            => uint.TryParse(ReadLine(), out result);

        public static ulong ReadUInt64()
            => ulong.Parse(ReadLine());

        public static bool TryReadUInt64(out ulong result)
            => ulong.TryParse(ReadLine(), out result);

        public static bool ReadBoolean()
            => bool.Parse(ReadLine());

        public static bool TryReadBoolean(out bool result)
            => bool.TryParse(ReadLine(), out result);
    }
}
