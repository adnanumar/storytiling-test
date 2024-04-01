using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytiling.ApplicationCore.Settings
{
    public class DynamoDBSettings
    {
        public const string Section = "DynamoDBSettings";
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
    }
}
