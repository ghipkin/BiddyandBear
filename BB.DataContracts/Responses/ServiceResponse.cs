using System.Runtime.Serialization;

namespace BB.DataContracts
{
    [DataContract]
    public class ServiceResponse
    {
        [DataMember(Order = 0)]
        public int CallResult { get; set; }

        /// <summary>
        /// see below for valid message types
        /// </summary>
        /// <remarks>
        /// <para>None = 0</para>
        /// <para>Information = 1</para>
        /// <para>Warning = 2</para>
        /// <para>Error = 3</para>
        /// </remarks>
        [DataMember(Order = 0)]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// A string containing an appropriate message - not mandatory
        /// </summary>
        [DataMember(Order = 0)]
        public string Message { get; set; }
    }

    public enum MessageType { none = 0, Information = 1, Warning = 2, Error = 3}
}