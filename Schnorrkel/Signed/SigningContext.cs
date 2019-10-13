namespace Schnorrkel.Signed
{
    using System;
    using Schnorrkel.Merlin;

    public class SigningContext
    {
        public Transcript ts;

        public SigningContext(byte[] context)
        {
            ts = new Transcript("SigningContext");
            ts.AppendMessage(string.Empty, context);
        }

        public Transcript Bytes(byte[] data)
        {
            var clone = ts.Clone();
            clone.AppendMessage("sign-bytes", data);
            return clone;
        }

        public Transcript GetTranscript()
        {
            return ts;
        }

        public Transcript Xof()
        {
            throw new NotImplementedException();
        }

        public Transcript Hash256()
        {
            throw new NotImplementedException();
        }

        public Transcript Hash512()
        {
            throw new NotImplementedException();
        }
    }
}
