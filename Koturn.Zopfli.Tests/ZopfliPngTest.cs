using System;
using System.Diagnostics;
using System.IO;
using Xunit.Extensions.AssemblyFixture;


namespace Koturn.Zopfli.Tests
{
    /// <summary>
    /// Test class for <see cref="ZopfliPng"/>.
    /// </summary>
    public class ZopfliPngTest : IAssemblyFixture<UnitTestAssemblyFixture>
    {
        /// <summary>
        /// Test for <see cref="ZopfliPng.OptimizePng(byte[])"/>
        /// </summary>
        [Fact]
        public void OptimizeTest01()
        {
            const string filePath = @"TestData\lena.png";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            var recompressedData = ZopfliPng.OptimizePng(data);

            Console.WriteLine($"Optimize {filePath}: Original=[{data.Length}]Bytes Compressed=[{recompressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");
        }

        /// <summary>
        /// Test for <see cref="ZopfliPng.OptimizePng(byte[], ZopfliPngOptions)"/>
        /// </summary>
        [Fact]
        public void OptimizeTest02()
        {
            const string filePath = @"TestData\lena.png";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            var opt = new ZopfliPngOptions(true, true, true, true, false, 30, 30);
            var recompressedData = ZopfliPng.OptimizePng(data, opt);

            Console.WriteLine($"Optimize {filePath}: Original=[{data.Length}]Bytes Compressed=[{recompressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");
        }

        /// <summary>
        /// Test for <see cref="ZopfliPng.OptimizePngUnmanaged(byte[])"/>
        /// </summary>
        [Fact]
        public void OptimizeUnmanagedTest01()
        {
            const string filePath = @"TestData\lena.png";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            var recompressedData = ZopfliPng.OptimizePngUnmanaged(data);

            Console.WriteLine($"Optimize {filePath}: Original=[{data.Length}]Bytes Compressed=[{recompressedData.ByteLength}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");
        }

        /// <summary>
        /// Test for <see cref="ZopfliPng.OptimizePngUnmanaged(byte[])"/>
        /// </summary>
        [Fact]
        public void OptimizeWithStreamTest()
        {
            const string filePath = @"TestData\lena.png";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            using (var oms = new MemoryStream())
            {
                using (var zs = new ZopfliPngStream(oms))
                using (var ims = new MemoryStream(data))
                {
                    ims.CopyTo(zs);
                }
                var recompressedData = oms.ToArray();

                Console.WriteLine($"Optimize {filePath}: Original=[{data.Length}]Bytes Compressed=[{recompressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");
            }
        }
    }
}
