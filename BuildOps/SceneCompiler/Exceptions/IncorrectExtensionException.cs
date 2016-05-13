using System;

namespace Zephyr.BuildOps.SceneCompiler.Exceptions
{
    public class IncorrectExtensionException : Exception
    {
        private const string DefaultMessage =
            "You have created a scene compilation xml without the .xml extension. Please select the xml extension when saving the file.";

        public IncorrectExtensionException() : base(DefaultMessage) { }
        public IncorrectExtensionException(string message) : base(message) { }
    }
}
