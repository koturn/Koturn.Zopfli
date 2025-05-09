using System;
using System.Runtime.CompilerServices;
#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif  // NETCOREAPP3_0_OR_GREATER


namespace Koturn.Zopfli.Checksums
{
    /// <summary>
    /// CRC-32 calculation class.
    /// </summary>
    public static class Crc32Util
    {
        /// <summary>
        /// Initial value of CRC-32.
        /// </summary>
        public const uint InitialValue = 0xffffffff;
        /// <summary>
        /// Generator Polynomial of CRC-32.
        /// </summary>
        private const uint Polynomial = 0xedb88320U;

        /// <summary>
        /// Cache of CRC-32 table.
        /// </summary>
        private static uint[]? _table;

        /// <summary>
        /// Compute CRC-32 value.
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <returns>CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Compute(byte[] buf)
        {
            return Compute(buf.AsSpan());
        }

        /// <summary>
        /// Compute CRC-32 value.
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <param name="offset">Offset of <paramref name="buf"/>.</param>
        /// <param name="count">Data count of <paramref name="buf"/>.</param>
        /// <returns>CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Compute(byte[] buf, int offset, int count)
        {
            return Compute(buf.AsSpan(offset, count));
        }

        /// <summary>
        /// Compute CRC-32 value.
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <returns>CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Compute(ReadOnlySpan<byte> buf)
        {
            return Finalize(Update(buf));
        }

        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Update(byte[] buf, uint crc = InitialValue)
        {
            return Update(buf.AsSpan(), crc);
        }

        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <param name="offset">Offset of <paramref name="buf"/>.</param>
        /// <param name="count">Data count of <paramref name="buf"/>.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Update(byte[] buf, int offset, int count, uint crc = InitialValue)
        {
            return Update(buf.AsSpan(offset, count), crc);
        }

        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Update(ReadOnlySpan<byte> buf, uint crc = InitialValue)
        {
#if NETCOREAPP3_0_OR_GREATER
            // This method call will be repplaced by calling UpdateSse41() or UpdateNaive() at JIT compiling time.
            return Sse41.IsSupported && Pclmulqdq.IsSupported ? UpdateSse41(buf, crc) : UpdateNaive(buf, crc);
#else
            return UpdateNaive(buf, crc);
#endif  // NETCOREAPP3_0_OR_GREATER
        }

        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// </summary>
        /// <param name="x">A value of <see cref="byte"/>.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Update(byte x, uint crc = InitialValue)
        {
            return GetTable()[(byte)crc ^ x] ^ (crc >> 8);
        }

