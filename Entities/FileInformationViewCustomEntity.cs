using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FileInformationViewCustomEntity
    {
        public string FileData { get; set; }
        public string FileAckData { get; set; }
        public List<string> FileError { get; set; }
    }
}
