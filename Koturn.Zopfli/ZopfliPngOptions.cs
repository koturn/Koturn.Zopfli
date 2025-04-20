using System;
using System.Collections.Generic;
using Koturn.Zopfli.Enums;
using Koturn.Zopfli.Internal;


namespace Koturn.Zopfli
{
    /// <summary>
    /// Structure of options for zopflipng.
    /// </summary>
    /// <remarks>
    /// Primary ctor: Create option instance for zopflipng.
    /// </remarks>
    /// <param name="lossyTransparent">Allow altering hidden colors of fully transparent pixels.</param>
    /// <param name="lossy8bit">Convert 16-bit per channel images to 8-bit per channel.</param>
    /// <param name="filterStrategies">Filter strategies to try.</param>
    /// <param name="autoFilterStrategy">Automatically choose filter strategy using less good compression.</param>
    /// <param name="keepColorType">Keep original color type (RGB, RGBA, gray, gray+alpha or palette) and bit depth of the PNG.</param>
    /// <param name="keepChunks">PNG chunks to keep.</param>
    /// <param name="useZopfli">Use Zopfli deflate compression.</param>
    /// <param name="numIterations">Zopfli number of iterations.</param>
    /// <param name="numIterationsLarge">Zopfli number of iterations on large images.</param>
    public class ZopfliPngOptions(
        bool lossyTransparent = ZopfliPngOptions.DefaultLossyTransparent,
        bool lossy8bit = ZopfliPngOptions.DefaultLossy8bit,
        List<ZopfliPngFilterStrategy>? filterStrategies = null,
        bool autoFilterStrategy = ZopfliPngOptions.DefaultAutoFilterStrategy,
        bool keepColorType = ZopfliPngOptions.DefaultKeepColorType,
        List<string>? keepChunks = null,
        bool useZopfli = ZopfliPngOptions.DefaultUseZopfli,
        int numIterations = ZopfliPngOptions.DefaultNumIterations,
        int numIterationsLarge = ZopfliPngOptions.DefaultNumIterationsLarge)
    {
        /// <summary>
        /// Default value for <see cref="LossyTransparent"/>.
        /// </summary>
        public const bool DefaultLossyTransparent = CZopfliPngOptions.DefaultLossyTransparent;
        /// <summary>
        /// Default value for <see cref="Lossy8bit"/>.
        /// </summary>
        public const bool DefaultLossy8bit = CZopfliPngOptions.DefaultLossy8bit;
        /// <summary>
        /// Default value for <see cref="AutoFilterStrategy"/>.
        /// </summary>
        public const bool DefaultAutoFilterStrategy = CZopfliPngOptions.DefaultAutoFilterStrategy;
        /// <summary>
        /// Default value for <see cref="KeepColorType"/>.
        /// </summary>
        public const bool DefaultKeepColorType = CZopfliPngOptions.DefaultKeepColorType;
        /// <summary>
        /// Default value for <see cref="UseZopfli"/>.
        /// </summary>
        public const bool DefaultUseZopfli = CZopfliPngOptions.DefaultUseZopfli;
        /// <summary>
        /// Default value for <see cref="NumIterations"/>.
        /// </summary>
        public const int DefaultNumIterations = CZopfliPngOptions.DefaultNumIterations;
        /// <summary>
        /// Default value for <see cref="NumIterationsLarge"/>.
        /// </summary>
        public const int DefaultNumIterationsLarge = CZopfliPngOptions.DefaultNumIterationsLarge;


        /// <summary>
        /// Allow altering hidden colors of fully transparent pixels.
        /// </summary>
        public bool LossyTransparent { get; set; } = lossyTransparent;
        /// <summary>
        /// Convert 16-bit per channel images to 8-bit per channel.
        /// </summary>
        public bool Lossy8bit { get; set; } = lossy8bit;
        /// <summary>
        /// Filter strategies to try.
        /// </summary>
        public List<ZopfliPngFilterStrategy> FilterStrategies { get; } = filterStrategies ?? [];
        /// <summary>
        /// Automatically choose filter strategy using less good compression.
        /// </summary>
        public bool AutoFilterStrategy { get; set; } = autoFilterStrategy;
        /// <summary>
        /// Keep original color type (RGB, RGBA, gray, gray+alpha or palette) and bit depth of the PNG.
        /// </summary>
        public bool KeepColorType { get; set; } = keepColorType;
        /// <summary>
        /// <para>PNG chunks to keep.</para>
        /// <para>Chunks to literally copy over from the original PNG to the resulting one.</para>
        /// </summary>
        public List<string> KeepChunks { get; } = keepChunks ?? [];
        /// <summary>
        /// Use Zopfli deflate compression.
        /// </summary>
        public bool UseZopfli { get; set; } = useZopfli;
        /// <summary>
        /// Zopfli number of iterations.
        /// </summary>
        public int NumIterations { get; set; } = numIterations;
        /// <summary>
        /// Zopfli number of iterations on large images.
        /// </summary>
        public int NumIterationsLarge { get; set; } = numIterationsLarge;


        /// <summary>
        /// Create option instance for zopflipng.
        /// </summary>
        /// <param name="lossyTransparent">Allow altering hidden colors of fully transparent pixels.</param>
        /// <param name="lossy8bit">Convert 16-bit per channel images to 8-bit per channel.</param>
        /// <param name="autoFilterStrategy">Automatically choose filter strategy using less good compression.</param>
        /// <param name="keepColorType">Keep original color type (RGB, RGBA, gray, gray+alpha or palette) and bit depth of the PNG.</param>
        /// <param name="useZopfli">Use Zopfli deflate compression.</param>
        /// <param name="numIterations">Zopfli number of iterations.</param>
        /// <param name="numIterationsLarge">Zopfli number of iterations on large images.</param>
        public ZopfliPngOptions(
            bool lossyTransparent = DefaultLossyTransparent,
            bool lossy8bit = DefaultLossy8bit,
            bool autoFilterStrategy = DefaultAutoFilterStrategy,
            bool keepColorType = DefaultKeepColorType,
            bool useZopfli = DefaultUseZopfli,
            int numIterations = DefaultNumIterations,
            int numIterationsLarge = DefaultNumIterationsLarge)
            : this(lossyTransparent, lossy8bit, null, autoFilterStrategy, keepColorType, null, useZopfli, numIterations, numIterationsLarge)
        {
        }


        /// <summary>
        /// Create option instance from <see cref="CZopfliPngOptions"/>.
        /// </summary>
        /// <param name="cPngOptions">Allow altering hidden colors of fully transparent pixels.</param>
        internal ZopfliPngOptions(in CZopfliPngOptions cPngOptions)
            : this(
                cPngOptions.LossyTransparent,
                cPngOptions.Lossy8bit,
                CreateFilterStrategies(cPngOptions.FilterStrategiesPointer, cPngOptions.NumFilterStrategies),
                cPngOptions.AutoFilterStrategy,
                cPngOptions.KeepColorType,
                CreateKeepChunks(cPngOptions.KeepChunksPointer, cPngOptions.NumKeepChunks),
                cPngOptions.UseZopfli,
                cPngOptions.NumIterations,
                cPngOptions.NumIterationsLarge)
        {
        }


        /// <summary>
        /// Get default option value from zopflipng.dll.
        /// </summary>
        /// <returns>Default option value.</returns>
        public static ZopfliPngOptions GetDefault()
        {
            using (var cPngOptions = CZopfliPngOptions.GetDefault())
            {
                return new ZopfliPngOptions(cPngOptions);
            }
        }


        /// <summary>
        /// Create filter strategy list from unmanaged memory in <see cref="CZopfliPngOptions"/>.
        /// </summary>
        /// <param name="filterStrategiesPointer">Unmanaged memory pointer to filter strategies. (<see cref="CZopfliPngOptions.FilterStrategiesPointer"/>)</param>
        /// <param name="numFilterStrategies">Number of filter strategies. (Unmanaged memory pointer of <see cref="CZopfliPngOptions.NumFilterStrategies"/>)</param>
        /// <returns>Created filter strategy list.</returns>
        private static List<ZopfliPngFilterStrategy> CreateFilterStrategies(IntPtr filterStrategiesPointer, int numFilterStrategies)
        {
            if (filterStrategiesPointer == IntPtr.Zero || numFilterStrategies == 0)
            {
                return [];
            }

            var filterStrategies = new List<ZopfliPngFilterStrategy>(numFilterStrategies);
            unsafe
            {
                var pFilterStrategies = (ZopfliPngFilterStrategy*)filterStrategiesPointer;
                for (int i = 0; i < numFilterStrategies; i++)
                {
                    filterStrategies.Add(pFilterStrategies[i]);
                }
            }

            return filterStrategies;
        }


        /// <summary>
        /// Create keep chunks list from unmanaged memory in <see cref="CZopfliPngOptions"/>.
        /// </summary>
        /// <param name="keepChunksPointer">Unmanaged memory pointer to keep chunks. (<see cref="CZopfliPngOptions.KeepChunksPointer"/>)</param>
        /// <param name="numKeepChunks">Number of keep chunks. (<see cref="CZopfliPngOptions.NumKeepChunks"/>)</param>
        /// <returns>Created keep chunks list.</returns>
        private static List<string> CreateKeepChunks(IntPtr keepChunksPointer, int numKeepChunks)
        {
            if (keepChunksPointer == IntPtr.Zero || numKeepChunks == 0)
            {
                return [];
            }

            var keepChunks = new List<string>(numKeepChunks);
            unsafe
            {
                var pKeepChunks = (sbyte**)keepChunksPointer;
                for (int i = 0; i < numKeepChunks; i++)
                {
                    keepChunks.Add(new string(pKeepChunks[i]));
                }
            }

            return keepChunks;
        }
    }
}
