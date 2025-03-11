using System.Text.Json.Serialization;
using VotingApp.API.DTOs.Party;

namespace VotingApp.API.DTOs
{
    public class ApiResponseDTO<T>
    {

        public bool Error { get; set; }
        public int Code { get; set; }
        public string ResponseCode { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public T? Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public bool? Success { get; set; }

        public ApiResponseDTO(bool error, int code, string responseCode, T? data = default, bool? success = null)
        {
            Error = error;
            Code = code;
            ResponseCode = responseCode;
            Data = data;
            Success = success;
        }

     
    }

}
