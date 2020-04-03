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
        public static byte ReadUInt8()
            => byte.Parse(ReadLine());
        public static bool TryReadUInt8(out byte result)
            => byte.TryParse(ReadLine(), out result);
        public static sbyte ReadInt8()
            => sbyte.Parse(ReadLine());
        public static bool TryReadInt8(out sbyte result)
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
        public static float ReadSingle()
            => float.Parse(ReadLine());
        public static bool TryReadSingle(out float result)
            => float.TryParse(ReadLine(), out result);
        public static double ReadDouble()
            => double.Parse(ReadLine());
        public static bool TryReadDouble(out double result)
            => double.TryParse(ReadLine(), out result);
        public static decimal ReadDecimal()
            => decimal.Parse(ReadLine());
        public static bool TryReadDecimal(out decimal result)
            => decimal.TryParse(ReadLine(), out result);
        public static TimeSpan ReadTimeSpan()
            => TimeSpan.Parse(ReadLine());
        public static bool TryReadTimeSpan(out TimeSpan result)
            => TimeSpan.TryParse(ReadLine(), out result);
        public static DateTime ReadDate()
            => DateTime.Parse(ReadLine());
        public static bool TryReadDate(out DateTime result)
            => DateTime.TryParse(ReadLine(), out result);
        public static DateTimeOffset ReadDateOffset()
            => DateTimeOffset.Parse(ReadLine());
        public static bool TryReadDateOffset(out DateTimeOffset result)
            => DateTimeOffset.TryParse(ReadLine(), out result);
    }
}
