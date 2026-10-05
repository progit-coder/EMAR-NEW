using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{    
    public class StagingMSHEntity
    {
        public int StagMsh_Id { get; set; }
        public Nullable<int> File_Id { get; set; }
        public string FieldSeperator { get; set; }
        public string EncodingCharacters { get; set; }
        public string SendingApplication { get; set; }
        public string SendingFacility { get; set; }
        public string ReceivingApplication { get; set; }
        public string ReceivingFacility { get; set; }
        public Nullable<System.DateTime> DateOfMessage { get; set; }
        public string Security { get; set; }
        public string MessageType { get; set; }
        public string MessageControlID { get; set; }
        public string ProcessingID { get; set; }
        public string VersionID { get; set; }
        public string SequenceNumber { get; set; }
        public string ContinuationPointer { get; set; }
        public string AcceptAckType { get; set; }
        public string ApplicationAckType { get; set; }
        public string CountryCode { get; set; }
        public string CharacterSet { get; set; }
        public string PrincipalLanguage { get; set; }
        public string AltCharacterSet { get; set; }
        public string MessageProfileIdentifier { get; set; }
        public virtual FileInformationEntity FileInformation { get; set; }

    }
}
