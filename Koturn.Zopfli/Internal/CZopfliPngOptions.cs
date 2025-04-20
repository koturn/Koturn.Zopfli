#if NET7_0_OR_GREATER
#    define RUNTIME_MARSHALLING_DISABLED
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Koturn.Zopfli.Enums;


namespace Koturn.Zopfli.Internal
{
    /// <summary>
    /// <para>Option value for zopflipng.</para>
    /// <para>This structure is used to interact with zopflipng.dll.</para>
    /// </summary>
    /// <remarks>
    /// This structure is used for following P/Invoke methods.
    /// <list type="bullet">
    ///   <item><seealso cref="ZopfliPng.SafeNativeMethods.CZopfliPngSetDefaults(out CZopfliPngOptions)"/></item>
    ///   <item><seealso cref="ZopfliPng.SafeNativeMethods.CZopfliPngOptimize(IntPtr, UIntPtr, in CZopfliPngOptions, bool, out MallocedMemoryHandle, out UIntPtr)"/></item>
    /// </list>
    /// </remarks>
    /// <remarks>
    /// Primary ctor: Create option instance for zopflipng with default parameters.
    /// </remarks>
    /// <param name="lossyTransparent">Allow altering hidden colors of fully transparent pixels.</param>
    /// <param name="lossy8bit">Convert 16-bit per channel images to 8-bit per channel.</param>
    /// <param name="autoFilterStrategy">Automatically choose filter strategy using less good compression.</param>
    /// <param name="keepColorType">Keep original color type (RGB, RGBA, gray, gray+alpha or palette) and bit depth of the PNG.</param>
    /// <param name="useZopfli">Use Zopfli deflate compression.</param>
    /// <param name="numIterations">Zopfli number of iterations.</param>
    /// <param name="numIterationsLarge">Zopfli number of iterations on large images.</param>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CZopfliPngOptions(
        bool lossyTransparent = CZopfliPngOptions.DefaultLossyTransparent,
        bool lossy8bit = CZopfliPngOptions.DefaultLossy8bit,
        bool autoFilterStrategy = CZopfliPngOptions.DefaultAutoFilterStrategy,
        bool keepColorType = CZopfliPngOptions.DefaultKeepColorType,
        bool useZopfli = CZopfliPngOptions.DefaultUseZopfli,
        int numIterations = CZopfliPngOptions.DefaultNumIterations,
        int numIterationsLarge = CZopfliPngOptions.DefaultNumIterationsLarge) : IDisposable
    {
        /// <summary>
        /// Default value for <see cref="LossyTransparent"/>.
        /// </summary>
        public const bool DefaultLossyTransparent = false;
        /// <summary>
        /// Default value for <see cref="Lossy8bit"/>.
        /// </summary>
        public const bool DefaultLossy8bit = false;
        /// <summary>
        /// Default value for <see cref="AutoFilterStrategy"/>.
        /// </summary>
        public const bool DefaultAutoFilterStrategy = true;
        /// <summary>
        /// Default value for <see cref="KeepColorType"/>.
        /// </summary>
        public const bool DefaultKeepColorType = false;
        /// <summary>
        /// Default value for <see cref="UseZopfli"/>.
        /// </summary>
        public const bool DefaultUseZopfli = true;
        /// <summary>
        /// Default value for <see cref="NumIterations"/>.
        /// </summary>
        public const int DefaultNumIterations = 15;
        /// <summary>
        /// Default value for <see cref="NumIterationsLarge"/>.
        /// </summary>
        public const int DefaultNumIterationsLarge = 5;


        /// <summary>
        /// Allow altering hidden colors of fully transparent pixels.
        /// </summary>
#if RUNTIME_MARSHALLING_DISABLED
        public bool LossyTransparent
        {
            readonly get => _lossyTransparent != 0;
            set => _lossyTransparent = value ? 1 : 0;
        }
        /// <summary>
        /// Actual value of <see cref="LossyTransparent"/>.
        /// </summary>
        private int _lossyTransparent = lossyTransparent ? 1 : 0;
#else
        [field: MarshalAs(UnmanagedType.Bool)]
        public bool LossyTransparent { get; set; } = lossyTransparent;
#endif  // RUNTIME_MARSHALLING_DISABLED
        /// <summary>
        /// Convert 16-bit per channel images to 8-bit per channel.
        /// </summary>
#if RUNTIME_MARSHALLING_DISABLED
        public bool Lossy8bit
        {
            readonly get => _lossy8bit != 0;
            set => _lossy8bit = value ? 1 : 0;
        }
        /// <summary>
        /// Actual value of <see cref="Lossy8bit"/>.
        /// </summary>
        private int _lossy8bit = lossy8bit ? 1 : 0;
#else
        [field: MarshalAs(UnmanagedType.Bool)]
        public bool Lossy8bit { get; set; } = lossy8bit;
#endif  // RUNTIME_MARSHALLING_DISABLED
        /// <summary>
        /// Filter strategies to try.
        /// </summary>
        public IntPtr FilterStrategiesPointer { get; private set; } = IntPtr.Zero;
        /// <summary>
        /// How many strategies to try.
        /// </summary>
        public int NumFilterStrategies { get; private set; } = 0;
        /// <summary>
        /// Automatically choose filter strategy using less good compression.
        /// </summary>
#if RUNTIME_MARSHALLING_DISABLED
        public bool AutoFilterStrategy
        {
            readonly get => _autoFilterStrategy != 0;
            set => _autoFilterStrategy = value ? 1 : 0;
        }
        /// <summary>
        /// Actual value of <see cref="AutoFilterStrategy"/>.
        /// </summary>
        private int _autoFilterStrategy = autoFilterStrategy ? 1 : 0;
#else
        [field: MarshalAs(UnmanagedType.Bool)]
        public bool AutoFilterStrategy { get; set; } = autoFilterStrategy;
#endif  // RUNTIME_MARSHALLING_DISABLED
        /// <summary>
        /// Keep original color type (RGB, RGBA, gray, gray+alpha or palette) and bit depth of the PNG.
        /// </summary>
#if RUNTIME_MARSHALLING_DISABLED
        public bool KeepColorType
        {
            readonly get => _keepColorType != 0;
            set => _keepColorType = value ? 1 : 0;
        }
        /// <summary>
        /// Actual value of <see cref="KeepColorType"/>.
        /// </summary>
        private int _keepColorType = keepColorType ? 1 : 0;
#else
        [field: MarshalAs(UnmanagedType.Bool)]
        public bool KeepColorType { get; set; } = keepColorType;
#endif  // RUNTIME_MARSHALLING_DISABLED
        /// <summary>
        /// <para>PNG chunks to keep</para>
        /// <para>chunks to literally copy over from the original PNG to the resulting one.</para>
        /// </summary>
        public IntPtr KeepChunksPointer { get; private set; } = IntPtr.Zero;
        /// <summary>
        /// How many entries in keepchunks.
        /// </summary>
        public int NumKeepChunks { get; private set; } = 0;
        /// <summary>
        /// Use Zopfli deflate compression.
        /// </summary>
#if RUNTIME_MARSHALLING_DISABLED
        public bool UseZopfli
        {
            readonly get => _useZopfli != 0;
            set => _useZopfli = value ? 1 : 0;
        }
        /// <summary>
        /// Actual value of <see cref="UseZopfli"/>.
        /// </summary>
        private int _useZopfli = useZopfli ? 1 : 0;
#else
        [field: MarshalAs(UnmanagedType.Bool)]
        public bool UseZopfli { get; set; } = useZopfli;
#endif  // RUNTIME_MARSHALLING_DISABLED
        /// <summary>
        /// Zopfli number of iterations.
        /// </summary>
        public int NumIterations { get; set; } = numIterations;
        /// <summary>
        /// Zopfli number of iterations on large images.
        /// </summary>
        public int NumIterationsLarge { get; set; } = numIterationsLarge;
        /// <summary>
        /// Unused, left for backwards compatiblity.
        /// </summary>
        private readonly int _blockSplitStrategy = 0;


        /// <summary>
        /// Create option instance from <see cref="ZopfliPngOptions"/>.
        /// </summary>
        /// <param name="options">Instance of <see cref="ZopfliPngOptions"/>.</param>
        public CZopfliPngOptions(ZopfliPngOptions options)
            : this(
                options.LossyTransparent,
                options.Lossy8bit,
                options.AutoFilterStrategy,
                options.KeepColorType,
                options.UseZopfli,
                options.NumIterations,
                options.NumIterationsLarge)
        {
            SetFilterStrategies(options.FilterStrategies);
            SetKeepChunks(options.KeepChunks);
        }


        /// <summary>
        /// Set <see cref="FilterStrategiesPointer"/> and <see cref="NumFilterStrategies"/> from a list.
        /// </summary>
        /// <param name="filterStrategies">List of filter strategies.</param>
        public void SetFilterStrategies(List<ZopfliPngFilterStrategy> filterStrategies)
        {
            DisposeFilterStrategies();
            (FilterStrategiesPointer, NumFilterStrategies) = CreateFilterStrategies(filterStrategies);
        }


        /// <summary>
        /// Set <see cref="KeepChunksPointer"/> and <see cref="NumKeepChunks"/> from a list.
        /// </summary>
        /// <param name="keepChunks">List of chunk names.</param>
        public void SetKeepChunks(List<string> keepChunks)
        {
            DisposeKeepChunks();
            (KeepChunksPointer, NumKeepChunks) = CreateKeepChunks(keepChunks);
        }


        /// <summary>
        /// <para>Free the allocated memory for <see cref="FilterStrategiesPointer"/> and set to <see cref="IntPtr.Zero"/>.</para>
        /// <para>Similarly, set <see cref="NumFilterStrategies"/> to 0.</para>
        /// </summary>
        public void DisposeFilterStrategies()
        {
            if (FilterStrategiesPointer != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(FilterStrategiesPointer);
                FilterStrategiesPointer = IntPtr.Zero;
            }
            NumFilterStrategies = 0;
        }


        /// <summary>
        /// <para>Free the allocated memory for <see cref="KeepChunksPointer"/> and set to <see cref="IntPtr.Zero"/>.</para>
        /// <para>Similarly, set <see cref="NumKeepChunks"/> to 0.</para>
        /// </summary>
        public void DisposeKeepChunks()
        {
            if (KeepChunksPointer != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(KeepChunksPointer);
                KeepChunksPointer = IntPtr.Zero;
            }
            NumKeepChunks = 0;
        }


        /// <summary>
        /// Dispose resource of <see cref="FilterStrategiesPointer"/>.
        /// </summary>
        public void Dispose()
        {
            DisposeFilterStrategies();
            DisposeKeepChunks();
        }


        /// <summary>
        /// Get default option value.
        /// </summary>
        /// <returns>Default option value.</returns>
        public static CZopfliPngOptions GetDefault()
        {
            ZopfliPng.SafeNativeMethods.CZopfliPngSetDefaults(out var cPngOptions);
            return cPngOptions;
        }


        /// <summary>
        /// Allocate and write filter strategies data.
        /// </summary>
        /// <param name="filterStrategies">List of filter strategies.</param>
        /// <returns>Tuple of pointer to the filter strategies and the number of them.</returns>
        private static (IntPtr FilterStrategiesPointer, int NumFilterStrategies) CreateFilterStrategies(List<ZopfliPngFilterStrategy> filterStrategies)
        {
            if (filterStrategies is null || filterStrategies.Count == 0)
            {
                return (IntPtr.Zero, 0);
            }

            var filterStrategiesCount = filterStrategies.Count;
            var filterStrategiesPointer = Marshal.AllocCoTaskMem(sizeof(ZopfliPngFilterStrategy) * filterStrategiesCount);
            var numFilterStrategies = filterStrategies.Count;

            unsafe
            {
                var p = (ZopfliPngFilterStrategy*)filterStrategiesPointer;
                for (int i = 0; i < filterStrategiesCount; i++)
                {
                    p[i] = filterStrategies[i];
                }
            }

            return (filterStrategiesPointer, numFilterStrategies);
        }


        /// <summary>
        /// Allocate and write chunk names to keep.
        /// </summary>
        /// <param name="keepChunks">List of chunk names.</param>
        /// <returns>Tuple of pointer to the chunk names and the number of them.</returns>
        private static (IntPtr KeepChunksPointer, int NumKeepChunks) CreateKeepChunks(List<string> keepChunks)
        {
            if (keepChunks is null || keepChunks.Count == 0)
            {
                return (IntPtr.Zero, 0);
            }

            unsafe
            {
                var memorySize = keepChunks.Aggregate(0, (acc, keepChunk) => acc + (keepChunk.Length + 1) * sizeof(char));

                // Memory:
                //   p_i: Pointer to c_i.
                //   c_i: Null-terminate string.
                //   [p_0][p_1][p_2]...[p_n][c_0][c_1][c_2]...[c_n]
                var keepChunksCount = keepChunks.Count;
                var keepChunksPointer = Marshal.AllocCoTaskMem(keepChunksCount * sizeof(byte*) + memorySize);

                var p = (byte**)keepChunksPointer;
                var q = (byte*)&p[keepChunksCount];
                for (int i = 0; i < keepChunksCount; i++)
                {
                    p[i] = q;
                    foreach (var c in Encoding.ASCII.GetBytes(keepChunks[i]))
                    {
                        *q++ = c;
                    }
                    *q++ = 0;  // Null-terminate
                }

                return (keepChunksPointer, keepChunksCount);
            }
        }
    }
}
