using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCGAuthoring.Models
{
    public class Request
    {
        public int Id { get; set; }                      // Request Id
    
        public DateTime CreatedDate { get; set; }
        public ReqState State { get; set; }               // State column

        public string JsonData { get; set; }            // Json data from the webserver
        public string ResultData { get; set; }          // Result data from the unity

        public Request()
        {
            this.CreatedDate = DateTime.UtcNow;
        }
    }

    public enum ReqState
    {
        CREATED,
        PICKED,
        GENERATED,
        COMPLETED
    }
}
