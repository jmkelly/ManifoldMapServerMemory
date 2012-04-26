using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LocationSolve.LayoutServer;
using Shouldly;
using System.Diagnostics;
using M = Manifold.Interop;

namespace TestingManifoldMemory
{
    [TestFixture]
    public class Test
    {

        string PdfOutputDirectory = @"C:\VS_Projects\TestingManifoldMemory\Maps";
        string MapFileLocation = @"C:\VS_Projects\TestingManifoldMemory\Maps\Testmap1.map";
        PerformanceCounter FreeMemory;

        [TestFixtureSetUp]
        public void Init()
        {
            FreeMemory = new PerformanceCounter("Memory", "Available MBytes");
        }

        [Test]
        public void InitialTest()
        {
            true.ShouldBe(true);
        }

        [Test]
        public void FireUpMapserver()
        {
            float StartMemory = FreeMemory.NextValue();
            MapServer ms = new MapServer(MapFileLocation,"Map");
            System.Threading.Thread.Sleep(1000);
            float MidMemory = FreeMemory.NextValue();
            ms.Dispose();
            System.Threading.Thread.Sleep(5000);
            float EndMemory = FreeMemory.NextValue();
            PrintMemoryInfo(StartMemory, MidMemory, EndMemory);           
        }

        [Test]
        public void FireUpMapserverManyTimes()
        {
            int iterations = 10;
            float memSum = 0;
            float startMemSum = 0;
            float endMemSum = 0;

            for (int i = 0; i < iterations;i++)
            {
                startMemSum = startMemSum + FreeMemory.NextValue();
                MapServer ms = new MapServer(MapFileLocation, "Map");
                System.Threading.Thread.Sleep(1000);
                memSum = memSum + FreeMemory.NextValue(); 
                ms.Dispose();
                System.Threading.Thread.Sleep(1000);
                endMemSum = endMemSum + FreeMemory.NextValue();    
            }
            float StartMemory = startMemSum / iterations; 
            float MidMemory = memSum / iterations; 
            float EndMemory = endMemSum / iterations;
            PrintMemoryInfo(StartMemory, MidMemory, EndMemory);      
        }

        [Test]
        public void NoLinkedComponentsLayoutPrintTest()
        {
            PrintLayoutFromMapserver(@"C:\VS_Projects\TestingManifoldMemory\Maps\Testmap1.map", 5);
            PrintLayoutFromMapserver(@"C:\VS_Projects\TestingManifoldMemory\Maps\Testmap1.map", 10);
        }

        [Test]
        public void LinkedComponentLayoutPrintTest()
        {

            PrintLayoutFromMapserver(@"C:\VS_Projects\TestingManifoldMemory\Maps\Testmap2.map", 5);
            PrintLayoutFromMapserver(@"C:\VS_Projects\TestingManifoldMemory\Maps\Testmap2.map", 10);
        }


        public void PrintLayoutFromMapserver(string MapFilePath, int iterations)
        {
            Console.WriteLine("Test start available free memory: " + FreeMemory.NextValue().ToString() + " MB");
            float memSum = 0;
            float startMemSum = 0;
            float endMemSum = 0;

            for (int i = 0; i < iterations; i++)
            {
                startMemSum = startMemSum + FreeMemory.NextValue();
                MapServer ms = new MapServer(MapFilePath, "Map");
                M.Document doc = ms.Document;
                doc.RefreshAllLinked();
                M.Layout layout = (M.Layout)doc.ComponentSet["Layout"];
                M.ExportPdf exporter = (M.ExportPdf)doc.NewExport("PDF");
                exporter.Export((M.Component)layout, PdfOutputDirectory + @"\test.pdf", M.ConvertPrompt.PromptNone);
                System.Threading.Thread.Sleep(1000);
                memSum = memSum + FreeMemory.NextValue();
                ms.Dispose();
                System.Threading.Thread.Sleep(1000);
                endMemSum = endMemSum + FreeMemory.NextValue();
            }
            float StartMemory = startMemSum / iterations;
            float MidMemory = memSum / iterations;
            float EndMemory = endMemSum / iterations;
            PrintMemoryInfo(StartMemory, MidMemory, EndMemory);     
 
            Console.WriteLine("Test end available free memory: " + FreeMemory.NextValue().ToString() + " MB" + Environment.NewLine);
        }

        public void PrintMemoryInfo(double StartValue, double MidValue, double EndValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Average Application Memory (per MapServer Instance): " + (StartValue - MidValue).ToString() + " MB");
            sb.Append("Average Memory Not released (per MapServer Instance): " + (StartValue - EndValue).ToString() + " MB");
            Console.WriteLine(sb.ToString());
        }

    }
  
}
