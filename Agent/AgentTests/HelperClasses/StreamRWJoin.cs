using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AgentTests.HelperClasses
{
    //required to simulate network stream in tests
    //combines two streams - will write to one and read from another
    //https://stackoverflow.com/questions/8388657/combine-two-one-way-streams-into-a-two-way-stream
    public class StreamRWJoin : Stream
    {
        public Stream WriteStream { get; set; }
        public Stream ReadStream { get; set; }
        private bool leaveOpen;

        public StreamRWJoin(Stream readfrom, Stream writeto, bool leaveOpen = false)
        {
            WriteStream = writeto; ReadStream = readfrom;
            this.leaveOpen = leaveOpen;
        }

        public override bool CanRead
        {
            get { return ReadStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return WriteStream.CanWrite; }
        }

        public override void Flush()
        {
            WriteStream.Flush();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return ReadStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            WriteStream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            if (!leaveOpen)
                try
                {
                    WriteStream.Close();
                }
                finally
                {
                    ReadStream.Close();
                }
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return ReadStream.BeginRead(buffer, offset, count, callback, state);
        }
        public override int EndRead(IAsyncResult asyncResult)
        {
            return ReadStream.EndRead(asyncResult);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return WriteStream.BeginWrite(buffer, offset, count, callback, state);
        }
        public override void EndWrite(IAsyncResult asyncResult)
        {
            WriteStream.EndWrite(asyncResult);
        }

        public override int ReadByte()
        {
            return ReadStream.ReadByte();
        }
        public override void WriteByte(byte value)
        {
            ReadStream.WriteByte(value);
        }

        public override int ReadTimeout
        {
            get
            {
                return ReadStream.ReadTimeout;
            }
            set
            {
                ReadStream.ReadTimeout = value;
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return WriteStream.WriteTimeout;
            }
            set
            {
                WriteStream.WriteTimeout = value;
            }
        }

        public override bool CanTimeout
        {
            get
            {
                return ReadStream.CanTimeout || WriteStream.CanTimeout;
            }
        }

        public override int GetHashCode()
        {
            return ReadStream.GetHashCode() ^ WriteStream.GetHashCode();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !leaveOpen)
            {
                try
                {
                    ReadStream.Dispose();
                }
                finally
                {
                    WriteStream.Dispose();
                }
            }
        }

        public override string ToString()
        {
            return "Read: " + ReadStream.ToString() + ", Write: " + WriteStream.ToString();
        }
    }
}
