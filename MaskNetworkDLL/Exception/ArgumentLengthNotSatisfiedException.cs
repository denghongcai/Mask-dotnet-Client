using System;
using System.Runtime.Serialization;

namespace MaskGame.Exception
{
    public class ArgumentLengthNotSatisfiedException : ArgumentException, ISerializable
    {
        public ArgumentLengthNotSatisfiedException() { }

        public virtual object ActualValue { get; }

        public override string Message { get; }
    }
}
