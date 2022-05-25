using System.IO;

namespace Framework.Infrastructure.Extensions
{
    public static class MemoryStreamExtension
    {

        public static void ReadFromAnotherStream(this MemoryStream memoryStream, Stream source)
        {
            CopyStream(source, memoryStream);
        }

        public static void ReadFromAnotherStreamAndFlush(this MemoryStream memoryStream, Stream source)
        {
            CopyStream(source, memoryStream);
            memoryStream.Flush();
            memoryStream.Position = 0;
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

    }
}
