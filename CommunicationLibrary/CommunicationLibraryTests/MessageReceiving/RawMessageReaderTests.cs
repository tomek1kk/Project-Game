using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationLibrary.RawMessageProcessing;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CommunicationLibrary.RawMessageProcessing.Tests
{
    [TestClass()]
    public class RawMessageReaderTests
    {
        [TestMethod()]
        public void TestGetNextMessageAsStringCanReadSimpleString()
        {
            //given
            String expected = "Hello world";

            byte[] inputBuffer = new byte[20];

            byte[] textBytes = Encoding.UTF8.GetBytes(expected);
            byte[] lengthBytes = BitConverter.GetBytes((ushort)textBytes.Length);
            if (!BitConverter.IsLittleEndian) Array.Reverse(lengthBytes);
            Array.Copy(lengthBytes, 0, inputBuffer, 0, 2);
            Array.Copy(textBytes, 0, inputBuffer, 2, textBytes.Length);
            Stream stream = new MemoryStream(inputBuffer);

            RawMessageReader reader = new RawMessageReader(
                (buffer, count, offset) => stream.Read(buffer, offset, count));

            //when
            String actual = reader.GetNextMessageAsString();

            //then
            Assert.AreEqual(expected, actual);

            stream.Dispose();
        }

        [TestMethod()]
        public void TestGetNextMessageAsStringCanReadEmptyString()
        {
            //given
            String expected = "";

            byte[] inputBuffer = new byte[20];

            byte[] textBytes = Encoding.UTF8.GetBytes(expected);
            byte[] lengthBytes = BitConverter.GetBytes((ushort)textBytes.Length);
            if (!BitConverter.IsLittleEndian) Array.Reverse(lengthBytes);
            Array.Copy(lengthBytes, 0, inputBuffer, 0, 2);
            Array.Copy(textBytes, 0, inputBuffer, 2, textBytes.Length);
            Stream stream = new MemoryStream(inputBuffer);

            RawMessageReader reader = new RawMessageReader(
                (buffer, count, offset) => stream.Read(buffer, offset, count));

            //when
            String actual = reader.GetNextMessageAsString();

            //then
            Assert.AreEqual(expected, actual);

            stream.Dispose();
        }
        [TestMethod()]
        public void TestGetNextMessageAsStringCanReadStringOfLength8KiB()
        {
            //given
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 8192; i++)
                sb.Append("a");
            string expected = sb.ToString();

            byte[] inputBuffer = new byte[8194];

            byte[] textBytes = Encoding.UTF8.GetBytes(expected);
            byte[] lengthBytes = BitConverter.GetBytes((ushort)textBytes.Length);
            if (!BitConverter.IsLittleEndian) Array.Reverse(lengthBytes);
            Array.Copy(lengthBytes, 0, inputBuffer, 0, 2);
            Array.Copy(textBytes, 0, inputBuffer, 2, textBytes.Length);
            Stream stream = new MemoryStream(inputBuffer);

            RawMessageReader reader = new RawMessageReader(
                (buffer, count, offset) => stream.Read(buffer, offset, count));

            //when
            String actual = reader.GetNextMessageAsString();

            //then
            Assert.AreEqual(expected, actual);

            stream.Dispose();
        }
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void TestGetNextMessageAsStringThrowsExceptionOnInvalidRead()
        {
            //given

            var stream = new MemoryStream(new byte[]{ 1 });
            RawMessageReader reader = new RawMessageReader(
                (buffer, count, offset) => stream.Read(buffer, offset, count));

            //when
            String result = reader.GetNextMessageAsString();

            //then
            //throws exception

            stream.Dispose();
        }
    }
}