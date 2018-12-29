namespace System
{
    public static partial class Utilities
    {
        /// <summary>
        /// Boxes the object without casting overhead.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object Box(object obj) => obj;

        /// <summary>
        /// Unboxes the object with a soft cast. unboxing with a direct cast has more overhead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Unbox<T>(object obj) where T : class 
            => obj as T;

        /// <summary>
        /// Unboxes the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="_"></param>
        /// <returns></returns>
        public static T Unbox<T>(object obj, object _ = null) where T : struct
            => (T)obj;

        /// <summary>
        /// Returns the same object with the type of a higher class in its hierachy avoiding casting overhead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Cast<T>(T obj) => obj;
    }
}
