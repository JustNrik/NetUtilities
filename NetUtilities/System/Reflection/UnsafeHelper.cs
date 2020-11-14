using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
    public static class UnsafeHelper
    {
        public static T PointerToStructure<T>(IntPtr pointer) where T : unmanaged
            => Unsafe.ReadUnaligned<T>(ref IntPtr<T>.GetByteReference(pointer));

        public static void StructureToPointer<T>(in IntPtr destination, T value) where T : unmanaged
            => Unsafe.WriteUnaligned(ref IntPtr<T>.GetByteReference(destination), value);

        public static ref byte GetByteReference<T>(IntPtr pointer) where T : unmanaged
            => ref IntPtr<T>.GetByteReference(pointer);

        public static ref T GetReference<T>(UIntPtr pointer) where T : unmanaged
            => ref UIntPtr<T>.GetReference(pointer);

        public static T PointerToStructure<T>(UIntPtr pointer) where T : unmanaged
            => Unsafe.ReadUnaligned<T>(ref UIntPtr<T>.GetByteReference(pointer));

        public static void StructureToPointer<T>(in UIntPtr destination, T value) where T : unmanaged
            => Unsafe.WriteUnaligned(ref UIntPtr<T>.GetByteReference(destination), value);

        public static ref byte GetByteReference<T>(UIntPtr pointer) where T : unmanaged
            => ref UIntPtr<T>.GetByteReference(pointer);

        public static ref T GetReference<T>(IntPtr pointer) where T : unmanaged
            => ref IntPtr<T>.GetReference(pointer);

        private static class IntPtr<T> where T : unmanaged
        {
            private delegate Span<T> Hack1337(IntPtr pointer);
            private static readonly Hack1337 func;

            static IntPtr()
            {
                var intPtr = Expression.Parameter(typeof(IntPtr));
                var call = Expression.Call(intPtr, typeof(IntPtr).GetMethod(nameof(IntPtr.ToPointer))!);
                var newSpan = Expression.New(typeof(Span<T>).GetConstructor(new Type[] { typeof(void*), typeof(int) })!, call, Expression.Constant(1));

                func = Expression.Lambda<Hack1337>(newSpan, intPtr).Compile();
            }

            public static Span<T> CreateSpan(IntPtr pointer)
                => func(pointer);

            public static ref byte GetByteReference(IntPtr pointer)
                => ref MemoryMarshal.GetReference(MemoryMarshal.AsBytes(func(pointer)));

            public static ref T GetReference(IntPtr pointer)
                => ref MemoryMarshal.GetReference(func(pointer));
        }
        private static class UIntPtr<T> where T : unmanaged
        {
            private delegate Span<T> Hack1337(UIntPtr pointer);
            private static readonly Hack1337 func;

            static UIntPtr()
            {
                var uIntPtr = Expression.Parameter(typeof(UIntPtr));
                var call = Expression.Call(uIntPtr, typeof(UIntPtr).GetMethod(nameof(UIntPtr.ToPointer))!);
                var newSpan = Expression.New(typeof(Span<T>).GetConstructor(new Type[] { typeof(void*), typeof(int) })!, call, Expression.Constant(1));

                func = Expression.Lambda<Hack1337>(newSpan, uIntPtr).Compile();
            }

            public static Span<T> CreateSpan(UIntPtr pointer)
                => func(pointer);

            public static ref byte GetByteReference(UIntPtr pointer)
                => ref MemoryMarshal.GetReference(MemoryMarshal.AsBytes(CreateSpan(pointer)));

            public static ref T GetReference(UIntPtr pointer)
                => ref MemoryMarshal.GetReference(func(pointer));
        }
    }
}
