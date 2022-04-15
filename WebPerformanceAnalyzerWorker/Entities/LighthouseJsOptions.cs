using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebPerformanceAnalyzerWorker.Entities
{
    public class LighthouseJsOptions
    {
        public IEnumerable<string> chromeFlags { get; set; }
        public int? maxWaitForLoad { get; set; }
        public IEnumerable<string> blockedUrlPatterns { get; set;}
        public bool? disableStorageReset { get; set;}
        public bool? disableDeviceEmulation { get; set; }
        /// <summary>
        /// For LH < version 7.0
        /// </summary>
        public string emulatedFormFactor { get; set; }
        /// <summary>
        /// For LH >= version 7.0
        /// </summary>
        public string preset { get; set; }

        public string output { get; set; }
        
        [JsonIgnore]
        public IEnumerable<Category> OnlyCategories {get; set;}
        public IEnumerable<string> onlyCategories
        {
            get
            {
                return OnlyCategories?.Select(s=> s.ToString());
            }
        }

        public ThrottlingSettings Throttling { get; set;}

        public  class ThrottlingSettings
        {
            
        }
    }
}