        /// <summary>
        /// Calculate CRC-32 value from intermidiate CRC-32 value.
        /// </summary>
        /// <param name="crc">Intermidiate CRC-32 value</param>
        /// <returns>CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Finalize(uint crc)
        {
            return ~crc;
        }


        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        public static uint UpdateNaive(ReadOnlySpan<byte> buf, uint crc = InitialValue)
        {
            var crcTable = GetTable();

            foreach (var x in buf)
            {
                crc = crcTable[(byte)crc ^ x] ^ (crc >> 8);
            }

            return crc;
        }

#if NETCOREAPP3_0_OR_GREATER
        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// <para>This method is implemented with SSE4.1 and PCLMULQDQ.</para>
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        /// <remarks>
        /// <para><seealso href="https://www.intel.com/content/dam/www/public/us/en/documents/white-papers/fast-crc-computation-generic-polynomials-pclmulqdq-paper.pdf"/></para>
        /// <para><seealso href="https://chromium.googlesource.com/chromium/src/+/master/third_party/zlib/crc32_simd.c"/></para>
        /// </remarks>
        private static uint UpdateSse41(ReadOnlySpan<byte> buf, uint crc = InitialValue)
        {
            var len = buf.Length;
            if (len < 64)
            {
                return UpdateNaive(buf, crc);
            }

            unsafe
            {
                fixed (byte* pp = buf)
                {
                    var p = pp;
                    var x1 = Sse2.Xor(
                        Sse2.LoadVector128(p).AsUInt32(),
                        Sse2.ConvertScalarToVector128UInt32(crc));
                    var x2 = Sse2.LoadVector128(p + 16).AsUInt32();
                    var x3 = Sse2.LoadVector128(p + 32).AsUInt32();
                    var x4 = Sse2.LoadVector128(p + 48).AsUInt32();

                    p += 64;
                    len -= 64;

                    // Parallel fold blocks of 64, if any.
                    var k1k2 = Vector128.Create(0x0000000154442bd4U, 0x00000001c6e41596U);
                    for (;  len >= 64; len -= 64)
                    {
                        x1 = Sse2.Xor(
                            Sse2.LoadVector128(p).AsUInt32(),
                            Sse2.Xor(
                                Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k1k2, 0x11),
                                Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k1k2, 0x00)).AsUInt32());
                        x2 = Sse2.Xor(
                            Sse2.LoadVector128(p + 16).AsUInt32(),
                            Sse2.Xor(
                                Pclmulqdq.CarrylessMultiply(x2.AsUInt64(), k1k2, 0x11),
                                Pclmulqdq.CarrylessMultiply(x2.AsUInt64(), k1k2, 0x00)).AsUInt32());
                        x3 = Sse2.Xor(
                            Sse2.LoadVector128(p + 32).AsUInt32(),
                            Sse2.Xor(
                                Pclmulqdq.CarrylessMultiply(x3.AsUInt64(), k1k2, 0x11),
                                Pclmulqdq.CarrylessMultiply(x3.AsUInt64(), k1k2, 0x00)).AsUInt32());
                        x4 = Sse2.Xor(
                            Sse2.LoadVector128(p + 48).AsUInt32(),
                            Sse2.Xor(
                                Pclmulqdq.CarrylessMultiply(x4.AsUInt64(), k1k2, 0x11),
                                Pclmulqdq.CarrylessMultiply(x4.AsUInt64(), k1k2, 0x00)).AsUInt32());
                        p += 64;
                    }

                    // Fold into 128-bits.
                    var k3k4 = Vector128.Create(0x00000001751997d0U, 0x00000000ccaa009eU);
                    x1 = Sse2.Xor(
                        Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k3k4, 0x00).AsUInt32(),
                        Sse2.Xor(
                            x2,
                            Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k3k4, 0x11).AsUInt32()));
                    x1 = Sse2.Xor(
                        Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k3k4, 0x00).AsUInt32(),
                        Sse2.Xor(
                            x3,
                            Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k3k4, 0x11).AsUInt32()));
                    x1 = Sse2.Xor(
                        Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k3k4, 0x00).AsUInt32(),
                        Sse2.Xor(
                            x4,
                            Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k3k4, 0x11).AsUInt32()));

                    // Single fold blocks of 16, if any.
                    for (; len >= 16; len -= 16)
                    {
                        x1 = Sse2.Xor(
                            Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k3k4, 0x00).AsUInt32(),
                            Sse2.Xor(
                                Sse2.LoadVector128((uint*)p),
                                Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k3k4, 0x11).AsUInt32()));
                        p += 16;
                    }

                    // Fold 128-bits to 64-bits.
                    var bwaFactor = Vector128.Create(0xffffffffU, 0x00000000U, 0xffffffffU, 0x00000000U);
                    x1 = Sse2.Xor(
                        Sse2.ShiftRightLogical128BitLane(x1, 8),
                        Pclmulqdq.CarrylessMultiply(x1.AsUInt64(), k3k4, 0x10).AsUInt32());
                    x1 = Sse2.Xor(
                        Sse2.ShiftRightLogical128BitLane(x1, 4),
                        Pclmulqdq.CarrylessMultiply(
                            Sse2.And(x1, bwaFactor).AsUInt64(),
                            Vector128.CreateScalar(0x0000000163cd6124U),
                            0x00).AsUInt32());

                    // Barret reduce to 32-bits.
                    var poly = Vector128.Create(0x00000001db710641U, 0x0000001f7011641U);
                    crc = Sse41.Extract(
                        Sse2.Xor(
                            x1,
                            Pclmulqdq.CarrylessMultiply(
                                poly,
                                Sse2.And(
                                    bwaFactor,
                                    Pclmulqdq.CarrylessMultiply(
                                        poly,
                                        Sse2.And(
                                            x1,
                                            bwaFactor).AsUInt64(),
                                        0x01).AsUInt32()).AsUInt64(),
                                0x00).AsUInt32()),
                        1);
                }
            }

            return len == 0 ? crc : UpdateNaive(buf[^len..], crc);
        }
#endif  // NETCOREAPP3_0_OR_GREATER

        /// <summary>
        /// <para>Get CRC-32 table cache.</para>
        /// <para>If the cache is not generated, generate and return it.</para>
        /// </summary>
        /// <returns>CRC-32 table</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint[] GetTable()
        {
            return _table ??= GenerateTable();
        }

        /// <summary>
        /// Generate CRC-32 value.
        /// This method only used in <see cref="GetTable"/>.
        /// </summary>
        /// <returns>CRC-32 table.</returns>
        /// <remarks>
        /// <see href="https://create.stephan-brumme.com/crc32/"/>
        /// </remarks>
        private static uint[] GenerateTable()
        {
            var crcTable = new uint[256];

            for (int n = 0; n < crcTable.Length; n++)
            {
                var c = (uint)n;
                for (var k = 0; k < 8; k++)
                {
                    c = (c >> 1) ^ ((uint)-(int)(c & 1) & Polynomial);
                }
                crcTable[n] = c;
            }

            return crcTable;
        }
    }
}
