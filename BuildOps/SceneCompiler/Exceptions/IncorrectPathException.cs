using System;

namespace Zephyr.BuildOps.SceneCompiler.Exceptions
{
    public class IncorrectPathException : Exception
    {
        private const string DefaultMessage = "You have created a scene compilation xml outside of the resources folder. " + "Please create the file in the resources folder.";

        public IncorrectPathException() : base(DefaultMessage) { }
        public IncorrectPathException(string message) : base(message) { }
    }
}