using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace UrbanFTProject.Models
{
    public class ErrorViewModel
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; } = default!;

        [NotMapped]
        public bool ShowError => ErrorCode!=0 & string.IsNullOrEmpty(Message);

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}